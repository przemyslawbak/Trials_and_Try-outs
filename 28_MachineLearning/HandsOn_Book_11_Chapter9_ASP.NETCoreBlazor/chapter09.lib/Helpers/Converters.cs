using chapter09.lib.Data;
using chapter09.lib.ML.Objects;

namespace chapter09.lib.Helpers
{
    //The Converters class provides an extension method to convert the
    //FileClassificationResponseItem class—reviewed earlier in this section—to the
    //FileData class
    public static class Converters
    {
        public static FileData ToFileData(this FileClassificationResponseItem fileClassification)
        {
            return new FileData
            {
                Is64Bit = fileClassification.Is64Bit,
                IsSigned = fileClassification.IsSigned,
                NumberImports = fileClassification.NumImports,
                NumberImportFunctions = fileClassification.NumImportFunctions,
                NumberExportFunctions = fileClassification.NumExportFunctions,
                FileSize = fileClassification.FileSize,
                Strings = fileClassification.Strings
            };
        }
    }
}