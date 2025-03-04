using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class ContentEndpoint
    {
        public static WebApplication MapContentEndpoints(this WebApplication app) 
        {
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

            return app;
        }
    }
}
