using Translator.Backend;
using Translator.Backend.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\Maksym\key.json");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(e => { e.MaximumReceiveMessageSize = 102400000; });
builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<ISessionRepository , SessionRepository>();
builder.Services.AddTransient<ISegmentRepository, SegmentRepository>();
RegisterCors(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<TranscriptionsHub>("/transcription");

app.Run();

static void RegisterCors(WebApplicationBuilder builder)
{
    var allowedOriginsCORS = builder.Configuration.GetSection("AllowedOriginsCORS").Get<string[]>()!;
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
            builder.WithOrigins(allowedOriginsCORS)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
        );
    });
}
