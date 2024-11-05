var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 52428800; // Limit to 50MB, for instance
});

var app = builder.Build();

// Enable serving static files from the wwwroot folder
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve the static index.html file at the root URL
app.MapGet("/", async context =>
{
    context.Response.Redirect("/api/files/index"); // Redirect to the index endpoint
});

// Keep your existing API route for files
app.MapControllers();

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();
