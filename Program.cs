using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 添加CORS服务
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000") // 替换为你的前端地址
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 配置CORS中间件
app.UseCors();

//app.UseHttpsRedirection();

// 添加异常处理中间件
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(static async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature != null)
        {
            var exception = exceptionHandlerFeature.Error;

            // 设置响应状态码
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // 设置响应内容类型
            context.Response.ContentType = "application/json";

            // 返回异常信息
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                context.Response.StatusCode,
                exception.Message
            }).ToString());
        }
    });
});


app.MapGet("/scans",()=>
{
    return new List<Scan>
    {
        new Scan("1", DateTime.Now, "This is a scan"),
        new Scan("2", DateTime.Now, "This is another scan"),
        new Scan("3", DateTime.Now, "This is yet another scan"),
        new Scan("4", DateTime.Now, "This is the last scan")
     };
}).WithName("GetScans");

app.MapGet("/scans/{id}/notes", (string id) =>
{
    return new List<Notes>{
        new Notes("1", id+"Note 1", "This is note 1", id),
        new Notes("2", id+"Note 2", "This is note 2", id),
        new Notes("3", id+"Note 3", "This is note 3", id),
        new Notes("4", id+"Note 4", "This is note 4", id)  
    };
}).WithName("GetScanById");

app.MapPost("/scans/{id}/notes", (string id, Notes note) =>
{
    // check if note.Title or note.Content is empty or null
    if (string.IsNullOrEmpty(note.Title))
    {
        throw new ArgumentException("Title cannot be null or empty");
    }
    if (string.IsNullOrEmpty(note.Content))
    {
        throw new ArgumentException("Content cannot be null or empty");
    }
    //todo saving to database

    return new Notes("5", note.Title, note.Content, id);
}).WithName("PostScanById");

app.Run();


record Scan(string ScanId, DateTime AddTime, string Description);
record Notes(string NotesId, string Title, string Content, string ScanId);