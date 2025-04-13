using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.domain.Entities
{
    public class UploadDictionaryFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string ExtractedText { get; set; } = string.Empty; // ✅ Add this line
        public DateTime UploadedAt { get; set; }
        public string ExtractedHtml { get; set; } = string.Empty;// 🆕 HTML property
    }

}
