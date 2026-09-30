using System.Xml.Linq;

namespace Kalma.Domain.Tests;

public sealed class RepositoryFoundationTests
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void Repository_contains_expected_backend_and_test_projects()
    {
        var sourceProjects = new[]
        {
            "Kalma.Domain", "Kalma.Application", "Kalma.Infrastructure", "Kalma.Api"
        };
        var testProjects = new[]
        {
            "Kalma.Domain.Tests", "Kalma.Application.Tests", "Kalma.Infrastructure.IntegrationTests"
        };

        Assert.True(Directory.Exists(Path.Combine(RepositoryRoot, "backend")));
        Assert.True(Directory.Exists(Path.Combine(RepositoryRoot, "web")));
        Assert.True(Directory.Exists(Path.Combine(RepositoryRoot, "docs", "adr")));
        AssertProjectFiles("backend", "src", sourceProjects);
        AssertProjectFiles("backend", "tests", testProjects);

        var solution = File.ReadAllText(Path.Combine(RepositoryRoot, "backend", "Kalma.sln"));
        foreach (var name in sourceProjects.Concat(testProjects))
            Assert.Contains($"{name}\\{name}.csproj", solution, StringComparison.Ordinal);
    }

    [Fact]
    public void Projects_target_net8_and_reference_only_the_required_inward_projects()
    {
        AssertProject("backend/src/Kalma.Domain/Kalma.Domain.csproj", []);
        AssertProject("backend/src/Kalma.Application/Kalma.Application.csproj", ["../Kalma.Domain/Kalma.Domain.csproj"]);
        AssertProject("backend/src/Kalma.Infrastructure/Kalma.Infrastructure.csproj", ["../Kalma.Application/Kalma.Application.csproj"]);
        AssertProject("backend/src/Kalma.Api/Kalma.Api.csproj", [
            "../Kalma.Infrastructure/Kalma.Infrastructure.csproj",
            "../Kalma.Application/Kalma.Application.csproj"
        ]);
        AssertProject("backend/tests/Kalma.Domain.Tests/Kalma.Domain.Tests.csproj", ["../../src/Kalma.Domain/Kalma.Domain.csproj"], isTest: true);
        AssertProject("backend/tests/Kalma.Application.Tests/Kalma.Application.Tests.csproj", ["../../src/Kalma.Application/Kalma.Application.csproj"], isTest: true);
        AssertProject("backend/tests/Kalma.Infrastructure.IntegrationTests/Kalma.Infrastructure.IntegrationTests.csproj", ["../../src/Kalma.Infrastructure/Kalma.Infrastructure.csproj"], isTest: true);
    }

    [Fact]
    public void Architecture_document_is_an_exact_copy()
    {
        var authoritative = File.ReadAllBytes(Path.Combine(RepositoryRoot, "docs", "Kalma — Documento de arquitectura.md"));
        var copy = File.ReadAllBytes(Path.Combine(RepositoryRoot, "docs", "kalma-arquitectura.md"));

        Assert.Equal(authoritative, copy);
    }

    private static void AssertProjectFiles(string root, string group, IEnumerable<string> names)
    {
        foreach (var name in names)
            Assert.True(File.Exists(Path.Combine(RepositoryRoot, root, group, name, $"{name}.csproj")), $"Missing {group} project: {name}");
    }

    private static void AssertProject(string relativePath, string[] expectedReferences, bool isTest = false)
    {
        var project = XDocument.Load(Path.Combine(RepositoryRoot, relativePath));
        var properties = project.Descendants("PropertyGroup").SelectMany(group => group.Elements())
            .ToDictionary(element => element.Name.LocalName, element => element.Value);
        Assert.Equal("net8.0", properties["TargetFramework"]);
        Assert.Equal(isTest ? "true" : null, properties.GetValueOrDefault("IsTestProject"));

        var references = project.Descendants("ProjectReference")
            .Select(reference => reference.Attribute("Include")!.Value.Replace('\\', '/'))
            .OrderBy(reference => reference, StringComparer.Ordinal);
        Assert.Equal(expectedReferences.OrderBy(reference => reference, StringComparer.Ordinal), references);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "docs", "Kalma — Documento de arquitectura.md")))
            directory = directory.Parent;

        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate Kalma repository root.");
    }
}
