using System.Xml.Linq;

namespace Kalma.Application.Tests;

public sealed class ProjectReferenceTests
{
    [Fact]
    public void Test_project_declares_its_application_project_reference()
    {
        var project = XDocument.Load(Path.Combine(FindRoot(), "backend", "tests", "Kalma.Application.Tests", "Kalma.Application.Tests.csproj"));

        Assert.Contains(project.Descendants("ProjectReference"), reference =>
            reference.Attribute("Include")?.Value == "../../src/Kalma.Application/Kalma.Application.csproj");
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "docs", "Kalma — Documento de arquitectura.md")))
            directory = directory.Parent;
        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate Kalma repository root.");
    }
}
