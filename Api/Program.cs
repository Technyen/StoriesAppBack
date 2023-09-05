using Api.Services;
using Azure.Identity;
using Microsoft.Azure.Cosmos;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //CORS configuration
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

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddSingleton((s) =>
            {
                return new CosmosClient(
                    accountEndpoint: "https://testingelias.documents.azure.com:443/",
                    tokenCredential: new DefaultAzureCredential(),
                    clientOptions: new CosmosClientOptions
                    {
                        SerializerOptions = new CosmosSerializationOptions
                        {
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                        }
                    });
            });

            //AutomappDI
            builder.Services.AddAutoMapper(typeof(Program));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<StoryService>();
            builder.Services.AddScoped<ICosmosService, CosmosService>();

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}