using chabcav.application.Commands;
using chabcav.application.Commands.CMS;
using chabcav.application.Commands.ForgotPassword;
using chabcav.application.Commands.Login;
using chabcav.application.Commands.RegisterUser;
using chabcav.application.Commands.ResetPassword;
using chabcav.application.Commands.SendOTP;
using chabcav.application.Commands.UpdatePassword;
using chabcav.application.Commands.UpdateProfileImage;
using Google.Apis.Drive.v3;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

            app.MapPost("/user/forgot-password", async (ForgotPasswordCommand command, IMediator mediator) =>
            {

                var response = await mediator.Send(command);

                if (!response)
                {
                    return Results.BadRequest(new { Error = "Update Password Failed" });
                }

                return Results.Ok(response);

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

            app.MapPost("/user/reset-password", async (ResetPasswordCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);

                if (response == "Reset Password Failed")
                {
                    return Results.BadRequest(new { Error = "Reset Password Failed" });
                }

                if (response == "Invalid OTP")
                {
                    return Results.BadRequest(new { Error = "Invalid OTP" });
                }

                return Results.Ok(response);
            });


            app.MapPost("/user/send-otp", async (SendOTPCommand command, IMediator mediator) =>
            {
                var response = await mediator.Send(command);

                if (!response)
                {
                    return Results.BadRequest(new { Error = "Send OTP Failed" });
                }

                return Results.Ok(response);
            });

            //app.MapPost("/user/cms", async (CMSCommand command, IMediator mediator) =>
            //{
            //    try
            //    {
            //        var response = await mediator.Send(command);

            //        if (string.IsNullOrEmpty(response))
            //        {
            //            return Results.BadRequest(new { Error = "CMS Failed" });
            //        }

            //        return Results.Ok(response);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Results.BadRequest(new { Error = ex.Message });
            //    }
            //}).WithTags("User");

            





            return app;





        }

        
    }
}
