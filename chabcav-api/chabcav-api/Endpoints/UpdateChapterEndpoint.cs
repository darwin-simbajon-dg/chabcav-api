using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using chabcav.application.Commands.UpdateChapter;
using chabcav.application.Commands.UpdateLesson;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class UpdateChapterEndpoint
    {
        public static WebApplication MapUpdateChapterEndpoints(this WebApplication app) 
        {
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

            return app;
        }
    }
}
