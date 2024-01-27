using Microsoft.Extensions.Configuration;
static class Configuration
{
    static public string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../IzinTalepUygulamasi.API"));
            configurationManager.AddJsonFile("appsettings.json");

            return configurationManager.GetConnectionString("MSSQL");
        }
    }
}
