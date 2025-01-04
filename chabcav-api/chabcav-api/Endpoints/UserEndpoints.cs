using chabcav.application.Commands;
using chabcav.application.Commands.RegisterUser;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication MapUserEndpoints(this WebApplication app) 
        {
            app.MapPost("/user/register", async (RegisterUserCommand command, IMediator mediator) =>
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
