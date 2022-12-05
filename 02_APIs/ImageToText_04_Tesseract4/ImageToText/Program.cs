using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string image = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "jcaptcha.jpg");
            string updated = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "jcaptchaXXX.jpg");

            string dirPath = @"C:\Program Files (x86)\Tesseract-OCR";

            bool dir = Directory.Exists(dirPath);
            TesseractService service = new TesseractService(@"C:\Program Files (x86)\Tesseract-OCR", "eng", @"C:\Program Files (x86)\Tesseract-OCR\tessdata");

            ClearImage(image, updated);

            FileStream stream = File.OpenRead(updated);
            var text = service.GetText(stream);
        }

        private static void ClearImage(string image, string updated)
        {
            Bitmap myBitmap = new Bitmap(image);
            const float limit = 0.3f;
            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    Color c = myBitmap.GetPixel(i, j);
                    if (c.GetBrightness() > limit)
                    {
                        myBitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            myBitmap.Save(updated);
        }
    }

    public class TesseractService
    {
        private readonly string _tesseractExePath;
        private readonly string _language;

        /// <summary>
        /// Initializes a new instance of the <see cref="TesseractService"/> class.
        /// </summary>
        /// <param name="tesseractDir">The path for the Tesseract4 installation folder (C:\Program Files\Tesseract-OCR).</param>
        /// <param name="language">The language used to extract text from images (eng, por, etc)</param>
        /// <param name="dataDir">The data with the trained models (tessdata). Download the models from https://github.com/tesseract-ocr/tessdata_fast</param>
        public TesseractService(string tesseractDir, string language = "en", string dataDir = null)
        {
            // Tesseract configs.
            _tesseractExePath = Path.Combine(tesseractDir, "tesseract.exe");
            _language = language;

            if (String.IsNullOrEmpty(dataDir))
                dataDir = Path.Combine(tesseractDir, "tessdata");

            Environment.SetEnvironmentVariable("TESSDATA_PREFIX", dataDir);
        }

        /// <summary>
        /// Read text from the images streams.
        /// </summary>
        /// <param name="images">The images streams.</param>
        /// <returns>The images text.</returns>
        public string GetText(params Stream[] images)
        {
            var output = string.Empty;

            if (images.Any())
            {
                var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempPath);
                var tempInputFile = NewTempFileName(tempPath);
                var tempOutputFile = NewTempFileName(tempPath);

                try
                {
                    WriteInputFiles(images, tempPath, tempInputFile);

                    var info = new ProcessStartInfo
                    {
                        FileName = _tesseractExePath,
                        Arguments = $"{tempInputFile} {tempOutputFile} -l {_language}",
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    using (var ps = Process.Start(info))
                    {
                        ps.WaitForExit();

                        var exitCode = ps.ExitCode;

                        if (exitCode == 0)
                        {
                            output = File.ReadAllText(tempOutputFile + ".txt");
                        }
                        else
                        {
                            var stderr = ps.StandardError.ReadToEnd();
                            throw new InvalidOperationException(stderr);
                        }
                    }
                }
                finally
                {
                    Directory.Delete(tempPath, true);
                }
            }

            return output;
        }



        private static void WriteInputFiles(Stream[] inputStreams, string tempPath, string tempInputFile)
        {
            // If there is more thant one image file, so build the list file using the images as input files.
            if (inputStreams.Length > 1)
            {
                var imagesListFileContent = new StringBuilder();

                foreach (var inputStream in inputStreams)
                {
                    var imageFile = NewTempFileName(tempPath);

                    using (var tempStream = File.OpenWrite(imageFile))
                    {
                        CopyStream(inputStream, tempStream);
                    }

                    imagesListFileContent.AppendLine(imageFile);
                }

                File.WriteAllText(tempInputFile, imagesListFileContent.ToString());
            }
            else
            {
                // If is only one image file, than use the image file as input file.
                using (var tempStream = File.OpenWrite(tempInputFile))
                {
                    CopyStream(inputStreams.First(), tempStream);
                }
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            if (input.CanSeek)
                input.Seek(0, SeekOrigin.Begin);

            input.CopyTo(output);
            input.Close();
        }

        private static string NewTempFileName(string tempPath)
        {
            return Path.Combine(tempPath, Guid.NewGuid().ToString());
        }
    }
}
