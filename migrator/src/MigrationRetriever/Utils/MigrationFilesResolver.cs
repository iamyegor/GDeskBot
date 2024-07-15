using System.Net.Http.Json;
using Migrator.Models;

namespace Migrator.Utils;

public class MigrationFilesResolver
{
    public async Task<List<MigrationFile>> GetMigrationsAsync(string url)
    {
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(3);

        return (await client.GetFromJsonAsync<List<MigrationFile>>(url))!;
    }
}
