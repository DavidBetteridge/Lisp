using System.Collections.Immutable;

namespace MyLisp
{
    internal class MinusStatementSyntax : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private readonly SyntaxToken _command;
        private readonly SyntaxToken _endToken;

        public ImmutableArray<StatementSyntax> Statements { get; }

        public override SyntaxKind Kind => SyntaxKind.MinusCommand;

        public MinusStatementSyntax(SyntaxToken openToken, SyntaxToken command, ImmutableArray<StatementSyntax> statements, SyntaxToken endToken)
        {
            _openToken = openToken;
            _command = command;
            Statements = statements;
            _endToken = endToken;
        }
    }
}