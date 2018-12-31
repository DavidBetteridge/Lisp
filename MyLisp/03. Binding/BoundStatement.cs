using System;

namespace MyLisp
{
    public abstract class BoundStatement
    {
        public abstract BoundNodeKind BoundNodeKind { get; }
    }
}