using System;
using System.Linq;

namespace chapter08.Helpers
{
    //provides a program-agnostic parser for handling command-line arguments
    public static class CommandLineParser
    {
        //1. First, we define the function prototype
        public static T ParseArguments<T>(string[] args)
        {
            //2. Next, we test for null arguments
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            //3. Then, we test for empty arguments and let the user know default values are
            //going to be used instead of failing, as in previous chapters
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments passed in - using defaults");

                return Activator.CreateInstance<T>();
            }

            //4. After null and empty checks are performed, we then perform a multiple of two
            //checks since all arguments are pairs
            if (args.Length % 2 != 0)
            {
                throw new ArgumentException($"Arguments must be in pairs, there were {args.Length} passed in");
            }

            //5. Continuing, we then create an object of the T type using the Activator.CreateInstance method
            T argumentObject = Activator.CreateInstance<T>();

            //6. Next, we utilize reflection to grab all of the properties of the T type
            System.Reflection.PropertyInfo[] properties = argumentObject.GetType().GetProperties();


            //7. Now that we have both the generic object created and the properties of that
            //object, we then loop through each of the argument key/ value pairs and set the
            //property in the object
            for (var x = 0; x < args.Length; x += 2)
            {
                System.Reflection.PropertyInfo property = properties.FirstOrDefault(a => a.Name.Equals(args[x], StringComparison.CurrentCultureIgnoreCase));

                if (property == null)
                {
                    Console.WriteLine($"{args[x]} is an invalid argument");

                    continue;
                }

                if (property.PropertyType.IsEnum)
                {
                    property.SetValue(argumentObject, Enum.Parse(property.PropertyType, args[x + 1], true));
                }
                else
                {
                    property.SetValue(argumentObject, args[x + 1]);
                }
            }

            return argumentObject;
        }
    }
}