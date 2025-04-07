using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using chabcav.application.Queries.GetAllLessons;
using chabcav.application.Commands.GetContent.GetLesson;
using chabcav.application.Commands.UpdateChapter;
using chabcav.application.Commands.UpdateLesson;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;
using chabcav.application.Commands.AddContent.UploadFileDictionary;
using chabcav.application.Interfaces;
using chabcav.application.Services;
using chabcav.application.Queries.Dictionary;

namespace chabcav_api.Endpoints
{
    public static class ContentEndpoint
    {
        public static WebApplication MapContentEndpoints(this WebApplication app) 
        {
            // Add Content Chapters and Lesson
            app.MapPost("/admin/create-content", async (AddContentCommand command, IMediator mediator) =>
            {
                try
                {
                    var userId = await mediator.Send(command);
                    return Results.Ok(new { UserId = userId });
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");

            //Updates Lesson and Chapters
            app.MapPost("/admin/update-chapter", async (UpdateChapterCommand command, IMediator mediator) =>
            {
                try
                {
                    var userId = await mediator.Send(command);
                    return Results.Ok(new { UserId = userId });
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");

            app.MapPost("/admin/update-lesson", async (UpdateLessonCommand command, IMediator mediator) =>
            {
                try
                {
                    var userId = await mediator.Send(command);
                    return Results.Ok(new { UserId = userId });
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");

            //Get all lesson and list
            app.MapGet("/user/get-all-lessons", async ([AsParameters] GetAllLessonsQuery query, IMediator mediator) =>
            {
                try
                {
                    var Alllessons = await mediator.Send(query);
                    return Results.Ok(new { Lessons = Alllessons });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");


            //Get Lesson base on lesson id
            app.MapPost("/user/get-content", async (GetLessonCommand query, IMediator mediator) =>
            {
                try
                {
                    var lessons = await mediator.Send(query);
                    return Results.Ok(new { Lessons = lessons });
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");

            //Get Lesson base on Chaptername selected
            app.MapGet("/user/get-lessons-by-chapter", async ([FromQuery] string chapterName, IMediator mediator) =>
            {
                try
                {
                    if (string.IsNullOrEmpty(chapterName))
                    {
                        return Results.BadRequest(new { Error = "Chapter name is required." });
                    }

                    var query = new GetLessonBaseOnChapterNameCommand(chapterName); // ✅ Now it works!
                    var lessons = await mediator.Send(query);

                    if (lessons == null || !lessons.Any())
                    {
                        return Results.NotFound(new { Error = "No lessons found for the selected chapter." });
                    }

                    return Results.Ok(new { Lessons = lessons });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");


            app.MapPost("/admin/upload-file-dictionary", async (HttpRequest request, IMediator mediator) =>
            {
                try
                {
                    var form = await request.ReadFormAsync();
                    var file = form.Files["file"]; // "file" is the key you send from the frontend

                    if (file == null || file.Length == 0)
                    {
                        return Results.BadRequest(new { Error = "No file uploaded." });
                    }

                    // Create the command and assign the file data
                    var command = new UploadDictionaryFileCommand
                    {
                        FileName = file.FileName, // File name from the uploaded file
                        FileContent = await FileHelpers.ConvertToByteArray(file), // Convert the file to byte[] for storage
                        File = file // Keep the IFormFile for any further processing if needed (like extracting text or other properties)
                    };

                    // Send the command via the mediator to handle the file upload
                    var result = await mediator.Send(command);

                    return result
                        ? Results.Ok(new { Message = "Upload successful." })
                        : Results.BadRequest(new { Error = "Upload failed." });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");



            app.MapGet("/api/dictionary/{id}", async (int id, IContentRepository repository, IDocumentService documentService) =>
            {
                try
                {
                    var dictionaryFile = await repository.GetDictionaryFileByIdAsync(id);

                    if (dictionaryFile == null)
                    {
                        return Results.NotFound(new { Error = "Dictionary file not found." });
                    }

                    // Use the injected IDocumentService to extract text from the file
                    var fileText = documentService.ExtractTextFromDocx(dictionaryFile.FileData);

                    return Results.Ok(new { DictionaryContent = fileText });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("Dictionary");



            app.MapGet("/api/dictionary/search", async (string query, IMediator mediator) =>
            {
                var result = await mediator.Send(new DictionarySearchQuery { Query = query });
                return Results.Ok(result);
            }).WithTags("Dictionary");




            return app;






        }
    }
}
