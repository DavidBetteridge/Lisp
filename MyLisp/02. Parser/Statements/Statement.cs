using System;

namespace MyLisp
{
    public abstract class Statement
    {
        public abstract NodeKind StatementNodeKind { get; }
    }
}