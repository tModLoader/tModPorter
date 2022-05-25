#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using UtfUnknown;
using static System.Console;

namespace tModPorter;

public class tModPorter {
	private int _totalDocuments;
	private int _processedDocuments;

	public async Task ProcessProject(string projectPath, int? maxThreads = null, IProgress<double>? progressReporter = null) {
		MSBuildLocator.RegisterDefaults();

		using MSBuildWorkspace workspace = MSBuildWorkspace.Create();
		workspace.WorkspaceFailed += (o, e) => {
			if (e.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
				throw new Exception(e.Diagnostic.ToString());

			Error.WriteLine(e.Diagnostic.ToString());
		};

		WriteLine($"Loading project: {projectPath}");
		// Attach progress reporter so we print projects as they are loaded.
		Project project = await workspace.OpenProjectAsync(projectPath, new ConsoleProgressReporter());

		WriteLine();
		_totalDocuments = project.Documents.Count();

		int numChunks = Math.Min(maxThreads ?? Environment.TickCount, _totalDocuments);
		int i = 0;
		IEnumerable<IEnumerable<Document>> chunks =
			from document in project.Documents
			group document by i++ % numChunks
			into part
			select part.AsEnumerable();

		List<Task> tasks = chunks.Select(chunk => Task.Run(() => ProcessChunk(chunk, progressReporter))).ToList();

		await Task.WhenAll(tasks);
	}

	private double UpdateProgress() {
		int processedDocs = Interlocked.Add(ref _processedDocuments, 1);
		return (double) processedDocs / _totalDocuments;
	}

	private async Task ProcessChunk(IEnumerable<Document> chunk, IProgress<double>? progress) {
		foreach (Document document in chunk)
			await ProcessFile(document, progress);
	}

	private async Task ProcessFile(Document doc, IProgress<double>? progress) {
		if (await Rewrite(doc) is Document newDoc) {
			await Update(newDoc);
		}

		progress?.Report(UpdateProgress());
	}

	private static async Task Update(Document doc) {
		var path = doc.FilePath ?? throw new NullReferenceException("No path? " + doc?.Name);

		Encoding encoding;
		using (Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
			DetectionResult detectionResult = CharsetDetector.DetectFromStream(fs);
			encoding = detectionResult.Detected.Encoding;
			if (detectionResult.Detected.Confidence < .95f)
				WriteLine($"\rLess than 95% confidence about the file encoding of: {doc.FilePath}");
		}

		int i = 2;
		string backupPath = $"{path}.bak";
		while (File.Exists(backupPath)) {
			backupPath = $"{path}.bak{i++}";
		}
		File.Move(path, backupPath);

		await File.WriteAllTextAsync(path, (await doc.GetTextAsync()).ToString(), encoding);
		WriteLine("\rUpdated: " + doc.Name);
	}

	public static async Task<Document?> Rewrite(Document doc) {
		var originalDoc = doc;
		while (true) {
			var docBeforePass = doc;

			foreach (var rewriter in Config.CreateRewriters()) {
				doc = await rewriter.Rewrite(doc);
			}

			if (doc == docBeforePass)
				break;
		}

		return doc == originalDoc ? null : doc;
	}

	private class ConsoleProgressReporter : IProgress<ProjectLoadProgress> {
		public void Report(ProjectLoadProgress loadProgress) {
			string? projectDisplay = Path.GetFileName(loadProgress.FilePath);
			if (loadProgress.TargetFramework != null) projectDisplay += $" ({loadProgress.TargetFramework})";

			WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
		}
	}
}