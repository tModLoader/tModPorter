using CodeChicken.DiffPatch;
using NUnit.Framework;
using System;
using System.Linq;

public static class FileAssert
{
	public static void Equal(string expected, string actual)
    {
		var aLines = actual.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		var eLines = expected.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
		var patches = new PatienceDiffer().MakePatches(aLines, eLines);
		if (!patches.Any()) return;

		Assert.Fail("Expected (+) vs Actual (-) patches\r\n" +
			string.Join("\r\n\r\n", patches));
	}
}
