using System.Configuration;
using System.Threading.RateLimiting;
using Api.Services;
using AspNetCoreRateLimit;
using Azure.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Azure;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            string frontEndUrl;

            if (builder.Environment.IsDevelopment())
            {
                frontEndUrl = "http://localhost:3000";
            }
            else
            {
                frontEndUrl = "https://storiesapp202307.netlify.app";
            }

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy
                                      .WithOrigins(frontEndUrl)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                }); ;

            builder.Services.AddSingleton((s) =>
            {
                return new CosmosClient(
                    accountEndpoint: "https://storiesapi.documents.azure.com:443/",
                    tokenCredential: new DefaultAzureCredential(),
                    clientOptions: new CosmosClientOptions
                    {
                        SerializerOptions = new CosmosSerializationOptions
                        {
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                        }
                    });
            });

            builder.Services.AddRateLimiter(options => {
                options.GlobalLimiter = System.Threading.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(), factory: partition => new FixedWindowRateLimiterOptions
                {
                    AutoReplenishment = true,
                    PermitLimit = 5,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1)
                }));
            });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<StoryService>();
            builder.Services.AddScoped<IRepositoryService, CosmosService>();
            builder.Services.AddScoped<IStorageService, StorageService>();

            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(new Uri("https://storiesstorageblob.blob.core.windows.net/"));
                clientBuilder.UseCredential(new DefaultAzureCredential());


            });

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            app.UseRateLimiter();

            app.Run();
        }
    }
}
