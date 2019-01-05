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
        IdentifierToken,
        StringToken,
        PercentToken,
        QuoteToken,
        OnePlusToken,
        OneMinusToken,
    }

    public enum CommandKind
    {
        PlusCommand,
        LiteralExpression,
        MinusCommand,
        DivideCommand,
        MultiplyCommand,
        OnePlusCommand,
        OneMinusCommand,
        DividendDivisorCommand,
        ModCommand,
        DefVarCommand,
        IdentifierExpression,
        DefFunCommand,
        FunctionCall
    }
}
