using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("migrations")]
public class MigrationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMigrations()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        DirectoryInfo srcFolderInfo = new DirectoryInfo(baseDirectory);

        while (!srcFolderInfo.Name.Equals("src", StringComparison.OrdinalIgnoreCase))
        {
            srcFolderInfo = srcFolderInfo.Parent!;
        }
        
        string migrationsFolderPath = Path.Combine(
            srcFolderInfo.FullName,
            "Infrastructure",
            "Data",
            "Migrations"
        );

        DirectoryInfo migrationsFolderInfo = new DirectoryInfo(migrationsFolderPath);
        FileInfo[] migrationFiles = migrationsFolderInfo.GetFiles();

        var migrationFilesContent = migrationFiles
            .Select(file => new
            {
                FileName = file.Name,
                Content = System.IO.File.ReadAllText(file.FullName)
            })
            .ToList();

        return Ok(migrationFilesContent);
    }
}
