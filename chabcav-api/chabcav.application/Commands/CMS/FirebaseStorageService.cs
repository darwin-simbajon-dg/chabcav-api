using chabcav.application.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class FirebaseStorageService 
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName = "chabcav-d81fa.firebasestorage.app"; // Update this


    public FirebaseStorageService()
    {
        var credentials = "{\r\n  \"type\": \"service_account\",\r\n  \"project_id\": \"chabcav-d81fa\",\r\n  \"private_key_id\": \"45d9507858ff8ea282bb300450de5f3d1b4e3570\",\r\n  \"private_key\": \"-----BEGIN PRIVATE KEY-----\\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDVGMCrtVh6abSN\\ndJl7gsGVyaHaLiSbfQgTbAGkONHk96eO8dNmgDJEz5cLwth2obzA5DxV33f1mScw\\n8+eq+84oK4ZxILPhVl9Bm5U3zmCBIrvtBC+tGHMn71zE1UCyYKIU2gFanhWjB42J\\nOUkbfjsKNv0eEngHJTyFqaOuWfh0qnR29nVFtZceO6bvcnq6L1e0cZHc3GkgoedQ\\n/eAYwz6D5l5uvqVgWEn2WI236j2JysTurA+UzlhWYTSPgldt5Jdmik6LTpUremiv\\ncDMO5AuCa4at6mF7f/Een9bunwHDxFzDDci/JMXgUIfQGFzxFYIm2n/X0PtA0uBp\\nfF07mDghAgMBAAECggEADHfSyhklsJ/frwkb89SONA/kluntPWW+47gFjpC+gymU\\nVbJDfrNQfaMxwL/pP2OuMhEIkLOvBqEr4Frc+q2Z/dKyHotdyvjtQN70B5xdHCEm\\norMEgRm1nygLov6hNu6dfR/WRXGLbYsDFnzSELg9hxb5j1lgSXa9yEpJJ+netiCp\\nrinZOZXvYMzruhuVg704ksLxvPzYcFcFGHKs+cvMFrJTkLIve4uFrG2NwZeDHRF3\\nwHuiLdYZWSyc3FAqOEJQxG6WHcOyJI726WQfWNbVFNmKg+JjkQKC+e9UW/X4b0IG\\nWxRjRwQKM/KDZi1axiX7U+EWT6bBmNeKLrcs1UYw+QKBgQDzM5APFwA1gHIvlJp1\\nJQ/GNQ3ja64monDek92XyDVW2G4VOXsY5HopBdZo5EEtmfO0TgESdo1o7nYx0ozt\\n0DDH8TL4v5wkBABEjPIzJ8aE2S3zoHsr9ZUIi6Lu4aT72/HmVJsPhc/jm6R3LvCx\\nkQUxeihg915UXYd0Awnb1/JPfQKBgQDgT52V7ACXaTmgAt0e9sfje527MsCbipyx\\nUNHO+eDuet9iAj2g3UYHkrm+mqCi8St/dNAQezOakPIvcL9u3KgomlZUmpsdYkmb\\nYb9G+UvMLo5/1HWOBfZkfRHIjYnvqJFLh5nsK4Yjy9nw3w9fCEmh1VwVrlaYXErJ\\nnd6XH660dQKBgH5TmE7eSbOAxs9ER372A12XjWFGO63BzxMuh3oh+uLjhTrtIq3p\\ncDMC3z6Y4epH/7j2k8P9ZLLOuwJiNeYJsG0LMsjW7soece0psV01Gf9DuITGJXTO\\nGd3YeofPGZ3hv6M+61SC01uNKz7lWQ1DIQl0RKkplQwJkSSHt4VEhI8ZAoGBAJzX\\nDljFq6Qy7xfy6Km95AYraR57XjFjuixBVnJiReR6BbeB2ZWhIlYpQbcEZ02HU+LK\\nyrC7dFme/7gsHa5mmy3IvJRrhgoFr3H15si5h0Y9R2YaKBgmEyIwUT7puIjaVL17\\nEyUPSbu9zaWeUiEgqlt90+VVATeHUMOonvsMLBwBAoGALc0aDrLBbJx6ny5tsafJ\\nZv4WwsoHbAN/uatTskdgmLGg5dX62qpTRe4h+woVnz/CZUaCP+Jw1Phy2XY9QEN7\\nu+t2Jn+eijmpXBYu15WmCu99SkCqWxpRlpgNiN6axbJygc3s5vSd95D1hF7rrxTk\\nU3ZqTmnL4Tb14sy/TCI2URA=\\n-----END PRIVATE KEY-----\\n\",\r\n  \"client_email\": \"chabcav-app@chabcav-d81fa.iam.gserviceaccount.com\",\r\n  \"client_id\": \"104743628429318325253\",\r\n  \"auth_uri\": \"https://accounts.google.com/o/oauth2/auth\",\r\n  \"token_uri\": \"https://oauth2.googleapis.com/token\",\r\n  \"auth_provider_x509_cert_url\": \"https://www.googleapis.com/oauth2/v1/certs\",\r\n  \"client_x509_cert_url\": \"https://www.googleapis.com/robot/v1/metadata/x509/chabcav-app%40chabcav-d81fa.iam.gserviceaccount.com\",\r\n  \"universe_domain\": \"googleapis.com\"\r\n}";
        var googleCredentials = GoogleCredential.FromJson(credentials);
        _storageClient = StorageClient.Create(googleCredentials);
    }

    public async Task UploadFilesAsync(List<IFormFile> files)
    {
        try
        {
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await formFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reset stream position before upload

                    var objectName = $"public/{formFile.FileName}";

                    await _storageClient.UploadObjectAsync(
                        bucket: _bucketName,
                        objectName: objectName,
                        contentType: formFile.ContentType,
                        source: memoryStream
                    );

                    Console.WriteLine($"Uploaded {objectName} to {_bucketName}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading files: {ex.Message}");
        }
       
    }


    public async Task UploadProfileImage(IFormFile file)
    {
        try
        {
            if (file != null && file.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Reset stream position before upload



                var objectName = $"public/{file.FileName}";

                await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: objectName,
                    contentType: file.ContentType,
                    source: memoryStream
                );

                Console.WriteLine($"Uploaded {objectName} to {_bucketName}.");
            }
            else
            {
                Console.WriteLine("No file selected or file is empty.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error uploading file: {ex.Message}");
        }
    }


}