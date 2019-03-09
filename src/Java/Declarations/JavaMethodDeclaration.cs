using System.Collections.Generic;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
	public class JavaMethodDeclaration : IJavaMethodDeclaration
	{
		// [<access>] [<modifiers>] <type> <name>(<args>)
		const string MethodPattern =
			@"^\s*" + JavaLanguage.AccessPattern + JavaLanguage.OperationModifiersPattern +
			@"(?<type>" + JavaLanguage.GenericTypePattern2 + @")\s+" +
			@"(?<name>" + JavaLanguage.GenericNamePattern + ")" +
			@"\((?<args>.*)\)" + JavaLanguage.DeclarationEnding;

		static readonly Regex methodRegex = new Regex(MethodPattern, RegexOptions.ExplicitCapture);

		readonly Match match;
		readonly HashSet<string> modifiers;

		public JavaMethodDeclaration(Match match)
		{
			if (!match.Success)
			{
				throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
			}

			this.match = match;
			this.modifiers = GetModifiers(match);
		}

		public static JavaMethodDeclaration Create(string declaration)
		{
			var match = methodRegex.Match(declaration);
			return new JavaMethodDeclaration(match);
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

		public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
		{
			get { return JavaArgumentListDeclaration.Create(match.Groups["args"].Value); }
		}

		public bool IsAbstract
		{
			get { return modifiers.Contains("abstract"); }
		}

		public bool IsSealed
		{
			get { return modifiers.Contains("sealed"); }
		}

		public bool IsStatic
		{
			get { return modifiers.Contains("static"); }
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