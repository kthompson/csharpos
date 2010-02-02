using System;
using System.Collections.Generic;
using System.IO;

namespace Compiler
{
    public class Section
    {
        private StringWriter _out;
        public SectionType SectionType { get; private set; }

        public Section(SectionType type)
        {
            this._out = new StringWriter();
            this.SectionType = type;

            this.Emit(SectionTypeToLabel(type));
            if(type == SectionType.ReadOnlyData)
                this.Emit("\t.align 4");
            if (type == SectionType.Text)
                this.Emit("\t.p2align 4,,15");

        }

        private readonly List<string> _labels = new List<string>();
        public SectionLabel Label(Action<Section> action)
        {
            return Label(string.Format("LC{0}", _labels.Count), action);
        }

        protected SectionLabel Label(string name, Action<Section> action)
        {
            Helper.IsNotNull(name, "name");
            Helper.IsNotNull(action, "action");

            if (_labels.Contains(name))
                throw new DuplicateLabelException(name);

            this.Emit("{0}:", name);
            action(this);
            return new SectionLabel(name);
        }

        #region emit methods
        public void Emit(string format, params object[] args)
        {
            _out.WriteLine(format, args);
        }

        public void EmitLong(long data)
        {
            this.Emit("\t.long {0}", data);
        }


      
        #endregion

        private static string SectionTypeToLabel(SectionType type)
        {
            switch(type)
            {
                case SectionType.Text:
                    return "\t.text";
                case SectionType.ReadOnlyData:
                    return "\t.section .rdata,\"dr\"";
                case SectionType.Data:
                case SectionType.Debug:
                case SectionType.Imports:
                case SectionType.Relocation:
                case SectionType.Resource:
                case SectionType.ThreadLocalStorage:
                    throw new NotImplementedException(string.Format("SectionType {0} is not yet implemented", type));
                default:
                    throw new NotImplementedException(string.Format("SectionType {0} is not yet implemented", type));
                
            }
        }

        public void Flush(TextWriter writer)
        {
            writer.Write(this._out.ToString());
        }
    }
}


