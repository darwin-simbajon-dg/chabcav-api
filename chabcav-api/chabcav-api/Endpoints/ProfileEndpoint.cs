using chabcav.application.Commands.AddProfile;
using chabcav.application.Commands.UpdateProfile;
using chabcav.application.Queries.GetProfile;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class ProfileEndpoint
    {
        public static WebApplication MapProfileEndpoint(this WebApplication app)
        { 
            app.MapGet("/profile/{userId}", async (Guid userId, IMediator mediator) =>
            {
                try
                {
                    var query = new GetProfileQuery { UserId = userId };
                    var profile = await mediator.Send(query);
                    return Results.Ok(profile);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { Error = ex.Message });
                }
            }).WithTags("Profile");

           app.MapPatch("/profile/update", async (UpdateProfileCommand command, IMediator mediator) => {

               try
               {
                   var profileId = await mediator.Send(command);

                   return Results.Ok(profileId);
               }
               catch (Exception ex)
               {

                   return Results.BadRequest(new {Error = ex.Message});
               }

           
           }).WithTags("Profile");

           app.MapPost("/profile/add", async (AddProfileCommand command, IMediator mediator) => {

                try
                {
                    var profileId = await mediator.Send(command);

                    return Results.Created($"/profile/{profileId}", profileId);
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }


            }).WithTags("Profile");

            return app;
        }

        
    }
}
