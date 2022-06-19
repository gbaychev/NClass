using NClass.Core;
using NClass.Translations;
using System.Text.RegularExpressions;

namespace NClass.Dart
{
    public class DartPropertyNameDeclaration
    {
        const string PropertyNamePattern =
            @"^\s*(?<name>" + DartLanguage.GenericOperationNamePattern + @")\s*$";

        private static readonly Regex nameRegex = new Regex(PropertyNamePattern, RegexOptions.ExplicitCapture);

        readonly Match match;

        public DartPropertyNameDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
        }

        public static DartPropertyNameDeclaration Create(string declaration)
        {
            var match = nameRegex.Match(declaration);
            return new DartPropertyNameDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }
    }
}
