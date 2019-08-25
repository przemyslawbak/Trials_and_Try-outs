using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ParamValue_
{
    //https://stackoverflow.com/q/75076/11972985
    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.Go(1);

            Console.ReadKey();
        }
    }

    public class A
    {
        internal void Go(int parameterGoA)
        {
            B b = new B();
            b.Go(4);
        }
    }

    public class B
    {
        internal void Go(int parameterGoB)
        {
            Console.WriteLine(GetStackTrace());

        }
        public static string GetStackTrace()
        {
            StringBuilder sb = new StringBuilder();
            StackTrace st = new StackTrace(true);
            StackFrame[] frames = st.GetFrames();

            foreach (StackFrame frame in frames)
            {
                MethodBase method = frame.GetMethod();

                sb.AppendFormat("{0} - {1}", method.DeclaringType, method.Name);
                ParameterInfo[] paramaters = method.GetParameters();
                foreach (ParameterInfo paramater in paramaters)
                {
                    sb.AppendFormat("{0}: {1}", paramater.Name, paramater.ToString());
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
