namespace Compiler
{
    public interface ICompilerStage
    {
        string Name { get; }
        /// <summary>
        /// Runs the compiler stage, possibly changing the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns>the original context or a new one if needed</returns>
        ICompilerContext Run(ICompilerContext context);
    }
}
