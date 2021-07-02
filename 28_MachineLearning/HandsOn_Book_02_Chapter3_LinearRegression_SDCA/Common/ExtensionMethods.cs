using System;
using System.Linq;

namespace chapter03.Common
{
    public static class ExtensionMethods
    {
        //extension method to return all of the properties in a class except the label
        public static string[] ToPropertyList<T>(this Type objType, string labelName) => objType.GetProperties().Where(a => a.Name != labelName).Select(a => a.Name).ToArray();
    }
}