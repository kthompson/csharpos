using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using Mono.Cecil;

namespace Indy.IL2CPU.Assembler
{
    public class Label : Instruction
    {
        public static string GetFullName(MethodReference method)
        {
            return GetFullName(method.Resolve());
        }

        public static string GetFullName(MethodDefinition method)
        {
            var builder = new StringBuilder();
            string[] parts = method.ToString().Split(' ');
            string[] parts2 = parts.Skip(1).ToArray();
            if (method != null)
            {
                builder.Append(method.ReturnType.ReturnType.FullName);
            }
            else
            {
                if (method.IsConstructor)
                {
                    builder.Append(TypeResolver.Void.FullName);
                }
                else
                {
                    builder.Append(parts[0]);
                }
            }
            builder.Append("  ");
            builder.Append(method.DeclaringType.FullName);
            builder.Append(".");
            builder.Append(method.Name);
            builder.Append("(");
            ParameterDefinitionCollection @params = method.Parameters;
            for (int i = 0; i < @params.Count; i++)
            {
                if (@params[i].Name == "aThis" && i == 0)
                {
                    continue;
                }
                builder.Append(@params[i].ParameterType.FullName);
                if (i < (@params.Count - 1))
                {
                    builder.Append(", ");
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        public static string FilterStringForIncorrectChars(string aName)
        {
            return DataMember.FilterStringForIncorrectChars(aName.Replace(".", "__DOT__"));
        }

        private string mName;
        public string Name
        {
            get { return mName; }
        }

        public static string GetLabel(object aObject)
        {
            Label xLabel = aObject as Label;
            if (xLabel == null)
                return "";
            return xLabel.Name;
        }

        public static string LastFullLabel
        {
            get;
            set;
        }

        public Label(string aName)
        {
            mName = aName;
            if (!aName.StartsWith("."))
            {
                LastFullLabel = aName;
                QualifiedName = aName;
            }
            else
            {
                QualifiedName = LastFullLabel + aName;
            }
        }

        public string QualifiedName
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Name + ":";
        }

        public static string GenerateLabelName(MethodReference method)
        {
            string xResult = DataMember.FilterStringForIncorrectChars(GetFullName(method));
            if (xResult.Length > 245)
            {
                using (var xHash = MD5.Create())
                {
                    xResult = xHash.ComputeHash(Encoding.Default.GetBytes(xResult)).Aggregate("_",
                                                                                              (r,
                                                                                               x) => r + x.ToString("X2"));
                }
            }
            return xResult;
        }

        public Label(MethodDefinition method)
            : this(GenerateLabelName(method))
        {
        }

        public override bool IsComplete(Assembler aAssembler)
        {
            return true;
        }

        public override byte[] GetData(Assembler aAssembler)
        {
            return new byte[0];
        }
    }
}
