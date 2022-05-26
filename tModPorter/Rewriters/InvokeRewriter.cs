using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tModPorter.Rewriters;

public class InvokeRewriter : BaseRewriter {
	public delegate SyntaxNode RewriteInvoke(InvocationExpressionSyntax invoke, SyntaxToken methodName);

	private static List<(string type, string name, bool isStatic, RewriteInvoke handler)> handlers = new();

	public static void RefactorInstanceMethodCall(string type, string name, RewriteInvoke handler) => handlers.Add((type, name, isStatic: false, handler));

	public static void RefactorStaticMethodCall(string type, string name, RewriteInvoke handler) => handlers.Add((type, name, isStatic: true, handler));

	public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node) {
		node = (InvocationExpressionSyntax)base.VisitInvocationExpression(node); // fix arguments and expression first

		if (model.GetOperation(node) is not IInvalidOperation op || node.ArgumentList.Arguments.Any(arg => model.GetOperation(arg) is IInvalidOperation))
			return node;

		switch (node.Expression) {
			case MemberAccessExpressionSyntax memberAccess:
				return Refactor(node, memberAccess.Name, model.GetTypeInfo(memberAccess.Expression).Type);

			case MemberBindingExpressionSyntax memberBinding:
				return Refactor(node, memberBinding.Name, ((IConditionalAccessInstanceOperation)op.Children.First()).Type);

			case IdentifierNameSyntax name:
				return RefactorViaLookup(node, name);
		}

		return node;
	}

	private static SyntaxNode Refactor(InvocationExpressionSyntax node, SimpleNameSyntax nameSyntax, ITypeSymbol targetType) {
		var nameToken = nameSyntax.Identifier;
		foreach (var (type, name, isStatic, handler) in handlers) {
			if (name != nameToken.Text || !targetType.InheritsFrom(type))
				continue;

			return handler(node, nameToken);
		}

		return node;
	}

	private SyntaxNode RefactorViaLookup(InvocationExpressionSyntax node, IdentifierNameSyntax nameSyntax) {
		// this won't work if all the static members of a type have been removed, but it's a nice way to handle both the static context from inheritance heirachy, and from using statements.
		INamedTypeSymbol[] _staticTypesWithMembersInCtx = null;
		INamedTypeSymbol[] GetStaticallyLocalTypes() => _staticTypesWithMembersInCtx ??= model
			.LookupStaticMembers(node.SpanStart)
			.OfType<IMethodSymbol>()
			.Select(m => m.ContainingType)
			.Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default)
			.ToArray();

		var enclosingMethod = model.GetEnclosingSymbol(node.SpanStart);
		var enclosingType = enclosingMethod.ContainingType;


		var nameToken = nameSyntax.Identifier;
		foreach (var (type, name, isStatic, handler) in handlers) {
			if (name != nameToken.Text)
				continue;

			if (isStatic ? !GetStaticallyLocalTypes().Any(t => t.InheritsFrom(type)) : enclosingMethod.IsStatic || !enclosingType.InheritsFrom(type))
				continue;

			return handler(node, nameToken);
		}

		return node;
	}

	#region Handlers
	public static RewriteInvoke ToFindTypeCall(string type) => (invoke, methodName) => {
		// TODO: we should replace the entire NameSyntax with a GenericName, to avoid breaking the tree, rather than making an invalid IdentifierNameSyntax
		// might be a problem for recursive calls
		invoke = invoke.ReplaceToken(methodName, methodName.WithText($"Find<{type}>"));
		return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, invoke.WithoutTrivia(), SyntaxFactory.IdentifierName("Type")).WithTriviaFrom(invoke);
	};
	#endregion
}

