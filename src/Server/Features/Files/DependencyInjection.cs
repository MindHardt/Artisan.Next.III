using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace Server.Features.Files;

public static class DependencyInjection
{
    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        FileStorageImplementation implementation)
    {
        switch (implementation)
        {
            case FileStorageImplementation.Local:
                services.AddLocalFileStorage();
                break;
            case FileStorageImplementation.S3:
                services.AddS3FileStorage();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(implementation));
        }
        return services;
    }

    public static IServiceCollection AddFileStorage(
        this IServiceCollection services,
        IHostEnvironment hostEnv)
    {
        services.AddFileStorage(hostEnv.IsDevelopment()
            ? FileStorageImplementation.Local
            : FileStorageImplementation.S3);
        return services;
    }

    private static IServiceCollection AddLocalFileStorage(this IServiceCollection services)
    {
        services.AddOptions<LocalFileStorageOptions>()
            .BindConfiguration(LocalFileStorageOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IFileStorage, LocalFileStorage>();
        return services;
    }

    private static IServiceCollection AddS3FileStorage(this IServiceCollection services)
    {
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
}