namespace Compiler.Framework
{
    public interface IAssemblyCompilerStage
    {
        string Name { get; }

        /// <summary>
        /// Runs the compiler stage, possibly changing the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns>the original context or a new one if needed</returns>
        IAssemblyCompilerContext Run(IAssemblyCompiler compiler, IAssemblyCompilerContext context);
    }
}