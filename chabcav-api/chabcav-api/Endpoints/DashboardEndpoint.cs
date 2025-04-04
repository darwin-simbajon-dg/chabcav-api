using chabcav.application.Queries.GetDashboard;
using MediatR;

namespace chabcav_api.Endpoints
{
    public static class DashboardEndpoint
    {
        public static WebApplication MapDashboardEndpoints(this WebApplication app)
        {
            app.MapGet("/dashboard", (IMediator mediator) =>
            {
                var query = new GetDashboardQuery();
                var data = mediator.Send(query);

                return Results.Ok(data);
            }).WithTags("Dashboard");
            return app;
        }
    }
}
