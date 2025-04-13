using OpenXmlPowerTools;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using chabcav.application.Services;

public class DocumentService : IDocumentService
{
    public string ExtractTextFromDocx(byte[] fileData)
    {
        using var memoryStream = new MemoryStream(fileData);
        using var wordDoc = WordprocessingDocument.Open(memoryStream, false);

        var body = wordDoc.MainDocumentPart?.Document?.Body;
        if (body == null) return string.Empty;

        var allText = new StringBuilder();

        // Extract text from paragraphs
        foreach (var paragraph in body.Descendants<Paragraph>())
        {
            allText.AppendLine(paragraph.InnerText);
        }

        // Extract text inside tables
        foreach (var table in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>())  // Fully qualified Table
        {
            foreach (var row in table.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableRow>())
            {
                foreach (var cell in row.Descendants<DocumentFormat.OpenXml.Wordprocessing.TableCell>())
                {
                    allText.AppendLine(cell.InnerText);
                }
            }
        }

        return allText.ToString();
    }

    public string ExtractHtmlFromDocx(byte[] fileBytes)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Write(fileBytes, 0, fileBytes.Length);
            memoryStream.Position = 0;

            using var wordDoc = WordprocessingDocument.Open(memoryStream, true); // true = writable

            var settings = new HtmlConverterSettings
            {
                PageTitle = "Extracted HTML"
            };

            var html = HtmlConverter.ConvertToHtml(wordDoc, settings);
            var htmlString = html.ToStringNewLineOnAttributes();

            // Clean up excessive spacing (basic version)
            htmlString = System.Text.RegularExpressions.Regex.Replace(htmlString, @"<p>\s*</p>", ""); // Remove empty <p> tags
            htmlString = System.Text.RegularExpressions.Regex.Replace(htmlString, @"(\r\n|\n|\r)", ""); // Remove line breaks
            htmlString = System.Text.RegularExpressions.Regex.Replace(htmlString, @"\s{2,}", " ");       // Remove multiple spaces

            return htmlString.Trim(); // Final trim
        }
        catch (Exception ex)
        {
            Console.WriteLine("HTML conversion failed: " + ex.Message);
            return "<html><body>Error extracting HTML</body></html>";
        }
    }
}
