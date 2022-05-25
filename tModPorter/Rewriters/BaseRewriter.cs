using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace tModPorter.Rewriters;

public abstract class BaseRewriter : CSharpSyntaxRewriter
{
	protected SemanticModel model;

	public BaseRewriter(SemanticModel model) {
		this.model = model;
	}

	public override SyntaxToken VisitToken(SyntaxToken token) => token;
}