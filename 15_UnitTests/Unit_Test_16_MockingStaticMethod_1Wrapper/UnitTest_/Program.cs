namespace UnitTest_
{
    //http://adventuresdotnet.blogspot.com/2011/03/mocking-static-methods-for-unit-testing.html
    public class Program
    {
        IStaticWrapper _wrapper;
        public Program(IStaticWrapper wrapper)
        {
            _wrapper = wrapper;
        }
        static void Main(string[] args)
        {
        }

        public int SomeMethod(string input)
        {
            var value = _wrapper.SomeStaticMethod(input);

            return value ? 1 : 0;
        }
    }
}
