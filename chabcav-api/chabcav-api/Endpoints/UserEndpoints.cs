using chabcav.application.Commands;
using chabcav.application.Commands.AddContent;
using chabcav.application.Commands.CMS;
using chabcav.application.Commands.EndUsers;
using chabcav.application.Commands.ForgotPassword;
using chabcav.application.Commands.Login;
using chabcav.application.Commands.RegisterUser;
using chabcav.application.Commands.ResetPassword;
using chabcav.application.Commands.SendOTP;
using chabcav.application.Commands.UpdatePassword;
using chabcav.application.Commands.UpdateProfileImage;
using chabcav.domain.Entities;
using chabcav.domain.Interfaces;
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


            app.MapPost("/users/users-progress", async (UsersProgressCommand command, IMediator mediator) =>
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


            app.MapGet("/users/users-completed-chapters", async (string userId, IUsersProgressRepository repository) =>
            {
                try
                {
                    var completedChapters = await repository.GetCompletedChaptersByUserIdAsync(userId);
                    return Results.Ok(completedChapters); // Returns List<string>
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("User");


            //For Admin users management

            app.MapPut("/admin/update-user", async (User user, IUserRepository userRepo) =>
            {
                try
                {
                    var result = await userRepo.UpdateUserAsync(user);
                    return result
                        ? Results.Ok(new { Message = "User updated successfully" })
                        : Results.NotFound(new { Error = "User not found or update failed" });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("Admin");


            app.MapDelete("/admin/delete-user/{id:guid}", async (Guid id, IUserRepository userRepo) =>
            {
                try
                {
                    var result = await userRepo.DeleteUserAsync(id);
                    return result
                        ? Results.Ok(new { Message = "User deleted successfully" })
                        : Results.NotFound(new { Error = "User not found or already deleted" });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("Admin");

            app.MapGet("/admin/get-all-users", async (IUserRepository userRepo) =>
            {
                try
                {
                    var users = await userRepo.GetAllAsync();
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).WithTags("Admin");



            app.MapPost("/admin/add-user", async (RegisterUserCommand command, IMediator mediator) =>
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
            }).WithTags("Admin");



            return app;





        }

        
    }
}
