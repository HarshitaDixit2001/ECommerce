using Tesseract;
namespace WebCode.DataService
{
    public class OcrService
    {
        public string ExtractText(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine("File not found");
                    return "";
                }

                // 👉 Tesseract path
                var tessPath = @"C:\Program Files\Tesseract-OCR\tessdata";

                using var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default);
                using var img = Pix.LoadFromFile(filePath);
                using var page = engine.Process(img);

                var text = page.GetText();

                Console.WriteLine("OCR TEXT: " + text);

                return text;
            }
            catch (Exception ex)
            {
                Console.WriteLine("OCR ERROR: " + ex.Message);
                return "";
            }
        } 
    } 
}
