using System.Xml.Linq;

namespace Kalma.Infrastructure.IntegrationTests;

public sealed class ProjectReferenceTests
{
    [Fact]
    public void Integration_test_project_declares_its_infrastructure_project_reference()
    {
        var project = XDocument.Load(Path.Combine(FindRoot(), "backend", "tests", "Kalma.Infrastructure.IntegrationTests", "Kalma.Infrastructure.IntegrationTests.csproj"));

        Assert.Contains(project.Descendants("ProjectReference"), reference =>
            reference.Attribute("Include")?.Value == "../../src/Kalma.Infrastructure/Kalma.Infrastructure.csproj");
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "docs", "Kalma — Documento de arquitectura.md")))
            directory = directory.Parent;
        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate Kalma repository root.");
    }
}
