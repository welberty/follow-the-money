using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IConsolidateClient, ConsolidateClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/consolidate", async (
    [FromQuery] DateTime Date,
    IConsolidateClient consolidateClient) =>
{

    var response = await consolidateClient.GetConsolidate(DateOnly.FromDateTime(Date));

    return Results.Ok(response);
});


app.MapPost("/api/transaction", async ([FromBody] TransactionRequest request) => { 



});

app.Run();

enum TransactionType
{
    Credit,
    Debit
}
record TransactionRequest(string Description, Double Value, TransactionType type) { }

record ConsolidateResponse(DateOnly Date, double Amount, int TotalTransactions);

interface IConsolidateClient
{
    Task<ConsolidateResponse> GetConsolidate(DateOnly date);
}

class ConsolidateClient : IConsolidateClient
{
    public async Task<ConsolidateResponse> GetConsolidate(DateOnly date)
    {
        string responseBody = "";
        ConsolidateResponse response = new ConsolidateResponse(date, 0, 0);
        try
        {
            HttpClient client = new HttpClient();
            using HttpResponseMessage _response = await client.GetAsync($"http://consolidate-api/consolidate?Date={date}");
            _response.EnsureSuccessStatusCode();
            responseBody = await _response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ConsolidateResponse>(responseBody);
            if(result == null) 
            {
                return response;
            }

            response = result;
        }
        catch (Exception e)
        {
            var error = e;
            
        }

        return response;
    }

}

interface ITransactionClient
{
    Task<IEnumerable<string>> AddTransaction(TransactionRequest transactionRequest);
}

class TransactionClient : ITransactionClient
{
    public Task<IEnumerable<string>> AddTransaction(TransactionRequest transactionRequest)
    {
        string responseBody = "";
        IEnumerable<string> response = new List<string>();
    }
}
