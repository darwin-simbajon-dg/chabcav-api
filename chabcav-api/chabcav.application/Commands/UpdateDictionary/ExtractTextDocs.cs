using chabcav.application.Services;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

public class DocumentService : IDocumentService
{
    public string ExtractTextFromDocx(byte[] fileData)
    {
        using var memoryStream = new MemoryStream(fileData);
        using var wordDoc = WordprocessingDocument.Open(memoryStream, false);

        var body = wordDoc.MainDocumentPart?.Document?.Body;
        if (body == null) return string.Empty;

        var allText = new StringBuilder();

        foreach (var paragraph in body.Descendants<Paragraph>())
        {
            allText.AppendLine(paragraph.InnerText);
        }

        // Extract text inside tables
        foreach (var table in body.Descendants<Table>())
        {
            foreach (var row in table.Descendants<TableRow>())
            {
                foreach (var cell in row.Descendants<TableCell>())
                {
                    allText.AppendLine(cell.InnerText);
                }
            }
        }

        return allText.ToString();
    }

}
