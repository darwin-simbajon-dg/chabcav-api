using chabcav.application.Commands;
using chabcav.application.Commands.Login;
using chabcav.application.Commands.RegisterUser;
using chabcav.application.Commands.UpdateProfileImage;
using Google.Apis.Drive.v3;
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

            app.MapPost("/upload", async (HttpRequest request, IMediator mediator) =>
            {
                try
                {
                    if (!request.HasFormContentType || !request.Form.Files.Any())
                    {
                        return Results.BadRequest(new { Error = "No file uploaded" });
                    }

                    var file = request.Form.Files[0];
                    var userId = request.Form["userId"].ToString();

                    if (string.IsNullOrEmpty(userId))
                    {
                        return Results.BadRequest(new { Error = "User ID is required" });
                    }

                    var command = new UpdateProfileImageCommand(file, Guid.Parse(userId));
                    var result = await mediator.Send(command);

                    if (result)
                    {
                        return Results.Ok("File successfully uploaded");
                    }
                    else
                    {
                        return Results.BadRequest(new { Error = "File upload failed" });
                    }
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
