using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var client = new DaprClientBuilder().Build();

app.MapPost("/queuetrigger", async (HttpRequest request) => {
    using var reader = new StreamReader(request.Body);
    var msg = await reader.ReadToEndAsync();
    var output = new { id = Guid.NewGuid().ToString(), message = msg };
    await client.InvokeBindingAsync("cosmosdb", "create", output);
    Console.WriteLine($"Message {msg}");
    return Results.Ok();
});

app.Run();