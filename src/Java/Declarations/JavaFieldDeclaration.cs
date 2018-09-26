using System.Collections.Generic;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
	public class JavaFieldDeclaration : IJavaFieldDeclaration
	{
		const string ModifiersPattern = @"((?<modifier>static|final|volatile)\s+)*";
		const string InitValuePattern = @"(?<initvalue>[^\s;](.*[^\s;])?)";

		// [<access>] [<modifiers>] <type> <name> [= <initvalue>]
		const string FieldPattern =
			@"^\s*" + JavaLanguage.AccessPattern + ModifiersPattern +
			@"(?<type>" + JavaLanguage.GenericTypePattern2 + @")\s+" +
			@"(?<name>" + JavaLanguage.NamePattern + @")" +
			@"(\s*=\s*" + InitValuePattern + ")?" + JavaLanguage.DeclarationEnding;

		static readonly Regex fieldRegex = new Regex(FieldPattern, RegexOptions.ExplicitCapture);

		readonly Match match;
		readonly HashSet<string> modifiers;

		public JavaFieldDeclaration(Match match)
		{
			if (!match.Success)
			{
				throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
			}

			this.match = match;
			this.modifiers = GetModifiers(match);
		}

		public static JavaFieldDeclaration Create(string declaration)
		{
			var match = fieldRegex.Match(declaration);
			return new JavaFieldDeclaration(match);
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
	
		public bool IsVolatile
		{
			get { return modifiers.Contains("volatile"); }
		}

		private static AccessModifier ParseAccessModifier(string value)
		{
			return JavaLanguage.Instance.TryParseAccessModifier(value);
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