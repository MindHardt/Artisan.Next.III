using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace Server.Features.Files;

public class S3FileStorage(IAmazonS3 s3, IOptions<S3FileStorageOptions> options) : IFileStorage
{
    public async Task<Stream> GetFileStream(FileHashString hash, CancellationToken ct = default)
        => (await s3.GetObjectAsync(new GetObjectRequest
        {
            Key = hash.Value
        }, ct)).ResponseStream;

    public Task SaveFile(Stream content, FileHashString hash, CancellationToken ct = default)
        => s3.PutObjectAsync(new PutObjectRequest
        {
            Key = hash.Value,
            InputStream = content,
            BucketName = options.Value.BucketName
        }, ct);

    public async Task<bool> FileExists(FileHashString hash, CancellationToken ct = default)
        => (await s3.ListObjectsAsync(new ListObjectsRequest
        {
            BucketName = options.Value.BucketName,
            Prefix = hash.Value,
            MaxKeys = 1
        }, ct)).S3Objects is [var s3Object, ..] && s3Object.Key == hash.Value;
}

public record S3FileStorageOptions
{
    public const string Section = "S3";
    
    public required string AccessKeyId { get; set; }
    public required string SecretAccessKey { get; set; }
    public required string BucketName { get; set; }
    
    public string ServiceUrl { get; set; } = "https://s3.yandexcloud.net";
}