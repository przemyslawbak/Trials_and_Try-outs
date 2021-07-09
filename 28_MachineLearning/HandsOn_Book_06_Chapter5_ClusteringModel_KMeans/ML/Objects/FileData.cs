using System;
using System.Linq;

using chapter05.Enums;

using Microsoft.ML.Data;

namespace chapter05.ML.Objects
{
    public class FileData
    {
        //1. First, we add constant values for True and False since k-means requires
        //floating-point values
        private const float TRUE = 1.0f;
        private const float FALSE = 0.0f;

        //2. Next, we create a constructor that supports both our prediction and training.
        //We also call helper methods to determine whether the file is binary, or not.
        public FileData(Span<byte> data, string fileName = null)
        {
            // Used for training purposes only
            if (!string.IsNullOrEmpty(fileName))
            {
                if (fileName.Contains("ps1"))
                {
                    Label = (float) FileTypes.Script;
                } else if (fileName.Contains("exe"))
                {
                    Label = (float) FileTypes.Executable;
                } else if (fileName.Contains("doc"))
                {
                    Label = (float) FileTypes.Document;
                }
            }

            IsBinary = HasBinaryContent(data) ? TRUE : FALSE;

            IsMZHeader = HasHeaderBytes(data.Slice(0, 2), "MZ") ? TRUE : FALSE;

            IsPKHeader = HasHeaderBytes(data.Slice(0, 2), "PK") ? TRUE : FALSE;
        }

        /// <summary>
        /// 3. Next, we also add an additional constructor to support the hard truth setting of
        /// values.
        /// Used for mapping cluster ids to results only
        /// </summary>
        /// <param name="fileType"></param>
        public FileData(FileTypes fileType)
        {
            Label = (float)fileType;

            switch (fileType)
            {
                case FileTypes.Document:
                    IsBinary = TRUE;
                    IsMZHeader = FALSE;
                    IsPKHeader = TRUE;
                    break;
                case FileTypes.Executable:
                    IsBinary = TRUE;
                    IsMZHeader = TRUE;
                    IsPKHeader = FALSE;
                    break;
                case FileTypes.Script:
                    IsBinary = FALSE;
                    IsMZHeader = FALSE;
                    IsPKHeader = FALSE;
                    break;
            }
        }

        //4. Next, we implement our two helper methods. The first, HasBinaryContent, as
        //the name implies, takes the raw binary data and searches for non-text characters
        //to ensure it is a binary file.Secondly, we define HasHeaderBytes; this method
        //takes an array of bytes, converts it into a UTF8 string, and then checks to see
        //whether the string matches the string passed in
        private static bool HasBinaryContent(Span<byte> fileContent) =>
            System.Text.Encoding.UTF8.GetString(fileContent.ToArray()).Any(a => char.IsControl(a) && a != '\r' && a != '\n');
        private static bool HasHeaderBytes(Span<byte> data, string match) => System.Text.Encoding.UTF8.GetString(data) == match;

        //5. Next, we add the properties used for prediction, training, and testing
        [ColumnName("Label")]
        public float Label { get; set; }

        public float IsBinary { get; set; }

        public float IsMZHeader { get; set; }

        public float IsPKHeader { get; set; }

        //6. Lastly, we override the ToString method to be used with the feature extraction
        public override string ToString() => $"{Label},{IsBinary},{IsMZHeader},{IsPKHeader}";
    }
}