using chabcav.application.Commands.AddContent.UploadFileDictionary;
using chabcav.application.Interfaces;
using chabcav.application.Services;
using chabcav.domain.Entities;
using MediatR;

public class UploadDictionaryFileCommandHandler : IRequestHandler<UploadDictionaryFileCommand, bool>
{
    private readonly IContentRepository _repository;
    private readonly IDocumentService _documentService;  // <-- IDocumentService here

    public UploadDictionaryFileCommandHandler(IContentRepository repository, IDocumentService documentService)
    {
        _repository = repository;
        _documentService = documentService;  // <-- Injecting DocumentService here
    }

    public async Task<bool> Handle(UploadDictionaryFileCommand request, CancellationToken cancellationToken)
    {
        if (request.FileContent == null || request.FileContent.Length == 0)
            return false;

        if (Path.GetExtension(request.FileName).ToLower() != ".docx")
            return false;

        var extractedText = _documentService.ExtractTextFromDocx(request.FileContent);
        var extractedHtml = _documentService.ExtractHtmlFromDocx(request.FileContent); // Assuming you have a method to convert text to HTML

        var fileEntity = new UploadDictionaryFile
        {
            FileName = request.FileName,
            FileData = request.FileContent,
            ExtractedText = extractedText,
            ExtractedHtml = extractedHtml, // Assuming you have a method to convert text to HTML
            UploadedAt = DateTime.UtcNow
        };

        return await _repository.UploadDictionaryFileAsync(fileEntity);
    }
}
