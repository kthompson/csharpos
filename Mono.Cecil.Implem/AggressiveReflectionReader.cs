/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;

    using Mono.Cecil;
    using Mono.Cecil.Metadata;
    using Mono.Cecil.Signatures;

    internal sealed class AggressiveReflectionReader : ReflectionReader {

        public AggressiveReflectionReader (ModuleDefinition module) : base (module)
        {
        }

        public override void Visit (ITypeDefinitionCollection types)
        {
            if (!((TypeDefinitionCollection)types).Loaded) {
                base.Visit (types);
                ReadClassLayoutInfos ();
                ReadFieldLayoutInfos ();
                ReadPInvokeInfos ();
                ReadProperties ();
                ReadEvents ();
                ReadSemantics ();
                ReadInterfaces ();
                ReadOverrides ();
            }
        }

        private void ReadClassLayoutInfos ()
        {
            ClassLayoutTable clTable = MetadataRoot.Streams.TablesHeap [typeof (ClassLayoutTable)] as ClassLayoutTable;
            for (int i = 0; i < clTable.Rows.Count; i++) {
                ClassLayoutRow clRow = clTable [i];
                TypeDefinition type = GetTypeDefAt ((int) clRow.Parent);
                type.PackingSize = clRow.PackingSize;
                type.ClassSize = clRow.ClassSize;
            }
        }

        private void ReadFieldLayoutInfos ()
        {
            FieldLayoutTable flTable = MetadataRoot.Streams.TablesHeap [typeof (FieldLayoutTable)] as FieldLayoutTable;
            for (int i = 0; i < flTable.Rows.Count; i++) {
                FieldLayoutRow flRow = flTable [i];
                FieldDefinition field = GetFieldDefAt ((int) flRow.Field);
                field.Offset = flRow.Offset;
            }
        }

        private void ReadPInvokeInfos ()
        {
            ImplMapTable imTable = MetadataRoot.Streams.TablesHeap [typeof (ImplMapTable)] as ImplMapTable;
            for (int i = 0; i < imTable.Rows.Count; i++) {
                ImplMapRow imRow = imTable [i];
                if (imRow.MemberForwarded.TokenType == TokenType.Method) { // should always be true
                    MethodDefinition meth = GetMethodDefAt ((int) imRow.MemberForwarded.RID - 1);
                    meth.PInvokeInfo = new PInvokeInfo (imRow.MappingFlags, MetadataRoot.Streams.StringsHeap [imRow.ImportName],
                                                        Module.ModuleReferences [(int) imRow.ImportScope - 1]);
                }
            }
        }

        private void ReadProperties ()
        {
            PropertyTable propsTable = m_root.Streams.TablesHeap [typeof (PropertyTable)] as PropertyTable;
            PropertyMapTable pmapTable = m_root.Streams.TablesHeap [typeof (PropertyMapTable)] as PropertyMapTable;
            m_properties = new PropertyDefinition [propsTable.Rows.Count];
            for (int i = 0; i < pmapTable.Rows.Count; i++) {
                PropertyMapRow pmapRow = pmapTable [i];
                TypeDefinition owner = GetTypeDefAt ((int) pmapRow.Parent);
                int start = (int) pmapRow.PropertyList, end;
                if (i < pmapTable.Rows.Count - 1)
                    end = (int) pmapTable [i + 1].PropertyList;
                else
                    end = propsTable.Rows.Count;

                for (int j = start; j <= end; j++) {
                    PropertyRow prow = propsTable [j - 1];
                    PropertySig psig = m_sigReader.GetPropSig (prow.Type);
                    PropertyDefinition pdef = new PropertyDefinition (MetadataRoot.Streams.StringsHeap [prow.Name],
                                                                      owner, this.GetTypeRefFromSig (psig.Type), prow.Flags);
                    owner.Properties [pdef.Name] = pdef;
                    ((PropertyDefinitionCollection)owner.Properties).Loaded = true;
                    m_properties [j - 1] = pdef;
                }
            }
        }

        private void ReadEvents ()
        {
            EventTable evtTable = m_root.Streams.TablesHeap [typeof (EventTable)] as EventTable;
            EventMapTable emapTable = m_root.Streams.TablesHeap [typeof (EventMapTable)] as EventMapTable;
            m_events = new EventDefinition [evtTable.Rows.Count];
            for (int i = 0; i < emapTable.Rows.Count; i++) {
                EventMapRow emapRow = emapTable [i];
                TypeDefinition owner = GetTypeDefAt ((int) emapRow.Parent);
                int start = (int) emapRow.EventList, end;
                if (i < (emapTable.Rows.Count - 1))
                    end = (int) emapTable [i + 1].EventList;
                else
                    end = evtTable.Rows.Count + 1;

                for (int j = start; j < end; j++) {
                    EventRow erow = evtTable [j - 1];
                    EventDefinition edef = new EventDefinition (m_root.Streams.StringsHeap [erow.Name], owner,
                                                                GetTypeDefOrRef (erow.EventType), erow.EventFlags);
                    owner.Events [edef.Name] = edef;
                    ((EventDefinitionCollection)owner.Events).Loaded = true;
                    m_events [j - 1] = edef;
                }
            }
        }

        private void ReadSemantics ()
        {
            MethodSemanticsTable semTable = m_root.Streams.TablesHeap [typeof (MethodSemanticsTable)] as MethodSemanticsTable;
            for (int i = 0; i < semTable.Rows.Count; i++) {
                MethodSemanticsRow semRow = semTable [i];
                MethodDefinition semMeth = GetMethodDefAt ((int) semRow.Method);
                semMeth.SemanticsAttributes = semRow.Semantics;
                switch (semRow.Association.TokenType) {
                case TokenType.Event :
                    EventDefinition evt = m_events [semRow.Association.RID - 1];
                    if (evt == null) {
                        Console.WriteLine ("warning: event not found {0}", semRow.Association.RID - 1);
                        continue;
                    }
                    if ((semRow.Semantics & MethodSemanticsAttributes.AddOn) != 0)
                        evt.AddMethod = semMeth;
                    else if ((semRow.Semantics & MethodSemanticsAttributes.Fire) != 0)
                        evt.InvokeMethod = semMeth;
                    else if ((semRow.Semantics & MethodSemanticsAttributes.RemoveOn) != 0)
                        evt.RemoveMethod = semMeth;
                    evt.Readed = true;
                    break;
                case TokenType.Property :
                    PropertyDefinition prop = m_properties [semRow.Association.RID - 1];
                    if (prop == null) {
                        Console.WriteLine ("warning: property not found {0}", semRow.Association.RID - 1);
                        continue;
                    }
                    if ((semRow.Semantics & MethodSemanticsAttributes.Getter) != 0)
                        prop.GetMethod = semMeth;
                    else if ((semRow.Semantics & MethodSemanticsAttributes.Setter) != 0)
                        prop.SetMethod = semMeth;
                    prop.Readed = true;
                    break;
                }
            }
        }

        private void ReadInterfaces ()
        {
            InterfaceImplTable intfsTable = m_root.Streams.TablesHeap [typeof (InterfaceImplTable)] as InterfaceImplTable;
            for (int i = 0; i < intfsTable.Rows.Count; i++) {
                InterfaceImplRow intfsRow = intfsTable [i];
                TypeDefinition owner = GetTypeDefAt ((int)intfsRow.Class);
                owner.Interfaces.Add (GetTypeDefOrRef (intfsRow.Interface));
                ((InterfaceCollection)owner.Interfaces).Loaded = true;
            }
        }

        private void ReadOverrides ()
        {
            MethodImplTable implTable = MetadataRoot.Streams.TablesHeap [typeof (MethodImplTable)] as MethodImplTable;
            for (int i = 0; i < implTable.Rows.Count; i++) {
                MethodImplRow implRow = implTable [i];
                if (implRow.MethodBody.TokenType == TokenType.Method) {
                    MethodDefinition owner = GetMethodDefAt ((int) implRow.MethodBody.RID);
                    switch (implRow.MethodDeclaration.TokenType) {
                    case TokenType.Method :
                        owner.Overrides.Add (GetMethodDefAt ((int) implRow.MethodDeclaration.RID));
                        break;
                    //TODO: handle memberref
                    }
                    ((OverrideCollection)owner.Overrides).Loaded = true;
                }
            }
        }
    }
}
