namespace Compiler
{
    public class SectionLabel
    {
        public string Name { get; private set; }

        public SectionLabel(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}


