using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/upload")]
public class UploadController : ControllerBase
{
    private readonly IConfiguration _config;

    public UploadController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var accessKey = _config["AWS:AccessKey"];
        var secretKey = _config["AWS:SecretKey"];
        var region = _config["AWS:Region"];
        var bucket = _config["AWS:Bucket"];

        var awsRegion = Amazon.RegionEndpoint.GetBySystemName(region);

        // Subir imagen a S3
        var s3 = new AmazonS3Client(accessKey, secretKey, awsRegion);
            
        var key = $"uploads/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        using var stream = file.OpenReadStream();

        await s3.PutObjectAsync(new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            ContentType = file.ContentType
        });

        // Lambda
        var lambda = new AmazonLambdaClient(accessKey, secretKey, awsRegion);

        var payload = new
        {
            filename = key,
            collectionId = "riwilens-faces"
        };

        var invokeRequest = new InvokeRequest
        {
            FunctionName = "TestAsistancePython",
            InvocationType = InvocationType.RequestResponse,
            Payload = JsonSerializer.Serialize(payload)
        };

        var response = await lambda.InvokeAsync(invokeRequest);

        var lambdaResponse =
            await new StreamReader(response.Payload).ReadToEndAsync();

        var result = JsonSerializer.Deserialize<object>(lambdaResponse);

        // 3️⃣ Response to frontend
        return Ok(new
        {
            key,
            rekognition = result
        });
    }
}