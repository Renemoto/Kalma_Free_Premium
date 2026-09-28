using System.Xml.Linq;

namespace Kalma.Application.Tests;

public sealed class ArchitectureTests
{
    [Fact]
    public void Application_does_not_reference_infrastructure()
    {
        var projectPath = FindBackendProject("src/Kalma.Application/Kalma.Application.csproj");
        var document = XDocument.Load(projectPath);
        var violations = document.Descendants()
            .Where(element => element.Name.LocalName is "ProjectReference" or "PackageReference" or "FrameworkReference" or "Reference")
            .SelectMany(element => element.Attributes("Include").Concat(element.Attributes("Update"))
                .Select(attribute => new { Kind = element.Name.LocalName, Name = (string?)attribute }))
            .Where(reference => reference.Name is not null &&
                (IsInfrastructure(reference.Name) ||
                 (reference.Kind == "ProjectReference" && IsApi(reference.Name))))
            .Select(reference => $"{reference.Kind}: {reference.Name}")
            .ToArray();

        Assert.True(violations.Length == 0,
            $"Application must not reference Infrastructure or Api projects. Found: {string.Join("; ", violations)}");
    }

    private static bool IsApi(string name)
    {
        name = name.Replace('\\', '/');
        return (name.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) &&
                Path.GetFileNameWithoutExtension(name).Equals("Kalma.Api", StringComparison.OrdinalIgnoreCase)) ||
               name.Equals("Kalma.Api", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsInfrastructure(string name)
    {
        name = name.Replace('\\', '/');
        return (name.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase) &&
                Path.GetFileNameWithoutExtension(name).Equals("Kalma.Infrastructure", StringComparison.OrdinalIgnoreCase)) ||
               name.Equals("Kalma.Infrastructure", StringComparison.OrdinalIgnoreCase) ||
               name.StartsWith("Kalma.Infrastructure.", StringComparison.OrdinalIgnoreCase);
    }

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
