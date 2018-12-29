using System.Collections.Immutable;

namespace MyLisp
{
    internal class PlusStatementSyntax : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private readonly SyntaxToken _command;
        private readonly SyntaxToken _endToken;

        public ImmutableArray<StatementSyntax> Statements { get; }

        public override SyntaxKind Kind => SyntaxKind.PlusCommand;

        public PlusStatementSyntax(SyntaxToken openToken, SyntaxToken command, ImmutableArray<StatementSyntax> statements, SyntaxToken endToken)
        {
            _openToken = openToken;
            _command = command;
            Statements = statements;
            _endToken = endToken;
        }
    }
}