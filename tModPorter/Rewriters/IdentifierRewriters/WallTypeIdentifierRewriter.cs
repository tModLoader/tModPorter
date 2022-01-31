﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace tModPorter.Rewriters.IdentifierRewriters
{
	public class WallTypeIdentifierRewriter : SimpleIdentifierRewriter
	{
		public WallTypeIdentifierRewriter(SemanticModel model, List<string> usingList,
			HashSet<(BaseRewriter rewriter, SyntaxNode originalNode)> nodesToRewrite,
			HashSet<(BaseRewriter rewriter, SyntaxToken originalToken)> tokensToRewrite)
			: base(model, usingList, nodesToRewrite, tokensToRewrite)
		{ }

		public override string OldIdentifier => "wall";
		public override string NewIdentifier => "WallType";
	}
}