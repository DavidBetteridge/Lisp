using System.Collections.Immutable;

namespace MyLisp
{
    internal class CommandStatementSyntax : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private readonly SyntaxToken _command;
        private readonly SyntaxToken _endToken;
        private readonly SyntaxKind _kind;

        public ImmutableArray<StatementSyntax> Statements { get; }

        public override SyntaxKind Kind => _kind;

        public CommandStatementSyntax(SyntaxToken openToken, 
                                      SyntaxToken command, 
                                      ImmutableArray<StatementSyntax> statements, 
                                      SyntaxToken endToken,
                                      SyntaxKind kind)
        {
            _openToken = openToken;
            _command = command;
            Statements = statements;
            _endToken = endToken;
            _kind = kind;
        }
    }
}