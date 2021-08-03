﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using tModPorter.Rewriters;
using tModPorter.Rewriters.IdentifierRewriters;

namespace tModPorter.Tests.RewriterTests.SimpleIdentifierTests {
	public sealed class ItemIdentifierRewriterTest : BaseIdentifierTest
	{
		protected override SimpleIdentifierRewriter CreateIdentifierRewriter(SemanticModel model,
			HashSet<(BaseRewriter rewriter, SyntaxNode originalNode)> nodeSet, List<string>? usingList) =>
			new ItemIdentifierRewriter(model, usingList, nodeSet);
	}
}