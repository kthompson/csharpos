namespace Compiler
{
    public interface IEmitter : Mono.Cecil.Cil.ICodeVisitor
    {
        Section Section(SectionType type);
    }
}


