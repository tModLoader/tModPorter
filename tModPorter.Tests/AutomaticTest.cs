using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using tModPorter.Rewriters;
using NUnit.Framework;

namespace tModPorter.Tests;

// TODO: Make test using tModPorter class
public class AutomaticTest {
	public static string TestModPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../TestData/TestData.csproj"));
	private static VisualStudioInstance instance = MSBuildLocator.RegisterDefaults();

	private static Compilation? _compilation;
	private static Project? _project;

	[OneTimeSetUp]
	public async Task Setup() {
		await LoadProject();
	}

	[TestCaseSource(nameof(GetTestCases))]
	public async Task RewriteCode(Document doc) {
		SyntaxTree tree = await doc.GetSyntaxTreeAsync() ?? throw new NullReferenceException("Node has no syntax tree");
		SemanticModel model = _compilation!.GetSemanticModel(tree);
		SyntaxNode rootNode = tree.GetRoot();

		CompilationUnitSyntax result = RewriteCodeOnce(doc, model, rootNode);

		string fixedFilePath = Path.ChangeExtension(tree.FilePath, ".Fix.cs");

		Assert.True(File.Exists(fixedFilePath), $"File '{fixedFilePath}' doesn't exist.");
		string fixedContent = File.ReadAllText(fixedFilePath);

		//File.WriteAllText(Path.ChangeExtension(tree.FilePath, ".Out.cs"), result.ToFullString());
		FileAssert.Equal(fixedContent, result.ToFullString());
	}

	[Test]
	public async Task FixedModCompiles() {
		using MSBuildWorkspace workspace = MSBuildWorkspace.Create();

		var proj = await workspace.OpenProjectAsync(TestModPath[..^".csproj".Length] + "Fixed.csproj");
		var comp = (await proj.GetCompilationAsync())!;
		using var peStream = new MemoryStream();
		var result = comp.Emit(peStream);
		foreach (var diag in result.Diagnostics) {
			if (diag.Severity == DiagnosticSeverity.Error)
				TestContext.Error.WriteLine(diag);
			else
				TestContext.Out.WriteLine(diag);
		}

		if (!result.Success) {
			Assert.Fail("Compilation Failed");
		}
	}
	
	[TestCaseSource(nameof(GetTestCases))]
	public async Task RewriteCodeTwice(Document doc) {
		SyntaxTree tree = await doc.GetSyntaxTreeAsync() ?? throw new NullReferenceException("Node has no syntax tree");
		SemanticModel model = _compilation!.GetSemanticModel(tree);
		SyntaxNode rootNode = await tree.GetRootAsync();

		CompilationUnitSyntax result = RewriteCodeOnce(doc, model, rootNode);
		
		// Write the rewritten file to disk, so that we can then load it again with the .csproj
		doc = doc.WithSyntaxRoot(result);
		tree = await doc.GetSyntaxTreeAsync() ?? throw new NullReferenceException("Node has no syntax tree");
		Compilation? newCompilation = await doc.Project.GetCompilationAsync();
		Assert.NotNull(newCompilation);
		model = newCompilation!.GetSemanticModel(tree);
		rootNode = await tree.GetRootAsync();

		result = RewriteCodeOnce(doc, model, rootNode);

		string fixedFilePath = Path.ChangeExtension(tree.FilePath, ".Fix.cs");

		Assert.True(File.Exists(fixedFilePath), $"File '{fixedFilePath}' doesn't exist.");
		string fixedContent = await File.ReadAllTextAsync(fixedFilePath);

		FileAssert.Equal(fixedContent, result.ToFullString());
	}
	
	private static CompilationUnitSyntax RewriteCodeOnce(Document document, SemanticModel model, SyntaxNode rootNode) {
		MainRewriter rewriter = new(document, model);
		rewriter.Visit(rootNode);
		CompilationUnitSyntax? result = rewriter.RewriteNodes(rootNode) as CompilationUnitSyntax;

		Assert.NotNull(result);
		return rewriter.AddUsingDirectives(result);
	}

	private static async Task LoadProject(bool force = false) {
		if (_project is not null && _compilation is not null) return;

		using MSBuildWorkspace workspace = MSBuildWorkspace.Create();

		if (!File.Exists(TestModPath)) {
			throw new FileNotFoundException("TestData.csproj not found.");
		}

		_project = await workspace.OpenProjectAsync(TestModPath);
		_compilation = await _project.GetCompilationAsync();
		if (_compilation is null) throw new NullReferenceException(nameof(_compilation));
	}

	public static IEnumerable<TestCaseData> GetTestCases() {
		LoadProject().GetAwaiter().GetResult();
		return _project!.Documents
			.Where(d => FilterTestCasePath(d.FilePath!.Replace('\\', '/')))
			.Select(d => new TestCaseData(d).SetArgDisplayNames(Path.GetFileName(d.FilePath)!))
			.ToArray();
	}

	private static bool FilterTestCasePath(string path) => !path.Contains("/Common/") && !path.Contains("/obj/");
}