using System;

namespace MyLisp
{
    class SyntaxFacts
    {
        internal static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "let":
                    return SyntaxKind.LetKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }

        internal static string GetText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return "+";
                case SyntaxKind.MinusToken:
                    return "-";
                case SyntaxKind.StarToken:
                    return "*";
                case SyntaxKind.SlashToken:
                    return "/";
                case SyntaxKind.OpenParenthesisToken:
                    return "(";
                case SyntaxKind.CloseParenthesisToken:
                    return ")";
                case SyntaxKind.LetKeyword:
                    return "let";
                default:
                    return null;
            }
        }
    }

}
