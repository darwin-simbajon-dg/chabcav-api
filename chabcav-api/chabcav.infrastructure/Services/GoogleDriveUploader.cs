using chabcav.infrastructure.Data.Abstractions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chabcav.infrastructure.Services
{
    public class GoogleDriveUploader : IGoogleDriveUploader
    {
        private readonly DriveService _driveService;
        private static readonly string[] Scopes = { DriveService.Scope.DriveFile };
        private static readonly string ApplicationName = "YourAppName";
        private static readonly string _credentials = "{\"web\":{\"client_id\":\"753882452575-mcq4g7mpgb2kfu29ed8ve7k7oev9n8e7.apps.googleusercontent.com\",\"project_id\":\"spirithub\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_secret\":\"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\",\"redirect_uris\":[\"http://localhost:80/authorize/\"],\"javascript_origins\":[\"http://localhost:80\"]}}";
        public GoogleDriveUploader()
        {
            //var credential = GoogleCredential.FromFile(credentialsPath)
            //    .CreateScoped(DriveService.ScopeConstants.Drive);

            //_driveService = new DriveService(new BaseClientService.Initializer
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = "MyAppDriveUploader"
            //});
        }

       
        public async Task<string> UploadFileToGoogleDrive(IFormFile file)
        {
            //UserCredential credential;

            //using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            //{
            //    string credPath = "token.json";
            //    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true));
            //}

            var credential = await GetOAuthCredentialFromString(_credentials);

            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = file.OpenReadStream())
            {
                request = service.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id";
                await request.UploadAsync();
            }

            var fileId = request.ResponseBody?.Id;

            return fileId;
        }

        public async Task<UserCredential> GetOAuthCredentialFromString(string credentialsJson)
        {
            try
            {
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(credentialsJson));

                string credPath = "token.json"; // still required to store tokens unless you store this elsewhere
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { DriveService.Scope.Drive },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)); // You can replace this with a custom store
            }
            catch (Exception ex)
            {

                throw ex;
            }


          
        }
    }
}
