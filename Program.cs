using static LackmannApi.DatabaseManager;
using LackmannApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello");
app.MapGet("/documents", () => GetAllDocuments());
app.MapGet("documents/{id}", (int id) => FetchFromDatabase(id));
app.MapGet("/documents/{id}/draw", (int id) => 
{
    MarketDocument document = FetchFromDatabase(id);
    //document.DrawGraph();
    return $"Document with id: {id} was drawn.";
});
app.MapGet("documents/{id}/hourly", (int id) =>
{
    MarketDocument document = FetchFromDatabase(id);
    document.Points = FetchPoints(document);
    return $"{document.HourlyPoints.Count}"; // this returns the length of the hourly array. Modify to return the array.
});
app.Run();
