using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
namespace RaceDay.Api.Services;
public class BlobStorageService {
    private readonly BlobContainerClient _container;
    public BlobStorageService(IConfiguration config) {
        _container = new BlobContainerClient(config["AzureBlobStorage:ConnectionString"], config["AzureBlobStorage:ContainerName"]);
    }
    public async Task<string> UploadAsync(IFormFile file, string folder) {
        if (file == null || file.Length == 0) return "";
        await _container.CreateIfNotExistsAsync(PublicAccessType.Blob);
        var blobName = $"{folder}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blob = _container.GetBlobClient(blobName);
        await using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
        return blob.Uri.ToString();
    }
}
