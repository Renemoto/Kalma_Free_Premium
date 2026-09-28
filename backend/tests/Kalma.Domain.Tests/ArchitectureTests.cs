using System.Xml.Linq;

namespace Kalma.Domain.Tests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Domain_does_not_reference_other_Kalma_projects_or_outer_layer_dependencies()
    {
        var projectPath = FindBackendProject("src/Kalma.Domain/Kalma.Domain.csproj");
        var document = XDocument.Load(projectPath);
        var violations = new List<string>();

        foreach (var element in document.Descendants())
        {
            var include = (string?)element.Attribute("Include") ?? (string?)element.Attribute("Update");
            if (include is null)
            {
                continue;
            }

            var name = Path.GetFileNameWithoutExtension(include.Replace('\\', '/'));
            if (element.Name.LocalName == "ProjectReference" &&
                name.StartsWith("Kalma.", StringComparison.OrdinalIgnoreCase))
            {
                violations.Add($"ProjectReference: {include}");
            }

            if (element.Name.LocalName == "PackageReference" && IsForbiddenPackage(include))
            {
                violations.Add($"PackageReference: {include}");
            }

            if (element.Name.LocalName == "FrameworkReference" && IsForbiddenAssembly(include))
            {
                violations.Add($"FrameworkReference: {include}");
            }

            if (element.Name.LocalName == "Reference" && IsForbiddenAssembly(include))
            {
                violations.Add($"Reference: {include}");
            }
        }

        var sdk = (string?)document.Root?.Attribute("Sdk");
        if (sdk?.Contains("Web", StringComparison.OrdinalIgnoreCase) == true)
        {
            violations.Add($"Web SDK: {sdk}");
        }

        Assert.True(violations.Count == 0,
            $"Domain must not reference other Kalma projects, EF Core, or ASP.NET. Found: {string.Join("; ", violations)}");
    }

    private static bool IsForbiddenPackage(string name) =>
        name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase) ||
        IsAspNet(name) ||
        name.StartsWith("Kalma.Infrastructure", StringComparison.OrdinalIgnoreCase);

    private static bool IsForbiddenAssembly(string name) =>
        name.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase) ||
        IsAspNet(name) ||
        name.StartsWith("Kalma.Infrastructure", StringComparison.OrdinalIgnoreCase);

    private static bool IsAspNet(string name) =>
        name.StartsWith("Microsoft.AspNetCore", StringComparison.OrdinalIgnoreCase);

    private static string FindBackendProject(string relativePath)
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            var candidate = Path.Combine(directory.FullName, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new DirectoryNotFoundException($"Could not locate backend project {relativePath} from {AppContext.BaseDirectory}.");
    }
}
