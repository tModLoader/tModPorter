using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tModPorter.Rewriters;

public class RenameRewriter : BaseRewriter
{
	private static List<(string type, string from, string to, bool isStatic)> fieldRenames = new();

	public static void RenameInstanceField(string type, string from, string to) => fieldRenames.Add((type, from, to, false));
	public static void RenameStaticField(string type, string from, string to) => fieldRenames.Add((type, from, to, true));

	public RenameRewriter(SemanticModel model) : base(model) { }

	public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node) {
		return node.Parent switch {
			MemberAccessExpressionSyntax memberAccess when node == memberAccess.Name && model.GetOperation(memberAccess) is IInvalidOperation =>
				Refactor(node, model.GetTypeInfo(memberAccess.Expression).Type),

			AssignmentExpressionSyntax assignment when assignment.Parent is InitializerExpressionSyntax && model.GetOperation(assignment) is IAssignmentOperation { Target: IInvalidOperation, Parent: var parent } =>
				Refactor(node, parent.Type),

			_ when model.GetOperation(node) is IInvalidOperation =>
				RefactorSpeculative(node),

			_ => base.VisitIdentifierName(node),
		};
	}

	private IdentifierNameSyntax RefactorSpeculative(IdentifierNameSyntax nameSyntax) {
		var nameToken = nameSyntax.Identifier;

		foreach (var (type, from, to, isStatic) in fieldRenames) {
			if (from != nameToken.Text)
				continue;

			var repl = nameSyntax.WithIdentifier(nameToken.WithText(to));
			var speculate = model.GetSpeculativeSymbolInfo(nameSyntax.SpanStart, repl, SpeculativeBindingOption.BindAsExpression);
			if (speculate.Symbol?.ContainingType?.ToString() == type)
				return repl;
		}

		return nameSyntax;
	}

	private IdentifierNameSyntax Refactor(IdentifierNameSyntax nameSyntax, ITypeSymbol instType) {
		if (instType == null)
			return nameSyntax;

		var nameToken = nameSyntax.Identifier;

		foreach (var (type, from, to, isStatic) in fieldRenames) {
			if (from != nameToken.Text || !instType.InheritsFrom(type))
				continue;

			return nameSyntax.WithIdentifier(nameToken.WithText(to));
		}

		return nameSyntax;
	}
}

