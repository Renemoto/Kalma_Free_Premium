using System.Reflection;
using System.Xml.Linq;

namespace Kalma.Application.Tests;

public sealed class ArchitectureTests
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    [Theory]
    [InlineData("Kalma.Domain", "Kalma.Domain.csproj")]
    [InlineData("Kalma.Application", "Kalma.Application.csproj")]
    public void Core_projects_do_not_declare_forbidden_dependencies(string projectName, string projectFile)
    {
        var projectPath = Path.Combine(RepositoryRoot, "backend", "src", projectName, projectFile);
        var project = XDocument.Load(projectPath);
        var dependencies = project.Descendants()
            .Where(element => element.Name.LocalName is "PackageReference" or "FrameworkReference" or "ProjectReference" or "Reference")
            .Select(element => element.Attribute("Include")?.Value ?? string.Empty);

        Assert.Empty(FindForbiddenDependencies(projectName, dependencies));
    }

    [Theory]
    [InlineData("Kalma.Domain")]
    [InlineData("Kalma.Application")]
    public void Core_assemblies_do_not_reference_forbidden_assemblies(string assemblyName)
    {
        var assemblyPath = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.dll");
        var references = Assembly.LoadFrom(assemblyPath).GetReferencedAssemblies().Select(reference => reference.Name ?? string.Empty);
        Assert.Empty(FindForbiddenDependencies(assemblyName, references));
    }

    private static IEnumerable<string> FindForbiddenDependencies(string projectName, IEnumerable<string> dependencies) =>
        projectName == "Kalma.Domain"
            ? dependencies.Where(IsDomainDependencyForbidden)
            : dependencies.Where(IsInfrastructureDependency);

    private static bool IsDomainDependencyForbidden(string dependency) =>
        IsInfrastructureDependency(dependency)
        || dependency.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.OrdinalIgnoreCase)
        || dependency.StartsWith("Microsoft.AspNetCore", StringComparison.OrdinalIgnoreCase);

    private static bool IsInfrastructureDependency(string dependency) =>
        dependency.Contains("Kalma.Infrastructure", StringComparison.OrdinalIgnoreCase);

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "docs", "Kalma — Documento de arquitectura.md")))
            directory = directory.Parent;

        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate Kalma repository root.");
    }
}
