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

        IntegerNumberToken,
        FloatingPointNumberToken,
        WhitespaceToken,
        IdentifierToken,

        LetKeyword,
        PlusCommand,
        LiteralExpression,
        MinusCommand,
        DivideCommand,
        MultiplyCommand,
        OnePlusToken,
        OneMinusToken,
        OnePlusCommand,
        OneMinusCommand,
        PercentToken,
        DividendDivisorCommand,
        ModKeyword,
        ModCommand,
        DefVarKeyword,
        DefVarCommand,
        IdentifierExpression,
        DefFunKeyword,
        DefFunCommand,
        FunctionCall
    }
}
