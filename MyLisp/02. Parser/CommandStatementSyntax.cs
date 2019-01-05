using System.Collections.Immutable;

namespace MyLisp
{
    internal class CommandStatementSyntax : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private readonly SyntaxToken _endToken;
        private readonly CommandKind _kind;

        public ImmutableArray<StatementSyntax> Statements { get; }

        public override CommandKind Kind => _kind;

        public SyntaxToken Command { get; }

        public CommandStatementSyntax(SyntaxToken openToken, 
                                      SyntaxToken command, 
                                      ImmutableArray<StatementSyntax> statements, 
                                      SyntaxToken endToken,
                                      CommandKind kind)
        {
            _openToken = openToken;
            Command = command;
            Statements = statements;
            _endToken = endToken;
            _kind = kind;
        }
    }
}