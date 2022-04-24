using System.Linq;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.CSharp
{
    public class CSharpDestructorDeclaration : ICSharpDestructorDeclaration
    {
        // ~<name>()
        const string DestructorPattern =
            @"^\s*~(?<name>" + CSharpLanguage.NamePattern + ")" +
            @"\(\s*\)" + CSharpLanguage.DeclarationEnding;

        static readonly Regex destructorRegex =
            new Regex(DestructorPattern, RegexOptions.ExplicitCapture);

        private readonly Match match;

        public CSharpDestructorDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static CSharpDestructorDeclaration Create(string declaration)
        {
            var match = destructorRegex.Match(declaration);
            return new CSharpDestructorDeclaration(match);
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
            get { return AccessModifier.Default; }
        }

        public IArgumentListDeclaration<IParameterDeclaration> ArgumentList
        {
            get { return new CSharpArgumentListDeclaration(Enumerable.Empty<CSharpParameterDeclaration>()); }
        }
    }
}
