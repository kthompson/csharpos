namespace Compiler
{
    public static class MathExtensions
    {
        public static unsafe int ToIEEE754(this float value)
        {
            return *(((int*)&value));
        }
    }
}


