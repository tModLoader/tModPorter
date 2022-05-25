using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace tModPorter.Rewriters;

public class RenameRewriter : BaseRewriter
{
	private static List<(string type, string from, string to, bool isStatic)> fieldRenames = new();

	public static void RenameInstanceField(string type, string from, string to) => fieldRenames.Add((type, from, to, false));
	public static void RenameStaticField(string type, string from, string to) => fieldRenames.Add((type, from, to, true));

	public RenameRewriter(SemanticModel model) : base(model) { }

	public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node) {
		if (Refactor(node, node) is IdentifierNameSyntax repl) {
			return repl;
		}

		return base.VisitIdentifierName(node);
	}

	public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node) {
		if (node.Name is IdentifierNameSyntax name && Refactor(node, name) is MemberAccessExpressionSyntax repl) {
			return repl;
		}

		return base.VisitMemberAccessExpression(node);
	}

	private SyntaxNode Refactor(SyntaxNode expr, IdentifierNameSyntax name) {
		if (!IsCompileError(expr))
			return null;

		foreach (var (type, from, to, isStatic) in fieldRenames) {
			if (from != name.Identifier.Text) continue;

			var repl = expr.ReplaceToken(name.Identifier, name.Identifier.WithText(to));
			var speculate = model.GetSpeculativeSymbolInfo(name.SpanStart, repl, SpeculativeBindingOption.BindAsExpression);
			if (speculate.Symbol?.ContainingType?.ToString() == type)
				return repl;
		}

		return null;
	}
}

