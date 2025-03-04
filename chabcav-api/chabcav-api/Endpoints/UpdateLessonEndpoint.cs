using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using chabcav.application.Commands.UpdateLesson;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class UpdateLessonEndpoint
    {
        public static WebApplication MapUpdateLessonEndpoints(this WebApplication app) 
        {
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

            return app;
        }
    }
}
