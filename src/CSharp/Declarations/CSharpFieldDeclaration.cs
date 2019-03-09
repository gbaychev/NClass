using System.Collections.Generic;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
	public class CSharpFieldDeclaration : ICSharpFieldDeclaration
	{
		const string ModifiersPattern = @"((?<modifier>static|readonly|const|new|volatile)\s+)*";
		const string InitValuePattern = @"(?<initvalue>[^\s;](.*[^\s;])?)";

		// [<access>] [<modifiers>] <type> <name> [= <initvalue>]
		const string FieldPattern =
			@"^\s*" + CSharpLanguage.AccessPattern + ModifiersPattern +
			@"(?<type>" + CSharpLanguage.GenericTypePattern2 + @")\s+" +
			@"(?<name>" + CSharpLanguage.NamePattern + @")" +
			@"(\s*=\s*" + InitValuePattern + ")?" + CSharpLanguage.DeclarationEnding;

		static readonly Regex fieldRegex = new Regex(FieldPattern, RegexOptions.ExplicitCapture);

		readonly Match match;
		readonly HashSet<string> modifiers;

		public CSharpFieldDeclaration(Match match)
		{
			if (!match.Success)
			{
				throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
			}

			this.match = match;
			this.modifiers = GetModifiers(match);
		}

		public static CSharpFieldDeclaration Create(string declaration)
		{
			var match = fieldRegex.Match(declaration);
			return new CSharpFieldDeclaration(match);
		}

		public string Name
		{
			get { return match.Groups["name"].Value; }
		}

		public string Type
		{
			get { return match.Groups["type"].Value; }
		}

		public AccessModifier AccessModifier
		{
			get { return ParseAccessModifier(match.Groups["access"].Value); }
		}

		public bool HasInitialValue
		{
			get { return match.Groups["initvalue"].Success; }
		}

		public string InitialValue
		{
			get { return match.Groups["initvalue"].Value; }
		}

		public bool IsStatic
		{
			get { return modifiers.Contains("static"); }
		}

		public bool IsReadonly
		{
			get { return modifiers.Contains("readonly"); }
		}

		public bool IsConstant
		{
			get { return modifiers.Contains("const"); }
		}

		public bool IsHider
		{
			get { return modifiers.Contains("new"); }
		}

		public bool IsVolatile
		{
			get { return modifiers.Contains("volatile"); }
		}

		private static AccessModifier ParseAccessModifier(string value)
		{
			return CSharpLanguage.Instance.TryParseAccessModifier(value);
		}

		private static HashSet<string> GetModifiers(Match match)
		{
			var captures = match.Groups["modifier"].Captures;
			var modifiers = new HashSet<string>();

			foreach (Capture capture in captures)
			{
				modifiers.Add(capture.Value);
			}

			return modifiers;
		}
	}
}