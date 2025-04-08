using Microsoft.AspNetCore.Http;

public static class FileHelpers
{
    public static async Task<byte[]> ConvertToByteArray(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
