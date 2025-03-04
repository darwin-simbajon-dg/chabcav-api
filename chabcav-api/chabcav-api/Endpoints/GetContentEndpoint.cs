using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using chabcav.application.Commands.GetContent.GetLesson;
using chabcav.application.Commands.UpdateLesson;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class GetContentEndpoint
    {
        public static WebApplication MapGetContentEndpoints(this WebApplication app) 
        {
            app.MapPost("/user/get-content", async (GetLessonCommand command, IMediator mediator) =>
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
