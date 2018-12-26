using System.Collections.Immutable;

namespace MyLisp
{
    internal class LineStatement : StatementSyntax
    {
        private readonly SyntaxToken _openToken;
        private ImmutableArray<StatementSyntax> _immutableArray;
        private readonly SyntaxToken _endToken;

        public SyntaxKind Command { get; }

        public LineStatement(SyntaxToken openToken, SyntaxKind command, ImmutableArray<StatementSyntax> immutableArray, SyntaxToken endToken)
        {
            _openToken = openToken;
            Command = command;
            _immutableArray = immutableArray;
            _endToken = endToken;
        }
    }
}