﻿using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

/* Alternative, use the dapr client:

using Dapr.Client;

var client = DaprClient.CreateInvokeHttpClient(appId: "order-processor");

client.PostAsJsonAsync("/orders", order);
*/

var baseURL = (Environment.GetEnvironmentVariable("BASE_URL") ?? "http://localhost") + ":" + (Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3500");

var client = new HttpClient();
client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
// Adding app id as part of the header
client.DefaultRequestHeaders.Add("dapr-app-id", "order-processor");

while (true)
{
    for (int i = 1; i <= 20; i++)
    {
        var order = new Order(i);
        var orderJson = JsonSerializer.Serialize<Order>(order);
        var content = new StringContent(orderJson, Encoding.UTF8, "application/json");

        // Invoking a service
        var response = await client.PostAsync($"{baseURL}/orders", content);
        Console.WriteLine("Order passed: " + order);

        await Task.Delay(TimeSpan.FromSeconds(1));
    }
    await Task.Delay(TimeSpan.FromSeconds(10));
}

public record Order([property: JsonPropertyName("orderId")] int OrderId);
