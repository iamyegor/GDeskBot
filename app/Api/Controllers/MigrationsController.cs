using System;
using System.IO;
using System.Linq;
using Infrastructure.Utils;
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

        DirectoryInfo dirInfo = new DirectoryInfo(baseDirectory);

        string migrationsFolderPath;
        if (EnvironmentResolver.IsDevelopment)
        {
            while (!dirInfo.Name.Equals("src", StringComparison.OrdinalIgnoreCase))
            {
                dirInfo = dirInfo.Parent!;
            }

            migrationsFolderPath = Path.Combine(
                dirInfo.FullName,
                "Infrastructure",
                "Data",
                "Migrations"
            );
        }
        else
        {
            migrationsFolderPath = Path.Combine(dirInfo.FullName, "Migrations");
        }

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
