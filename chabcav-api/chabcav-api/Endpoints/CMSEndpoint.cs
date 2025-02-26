using chabcav.application.Commands.AddConfiguration;
using chabcav.application.Commands.GetConfiguration;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class CMSEndpoint
    {
        public static WebApplication MapCMSEndpoints(this WebApplication app)
        {
            app.MapGet("/cms/configurations", (IMediator mediator) =>
            {
                try
                {
                    var command = new GetConfigurationCommand();
                    var configurations = mediator.Send(command);

                    return Results.Ok(configurations.Result);
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }


            }).WithTags("CMS");

            app.MapPost("chbcav-api/cms/configurations", (AddConfigurationCommand command, IMediator mediator) =>
            {
                try
                {
                    var data = mediator.Send(command);

                    return Results.Ok(data);
                }
                catch (Exception ex)
                {

                    return Results.BadRequest(new { Error = ex.Message });
                }

            }).WithTags("CMS");


            return app;
        }
    }
}
