using System.Collections.Immutable;

namespace MyLisp
{
    internal class OnePlusStatementSyntax : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private readonly SyntaxToken _command;
        private readonly SyntaxToken _endToken;

        public StatementSyntax Statement { get; }

        public override SyntaxKind Kind => SyntaxKind.OnePlusCommand;

        public OnePlusStatementSyntax(SyntaxToken openToken, SyntaxToken command, StatementSyntax statement, SyntaxToken endToken)
        {
            _openToken = openToken;
            _command = command;
            Statement = statement;
            _endToken = endToken;
        }
    }
}