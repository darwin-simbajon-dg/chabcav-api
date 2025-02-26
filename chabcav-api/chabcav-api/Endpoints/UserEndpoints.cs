using chabcav.application.Commands;
using chabcav.application.Commands.Login;
using chabcav.application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

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
                    var result = await mediator.Send(command);

                    if (result.IsSuccessful)
                    {
                        return Results.Ok();
                    }

                    return Results.BadRequest(result.Message);
                    
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");

            app.MapPost("/user/login", async (LoginCommand command, IMediator mediator) =>
            {

                var response = await mediator.Send(command);

                return Results.Ok(response);

                if (string.IsNullOrEmpty(response))
                {
                    return Results.BadRequest(new { Error = "Login Failed" });
                }


            }).WithTags("User");

            return app;
        }
    }
}
