namespace MyLisp
{
    public enum SyntaxKind
    {
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,

        BadToken,
        EndOfFileToken,

        OpenParenthesisToken,
        CloseParenthesisToken,

        NumberToken,
        WhitespaceToken,
        IdentifierToken,

        LetKeyword,
        PlusCommand,
        LiteralExpression,
        MinusCommand,
        DivideCommand,
        MultiplyCommand
    }
}
