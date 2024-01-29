using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    public class CSharpConstructorDeclaration : ICSharpConstructorDeclaration
    {
        // [<access>] [static] <name>[<T>]([<args>])
        const string ConstructorPattern =
            @"^\s*" + CSharpLanguage.AccessPattern + @"(?<static>static\s+)?" +
            @"(?<name>" + CSharpLanguage.NamePattern + ")" +
            @"(<\w+>)?" +
            @"\((?(static)|(?<args>.*))\)" + CSharpLanguage.DeclarationEnding;

        static readonly Regex constructorRegex =
            new Regex(ConstructorPattern, RegexOptions.ExplicitCapture);

        private readonly Match match;

        public CSharpConstructorDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static CSharpConstructorDeclaration Create(string declaration)
        {
            var match = constructorRegex.Match(declaration);
            return new CSharpConstructorDeclaration(match);
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
            get { return CSharpLanguage.Instance.TryParseAccessModifier(match.Groups["access"].Value); }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return CSharpArgumentListDeclaration.Create(match.Groups["args"].Value); }
        }

        public bool IsStatic
        {
            get { return match.Groups["static"].Success; }
        }
    }
}
