namespace UnitTest_
{
    public class StaticWrapper : IStaticWrapper
    {
        public bool SomeStaticMethod(string input)
        {
            return SomeStaticClass.SomeStaticMethod(input);
        }
    }
}
