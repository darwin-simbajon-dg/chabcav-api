using chabcav.application.Commands.AddConfiguration;
using chabcav.application.Commands.CMS;
using chabcav.application.Commands.GetConfiguration;
using chabcav.domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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



            app.MapPost("/api/cms/upload", async (HttpRequest request, IMediator mediator) =>
            {
                try
                {
                    var file = request.Form.Files[0];
                    var cms = new CMSDto()
                    {
                        banner = request.Form.Files["banner"]?.FileName,
                        //headline = request.Form.Files["headline"].FileName,
                        midcontentimage = request.Form.Files["midcontentimage"]?.FileName,
                        //content = request.Form.Files["content"].FileName,
                        card1 = request.Form.Files["card1"]?.FileName,
                        card2 = request.Form.Files["card2"]?.FileName,
                        card3 = request.Form.Files["card3"]?.FileName,
                        card4 = request.Form.Files["card4"]?.FileName,
                        card5 = request.Form.Files["card5"]?.FileName,
                        card6 = request.Form.Files["card6"]?.FileName,
                        card7 = request.Form.Files["card7"]?.FileName,
                        card8 = request.Form.Files["card8"]?.FileName

                    };

                    var command = new CMSCommand(cms, request.Form.Files.ToList());

                    
                    
                    //foreach (var formFile in request.Form.Files)
                    //{ 
                    //    if (formFile.Length > 0)
                    //    {
                    //        var filePath = Path.Combine("wwwroot\\uploads", formFile.FileName);
                    //        if (!File.Exists(filePath))
                    //        {
                    //            using (var stream = new FileStream(filePath, FileMode.Create))
                    //            {
                    //                await formFile.CopyToAsync(stream);
                    //            }
                    //        }
                    //    }
                       
                      
                    //}

                   var result = await mediator.Send(command);

                    if (!result)
                    {
                        return Results.BadRequest(new { Error = "Failed to upload files." });
                    }


                    return Results.Ok();
                   
                }
                catch (Exception ex)
                {
                    // Handle errors
                    return Results.BadRequest();
                }
            }).WithTags("CMS");


            return app;
        }
    }
}
