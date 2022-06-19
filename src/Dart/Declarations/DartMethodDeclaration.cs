using NClass.Core;
using NClass.Translations;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NClass.Dart
{
    public class DartMethodDeclaration : IDartMethodDeclaration
    {
        const string OverloadableOperators =
            @"(\+|-|\*|/|%|&|\||\^|!|~|\+\+|--|<<|>>|==|!=|>|<|>=|<=)";

        // operator <operator>
        const string OperatorPattern = @"operator\s*(?<operator>" + OverloadableOperators + ")";

        // {implicit | explicit} operator <operator>
        const string ConvOperatorPattern = @"(?<convop>implicit|explicit)" + @"\s+operator\s+(?<operator>" + DartLanguage.GenericTypePattern2 + ")";

        // [<access>] [<modifier>]
        // { {<type>} operator <operator>(<args>) | <type> <name>(<args>) | <type> get <name> | set <name>(<args>)}
        const string MethodPattern =
            @"^\s*" + DartLanguage.AccessPattern + DartLanguage.OperationModifiersPattern +
            @"((?<name>" + ConvOperatorPattern + ")|" +
            @"(?<type>" + DartLanguage.GenericTypePattern2 + @")\s+" +
            @"(?<name>" + OperatorPattern + "|" + DartLanguage.GenericOperationNamePattern + "))" +
            @"\s*\((?<args>.*)\)" + DartLanguage.DeclarationEnding;

        private static readonly Regex methodRegex = new Regex(MethodPattern, RegexOptions.ExplicitCapture);
        private static readonly Regex getRegex = new Regex(DartLanguage.GetPattern);
        private static readonly Regex setRegex = new Regex(DartLanguage.SetPattern);
        readonly Match match;
        readonly HashSet<string> modifiers;

        public DartMethodDeclaration(Match match)
        {
            if (!match.Success)
            {
                throw new BadSyntaxException(Strings.ErrorInvalidDeclaration);
            }

            this.match = match;
            this.modifiers = GetModifiers(match);
        }

        public static DartMethodDeclaration Create(string declaration)
        {
            var match = methodRegex.Match(declaration);
            if (match.Success)
                return new DartMethodDeclaration(match);
            match = getRegex.Match(declaration);
            if (match.Success)
                return new DartMethodDeclaration(match);
            match = setRegex.Match(declaration);
            if (match.Success)
                return new DartMethodDeclaration(match);
            return null;
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
            get { return DartArgumentListDeclaration.Create(match.Groups["args"].Value); }
        }

        public bool IsOperator
        {
            get { return match.Groups["operator"].Success; }
        }

        public bool IsConversionOperator
        {
            get { return match.Groups["convop"].Success; }
        }

        public bool IsExplicitImplementation
        {
            get { return match.Groups["namedot"].Success; }
        }

        public bool IsVirtual
        {
            get { return modifiers.Contains("virtual"); }
        }

        public bool IsAbstract
        {
            get { return modifiers.Contains("abstract"); }
        }

        public bool IsOverride
        {
            get { return modifiers.Contains("override"); }
        }

        public bool IsSealed
        {
            get { return modifiers.Contains("sealed"); }
        }

        public bool IsStatic
        {
            get { return modifiers.Contains("static"); }
        }

        public bool IsHider
        {
            get { return modifiers.Contains("new"); }
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
