using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Dart
{
    public class DartParameterDeclaration : IDartParameterDeclaration
    {
        // [this] [<modifiers>] <type> <name> [,]
        const string ParameterPattern =
            @"(?<this>this)?\s*" +
            @"(?<type>" + DartLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + DartLanguage.NamePattern + @")" +
            @"(\s*=\s*(?<defval>([^,""]+|""(\\""|[^""])*"")))?";

        private static readonly Regex parameterRegex =
            new Regex("^" + ParameterPattern + "$", RegexOptions.ExplicitCapture);

        private readonly Match match;

        public DartParameterDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidParameterDeclaration);
            }

            this.match = match;
        }

        public static DartParameterDeclaration Create(string declaration)
        {
            var match = parameterRegex.Match(declaration);
            return new DartParameterDeclaration(match);
        }

        public string Name
        {
            get { return match.Groups["name"].Value; }
        }

        public string Type
        {
            get { return match.Groups["type"].Value; }
        }

        public ParameterModifier Modifier
        {
            get { return ParseParameterModifier(match.Groups["modifier"].Value); }
        }

        public bool HasDefaultValue
        {
            get { return match.Groups["defval"].Success; }
        }

        public string DefaultValue
        {
            get { return match.Groups["defval"].Value; }
        }

        private static ParameterModifier ParseParameterModifier(string modifierString)
        {
            switch (modifierString)
            {
                case "ref":
                    return ParameterModifier.Inout;

                case "out":
                    return ParameterModifier.Out;

                case "params":
                    return ParameterModifier.Params;

                default:
                    return ParameterModifier.In;
            }
        }
    }
}
