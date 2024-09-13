using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

string DAPR_STORE_NAME = "statestore";
string DAPR_STORE_KEY = "nameList";
var client = new DaprClientBuilder().Build();

app.MapGet("/hello/{name}", async (string name) => {
    // store the name in a state store provided by dapr
    var names = await client.GetStateAsync<string[]>(DAPR_STORE_NAME, DAPR_STORE_KEY);
    if (names == null)
    {
        names = new string[] { name };
    }
    else
    {
        names = names.Append(name).ToArray();
    }
    await client.SaveStateAsync(DAPR_STORE_NAME, DAPR_STORE_KEY, names);
    Console.WriteLine("Name saved: " + name);
    return $"Hello, {name}!";
});

app.MapGet("/names", async () => {
    // get all the names from the state store
    return await client.GetStateAsync<string[]>(DAPR_STORE_NAME, DAPR_STORE_KEY);
});

app.MapDelete("/goodbye/{name}", async (string name) => {
    // delete the name from the state store
    var names = await client.GetStateAsync<string[]>(DAPR_STORE_NAME, DAPR_STORE_KEY);
    names = names.Where(n => n != name).ToArray();
    await client.SaveStateAsync(DAPR_STORE_NAME, DAPR_STORE_KEY, names);
    Console.WriteLine("Name deleted: " + name);
    return $"Goodbye, {name}!";
});

app.Run();
