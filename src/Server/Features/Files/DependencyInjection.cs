using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace Server.Features.Files;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        FileStorageImplementation implementation = FileStorageImplementation.Auto,
        IHostEnvironment? environment = null)
    {
        switch (implementation, environment?.IsDevelopment())
        {
            case (FileStorageImplementation.Local, _) or (FileStorageImplementation.Auto, true):
                services.AddOptions<LocalFileStorageOptions>()
                    .BindConfiguration(LocalFileStorageOptions.Section)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();
                services.AddScoped<IFileStorage, LocalFileStorage>();
                break;

            case (FileStorageImplementation.S3, _) or (FileStorageImplementation.Auto, false):
                services.AddScoped<IFileStorage, S3FileStorage>();
                services.AddOptions<S3FileStorageOptions>()
                    .BindConfiguration(S3FileStorageOptions.Section)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

                services.AddScoped<IAmazonS3>(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<S3FileStorageOptions>>().Value;
                    return new AmazonS3Client(
                        new BasicAWSCredentials(options.AccessKeyId, options.SecretAccessKey),
                        new AmazonS3Config
                        {
                            ServiceURL = options.ServiceUrl
                        });
                });
                break;

            default: throw new InvalidOperationException();
        }

        return services;
    }
}

public enum FileStorageImplementation
{
    /// <summary>
    /// Files are stored in local file system.
    /// </summary>
    Local,
    /// <summary>
    /// Files are stored in S3 storage.
    /// </summary>
    S3,
    /// <summary>
    /// Resolves depending on an environment.
    /// When in development <see cref="Local"/> is used,
    /// otherwise <see cref="S3"/>.
    /// </summary>
    Auto
}