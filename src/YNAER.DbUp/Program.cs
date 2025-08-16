using DbUp;
using Microsoft.Extensions.Configuration;
using YNAER.Infrastructure;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true);

var config = builder.Build();

var connectionString = config.GetConnectionString("Database");
Console.WriteLine(connectionString);

var upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .JournalToPostgresqlTable("dbup_journal", "schemaversions")
        .WithScriptsEmbeddedInAssembly(typeof(IInfrastructureAssemblyMarker).Assembly)
        .LogToConsole()
        .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
    Console.ReadLine();
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;