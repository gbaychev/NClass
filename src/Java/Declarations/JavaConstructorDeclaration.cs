using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Java
{
    public class JavaConstructorDeclaration : IJavaConstructorDeclaration
    {
        // [<access>] <name>([<args>])
        const string ConstructorPattern =
            @"^\s*" + JavaLanguage.AccessPattern +
            @"(?<name>" + JavaLanguage.NamePattern + ")" +
            @"\((?(static)|(?<args>.*))\)" + JavaLanguage.DeclarationEnding;

        static readonly Regex constructorRegex =
            new Regex(ConstructorPattern, RegexOptions.ExplicitCapture);

        private readonly Match match;

        public JavaConstructorDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static JavaConstructorDeclaration Create(string declaration)
        {
            var match = constructorRegex.Match(declaration);
            return new JavaConstructorDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public string Type
        {
            get { return string.Empty; }
        }

        public AccessModifier AccessModifier
        {
            get { return JavaLanguage.Instance.TryParseAccessModifier(match.Groups["access"].Value); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return JavaArgumentListDeclaration.Create(match.Groups["args"].Value); }
        }

        public bool IsStatic
        {
            get { return match.Groups["static"].Success; }
        }
    }
}
