using static LackmannApi.DatabaseManager;
using LackmannApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "API Default Path");
app.MapGet("/documents", () => GetAllDocuments())
.WithTags("All Documents");

app.MapGet("documents/{id}", (int id) => FetchFromDatabase(id));
app.MapGet("/documents/{id}/{interval}", (int id, string interval) => 
{
    MarketDocument document = FetchFromDatabase(id);
    document.Points = FetchPoints(document);

    if(interval.Equals("default"))
        return document.Points;
    if(interval.Equals("hourly"))
        return document.HourlyPoints;
    if(interval.Equals("daily"))
        return document.DailyPoints;
    if(interval.Equals("weekly"))
        return document.WeeklyPoints;
    if(interval.Equals("monthly"))
        return document.MonthlyPoints;
    
    return document.Points;
});
app.MapGet("/documents/{id}/{interval}/draw", (int id, string interval) =>
{
    MarketDocument document = FetchFromDatabase(id);
    document.Points = FetchPoints(document);

    document.DrawGraph(interval);
    return $"{interval} Graph with MRID: {document.MRID} was drawn.";
});
app.Run();