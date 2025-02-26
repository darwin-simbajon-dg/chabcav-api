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

            return app;
        }
    }
}
