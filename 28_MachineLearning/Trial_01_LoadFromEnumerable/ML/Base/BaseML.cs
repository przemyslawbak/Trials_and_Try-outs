using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using chapter03_logistic_regression.Common;

using Microsoft.ML;

namespace chapter03_logistic_regression.ML.Base
{
    public class BaseML
    {
        protected static string ModelPath => Path.Combine(AppContext.BaseDirectory, Constants.MODEL_FILENAME);

        protected readonly MLContext MlContext;

        private static Regex _stringRex;

        protected BaseML()
        {
            MlContext = new MLContext(2020);

            //Encoding.RegisterProvider is critical to utilize the Windows-1252
            //encoding.This encoding is the encoding Windows Executables utilize
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _stringRex = new Regex(@"[ -~\t]{8,}", RegexOptions.Compiled);
        }

        //1. To begin, we define the method definition and initialize the stringLines
        //variable to hold the strings
        protected string GetStrings(byte[] data)
        {
            StringBuilder stringLines = new StringBuilder();

            //2. Next, we will sanity check the input data is not null or empty
            if (data == null || data.Length == 0)
            {
                return stringLines.ToString();
            }

            //3. The next block of code we open a MemoryStream object and then a
            //StreamReader object
            using (MemoryStream ms = new MemoryStream(data, false))
            {
                using (StreamReader streamReader = new StreamReader(ms, Encoding.GetEncoding(1252), false, 2048, false))
                {
                    //4. We will then loop through the streamReader object until an EndOfStream
                    //condition is reached, reading line by line
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();

                        //5. We then will apply some string clean up of the data and handle whether the line
                        //is empty or not gracefully
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        line = line.Replace("^", "").Replace(")", "").Replace("-", "");

                        //6. Then, we will append the regular expression matches and append those matches
                        //to the previously defined stringLines variable
                        stringLines.Append(string.Join(string.Empty,
                            _stringRex.Matches(line).Where(a => !string.IsNullOrEmpty(a.Value) && !string.IsNullOrWhiteSpace(a.Value)).ToList()));
                    }
                }
            }

            //7. Lastly, we will return the stringLines variable converted into a single string
            //using the string.Join method:
            return string.Join(string.Empty, stringLines);
        }
    }
}