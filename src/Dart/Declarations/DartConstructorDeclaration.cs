using System;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Dart
{
    public class DartConstructorDeclaration : IDartConstructorDeclaration
    {
        // [<access>] [static] <name>([<args>])
        private const string ConstructorPattern =
            @"^\s*" + DartLanguage.AccessPattern + @"(?<modifier>factory\s+)?" +
            @"(?<name>" + DartLanguage.NamePattern + ")" +
            @"(?<namedconstructor>" + DartLanguage.NamedConstructorPattern + ")";

        private static readonly Regex constructorRegex =
            new Regex(ConstructorPattern, RegexOptions.ExplicitCapture);

        private readonly Match match;

        public DartConstructorDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static DartConstructorDeclaration Create(string declaration)
        {
            var match = constructorRegex.Match(declaration);
            return new DartConstructorDeclaration(match);
        }

        public string Name
        {

            get
            {
                var name = match.Groups["name"].Value;
                var named = match.Groups["namedconstructor"].Value;
                return string.IsNullOrEmpty(named) ? name : name + named;
            }
        }

        public string Type
        {
            get { return string.Empty; }
        }

        public AccessModifier AccessModifier
        {
            get { return DartLanguage.Instance.TryParseAccessModifier(match.Groups["access"].Value); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return DartArgumentListDeclaration.Create(match.Groups["args"].Value); }
        }

        public bool IsStatic
        {
            get
            {
                return false;
            }
        }

        public bool IsFactory
        {
            get
            {
                if (match.Groups["modifier"].Success)
                {
                    return match.Groups["modifier"].Value == "factory ";
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
