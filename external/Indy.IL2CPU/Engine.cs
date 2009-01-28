#define VERBOSE_DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using Indy.IL2CPU.Assembler;
using Indy.IL2CPU.Assembler.X86;
using Indy.IL2CPU.IL;
using Indy.IL2CPU.Plugs;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Threading;
using System.Diagnostics;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU
{
    public enum DebugMode { None, IL, Source, MLUsingGDB }

    public class MethodBaseComparer : IComparer<MethodBase>
    {
        #region IComparer<MethodBase> Members

        public int Compare(MethodBase x,
                           MethodBase y)
        {
            return x.GetFullName().CompareTo(y.GetFullName());
        }

        #endregion
    }

    public class FieldDefinitionComparer : IComparer<FieldDefinition>
    {
        #region IComparer<FieldDefinition> Members

        public int Compare(FieldDefinition x, FieldDefinition y)
        {
            return x.ToString().CompareTo(y.ToString());
        }

        #endregion
    }

    public class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x,
                           Type y)
        {
            return x.AssemblyQualifiedName.CompareTo(y.AssemblyQualifiedName);
        }
    }

    public class TypeEqualityComparer : IEqualityComparer<TypeDefinition>
    {
        public bool Equals(TypeDefinition x, TypeDefinition y)
        {
            return x.FullName.Equals(y.FullName);
        }

        public int GetHashCode(TypeDefinition obj)
        {
            return obj.FullName.GetHashCode();
        }
    }

    public class AssemblyEqualityComparer : IEqualityComparer<Assembly>
    {
        public bool Equals(Assembly x,
                           Assembly y)
        {
            return x.GetName().FullName.Equals(y.GetName().FullName);
        }

        public int GetHashCode(Assembly obj)
        {
            return obj.GetName().FullName.GetHashCode();
        }
    }

    public enum LogSeverityEnum : byte { Warning = 0, Error = 1, Informational = 2 }

    public delegate void DebugLogHandler(LogSeverityEnum aSeverity, string aMessage);

    public enum TargetPlatformEnum { X86 }

    public enum TraceAssemblies { All, Cosmos, User };

    public class QueuedMethodInformation
    {
        public bool Processed;
        public bool PreProcessed;
        public int Index;
        public MLDebugSymbol[] Instructions;
        public readonly SortedList<string, object> Info = new SortedList<string, object>(StringComparer.InvariantCultureIgnoreCase);
        public MethodDefinition Implementation;
    }

    public class QueuedStaticFieldInformation
    {
        public bool Processed;
    }

    public class Engine
    {
        protected static Engine _current;
        protected AssemblyDefinition _crawledAssembly;
        protected OpCodeMap _map;
        protected Assembler.Assembler _assembler;

        public TraceAssemblies TraceAssemblies { get; set; }

        private SortedList<string, MethodDefinition> _plugMethods;
        private SortedList<Type, Dictionary<string, PlugFieldAttribute>> _plugFields;

        /// <summary>
        /// Contains a list of all methods. This includes methods to be processed and already processed.
        /// </summary>
        protected IDictionary<MethodDefinition, QueuedMethodInformation> _methods = new SortedList<MethodDefinition, QueuedMethodInformation>(new MethodDefinitionComparer());
        protected ReaderWriterLocker _methodsLocker = new ReaderWriterLocker();

        /// <summary>
        /// Contains a list of all static fields. This includes static fields to be processed and already processed.
        /// </summary>
        protected IDictionary<FieldDefinition, QueuedStaticFieldInformation> _staticFields = new SortedList<FieldDefinition, QueuedStaticFieldInformation>(new FieldDefinitionComparer());
        protected ReaderWriterLocker mStaticFieldsLocker = new ReaderWriterLocker();
        protected TypeDefinitionCollection _types = new TypeDefinitionCollection(null);
        protected ReaderWriterLocker _typesLocker = new ReaderWriterLocker();
        protected TypeEqualityComparer mTypesEqualityComparer = new TypeEqualityComparer();
        public DebugMode DebugMode { get; set; }
        public TargetPlatformEnum TargetPlatform { get; set; }
        private List<MLDebugSymbol> mSymbols = new List<MLDebugSymbol>();
        private ReaderWriterLocker mSymbolsLocker = new ReaderWriterLocker();
        public string OutputDirectory { get; set; }

        public event Action<int, int> CompilingMethods;
        public event Action<int, int> CompilingStaticFields;

        public Engine()
        {
            this.TargetPlatform = TargetPlatformEnum.X86;
        }

        /// <summary>
        /// Compiles an assembly to CPU-specific code. The entrypoint of the assembly will be 
        /// crawled to see what is neccessary, same goes for all dependencies.
        /// </summary>
        /// <remarks>For now, only entrypoints without params are supported!</remarks>
        /// <param name="aAssembly">The assembly of which to crawl the entry-point method.</param>
        /// <param name="aTargetPlatform">The platform to target when assembling the code.</param>
        /// <param name="aOutput"></param>

        //TODO: Way too many params, these should be properties
        public void Execute(string assemblyPath, Func<string, string> getFileNameForGroup, IEnumerable<string> aPlugs, bool aGDBDebug, bool aUseBinaryEmission)
        {
            _current = this;
            try
            {
                if (getFileNameForGroup == null)
                    getFileNameForGroup = g => Path.Combine(OutputDirectory, g + ".asm");

                _crawledAssembly = AssemblyFactory.GetAssembly(assemblyPath);

                var entryPoint = _crawledAssembly.EntryPoint;
                if (entryPoint == null)
                    throw new NotSupportedException("No EntryPoint found!");

                var entryPointType = TypeResolver.Resolve(entryPoint.DeclaringType);

                entryPoint = entryPointType.Methods.GetMethod("Init", new Type[0]);
                AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(assemblyPath));

                switch (this.TargetPlatform)
                {
                    case TargetPlatformEnum.X86:
                        {
                            _map = new Indy.IL2CPU.IL.X86.X86OpCodeMap();
                            _assembler = new Assembler.X86.CosmosAssembler();
                            break;
                        }
                    default:
                        throw new NotSupportedException("TargetPlatform '" + this.TargetPlatform + "' not supported!");
                }
                InitializePlugs(aPlugs);
                using (_assembler)
                {
                    _assembler.Initialize();
                    //mAssembler.OutputType = Assembler.Win32.Assembler.OutputTypeEnum.Console;
                    //foreach (string xPlug in aPlugs) {
                    //this.I
                    var appDefs = new List<AssemblyDefinition>();
                    appDefs.Add(_crawledAssembly);
                    var comparer = new AssemblyEqualityComparer();
                    foreach (Assembly xAsm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        var assemblyDef = AssemblyFactory.GetAssembly(xAsm.Location);
                        if (!appDefs.Contains(assemblyDef))
                            appDefs.Add(assemblyDef);
                    }
                    _map.Initialize(_assembler, appDefs);
                    //!String.IsNullOrEmpty(aDebugSymbols);
                    IL.Op.QueueMethod += QueueMethod;
                    IL.Op.QueueStaticField += QueueStaticField;
                    try
                    {
                        using (_typesLocker.AcquireWriterLock())
                            _types.Add(TypeResolver.Resolve<object>());

                        using (_methodsLocker.AcquireWriterLock())
                        {
                            _methods.Add(RuntimeEngineRefs.InitializeApplicationRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(RuntimeEngineRefs.FinalizeApplicationRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(TypeResolver.Resolve<Assembler.Assembler>().Methods.GetMethod("PrintException")[0], new QueuedMethodInformation() { Index = _methods.Count });
                            _methods.Add(VTablesImplRefs.LoadTypeTableRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(VTablesImplRefs.SetMethodInfoRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(VTablesImplRefs.IsInstanceRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(VTablesImplRefs.SetTypeInfoRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(VTablesImplRefs.GetMethodAddressForTypeRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(GCImplementationRefs.IncRefCountRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(GCImplementationRefs.DecRefCountRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(GCImplementationRefs.AllocNewObjectRef, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                            _methods.Add(entryPoint, new QueuedMethodInformation() { Processed = false, Index = _methods.Count });
                        }
                        ScanAllMethods();
                        ScanAllStaticFields();
                        _map.PreProcess(_assembler);
                        do
                        {
                            int xOldCount;
                            using (_methodsLocker.AcquireReaderLock())
                            {
                                xOldCount = _methods.Count;
                            }
                            ScanAllMethods();
                            ScanAllStaticFields();
                            ScanForMethodsToIncludeForVMT();
                            int xNewCount;
                            using (_methodsLocker.AcquireReaderLock())
                            {
                                xNewCount = _methods.Count;
                            }
                            if (xOldCount == xNewCount)
                            {
                                break;
                            }
                        } while (true);
                        // initialize the runtime engine
                        MainEntryPointOp entryPointOp = (MainEntryPointOp)GetOpFromType(_map.MainEntryPointOp, null, null);
                        entryPointOp.Assembler = _assembler;
                        entryPointOp.Enter(Assembler.Assembler.EntryPointName);
                        entryPointOp.Call(RuntimeEngineRefs.InitializeApplicationRef);
                        entryPointOp.Call("____INIT__VMT____");
                        using (_typesLocker.AcquireWriterLock())
                        {
                            // call all static constructors
                            foreach (TypeDefinition type in _types)
                                foreach (MethodDefinition method in type.Constructors)
                                    if (method.IsStatic)
                                        entryPointOp.Call(method);
                        }
                        entryPointOp.Call(entryPoint);
                        if (entryPoint.ReturnType.ReturnType.Name == TypeResolver.Resolve(typeof(void)).FullName)
                        {
                            entryPointOp.Push(0);
                        }
                        // todo: implement support for returncodes?
                        entryPointOp.Call(RuntimeEngineRefs.FinalizeApplicationRef);
                        entryPointOp.Exit();
                        using (_methodsLocker.AcquireWriterLock())
                        {
                            _methods = new ReadOnlyDictionary<MethodDefinition, QueuedMethodInformation>(_methods);
                        }
                        using (mStaticFieldsLocker.AcquireWriterLock())
                        {
                            _staticFields = new ReadOnlyDictionary<FieldInfo, QueuedStaticFieldInformation>(_staticFields);
                        }
                        ProcessAllMethods();
                        _map.PostProcess(_assembler);
                        ProcessAllStaticFields();
                        GenerateVMT(this.DebugMode != DebugMode.None);
                        using (mSymbolsLocker.AcquireReaderLock())
                        {
                            if (mSymbols != null)
                            {
                                string xOutputFile = Path.Combine(this.OutputDirectory, "debug.cxdb");
                                MLDebugSymbol.WriteSymbolsListToFile(mSymbols, xOutputFile);
                            }
                        }
                    }
                    finally
                    {
                        if (aUseBinaryEmission)
                        {
                            using (Stream xOutStream = new FileStream(Path.Combine(this.OutputDirectory, "output.bin"), FileMode.Create))
                            {
                                Stopwatch xSW = new Stopwatch();
                                xSW.Start();
                                try
                                {
                                    _assembler.FlushBinary(xOutStream, 0x200000);
                                }
                                finally
                                {
                                    xSW.Stop();
                                    Debug.WriteLine(String.Format("Binary Emission took: {0}", xSW.Elapsed));
                                }
                            }
                        }
                        else
                        {
                            using (StreamWriter xOutput = new StreamWriter(getFileNameForGroup("main")))
                            {
                                _assembler.FlushText(xOutput);
                            }
                        }
                        IL.Op.QueueMethod -= QueueMethod;
                        IL.Op.QueueStaticField -= QueueStaticField;
                    }
                }
            }
            finally
            {
                _current = null;
            }
        }

        // EDIT BELOW TO CHANGE THREAD COUNT:
        private int mThreadCount = 1;// Environment.ProcessorCount;
        private AutoResetEvent[] mThreadEvents = new AutoResetEvent[1];//new AutoResetEvent[mThreadCount];

        private void ScanAllMethods()
        {
            for (int i = 0; i < mThreadCount; i++)
            {
                mThreadEvents[i] = new AutoResetEvent(false);
                var xThread = new Thread(DoScanMethods);
                xThread.Start(i);
            }
            int xFinishedThreads = 0;
            while (xFinishedThreads < mThreadCount)
            {
                for (int i = 0; i < mThreadCount; i++)
                {
                    if (mThreadEvents[i] != null)
                    {
                        if (mThreadEvents[i].WaitOne(10, false))
                        {
                            mThreadEvents[i].Close();
                            mThreadEvents[i] = null;
                            xFinishedThreads++;
                        }
                    }
                }
            }
        }

        private void DoScanMethods(object aData)
        {
            //ProgressChanged.Invoke("Scanning methods");
            int xThreadIndex = (int)aData;
            try
            {
                int index = -1;
                MethodDefinition currentMethod;
                while (true)
                {
                    index++;
                    if ((index % mThreadCount) != xThreadIndex)
                    {
                        continue;
                    }
                    using (_methodsLocker.AcquireReaderLock())
                    {
                        currentMethod = (from item in _methods.Keys
                                         where !_methods[item].PreProcessed
                                         select item).FirstOrDefault();
                    }
                    if (currentMethod == null)
                    {
                        break;
                    }
                    //ProgressChanged.Invoke(String.Format("Scanning method: {0}", xCurrentMethod.GetFullName()));
                    EmitDependencyGraphLine(true, currentMethod.ToString());
                    try
                    {
                        RegisterType(currentMethod.DeclaringType);
                        using (_methodsLocker.AcquireReaderLock())
                        {
                            _methods[currentMethod].PreProcessed = true;
                        }
                        if (currentMethod.IsAbstract)
                        {
                            continue;
                        }
                        string methodName = Label.GenerateLabelName(currentMethod);
                        TypeInformation xTypeInfo = null;
                        if (!currentMethod.IsStatic)
                        {
                            xTypeInfo = GetTypeInfo(currentMethod.DeclaringType);
                        }
                        MethodInformation xMethodInfo;
                        using (_methodsLocker.AcquireReaderLock())
                        {
                            xMethodInfo = GetMethodInfo(currentMethod, currentMethod, methodName, xTypeInfo, this.DebugMode != DebugMode.None, _methods[currentMethod].Info);
                        }

                        var customImplementation = GetCustomMethodImplementation(methodName);
                        if (customImplementation != null)
                        {
                            QueueMethod(customImplementation);
                            using (_methodsLocker.AcquireReaderLock())
                            {
                                _methods[currentMethod].Implementation = customImplementation;
                            }
                            continue;
                        }
                        Type xOpType = _map.GetOpForCustomMethodImplementation(methodName);
                        if (xOpType != null)
                        {
                            Op xMethodOp = GetOpFromType(xOpType, null, xMethodInfo);
                            if (xMethodOp != null)
                            {
                                continue;
                            }
                        }
                        if (_map.HasCustomAssembleImplementation(xMethodInfo))
                        {
                            _map.ScanCustomAssembleImplementation(xMethodInfo);
                            continue;
                        }

                        //xCurrentMethod.GetMethodImplementationFlags() == MethodImplAttributes.
                        var body = currentMethod.Body;
                        // todo: add better detection of implementation state
                        if (body != null)
                        {
                            mInstructionsToSkip = 0;
                            _assembler.StackContents.Clear();
                            ILReader xReader = new ILReader(currentMethod);
                            var xInstructionInfos = new List<DebugSymbolsAssemblyTypeMethodInstruction>();
                            while (xReader.Read())
                            {
                                SortedList<string, object> xInfo = null;
                                using (_methodsLocker.AcquireReaderLock())
                                {
                                    xInfo = _methods[currentMethod].Info;
                                }
                                _map.ScanILCode(xReader, xMethodInfo, xInfo);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        OnDebugLog(LogSeverityEnum.Error, currentMethod.ToString());
                        OnDebugLog(LogSeverityEnum.Warning, e.ToString());
                        throw;
                    }
                }
                using (_typesLocker.AcquireReaderLock())
                {
                    _types.Accept(new AbstractReflectionVisitor{ 
                        VisitTypeDefinition = (a,b) => b.Constructors.Accept(a), 
                        VisitConstructor = delegate(AbstractReflectionVisitor a, MethodDefinition method){ 
                            if(method.IsStatic)
                                QueueMethod(method);
                        }
                    });
                }
            }
            finally
            {
                mThreadEvents[xThreadIndex].Set();
            }
        }

        private void ScanAllStaticFields()
        {
        }

        private void GenerateDebugSymbols()
        {
            /*var xAssemblyComparer = new AssemblyEqualityComparer();
			var xTypeComparer = new TypeEqualityComparer();
			var xDbgAssemblies = new List<DebugSymbolsAssembly>();
			int xTypeCount = mTypes.Count;
			try {
				foreach (var xAssembly in (from item in mTypes
										   select item.Assembly).Distinct(xAssemblyComparer)) {
					var xDbgAssembly = new DebugSymbolsAssembly();
					var xDbgAssemblyTypes = new List<DebugSymbolsAssemblyType>();
					xDbgAssembly.FileName = xAssembly.Location;
					xDbgAssembly.FullName = xAssembly.GetName().FullName;
					//if (xDbgAssembly.FullName == "Cosmos.Hardware, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5ae71220097cb983") {
					//    System.Diagnostics.Debugger.Break();
					//}
					for (int xIdxTypes = 0; xIdxTypes < mTypes.Count; xIdxTypes++) {
						var xType = mTypes[xIdxTypes];
						if (!xAssemblyComparer.Equals(xAssembly, xType.Assembly)) {
							continue;
						}
						var xDbgType = new DebugSymbolsAssemblyType();
						//if (xType.FullName == "Cosmos.Hardware.Screen.Text") {
						//    System.Diagnostics.Debugger.Break();
						//}
						if (xType.BaseType != null) {
							xDbgType.BaseTypeId = GetTypeId(xType.BaseType);
						}
						xDbgType.TypeId = xIdxTypes;
						xDbgType.FullName = xType.FullName;
						var xTypeFields = new List<DebugSymbolsAssemblyTypeField>();
						var xTypeInfo = GetTypeInfo(xType);
						xDbgType.StorageSize = GetFieldStorageSize(xType);
						foreach (var xField in xType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
							var xDbgField = new DebugSymbolsAssemblyTypeField();
							xDbgField.Name = xField.Name;
							xDbgField.IsStatic = xField.IsStatic;
							if (xField.IsPublic) {
								xDbgField.Visibility = "Public";
							} else {
								if (xField.IsPrivate) {
									xDbgField.Visibility = "Private";
								} else {
									if (xField.IsFamily) {
										xDbgField.Visibility = "Protected";
									} else {
										xDbgField.Visibility = "Internal";
									}
								}
							}
							xDbgField.FieldType = GetTypeId(xField.FieldType);
							if (xDbgField.IsStatic) {
								xDbgField.Address = DataMember.GetStaticFieldName(xField);
							} else {
								xDbgField.Address = "+" + xTypeInfo.Fields[xField.GetFullName()].Offset;
							}
							xTypeFields.Add(xDbgField);
						}
						xDbgType.Field = xTypeFields.ToArray();
						var xTypeMethods = new List<DebugSymbolsAssemblyTypeMethod>();
						foreach (var xMethod in xType.GetMethods(BindingFlags.ExactBinding | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance).Cast<MethodBase>().Union(xType.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))) {
							var xIdxMethods = mMethods.IndexOfKey(xMethod);
							if (xIdxMethods == -1) {
								continue;
							}
							//var xMethod = mMethods.Keys[xIdxMethods];
							//if (!xTypeComparer.Equals(xMethod.DeclaringType, xType)) {
							//    continue;
							//}
							var xDbgMethod = new DebugSymbolsAssemblyTypeMethod();
							xDbgMethod.Name = xMethod.Name;
							xDbgMethod.MethodId = xIdxMethods;
							xDbgMethod.Address = Label.GenerateLabelName(xMethod);
							if (xMethod is ConstructorInfo) {
								xDbgMethod.ReturnTypeId = GetTypeId(typeof(void));
							} else {
								var xTheMethod = xMethod as MethodInfo;
								if (xTheMethod != null) {
									xDbgMethod.ReturnTypeId = GetTypeId(xTheMethod.ReturnType);
								} else {
									xDbgMethod.ReturnTypeId = GetTypeId(typeof(void));
								}
							}
							if (xMethod.IsPublic) {
								xDbgMethod.Visibility = "Public";
							} else {
								if (xMethod.IsPrivate) {
									xDbgMethod.Visibility = "Private";
								} else {
									if (xMethod.IsFamily) {
										xDbgMethod.Visibility = "Protected";
									} else {
										xDbgMethod.Visibility = "Internal";
									}
								}
							}
							xTypeMethods.Add(xDbgMethod);
							MethodBody xBody = xMethod.GetMethodBody();
							if (xBody != null) {
								var xDbgLocals = new List<DebugSymbolsAssemblyTypeMethodLocal>();
								var xMethodInfo = GetMethodInfo(xMethod, xMethod, Label.GenerateLabelName(xMethod), xTypeInfo);
								if (xBody.LocalVariables != null) {
									foreach (var xLocal in xBody.LocalVariables) {
										var xDbgLocal = new DebugSymbolsAssemblyTypeMethodLocal();
										xDbgLocal.Name = xLocal.LocalIndex.ToString();
										xDbgLocal.LocalTypeId = GetTypeId(xLocal.LocalType);
										xDbgLocal.RelativeStartAddress = xMethodInfo.Locals[xLocal.LocalIndex].VirtualAddresses.First();
										xDbgLocals.Add(xDbgLocal);
									}
								}
								xDbgMethod.Local = xDbgLocals.ToArray();
							}
							xDbgMethod.Body = mMethods.Values[xIdxMethods].Instructions;
						}
						xDbgType.Method = xTypeMethods.ToArray();
						xDbgAssemblyTypes.Add(xDbgType);
					}
					xDbgAssembly.Type = xDbgAssemblyTypes.ToArray();
					xDbgAssemblies.Add(xDbgAssembly);
				}
			} finally {
				if (xTypeCount != mTypes.Count) {
					Console.WriteLine("TypeCount changed (was {0}, new {1})", xTypeCount, mTypes.Count);
					Console.WriteLine("Last Type: {0}", mTypes.Last().FullName);
				}
			}*/
        }

        private void GenerateVMT(bool aDebugMode)
        {
            Op xOp = GetOpFromType(_map.MethodHeaderOp,
                                   null,
                                   new MethodInformation("____INIT__VMT____",
                                                         new MethodInformation.Variable[0],
                                                         new MethodInformation.Argument[0],
                                                         0,
                                                         false,
                                                         null,
                                                         null,
                                                         typeof(void),
                                                         aDebugMode,
                                                         new Dictionary<string, object>()));
            xOp.Assembler = _assembler;
            xOp.Assemble();
            InitVmtImplementationOp xInitVmtOp = (InitVmtImplementationOp)GetOpFromType(_map.InitVmtImplementationOp,
                                                                                        null,
                                                                                        null);
            xInitVmtOp.Assembler = _assembler;
            xInitVmtOp.Types = _types;
            xInitVmtOp.SetTypeInfoRef = VTablesImplRefs.SetTypeInfoRef;
            xInitVmtOp.SetMethodInfoRef = VTablesImplRefs.SetMethodInfoRef;
            xInitVmtOp.LoadTypeTableRef = VTablesImplRefs.LoadTypeTableRef;
            xInitVmtOp.TypesFieldRef = VTablesImplRefs.VTablesImplDef.Fields.GetField("mTypes");
            using (_methodsLocker.AcquireReaderLock())
            {
                xInitVmtOp.Methods = _methods.Keys.ToList();
            }
            xInitVmtOp.VTableEntrySize = GetFieldStorageSize(GetType("",
                                                                     typeof(VTable).FullName.Replace('+',
                                                                                                     '.')));
            xInitVmtOp.GetMethodIdentifier += delegate(MethodDefinition method)
            {
                if (method.ToString() == "System.Reflection.Cache.InternalCache System.Reflection.MemberInfo.get_Cache()")
                {
                    System.Diagnostics.Debugger.Break();
                }
                ParameterDefinitionCollection xParams = method.Parameters;
                TypeReference[] xParamTypes = new TypeReference[xParams.Count];
                for (int i = 0; i < xParams.Count; i++)
                {
                    xParamTypes[i] = xParams[i].ParameterType;
                }
                var xMethod = GetUltimateBaseMethod(method,
                                                           xParamTypes,
                                                           method.DeclaringType);
                return GetMethodIdentifier(xMethod);
            };
            using (_typesLocker.AcquireWriterLock())
            {
                xInitVmtOp.Assemble();
            }
            xOp = GetOpFromType(_map.MethodFooterOp,
                                null,
                                new MethodInformation("____INIT__VMT____",
                                                      new MethodInformation.Variable[0],
                                                      new MethodInformation.Argument[0],
                                                      0,
                                                      false,
                                                      null,
                                                      null,
                                                      typeof(void),
                                                      aDebugMode,
                                                      new Dictionary<string, object>()));
            xOp.Assembler = _assembler;
            xOp.Assemble();
        }

        private void ScanForMethodsToIncludeForVMT()
        {
            List<TypeDefinition> checkedTypes = new List<TypeDefinition>();
            int i = -1;
            while (true)
            {
                i++;
                MethodDefinition method;
                using (_methodsLocker.AcquireReaderLock())
                {
                    if (i == _methods.Count)
                    {
                        break;
                    }
                    method = _methods.ElementAt(i).Key;
                }
                if (method.IsStatic)
                {
                    continue;
                }
                var currentType = TypeResolver.Resolve(method.DeclaringType);
                if (!checkedTypes.Contains(currentType, mTypesEqualityComparer))
                {
                    checkedTypes.Add(currentType);
                }

                QueueMethod(GetUltimateBaseMethod(method,
                                                  (from item in method.Parameters.Cast<ParameterDefinition>()
                                                   select item.ParameterType).ToArray(),
                                                  currentType));
            }
            using (_typesLocker.AcquireReaderLock())
            {
                foreach (TypeDefinition type in _types)
                {
                    if (!checkedTypes.Contains(type, mTypesEqualityComparer))
                    {
                        checkedTypes.Add(type);
                    }
                }
            }
            for (i = 0; i < checkedTypes.Count; i++)
            {
                Type xCurrentType = checkedTypes[i];
                while (xCurrentType != null)
                {
                    if (!checkedTypes.Contains(xCurrentType, mTypesEqualityComparer))
                    {
                        checkedTypes.Add(xCurrentType);
                    }
                    if (xCurrentType.FullName == "System.Object")
                    {
                        break;
                    }
                    if (xCurrentType.BaseType == null)
                    {
                        break;
                    }
                    xCurrentType = xCurrentType.BaseType;
                }
            }
            foreach (Type xTD in checkedTypes)
            {
                foreach (MethodBase xMethod in xTD.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (!xMethod.IsStatic)
                    {
                        if (xTD.BaseType == null)
                        {
                            continue;
                        }
                        if (xMethod.IsVirtual && !xMethod.IsConstructor)
                        {
                            Type xCurrentInspectedType = xTD.BaseType;
                            ParameterInfo[] xParams = xMethod.GetParameters();
                            Type[] xMethodParams = new Type[xParams.Length];
                            for (int k = 0; k < xParams.Length; k++)
                            {
                                xMethodParams[k] = xParams[k].ParameterType;
                            }
                            var xBaseMethod = GetUltimateBaseMethod(xMethod,
                                                                           xMethodParams,
                                                                           xTD);
                            if (xBaseMethod != null && xBaseMethod != xMethod)
                            {
                                bool xNeedsRegistering = false;
                                using (_methodsLocker.AcquireReaderLock())
                                {
                                    xNeedsRegistering = _methods.ContainsKey(xBaseMethod);
                                }
                                if (xNeedsRegistering)
                                {
                                    QueueMethod(xMethod);
                                }
                            }
                        }
                    }
                }
            }
            int j = -1;
            while (true)
            {
                j++;
                KeyValuePair<MethodBase, QueuedMethodInformation> xMethod;
                using (_methodsLocker.AcquireReaderLock())
                {
                    if (j == _methods.Count)
                    {
                        break;
                    }
                    xMethod = _methods.Skip(j).First();
                }
                if (xMethod.Key.DeclaringType.IsInterface)
                {
                    var xInterface = xMethod.Key.DeclaringType;
                    i = -1;
                    while (true)
                    {
                        Type xImplType;
                        i++;
                        using (_typesLocker.AcquireReaderLock())
                        {
                            if (i == _types.Count)
                            {
                                break;
                            }
                            xImplType = _types.ElementAt(i);
                        }
                        if (xImplType.IsInterface)
                        {
                            continue;
                        }
                        if (!xInterface.IsAssignableFrom(xImplType))
                        {
                            continue;
                        }

                        var xActualMethod = xImplType.GetMethod(xInterface.FullName + "." + xMethod.Key.Name,
                                                                (from xParam in xMethod.Key.GetParameters()
                                                                 select xParam.ParameterType).ToArray());

                        if (xActualMethod == null)
                        {
                            // get private implemenation
                            xActualMethod = xImplType.GetMethod(xMethod.Key.Name,
                                                                (from xParam in xMethod.Key.GetParameters()
                                                                 select xParam.ParameterType).ToArray());
                        }
                        if (xActualMethod == null)
                        {
                            try
                            {
                                var xMap = xImplType.GetInterfaceMap(xInterface);
                                for (int k = 0; k < xMap.InterfaceMethods.Length; k++)
                                {
                                    if (xMap.InterfaceMethods[k] == xMethod.Key)
                                    {
                                        xActualMethod = xMap.TargetMethods[k];
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        if (xActualMethod != null)
                        {
                            QueueMethod(xActualMethod);
                        }
                    }
                }
            }
        }

        private static MethodDefinition GetUltimateBaseMethod(MethodDefinition aMethod,
                                                        TypeReference[] aMethodParams,
                                                        TypeReference aCurrentInspectedType)
        {
            MethodBase xBaseMethod = null;
            //try {
            while (true)
            {
                if (aCurrentInspectedType.BaseType == null)
                {
                    break;
                }
                aCurrentInspectedType = aCurrentInspectedType.BaseType;
                MethodBase xFoundMethod = aCurrentInspectedType.GetMethod(aMethod.Name,
                                                                          BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                                                                          Type.DefaultBinder,
                                                                          aMethodParams,
                                                                          new ParameterModifier[0]);
                if (xFoundMethod == null)
                {
                    break;
                }
                ParameterInfo[] xParams = xFoundMethod.GetParameters();
                bool xContinue = true;
                for (int i = 0; i < xParams.Length; i++)
                {
                    if (xParams[i].ParameterType != aMethodParams[i])
                    {
                        xContinue = false;
                        continue;
                    }
                }
                if (!xContinue)
                {
                    continue;
                }
                if (xFoundMethod != null)
                {
                    xBaseMethod = xFoundMethod;

                    if (xFoundMethod.IsVirtual == aMethod.IsVirtual && xFoundMethod.IsPrivate == false && xFoundMethod.IsPublic == aMethod.IsPublic && xFoundMethod.IsFamily == aMethod.IsFamily && xFoundMethod.IsFamilyAndAssembly == aMethod.IsFamilyAndAssembly && xFoundMethod.IsFamilyOrAssembly == aMethod.IsFamilyOrAssembly && xFoundMethod.IsFinal == false)
                    {
                        var xFoundMethInfo = xFoundMethod as MethodInfo;
                        var xBaseMethInfo = xBaseMethod as MethodInfo;
                        if (xFoundMethInfo == null && xBaseMethInfo == null)
                        {
                            xBaseMethod = xFoundMethod;
                        }
                        if (xFoundMethInfo != null && xBaseMethInfo != null)
                        {
                            if (xFoundMethInfo.ReturnType.AssemblyQualifiedName.Equals(xBaseMethInfo.ReturnType.AssemblyQualifiedName))
                            {
                                xBaseMethod = xFoundMethod;
                            }
                        }
                        //xBaseMethod = xFoundMethod;
                    }
                }
                //else
                //{
                //    xBaseMethod = xFoundMethod;
                //}
            }
            //} catch (Exception) {
            // todo: try to get rid of the try..catch
            //}
            return xBaseMethod ?? aMethod;
        }

        //todo: remove?
        public static MethodBase GetDefinitionFromMethodBase2(MethodBase aRef)
        {
            Type xTypeDef;
            bool xIsArray = false;
            if (aRef.DeclaringType.FullName.Contains("[]") || aRef.DeclaringType.FullName.Contains("[,]") || aRef.DeclaringType.FullName.Contains("[,,]"))
            {
                xTypeDef = typeof(Array);
                xIsArray = true;
            }
            else
            {
                xTypeDef = aRef.DeclaringType;
            }
            MethodBase xMethod = null;
            if (xIsArray)
            {
                Type[] xParams = (from item in aRef.GetParameters()
                                  select item.ParameterType).ToArray();
                if (aRef.Name == "Get")
                {
                    xMethod = xTypeDef.GetMethod("GetValue",
                                                 xParams);
                }
                if (aRef.Name == "Set")
                {
                    xMethod = xTypeDef.GetMethod("SetValue",
                                                 xParams);
                }
            }
            if (xMethod == null)
            {
                foreach (MethodBase xFoundMethod in xTypeDef.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (xFoundMethod.Name != aRef.Name)
                    {
                        continue;
                    }
                    string[] xRefNameParts = aRef.ToString().Split(' ');
                    string[] xFoundNameParts = xFoundMethod.ToString().Split(' ');
                    if (xFoundNameParts[0] != xRefNameParts[0])
                    {
                        //if (!(xFoundMethod.ReturnType.ReturnType is GenericParameter && aRef.ReturnType.ReturnType is GenericParameter)) {
                        //    ArrayType xFoundArray = xFoundMethod.ReturnType.ReturnType as ArrayType;
                        //    ArrayType xArray = aRef.ReturnType.ReturnType as ArrayType;
                        //    if (xArray != null && xFoundArray != null) {
                        //        if (xArray.Dimensions.Count != xFoundArray.Dimensions.Count) {
                        //            continue;
                        //        }
                        //        GenericParameter xGenericParam = xArray.ElementType as GenericParameter;
                        //        GenericParameter xFoundGenericParam = xFoundArray.ElementType as GenericParameter;
                        //        if (xGenericParam != null && xFoundGenericParam != null) {
                        //            if (xGenericParam.NextPosition != xFoundGenericParam.NextPosition) {
                        //                continue;
                        //            }
                        //        }
                        //    }
                        //}
                        continue;
                    }
                    ParameterInfo[] xFoundParams = xFoundMethod.GetParameters();
                    ParameterInfo[] xRefParams = aRef.GetParameters();
                    if (xFoundParams.Length != xRefParams.Length)
                    {
                        continue;
                    }
                    bool xMismatch = false;
                    for (int i = 0; i < xFoundParams.Length; i++)
                    {
                        if (xFoundParams[i].ParameterType.FullName != xRefParams[i].ParameterType.FullName)
                        {
                            //if (xFoundMethod.Parameters[i].ParameterType is GenericParameter && aRef.Parameters[i].ParameterType is GenericParameter) {
                            //	continue;
                            //}
                            xMismatch = true;
                            break;
                        }
                    }
                    if (!xMismatch)
                    {
                        xMethod = xFoundMethod;
                    }
                }
            }
            if (xMethod != null)
            {
                return xMethod;
            }
            //xMethod = xTypeDef.GetConstructor(aRef.Name == MethodBase.Cctor, aRef.Parameters);
            //if (xMethod != null && (aRef.Name == MethodBase.Cctor || aRef.Name == MethodBase.Ctor)) {
            //    return xMethod;
            //}
            throw new Exception("Couldn't find Method! ('" + aRef.GetFullName() + "'");
        }

        /// <summary>
        /// Gives the size to store an instance of the <paramref name="aType"/> for use in a field.
        /// </summary>
        /// <remarks>For classes, this is the pointer size.</remarks>
        /// <param name="aType"></param>
        /// <returns></returns>
        public static uint GetFieldStorageSize(TypeReference typeRef)
        {
            TypeDefinition type;

            if (typeRef is TypeDefinition)
                type = typeRef;
            else
                type = TypeResolver.Resolve(typeRef);

            if (type.FullName == "System.Void")
            {
                return 0;
            }
            if ((!type.IsValueType && type.IsClass) || type.IsInterface)
            {
                return 4;
            }
            switch (type.FullName)
            {
                case "System.Char":
                    return 2;
                case "System.Byte":
                case "System.SByte":
                    return 1;
                case "System.UInt16":
                case "System.Int16":
                    return 2;
                case "System.UInt32":
                case "System.Int32":
                    return 4;
                case "System.UInt64":
                case "System.Int64":
                    return 8;
                // for now hardcode IntPtr and UIntPtr to be 32-bit
                case "System.UIntPtr":
                case "System.IntPtr":
                    return 4;
                case "System.Boolean":
                    return 1;
                case "System.Single":
                    return 4;
                case "System.Double":
                    return 8;
                case "System.Decimal":
                    return 16;
                case "System.Guid":
                    return 16;
                case "System.DateTime":
                    return 8; // todo: check for correct size
            }
            if (type.FullName.EndsWith("*"))
            {
                // pointer
                return 4;
            }
            // array
            //TypeSpecification xTypeSpec = aType as TypeSpecification;
            //if (xTypeSpec != null) {
            //    return 4;
            //}
            if (type.IsEnum)
            {
                return GetFieldStorageSize(type.Fields.GetField("value__").FieldType);
            }
            if (type.IsValueType)
            {
                StructLayoutAttribute xSLA = type.StructLayoutAttribute;
                if (xSLA != null)
                {
                    if (xSLA.Size > 0)
                    {
                        return (uint)xSLA.Size;
                    }
                }
            }
            uint xResult;
            GetTypeFieldInfo(type,
                             out xResult);
            return xResult;
        }

        private static string GetGroupForType(Type aType)
        {
            return aType.Module.Assembly.GetName().Name;
        }

        protected void EmitTracer(Op aOp, string aNamespace, int aPos, int[] aCodeOffsets, string aLabel)
        {
            // NOTE - These if statemens can be optimized down - but clarity is
            // more importnat the optimizations would not offer much benefit

            // Determine if a new DebugStub should be emitted
            //bool xEmit = false;
            // Skip NOOP's so we dont have breakpoints on them
            //TODO: Each IL op should exist in IL, and descendants in IL.X86.
            // Because of this we have this hack
            if (aOp.ToString() == "Indy.IL2CPU.IL.X86.Nop")
            {
                return;
            }
            else if (this.DebugMode == DebugMode.None)
            {
                return;
            }
            else if (this.DebugMode == DebugMode.Source)
            {
                // If the current position equals one of the offsets, then we have
                // reached a new atomic C# statement
                if (aCodeOffsets != null)
                {
                    if (aCodeOffsets.Contains(aPos) == false)
                    {
                        return;
                    }
                }
            }

            // Check options for Debug Level
            // Set based on TracedAssemblies
            if (TraceAssemblies == TraceAssemblies.Cosmos || TraceAssemblies == TraceAssemblies.User)
            {
                if (aNamespace.StartsWith("System.", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (aNamespace.ToLower() == "system")
                {
                    return;
                }
                else if (aNamespace.StartsWith("Microsoft.", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }
            if (TraceAssemblies == TraceAssemblies.User)
            {
                //TODO: Maybe an attribute that could be used to turn tracing on and off
                //TODO: This doesnt match Cosmos.Kernel exact vs Cosmos.Kernel., so a user 
                // could do Cosmos.KernelMine and it will fail. Need to fix this
                if (aNamespace.StartsWith("Cosmos.Kernel", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (aNamespace.StartsWith("Cosmos.Sys", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (aNamespace.StartsWith("Cosmos.Hardware", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                else if (aNamespace.StartsWith("Indy.IL2CPU", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
            }
            // If we made it this far, emit the Tracer
            _map.EmitOpDebugHeader(_assembler, 0, aLabel);
        }

        private void ProcessAllStaticFields()
        {
            int i = -1;
            int xCount = 0;
            while (true)
            {
                i++;
                FieldDefinition currentField;
                using (mStaticFieldsLocker.AcquireReaderLock())
                {
                    xCount = _staticFields.Count;
                    if (i == xCount)
                    {
                        break;
                    }
                    currentField = _staticFields.Keys.ElementAt(i);
                }
                OnCompilingStaticFields(i, xCount);
                //ProgressChanged.Invoke(String.Format("Processing static field: {0}", xCurrentField.GetFullName()));
                string xFieldName = currentField.GetFullName();
                xFieldName = DataMember.GetStaticFieldName(currentField);
                if (_assembler.DataMembers.Count(x => x.Name == xFieldName) == 0)
                {
                    var xItem = (from item in currentField.GetCustomAttributes(false)
                                 where item.GetType().FullName == "ManifestResourceStreamAttribute"
                                 select item).FirstOrDefault();
                    string xManifestResourceName = null;
                    if (xItem != null)
                    {
                        var xItemType = xItem.GetType();
                        xManifestResourceName = (string)xItemType.GetField("ResourceName").GetValue(xItem);
                    }
                    if (xManifestResourceName != null)
                    {
                        //RegisterType(xCurrentField.FieldType);
                        //string xFileName = Path.Combine(mOutputDir,
                        //                                (xCurrentField.DeclaringType.Assembly.FullName + "__" + xManifestResourceName).Replace(",",
                        //                                                                                                                       "_") + ".res");
                        //using (var xStream = xCurrentField.DeclaringType.Assembly.GetManifestResourceStream(xManifestResourceName)) {
                        //    if (xStream == null) {
                        //        throw new Exception("Resource '" + xManifestResourceName + "' not found!");
                        //    }
                        //    using (var xTarget = File.Create(xFileName)) {
                        //        // todo: abstract this array code out.
                        //        xTarget.Write(BitConverter.GetBytes(Engine.RegisterType(Engine.GetType("mscorlib",
                        //                                                                               "System.Array"))),
                        //                      0,
                        //                      4);
                        //        xTarget.Write(BitConverter.GetBytes((uint)InstanceTypeEnum.StaticEmbeddedArray),
                        //                      0,
                        //                      4);
                        //        xTarget.Write(BitConverter.GetBytes((int)xStream.Length), 0, 4);
                        //        xTarget.Write(BitConverter.GetBytes((int)1), 0, 4);
                        //        var xBuff = new byte[128];
                        //        while (xStream.Position < xStream.Length) {
                        //            int xBytesRead = xStream.Read(xBuff, 0, 128);
                        //            xTarget.Write(xBuff, 0, xBytesRead);
                        //        }
                        //    }
                        //}
                        //mAssembler.DataMembers.Add(new DataMember("___" + xFieldName + "___Contents",
                        //                                          "incbin",
                        //                                          "\"" + xFileName + "\""));
                        //mAssembler.DataMembers.Add(new DataMember(xFieldName,
                        //                                          "dd",
                        //                                          "___" + xFieldName + "___Contents"));
                        throw new NotImplementedException();
                    }
                    else
                    {
                        RegisterType(currentField.FieldType);
                        uint xTheSize;
                        //string theType = "db";
                        Type xFieldTypeDef = currentField.FieldType;
                        if (!xFieldTypeDef.IsClass || xFieldTypeDef.IsValueType)
                        {
                            xTheSize = GetFieldStorageSize(currentField.FieldType);
                        }
                        else
                        {
                            xTheSize = 4;
                        }
                        byte[] xData = new byte[xTheSize];
                        try
                        {
                            object xValue = currentField.GetValue(null);
                            if (xValue != null)
                            {
                                try
                                {
                                    xData = new byte[xTheSize];
                                    if (xValue.GetType().IsValueType)
                                    {
                                        for (int x = 0; x < xTheSize; x++)
                                        {
                                            xData[x] = Marshal.ReadByte(xValue,
                                                                        x);
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                        }
                        _assembler.DataMembers.Add(new DataMember(xFieldName, xData));
                    }
                }
                using (mStaticFieldsLocker.AcquireReaderLock())
                {
                    _staticFields[currentField].Processed = true;
                }
            }
            OnCompilingStaticFields(i, xCount);
        }

        private void OnCompilingStaticFields(int i, int xCount)
        {
            Action<int, int> handler = this.CompilingStaticFields;
            if (handler != null)
                handler(i, xCount);
        }

        //private ISymbolReader GetSymbolReaderForAssembly(Assembly aAssembly) {
        //    return SymbolAccess.GetReaderForFile(aAssembly.Location);
        //}

        private void ProcessAllMethods()
        {
            int i = -1;
            int xCount = 0;
            while (true)
            {
                i++;
                MethodDefinition currentMethod;
                using (_methodsLocker.AcquireReaderLock())
                {
                    xCount = _methods.Count;
                    if (i == xCount)
                    {
                        break;
                    }
                    currentMethod = _methods.Keys.ElementAt(i);
                }
                OnCompilingMethods(i, xCount);
                OnDebugLog(LogSeverityEnum.Informational, "Processing method {0}", currentMethod.ToString());
                try
                {
                    EmitDependencyGraphLine(true, currentMethod.ToString());
                    RegisterType(currentMethod.DeclaringType);
                    if (currentMethod.IsAbstract)
                    {
                        using (_methodsLocker.AcquireReaderLock())
                        {
                            _methods[currentMethod].Processed = true;
                        }
                        continue;
                    }
                    string xMethodName = Label.GenerateLabelName(currentMethod);
                    TypeInformation xTypeInfo = null;
                    if (!currentMethod.IsStatic)
                    {
                        xTypeInfo = GetTypeInfo(currentMethod.DeclaringType);
                    }
                    SortedList<string, object> xMethodScanInfo;
                    using (_methodsLocker.AcquireReaderLock())
                    {
                        xMethodScanInfo = _methods[currentMethod].Info;
                    }
                    MethodInformation xMethodInfo = GetMethodInfo(currentMethod, currentMethod, xMethodName, xTypeInfo, this.DebugMode != DebugMode.None, xMethodScanInfo);

                    Op xOp = GetOpFromType(_map.MethodHeaderOp, null, xMethodInfo);
                    xOp.Assembler = _assembler;
#if VERBOSE_DEBUG
                    string comment = "(No Type Info available)";
                    if (xMethodInfo.TypeInfo != null)
                    {
                        comment = "Type Info:\r\n \r\n" + xMethodInfo.TypeInfo;
                    }
                    foreach (string s in comment.Trim().Split(new string[] { "\r\n" }
                     , StringSplitOptions.RemoveEmptyEntries))
                    {
                        new Comment(s);
                    }
                    comment = xMethodInfo.ToString();
                    foreach (string s in comment.Trim().Split(new string[] { "\r\n" }
                     , StringSplitOptions.RemoveEmptyEntries))
                    {
                        new Comment(s);
                    }
#endif
                    xOp.Assemble();
                    MethodBase xCustomImplementation = GetCustomMethodImplementation(xMethodName);
                    bool xIsCustomImplementation = (xCustomImplementation != null);
                    // what to do if a method doesn't have a body?
                    bool xContentProduced = false;
                    if (xIsCustomImplementation)
                    {
                        // this is for the support for having extra fields on types, and being able to use
                        // them in custom implementation methods
                        CustomMethodImplementationProxyOp xProxyOp
                         = (CustomMethodImplementationProxyOp)GetOpFromType(
                         _map.CustomMethodImplementationProxyOp, null, xMethodInfo);
                        xProxyOp.Assembler = _assembler;
                        xProxyOp.ProxiedMethod = xCustomImplementation;
                        xProxyOp.Assemble();
                        xContentProduced = true;
                    }
                    if (!xContentProduced)
                    {
                        Type xOpType = _map.GetOpForCustomMethodImplementation(xMethodName);
                        if (xOpType != null)
                        {
                            Op xMethodOp = GetOpFromType(xOpType, null, xMethodInfo);
                            if (xMethodOp != null)
                            {
                                xMethodOp.Assembler = _assembler;
                                xMethodOp.Assemble();
                                xContentProduced = true;
                            }
                        }
                    }
                    if (!xContentProduced)
                    {
                        if (_map.HasCustomAssembleImplementation(xMethodInfo))
                        {
                            _map.DoCustomAssembleImplementation(_assembler, xMethodInfo);
                            // No plugs, we need to compile the IL from the method
                        }
                        else
                        {
                            var xBody = currentMethod.Body;
                            // todo: add better detection of implementation state
                            if (xBody != null)
                            {
                                mInstructionsToSkip = 0;
                                _assembler.StackContents.Clear();
                                var xInstructionInfos = new List<DebugSymbolsAssemblyTypeMethodInstruction>();

                                // Section currently is dead code. Working on matching it up 
                                // with contents from inside the read
                                int[] xCodeOffsets = null;
                                //if (mDebugMode == DebugMode.Source) {
                                //    var xSymbolReader = GetSymbolReaderForAssembly(xCurrentMethod.DeclaringType.Assembly);
                                //    if (xSymbolReader != null) {
                                //        var xSmbMethod = xSymbolReader.GetMethod(new SymbolToken(xCurrentMethod.MetadataToken));
                                //        // This gets the Sequence Points.
                                //        // Sequence Points are spots that identify what the compiler/debugger says is a spot
                                //        // that a breakpoint can occur one. Essentially, an atomic source line in C#
                                //        if (xSmbMethod != null) {
                                //            xCodeOffsets = new int[xSmbMethod.SequencePointCount];
                                //            var xCodeDocuments = new ISymbolDocument[xSmbMethod.SequencePointCount];
                                //            var xCodeLines = new int[xSmbMethod.SequencePointCount];
                                //            var xCodeColumns = new int[xSmbMethod.SequencePointCount];
                                //            var xCodeEndLines = new int[xSmbMethod.SequencePointCount];
                                //            var xCodeEndColumns = new int[xSmbMethod.SequencePointCount];
                                //            xSmbMethod.GetSequencePoints(xCodeOffsets, xCodeDocuments
                                //             , xCodeLines, xCodeColumns, xCodeEndLines, xCodeEndColumns);
                                //        }
                                //    }
                                //}

                                // Scan each IL op in the method
                                currentMethod.Accept(new AbstractCodeVisitor { });

                                while (xReader.Read())
                                {
                                    ExceptionHandlingClause xCurrentHandler = null;

                                    #region Exception handling support code
                                    // todo: add support for nested handlers using a stack or so..
                                    foreach (ExceptionHandlingClause xHandler in xBody.ExceptionHandlingClauses)
                                    {
                                        if (xHandler.TryOffset > 0)
                                        {
                                            if (xHandler.TryOffset <= xReader.NextPosition && (xHandler.TryLength + xHandler.TryOffset) > xReader.NextPosition)
                                            {
                                                if (xCurrentHandler == null)
                                                {
                                                    xCurrentHandler = xHandler;
                                                    continue;
                                                }
                                                else if (xHandler.TryOffset > xCurrentHandler.TryOffset && (xHandler.TryLength + xHandler.TryOffset) < (xCurrentHandler.TryLength + xCurrentHandler.TryOffset))
                                                {
                                                    // only replace if the current found handler is narrower
                                                    xCurrentHandler = xHandler;
                                                    continue;
                                                }
                                            }
                                        }
                                        if (xHandler.HandlerOffset > 0)
                                        {
                                            if (xHandler.HandlerOffset <= xReader.NextPosition && (xHandler.HandlerOffset + xHandler.HandlerLength) > xReader.NextPosition)
                                            {
                                                if (xCurrentHandler == null)
                                                {
                                                    xCurrentHandler = xHandler;
                                                    continue;
                                                }
                                                else if (xHandler.HandlerOffset > xCurrentHandler.HandlerOffset && (xHandler.HandlerOffset + xHandler.HandlerLength) < (xCurrentHandler.HandlerOffset + xCurrentHandler.HandlerLength))
                                                {
                                                    // only replace if the current found handler is narrower
                                                    xCurrentHandler = xHandler;
                                                    continue;
                                                }
                                            }
                                        }
                                        if ((xHandler.Flags & ExceptionHandlingClauseOptions.Filter) > 0)
                                        {
                                            if (xHandler.FilterOffset > 0)
                                            {
                                                if (xHandler.FilterOffset <= xReader.NextPosition)
                                                {
                                                    if (xCurrentHandler == null)
                                                    {
                                                        xCurrentHandler = xHandler;
                                                        continue;
                                                    }
                                                    else if (xHandler.FilterOffset > xCurrentHandler.FilterOffset)
                                                    {
                                                        // only replace if the current found handler is narrower
                                                        xCurrentHandler = xHandler;
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    xMethodInfo.CurrentHandler = xCurrentHandler;
                                    xOp = GetOpFromType(_map.GetOpForOpCode(xReader.OpCode), xReader
                                     , xMethodInfo);

                                    xOp.Assembler = _assembler;
                                    new Comment("StackItems = " + _assembler.StackContents.Count);
                                    foreach (var xStackContent in _assembler.StackContents)
                                    {
                                        new Comment("    " + xStackContent.Size);
                                    }

                                    // Create label for current point
                                    string xLabel = Op.GetInstructionLabel(xReader);
                                    if (xLabel.StartsWith("."))
                                    {
                                        xLabel = DataMember.FilterStringForIncorrectChars(
                                            Label.LastFullLabel + "__DOT__" + xLabel.Substring(1));
                                    }

                                    // Possibly emit Tracer call
                                    EmitTracer(xOp, currentMethod.DeclaringType.Namespace, (int)xReader.Position,
                                        xCodeOffsets, xLabel);

                                    using (mSymbolsLocker.AcquireWriterLock())
                                    {
                                        if (mSymbols != null)
                                        {
                                            var xMLSymbol = new MLDebugSymbol();
                                            xMLSymbol.LabelName = xLabel;
                                            int xStackSize = (from item in _assembler.StackContents
                                                              let xSize = (item.Size % 4 == 0)
                                                                              ? item.Size
                                                                              : (item.Size + (4 - (item.Size % 4)))
                                                              select xSize).Sum();
                                            xMLSymbol.StackDifference = xMethodInfo.LocalsSize + xStackSize;
                                            xMLSymbol.AssemblyFile = currentMethod.DeclaringType.Module.Image.FileInformation.FullName;
                                            xMLSymbol.MethodToken = currentMethod.MetadataToken;
                                            xMLSymbol.TypeToken = currentMethod.DeclaringType.MetadataToken;
                                            xMLSymbol.ILOffset = (int)xReader.Position;
                                            mSymbols.Add(xMLSymbol);
                                        }
                                    }
                                    xOp.Assemble();
                                    //if (xInstructionInfo != null) {
                                    //    int xNewStack = (from item in mAssembler.StackContents
                                    //                     let xSize = (item.Size % 4 == 0) ? item.Size : (item.Size + (4 - (item.Size % 4)))
                                    //                     select xSize).Sum();
                                    //    xInstructionInfo.StackResult = xNewStack - xCurrentStack;
                                    //    xInstructionInfo.StackResultSpecified = true;
                                    //    xInstructionInfos.Add(xInstructionInfo);
                                    //}
                                }
                                if (mSymbols != null)
                                {
                                    MLDebugSymbol[] xSymbols;
                                    using (mSymbolsLocker.AcquireReaderLock())
                                    {
                                        xSymbols = mSymbols.ToArray();
                                    }
                                    using (_methodsLocker.AcquireReaderLock())
                                    {
                                        _methods[currentMethod].Instructions = xSymbols;
                                    }
                                }
                            }
                            else
                            {
                                if ((currentMethod.Attributes & Mono.Cecil.MethodAttributes.PInvokeImpl) != 0)
                                {
                                    OnDebugLog(LogSeverityEnum.Error,
                                               "Method '{0}' not generated!",
                                               currentMethod.ToString());
                                    new Comment("Method not being generated yet, as it's handled by a PInvoke");
                                }
                                else
                                {
                                    OnDebugLog(LogSeverityEnum.Error,
                                               "Method '{0}' not generated!",
                                               currentMethod.ToString());
                                    new Comment("Method not being generated yet, as it's handled by an iCall");
                                }
                            }
                        }
                    }
                    xOp = GetOpFromType(_map.MethodFooterOp, null, xMethodInfo);
                    xOp.Assembler = _assembler;
                    xOp.Assemble();
                    _assembler.StackContents.Clear();
                    using (_methodsLocker.AcquireReaderLock())
                    {
                        _methods[currentMethod].Processed = true;
                    }
                }
                catch (Exception e)
                {
                    OnDebugLog(LogSeverityEnum.Error, currentMethod.ToString());
                    OnDebugLog(LogSeverityEnum.Warning, e.ToString());
                    throw;
                }
            }
            OnCompilingMethods(i, xCount);
        }

        private void OnCompilingMethods(int i, int xCount)
        {
            Action<int, int> handler = this.CompilingMethods;
            if (handler != null)
                handler(i, xCount);
        }

        private IList<Assembly> GetPlugAssemblies()
        {
            var xResult = this._map.GetPlugAssemblies();
            xResult.Add(typeof(Engine).Assembly);
            return xResult;
        }

        /// <summary>
        /// Gets the full name of a method, without the defining type included
        /// </summary>
        /// <param name="aMethod"></param>
        /// <returns></returns>
        private static string GetStrippedMethodBaseFullName(MethodBase aMethod,
                                                            MethodBase aRefMethod)
        {
            StringBuilder xBuilder = new StringBuilder();
            string[] xParts = aMethod.ToString().Split(' ');
            string[] xParts2 = xParts.Skip(1).ToArray();
            MethodInfo xMethodInfo = aMethod as MethodInfo;
            if (xMethodInfo != null)
            {
                xBuilder.Append(xMethodInfo.ReturnType.FullName);
            }
            else
            {
                if (aMethod is ConstructorInfo)
                {
                    xBuilder.Append(typeof(void).FullName);
                }
                else
                {
                    xBuilder.Append(xParts[0]);
                }
            }
            xBuilder.Append("  ");
            xBuilder.Append(".");
            xBuilder.Append(aMethod.Name);
            xBuilder.Append("(");
            ParameterInfo[] xParams = aMethod.GetParameters();
            bool xParamAdded = false;
            for (int i = 0; i < xParams.Length; i++)
            {
                if (i == 0 && (aRefMethod != null && !aRefMethod.IsStatic))
                {
                    continue;
                }
                if (xParams[i].IsDefined(typeof(FieldAccessAttribute), true))
                {
                    continue;
                }
                if (xParamAdded)
                {
                    xBuilder.Append(", ");
                }
                xBuilder.Append(xParams[i].ParameterType.FullName);
                xParamAdded = true;
            }
            xBuilder.Append(")");
            return xBuilder.ToString();
        }

        private void InitializePlugs(IEnumerable<string> aPlugs)
        {
            if (_plugMethods != null)
            {
                throw new Exception("PlugMethods list already initialized!");
            }
            if (_plugFields != null)
            {
                throw new Exception("PlugFields list already initialized!");
            }

            _plugMethods = new SortedList<string, MethodBase>();
            _plugFields = new SortedList<Type, Dictionary<string, PlugFieldAttribute>>(new TypeComparer());

            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            foreach (var xAsm in AppDomain.CurrentDomain.GetAssemblies())
            {
                CheckAssemblyForPlugAssemblies(xAsm);
            }
            List<Assembly> xPlugs = new List<Assembly>();
            var xComparer = new AssemblyEqualityComparer();

            foreach (string s in aPlugs)
            {
                Assembly a = Assembly.LoadFrom(s);
                a.GetTypes();
                if (!xPlugs.Contains(a,
                                     xComparer))
                {
                    xPlugs.Add(a);
                }
            }

            foreach (var item in GetPlugAssemblies())
            {
                if (!xPlugs.Contains(item,
                                     xComparer))
                {
                    xPlugs.Add(item);
                }
            }

            foreach (Assembly xAssemblyDef in xPlugs)
            {
                LoadPlugAssembly(xAssemblyDef);
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender,
                                                       ResolveEventArgs args)
        {
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                         args.Name + ".dll")))
            {
                return Assembly.ReflectionOnlyLoadFrom(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                                                    args.Name + ".dll"));
            }
            return null;
        }

        private void CurrentDomain_AssemblyLoad(object sender,
                                                AssemblyLoadEventArgs args)
        {
            CheckAssemblyForPlugAssemblies(args.LoadedAssembly);
        }

        /// <summary>
        /// Load any plug assemblies referred to in this assembly's .config file.
        /// </summary>
        private void CheckAssemblyForPlugAssemblies(Assembly aAssembly)
        {
            //If in the GAC, then ignore assembly
            if (aAssembly.GlobalAssemblyCache)
            {
                return;
            }

            //Search for related .config file
            string configFile = aAssembly.Location + ".cosmos-config";
            if (System.IO.File.Exists(configFile))
            {
                //Load and parse all PlugAssemblies referred to in the .config file
                foreach (Assembly xAssembly in GetAssembliesFromConfigFile(configFile))
                {
                    LoadPlugAssembly(xAssembly);
                }
            }
        }

        /// <summary>
        /// Retrieves a list of plug assemblies from the given .config file.
        /// </summary>
        /// <param name="configFile"></param>
        private IEnumerable<Assembly> GetAssembliesFromConfigFile(string configFile)
        {
            //Parse XML and get all the PlugAssembly names
            XmlDocument xml = new XmlDocument();
            xml.Load(configFile);
            // do version check:
            if (xml.DocumentElement.Attributes["version"] == null || xml.DocumentElement.Attributes["version"].Value != "1")
            {
                throw new Exception(".DLL configuration version mismatch!");
            }

            string xHintPath = null;
            if (xml.DocumentElement.Attributes["hintpath"] != null)
            {
                xHintPath = xml.DocumentElement.Attributes["hintpath"].Value;
            }
            foreach (XmlNode assemblyName in xml.GetElementsByTagName("plug-assembly"))
            {
                string xName = assemblyName.InnerText;
                if (xName.EndsWith(".dll",
                                   StringComparison.InvariantCultureIgnoreCase) || xName.EndsWith(".exe",
                                                                                                  StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!String.IsNullOrEmpty(xHintPath))
                    {
                        yield return Assembly.LoadFile(Path.Combine(xHintPath,
                                                                    xName));
                        continue;
                    }
                }
                yield return Assembly.Load(assemblyName.InnerText);
            }
        }

        /// <summary>
        /// Searches assembly for methods or fields marked with custom attributes PlugMethodAttribute or PlugFieldAttribute.
        /// Matches found are inserted in SortedLists mPlugMethods and mPlugFields.
        /// </summary>
        private void LoadPlugAssembly(Assembly aAssemblyDef)
        {
            foreach (var xType in (from item in aAssemblyDef.GetTypes()
                                   let xCustomAttribs = item.GetCustomAttributes(typeof(PlugAttribute),
                                                                                 false)
                                   where xCustomAttribs != null && xCustomAttribs.Length > 0
                                   select new KeyValuePair<Type, PlugAttribute>(item,
                                                                                (PlugAttribute)xCustomAttribs[0])))
            {
                PlugAttribute xPlugAttrib = xType.Value;
                if (xPlugAttrib.IsMonoOnly && !RunningOnMono)
                {
                    continue;
                }
                if (xPlugAttrib.IsMicrosoftdotNETOnly && RunningOnMono)
                {
                    continue;
                }
                Type xTypeRef = xPlugAttrib.Target;
                if (xTypeRef == null)
                {
                    xTypeRef = Type.GetType(xPlugAttrib.TargetName,
                                            true);
                }

                PlugFieldAttribute[] xTypePlugFields = xType.Key.GetCustomAttributes(typeof(PlugFieldAttribute),
                                                                                     false) as PlugFieldAttribute[];
                if (xTypePlugFields != null && xTypePlugFields.Length > 0)
                {
                    Dictionary<string, PlugFieldAttribute> xPlugFields;
                    if (_plugFields.ContainsKey(xTypeRef))
                    {
                        xPlugFields = _plugFields[xTypeRef];
                    }
                    else
                    {
                        _plugFields.Add(xTypeRef,
                                        xPlugFields = new Dictionary<string, PlugFieldAttribute>());
                    }
                    foreach (var xPlugField in xTypePlugFields)
                    {
                        if (xPlugAttrib.IsMonoOnly && !RunningOnMono)
                        {
                            continue;
                        }
                        if (xPlugAttrib.IsMicrosoftdotNETOnly && RunningOnMono)
                        {
                            continue;
                        }
                        if (!xPlugFields.ContainsKey(xPlugField.FieldId))
                        {
                            xPlugFields.Add(xPlugField.FieldId,
                                            xPlugField);
                        }
                    }
                }

                foreach (MethodBase xMethod in xType.Key.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    PlugMethodAttribute xPlugMethodAttrib = xMethod.GetCustomAttributes(typeof(PlugMethodAttribute),
                                                                                        true).Cast<PlugMethodAttribute>().FirstOrDefault();
                    string xSignature = String.Empty;
                    if (xPlugMethodAttrib != null)
                    {
                        xSignature = xPlugMethodAttrib.Signature;
                        if (!xPlugMethodAttrib.Enabled)
                        {
                            continue;
                        }
                        if (xPlugAttrib.IsMonoOnly && !RunningOnMono)
                        {
                            continue;
                        }
                        if (xPlugAttrib.IsMicrosoftdotNETOnly && RunningOnMono)
                        {
                            continue;
                        }
                        if (!String.IsNullOrEmpty(xSignature))
                        {
                            if (!_plugMethods.ContainsKey(xSignature))
                            {
                                _plugMethods.Add(xSignature,
                                                 xMethod);
                            }
                            continue;
                        }
                    }
                    foreach (MethodBase xOrigMethodDef in xTypeRef.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        string xStrippedSignature = GetStrippedMethodBaseFullName(xMethod,
                                                                                  xOrigMethodDef);
                        string xOrigStrippedSignature = GetStrippedMethodBaseFullName(xOrigMethodDef,
                                                                                      null);
                        if (xOrigStrippedSignature == xStrippedSignature)
                        {
                            if (!_plugMethods.ContainsKey(Label.GenerateLabelName(xOrigMethodDef)))
                            {
                                _plugMethods.Add(Label.GenerateLabelName(xOrigMethodDef),
                                                 xMethod);
                            }
                        }
                    }
                    foreach (MethodBase xOrigMethodDef in xTypeRef.GetConstructors(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic))
                    {
                        string xStrippedSignature = GetStrippedMethodBaseFullName(xMethod,
                                                                                  xOrigMethodDef);
                        string xOrigStrippedSignature = GetStrippedMethodBaseFullName(xOrigMethodDef,
                                                                                      null);
                        if (xOrigStrippedSignature == xStrippedSignature)
                        {
                            if (_plugMethods.ContainsKey(Label.GenerateLabelName(xOrigMethodDef)))
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                            _plugMethods.Add(Label.GenerateLabelName(xOrigMethodDef),
                                             xMethod);
                        }
                    }
                }
            }
        }

        private MethodDefinition GetCustomMethodImplementation(string aMethodName)
        {
            if (_plugMethods.ContainsKey(aMethodName))
            {
                return _plugMethods[aMethodName];
            }
            return null;
        }

        public static TypeInformation GetTypeInfo(TypeReference typeRef)
        {
            TypeDefinition type;
            if (typeRef is TypeDefinition)
                type = typeRef;
            else
                type = TypeResolver.Resolve(typeRef);

            TypeInformation xTypeInfo;
            uint xObjectStorageSize;
            var xTypeFields = GetTypeFieldInfo(type, out xObjectStorageSize);
            xTypeInfo = new TypeInformation(xObjectStorageSize, xTypeFields, type, (!type.IsValueType) && type.IsClass);
            return xTypeInfo;
        }

        public static MethodInformation GetMethodInfo(MethodDefinition aCurrentMethodForArguments, MethodDefinition aCurrentMethodForLocals, string aMethodName, TypeInformation aTypeInfo, bool aDebugMode)
        {
            return GetMethodInfo(aCurrentMethodForArguments, aCurrentMethodForLocals, aMethodName, aTypeInfo, aDebugMode, null);
        }

        public static MethodInformation GetMethodInfo(MethodDefinition aCurrentMethodForArguments, MethodDefinition aCurrentMethodForLocals, string aMethodName, TypeInformation aTypeInfo, bool aDebugMode, IDictionary<string, object> aMethodData)
        {
            MethodInformation xMethodInfo;
            {

                MethodInformation.Variable[] xVars = new MethodInformation.Variable[0];
                int xCurOffset = 0;
                // todo:implement check for body
                //if (aCurrentMethodForLocals.HasBody) {
                var body = aCurrentMethodForLocals.Body;
                if (body != null)
                {
                    xVars = new MethodInformation.Variable[body.Variables.Count];
                    foreach (VariableDefinition varDef in body.Variables)
                    {
                        int xVarSize = (int)GetFieldStorageSize(varDef.VariableType);
                        if ((xVarSize % 4) != 0)
                        {
                            xVarSize += 4 - (xVarSize % 4);
                        }
                        xVars[varDef.LocalIndex] = new MethodInformation.Variable(xCurOffset,
                                                                                   xVarSize,
                                                                                   !varDef.LocalType.IsValueType,
                                                                                   varDef.LocalType);
                        // todo: implement support for generic parameters?
                        //if (!(xVarDef.VariableType is GenericParameter)) {
                        RegisterType(varDef.LocalType);
                        //}
                        xCurOffset += xVarSize;
                    }
                }
                MethodInformation.Argument[] xArgs;
                if (!aCurrentMethodForArguments.IsStatic)
                {
                    ParameterDefinitionCollection xParameters = aCurrentMethodForArguments.Parameters;
                    xArgs = new MethodInformation.Argument[xParameters.Count + 1];
                    xCurOffset = 0;
                    uint xArgSize;
                    for (int i = xArgs.Length - 1; i > 0; i--)
                    {
                        ParameterInfo xParamDef = xParameters[i - 1];
                        xArgSize = GetFieldStorageSize(xParamDef.ParameterType);
                        if ((xArgSize % 4) != 0)
                        {
                            xArgSize += 4 - (xArgSize % 4);
                        }
                        MethodInformation.Argument.KindEnum xKind = MethodInformation.Argument.KindEnum.In;
                        if (xParamDef.IsOut)
                        {
                            if (xParamDef.IsIn)
                            {
                                xKind = MethodInformation.Argument.KindEnum.ByRef;
                            }
                            else
                            {
                                xKind = MethodInformation.Argument.KindEnum.Out;
                            }
                        }
                        xArgs[i] = new MethodInformation.Argument(xArgSize,
                                                                  xCurOffset,
                                                                  xKind,
                                                                  !xParamDef.ParameterType.IsValueType,
                                                                  GetTypeInfo(xParamDef.ParameterType),
                                                                  xParamDef.ParameterType);
                        xCurOffset += (int)xArgSize;
                    }
                    xArgSize = 4;
                    // this
                    xArgs[0] = new MethodInformation.Argument(xArgSize,
                                                              xCurOffset,
                                                              MethodInformation.Argument.KindEnum.In,
                                                              !aCurrentMethodForArguments.DeclaringType.IsValueType,
                                                              GetTypeInfo(aCurrentMethodForArguments.DeclaringType),
                                                              aCurrentMethodForArguments.DeclaringType);
                }
                else
                {
                    ParameterInfo[] xParameters = aCurrentMethodForArguments.GetParameters();
                    xArgs = new MethodInformation.Argument[xParameters.Length];
                    xCurOffset = 0;
                    for (int i = xArgs.Length - 1; i >= 0; i--)
                    {
                        ParameterInfo xParamDef = xParameters[i]; //xArgs.Length - i - 1];
                        uint xArgSize = GetFieldStorageSize(xParamDef.ParameterType);
                        if ((xArgSize % 4) != 0)
                        {
                            xArgSize += 4 - (xArgSize % 4);
                        }
                        MethodInformation.Argument.KindEnum xKind = MethodInformation.Argument.KindEnum.In;
                        if (xParamDef.IsOut)
                        {
                            if (xParamDef.IsIn)
                            {
                                xKind = MethodInformation.Argument.KindEnum.ByRef;
                            }
                            else
                            {
                                xKind = MethodInformation.Argument.KindEnum.Out;
                            }
                        }
                        xArgs[i] = new MethodInformation.Argument(xArgSize,
                                                                  xCurOffset,
                                                                  xKind,
                                                                  !xParamDef.ParameterType.IsValueType,
                                                                  GetTypeInfo(xParamDef.ParameterType),
                                                                  xParamDef.ParameterType);
                        xCurOffset += (int)xArgSize;
                    }
                }
                int xResultSize = 0;
                //= GetFieldStorageSize(aCurrentMethodForArguments.ReturnType.ReturnType);
                MethodInfo xMethInfo = aCurrentMethodForArguments as MethodInfo;
                Type xReturnType = typeof(void);
                if (xMethInfo != null)
                {
                    xResultSize = (int)GetFieldStorageSize(xMethInfo.ReturnType);
                    xReturnType = xMethInfo.ReturnType;
                }
                xMethodInfo = new MethodInformation(aMethodName,
                                                    xVars,
                                                    xArgs,
                                                    (uint)xResultSize,
                                                    !aCurrentMethodForArguments.IsStatic,
                                                    aTypeInfo,
                                                    aCurrentMethodForArguments,
                                                    xReturnType,
                                                    aDebugMode,
                                                    aMethodData);
            }
            return xMethodInfo;
        }

        public static Dictionary<string, TypeInformation.Field> GetTypeFieldInfo(MethodBase aCurrentMethod,
                                                                                 out uint aObjectStorageSize)
        {
            Type xCurrentInspectedType = aCurrentMethod.DeclaringType;
            return GetTypeFieldInfo(xCurrentInspectedType,
                                    out aObjectStorageSize);
        }

        private static void GetTypeFieldInfoImpl(List<KeyValuePair<string, TypeInformation.Field>> aTypeFields,
                                                 Type aType,
                                                 ref uint aObjectStorageSize)
        {
            Type xActualType = aType;
            Dictionary<string, PlugFieldAttribute> xCurrentPlugFieldList = new Dictionary<string, PlugFieldAttribute>();
            do
            {
                if (_current._plugFields.ContainsKey(aType))
                {
                    var xOrigList = _current._plugFields[aType];
                    foreach (var item in xOrigList)
                    {
                        xCurrentPlugFieldList.Add(item.Key,
                                                  item.Value);
                    }
                }
                foreach (FieldInfo xField in aType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    if (xField.IsStatic)
                    {
                        continue;
                    }
                    //if (xField.HasConstant) {
                    //    Console.WriteLine("Field is constant: " + xField.GetFullName());
                    //}
                    // todo: add support for constants?
                    PlugFieldAttribute xPlugFieldAttr = null;
                    if (xCurrentPlugFieldList.ContainsKey(xField.GetFullName()))
                    {
                        xPlugFieldAttr = xCurrentPlugFieldList[xField.GetFullName()];
                        xCurrentPlugFieldList.Remove(xField.GetFullName());
                    }
                    Type xFieldType = null;
                    int xFieldSize;
                    string xFieldId;
                    if (xPlugFieldAttr != null)
                    {
                        xFieldType = xPlugFieldAttr.FieldType;
                        xFieldId = xPlugFieldAttr.FieldId;
                    }
                    else
                    {
                        xFieldId = xField.GetFullName();
                    }
                    if (xFieldType == null)
                    {
                        xFieldType = xField.FieldType;
                    }
                    //if ((!xFieldType.IsValueType && aGCObjects && xFieldType.IsClass) || (xPlugFieldAttr != null && xPlugFieldAttr.IsExternalValue && aGCObjects)) {
                    //    continue;
                    //}
                    if ((xFieldType.IsClass && !xFieldType.IsValueType) || (xPlugFieldAttr != null && xPlugFieldAttr.IsExternalValue))
                    {
                        xFieldSize = 4;
                    }
                    else
                    {
                        xFieldSize = (int)GetFieldStorageSize(xFieldType);
                    }
                    //}
                    if ((from item in aTypeFields
                         where item.Key == xFieldId
                         select item).Count() > 0)
                    {
                        continue;
                    }
                    int xOffset = (int)aObjectStorageSize;
                    FieldOffsetAttribute xOffsetAttrib = xField.GetCustomAttributes(typeof(FieldOffsetAttribute),
                                                                                    true).FirstOrDefault() as FieldOffsetAttribute;
                    if (xOffsetAttrib != null)
                    {
                        xOffset = xOffsetAttrib.Value;
                    }
                    else
                    {
                        aObjectStorageSize += (uint)xFieldSize;
                        xOffset = -1;
                    }
                    aTypeFields.Insert(0,
                                       new KeyValuePair<string, TypeInformation.Field>(xField.GetFullName(),
                                                                                       new TypeInformation.Field(xFieldSize,
                                                                                                                 xFieldType.IsClass && !xFieldType.IsValueType,
                                                                                                                 xFieldType,
                                                                                                                 (xPlugFieldAttr != null && xPlugFieldAttr.IsExternalValue))
                                                                                                                 {
                                                                                                                     Offset = xOffset
                                                                                                                 }));
                }
                while (xCurrentPlugFieldList.Count > 0)
                {
                    var xItem = xCurrentPlugFieldList.Values.First();
                    xCurrentPlugFieldList.Remove(xItem.FieldId);
                    Type xFieldType = xItem.FieldType;
                    int xFieldSize;
                    string xFieldId = xItem.FieldId;
                    if (xFieldType == null)
                    {
                        xFieldType = xItem.FieldType;
                    }
                    if (xFieldType == null)
                    {
                        Engine._current.OnDebugLog(LogSeverityEnum.Error, "Plugged field {0} not found! (On Type {1})", xItem.FieldId, aType.AssemblyQualifiedName);
                    }
                    if (xItem.IsExternalValue || (xFieldType.IsClass && !xFieldType.IsValueType))
                    {
                        xFieldSize = 4;
                    }
                    else
                    {
                        xFieldSize = (int)GetFieldStorageSize(xFieldType);
                    }
                    int xOffset = (int)aObjectStorageSize;
                    aObjectStorageSize += (uint)xFieldSize;
                    aTypeFields.Insert(0,
                                       new KeyValuePair<string, TypeInformation.Field>(xItem.FieldId,
                                                                                       new TypeInformation.Field(xFieldSize,
                                                                                                                 xFieldType.IsClass && !xFieldType.IsValueType,
                                                                                                                 xFieldType,
                                                                                                                 xItem.IsExternalValue)));
                }
                if (aType.FullName != "System.Object" && aType.BaseType != null)
                {
                    aType = aType.BaseType;
                }
                else
                {
                    break;
                }
            } while (true);
        }

        public static Dictionary<string, TypeInformation.Field> GetTypeFieldInfo(TypeDefinition aType, out uint aObjectStorageSize)
        {
            var xTypeFields = new List<KeyValuePair<string, TypeInformation.Field>>();
            aObjectStorageSize = 0;
            GetTypeFieldInfoImpl(xTypeFields,
                                 aType,
                                 ref aObjectStorageSize);
            if (aType.IsExplicitLayout)
            {
                var xStructLayout = aType.StructLayoutAttribute;
                if (xStructLayout.Size == 0)
                {
                    aObjectStorageSize = (uint)((from item in xTypeFields
                                                 let xSize = item.Value.Offset + item.Value.Size
                                                 orderby xSize descending
                                                 select xSize).FirstOrDefault());
                }
                else
                {
                    aObjectStorageSize = (uint)xStructLayout.Size;
                }
            }
            int xOffset = 0;
            Dictionary<string, TypeInformation.Field> xResult = new Dictionary<string, TypeInformation.Field>();
            foreach (var item in xTypeFields)
            {
                var xItem = item.Value;
                if (item.Value.Offset == -1)
                {
                    xItem.Offset = xOffset;
                    xOffset += xItem.Size;
                }
                xResult.Add(item.Key,
                            xItem);
            }
            return xResult;
        }

        private static Op GetOpFromType(Type aType, ILReader aReader, MethodInformation aMethodInfo)
        {
            return (Op)Activator.CreateInstance(aType, aReader, aMethodInfo);
        }

        public static void QueueStaticField(FieldDefinition aField)
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            using (_current.mStaticFieldsLocker.AcquireReaderLock())
            {
                if (_current._staticFields.ContainsKey(aField))
                {
                    return;
                }
            }
            using (_current.mStaticFieldsLocker.AcquireWriterLock())
            {
                if (!_current._staticFields.ContainsKey(aField))
                {
                    _current._staticFields.Add(aField,
                                               new QueuedStaticFieldInformation());
                }
            }
        }

        public static void QueueStaticField(string aAssembly,
                                            string aType,
                                            string aField,
                                            out string aFieldName)
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            Type xTypeDef = GetType(aAssembly,
                                    aType);
            var xFieldDef = xTypeDef.GetField(aField);
            if (xFieldDef != null)
            {
                QueueStaticField(xFieldDef);
                aFieldName = DataMember.GetStaticFieldName(xFieldDef);
                return;
            }
            throw new Exception("Field not found!(" + String.Format("{0}/{1}/{2}",
                                                                    aAssembly,
                                                                    aType,
                                                                    aField));
        }

        public static void QueueStaticField(FieldDefinition aField,
                                            out string aDataName)
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            if (!aField.IsStatic)
            {
                throw new Exception("Cannot add an instance field to the StaticField queue!");
            }
            aDataName = DataMember.GetStaticFieldName(aField);
            QueueStaticField(aField);
        }

        // MtW: 
        //		Right now, we only support one engine at a time per AppDomain. This might be changed
        //		later. See for example NHibernate does this with the ICurrentSessionContext interface
        public static void QueueMethod(MethodDefinition aMethod)
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            if (!aMethod.IsStatic)
            {
                RegisterType(aMethod.DeclaringType);
            }
            using (_current._methodsLocker.AcquireReaderLock())
            {
                if (_current._methods.ContainsKey(aMethod))
                {
                    return;
                }
            }
            using (_current._methodsLocker.AcquireWriterLock())
            {
                if (!_current._methods.ContainsKey(aMethod))
                {
                    if (_current._methods is ReadOnlyDictionary<MethodBase, QueuedMethodInformation>)
                    {
                        EmitDependencyGraphLine(false, aMethod.ToString());
                        throw new Exception("Cannot queue " + aMethod.ToString());
                    }
                    EmitDependencyGraphLine(false, aMethod.ToString());
                    _current._methods.Add(aMethod,
                                          new QueuedMethodInformation()
                                          {
                                              Processed = false,
                                              PreProcessed = false,
                                              Index = _current._methods.Count
                                          });
                }
            }
        }

        public static int GetMethodIdentifier(MethodDefinition aMethod)
        {
            QueueMethod(aMethod);
            using (_current._methodsLocker.AcquireReaderLock())
            {
                return _current._methods[aMethod].Index;
            }
        }

        [Obsolete("Please use RegisterType<T>() or RegisterType(TypeDefinition type)")]
        public static int RegisterType(Type t)
        {
            return RegisterType(TypeResolver.Resolve(t));
        }

        public static int RegisterType<T>()
        {
            return RegisterType(TypeResolver.Resolve<T>());
        }

        /// <summary>
        /// Registers_Old a type and returns the Type identifier
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
        public static int RegisterType(TypeReference typeRef)
        {
            TypeDefinition type;
            if (typeRef is TypeDefinition)
                type = typeRef;
            else
                type = TypeResolver.Resolve(typeRef);

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            if (type.IsArray || type.IsPointer)
            {
                if (type.IsArray && type.GetArrayRank() != 1)
                {
                    throw new Exception("Multidimensional arrays are not yet supported!");
                }
                if (type.IsArray)
                {
                    type = typeof(Array);
                }
                else
                {
                    type = type.GetElementType();
                }
            }
            using (_current._typesLocker.AcquireReaderLock())
            {
                var xItem = _current._types.FirstOrDefault(x => x.FullName.Equals(type.FullName));
                if (xItem != null)
                {
                    return _current._types.IndexOf(xItem);
                }
            }
            Type xFoundItem;
            using (_current._typesLocker.AcquireWriterLock())
            {
                xFoundItem = _current._types.FirstOrDefault(x => x.FullName.Equals(type.FullName));

                if (xFoundItem == null)
                {
                    _current._types.Add(type);
                    if (type.FullName != "System.Object" && type.BaseType != null)
                    {
                        Type xCurInspectedType = type.BaseType;
                        RegisterType(xCurInspectedType);
                    }
                    return RegisterType(type);
                }
                else
                {
                    return _current._types.IndexOf(xFoundItem);
                }
            }
        }

        public static Assembly GetCrawledAssembly()
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            return _current._crawledAssembly;
        }

        public static void QueueMethod2(string aAssembly,
                                        string aType,
                                        string aMethod)
        {
            MethodBase xMethodDef;
            QueueMethod2(aAssembly,
                         aType,
                         aMethod,
                         out xMethodDef);
        }

        public static void QueueMethod2(string aAssembly,
                                        string aType,
                                        string aMethod,
                                        out MethodBase aMethodDef)
        {
            Type xTypeDef = GetType(aAssembly,
                                    aType);
            // todo: find a way to specify one overload of a method
            int xCount = 0;
            aMethodDef = null;
            foreach (MethodBase xMethodDef in xTypeDef.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (xMethodDef.Name == aMethod)
                {
                    QueueMethod(xMethodDef);
                    if (aMethodDef == null)
                    {
                        aMethodDef = xMethodDef;
                    }
                    xCount++;
                }
            }
            foreach (MethodBase xMethodDef in xTypeDef.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
                if (xMethodDef.Name == aMethod)
                {
                    QueueMethod(xMethodDef);
                    xCount++;
                }
            }
            if (xCount == 0)
            {
                throw new Exception("Method '" + aType + "." + aMethod + "' not found in assembly '" + aAssembly + "'!");
            }
        }

        public event DebugLogHandler DebugLog;

        private void OnDebugLog(LogSeverityEnum aSeverity, string aMessage, params object[] args)
        {
            var handler = this.DebugLog;
            if (handler != null)
                handler(aSeverity, String.Format(aMessage, args));
        }

        private SortedList<string, Assembly> mAssemblyDefCache = new SortedList<string, Assembly>();

        public static TypeDefinition GetType(string aAssembly, string aType)
        {
            Assembly xAssemblyDef;
            if (_current.mAssemblyDefCache.ContainsKey(aAssembly))
            {
                xAssemblyDef = _current.mAssemblyDefCache[aAssembly];
            }
            else
            {
                //
                //				Assembly xAssembly = (from item in AppDomain.CurrentDomain.GetAssemblies()
                //									  where item.FullName == aAssembly || item.GetName().Name == aAssembly
                //									  select item).FirstOrDefault();
                //				if (xAssembly == null) {
                //					if (String.IsNullOrEmpty(aAssembly) || aAssembly == typeof(Engine).Assembly.GetName().Name || aAssembly == typeof(Engine).Assembly.GetName().FullName) {
                //						xAssembly = typeof(Engine).Assembly;
                //					}
                //				}
                //				if (xAssembly != null) {
                //					if (aAssembly.StartsWith("mscorlib"))
                //						throw new Exception("Shouldn't be used!");
                //					Console.WriteLine("Using AssemblyFactory for '{0}'", aAssembly);
                //					xAssemblyDef = AssemblyFactory.GetAssembly(xAssembly.Location);
                //				} else {
                //					xAssemblyDef = mCurrent.mCrawledAssembly.Resolver.Resolve(aAssembly);
                //				}
                //				mCurrent.mAssemblyDefCache.Add(aAssembly, xAssemblyDef);
                if (String.IsNullOrEmpty(aAssembly) || aAssembly == typeof(Engine).Assembly.GetName().Name || aAssembly == typeof(Engine).Assembly.GetName().FullName)
                {
                    aAssembly = typeof(Engine).Assembly.FullName;
                }
                xAssemblyDef = Assembly.Load(aAssembly);
            }
            return GetType(xAssemblyDef,
                           aType);
        }

        public static Type GetType(Assembly aAssembly,
                                   string aType)
        {
            if (_current == null)
            {
                throw new Exception("ERROR: No Current Engine found!");
            }
            string xActualTypeName = aType;
            if (xActualTypeName.Contains("<") && xActualTypeName.Contains(">"))
            {
                xActualTypeName = xActualTypeName.Substring(0,
                                                            xActualTypeName.IndexOf("<"));
            }
            Type xResult = aAssembly.GetType(aType,
                                             false);
            if (xResult != null)
            {
                RegisterType(xResult);
                return xResult;
            }
            throw new Exception("Type '" + aType + "' not found in assembly '" + aAssembly + "'!");
        }

        [Obsolete("Please use MethodDefinition GetMethodDefinition(TypeDefinition type, string aMethod, params string[] paramTypes)")]
        public static MethodDefinition GetMethodDefinition(Type type, string aMethod, params string[] paramTypes)
        {
            return GetMethodDefinition(TypeResolver.Resolve(type), aMethod, paramTypes);
        }

        public static MethodDefinition GetMethodDefinition(TypeDefinition type, string aMethod, params string[] paramTypes)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                if (method.Name != aMethod)
                {
                    continue;
                }
                var @params = method.Parameters;
                if (@params.Count != paramTypes.Length)
                {
                    continue;
                }
                bool errorFound = false;
                for (int i = 0; i < @params.Count; i++)
                {
                    if (@params[i].ParameterType.FullName != paramTypes[i])
                    {
                        errorFound = true;
                        break;
                    }
                }
                if (!errorFound)
                {
                    return method;
                }
            }
            foreach (MethodDefinition xMethod in type.Constructors)
            {
                if (xMethod.Name != aMethod)
                {
                    continue;
                }
                ParameterInfo[] xParams = xMethod.GetParameters();
                if (xParams.Length != paramTypes.Length)
                {
                    continue;
                }
                bool errorFound = false;
                for (int i = 0; i < xParams.Length; i++)
                {
                    if (xParams[i].ParameterType.FullName != paramTypes[i])
                    {
                        errorFound = true;
                        break;
                    }
                }
                if (!errorFound)
                {
                    return xMethod;
                }
            }
            throw new Exception("Method not found!");
        }
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            using (_current._methodsLocker.AcquireReaderLock())
            {
                return (from item in _current._methods.Keys
                        select item.DeclaringType.Module.Assembly).Distinct(new AssemblyEqualityComparer()).ToArray();
            }
        }

        private int mInstructionsToSkip = 0;

        public static void SetInstructionsToSkip(int aCount)
        {
            if (_current == null)
            {
                throw new Exception("No Current Engine!");
            }
            _current.mInstructionsToSkip = aCount;
        }

        #region Dependency graph code

        private static bool mEmitDependencyGraph = false;

        public static void EmitDependencyGraphLine(bool aIsContainer, string aMessage)
        {
        }

        static Engine()
        {
            RunningOnMono = Type.GetType("Mono.Runtime") != null;
        }

        #endregion

        public static readonly bool RunningOnMono;
    }
}
