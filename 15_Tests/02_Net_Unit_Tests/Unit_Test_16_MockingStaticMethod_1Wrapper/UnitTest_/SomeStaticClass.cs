namespace UnitTest_
{
    public static class SomeStaticClass
    {
        public static bool SomeStaticMethod(string input)
        {
            // Let's pretend this method hits a database or service.
            return !string.IsNullOrEmpty(input);
        }
    }
}
