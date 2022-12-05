using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace chapter09.lib.Helpers
{
    public static class ExtensionMethods
    {
        //1. First, we define two new constants for handling the buffer size and the encoding
        private const int BUFFER_SIZE = 2048;
        private const int FILE_ENCODING = 1252;

        public static string[] ToPropertyList<T>(this Type objType, string labelName) => 
            objType.GetProperties().Where(a => a.Name != labelName).Select(a => a.Name).ToArray();

        //2. The next change is the addition of the ToStringsExtraction method itself and
        //defining our regular expression, as follows
        public static string ToStringsExtraction(this byte[] data)
        {
            Regex stringRex = new Regex(@"[ -~\t]{8,}", RegexOptions.Compiled);

            //3. Next, we initialize the StringBuilder class and check if the passed-in byte
            //array is null or empty(if it is, we can't process it), like this
            StringBuilder stringLines = new StringBuilder();

            if (data == null || data.Length == 0)
            {
                return stringLines.ToString();
            }

            //4. Now that we have confirmed there are bytes in the passed-in array, we only want
            //to take up to 65536 bytes
            byte[] dataToProcess = data.Length > 65536 ? data.Take(65536).ToArray() : data;

            //5. Now that we have the bytes we are going to analyze, we will loop through and
            //extract lines of text found in the bytes, as follows
            using (var ms = new MemoryStream(dataToProcess, false))
            {
                using (var streamReader = new StreamReader(ms, Encoding.GetEncoding(FILE_ENCODING), false, BUFFER_SIZE, false))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();

                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        line = line.Replace("^", "").Replace(")", "").Replace("-", "");

                        stringLines.Append(string.Join(string.Empty,
                            stringRex.Matches(line).Where(a => !string.IsNullOrEmpty(a.Value) && !string.IsNullOrWhiteSpace(a.Value)).ToList()));
                    }
                }
            }

            //6. Finally, we simply return the lines joined into a single string, like this
            return string.Join(string.Empty, stringLines);
        }
    }
}