using System.Collections.Generic;
using System.Text.RegularExpressions;
using NClass.Core;
using NClass.Translations;

namespace NClass.Dart
{
    public class DartFieldDeclaration : IDartFieldDeclaration
    {
        const string ModifiersPattern = @"((?<modifier>static|get|set|const)\s+)*";
        const string InitValuePattern = @"(?<initvalue>[^\s;](.*[^\s;])?)";

        // [<access>] [<modifiers>] <type> <name> [= <initvalue>]
        const string FieldPattern =
            @"^\s*" + DartLanguage.AccessPattern + ModifiersPattern +
            @"(?<type>" + DartLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + DartLanguage.NamePattern + @")" +
            @"(\s*=\s*" + InitValuePattern + ")?" + DartLanguage.DeclarationEnding;

        private static readonly Regex fieldRegex = new Regex(FieldPattern, RegexOptions.ExplicitCapture);

        readonly Match match;
        readonly HashSet<string> modifiers;

        public DartFieldDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
            this.modifiers = GetModifiers(match);
        }

        public static DartFieldDeclaration Create(string declaration)
        {
            var match = fieldRegex.Match(declaration);
            return new DartFieldDeclaration(match);
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

        public bool IsHider
        {
            get { return false; }
        }

        public bool IsReadonly
        {
            get { return false; }
        }

        public bool IsVolatile
        {
            get { return false; }
        }

        public bool IsStatic
        {
            get { return modifiers.Contains("static"); }
        }

        public bool IsGetter
        {
            get { return modifiers.Contains("get"); }
        }

        public bool IsConstant
        {
            get { return modifiers.Contains("const"); }
        }

        public bool IsSetter
        {
            get { return modifiers.Contains("set"); }
        }

 

        private static AccessModifier ParseAccessModifier(string value)
        {
            return DartLanguage.Instance.TryParseAccessModifier(value);
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
