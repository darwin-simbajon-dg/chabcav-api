using chabcav.application.Commands;
using chabcav.application.Commands.Login;
using chabcav.application.Commands.RegisterUser;
using chabcav.application.Commands.UpdatePassword;
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

                if (string.IsNullOrEmpty(response))
                {
                    return Results.BadRequest(new { Error = "Login Failed" });
                }

                return Results.Ok(response);

               


            }).WithTags("User");

            app.MapPost("/user/update-password", async (UpdatePasswordCommand command, IMediator mediator) =>
            {

                var response = await mediator.Send(command);

                if (!response)
                {
                    return Results.BadRequest(new { Error = "Update Password Failed" });
                }

                return Results.Ok(response);

               


            }).WithTags("User");

            return app;
        }
    }
}
