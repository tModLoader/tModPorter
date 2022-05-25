using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Operations;

namespace tModPorter.Rewriters;

public abstract class BaseRewriter : CSharpSyntaxRewriter
{
	protected SemanticModel model;

	public BaseRewriter(SemanticModel model) {
		this.model = model;
	}

	public override SyntaxToken VisitToken(SyntaxToken token) => token;

	protected bool IsCompileError(SyntaxNode node) => model.GetOperation(node) is IInvalidOperation;
}