using System;

namespace MyLisp
{
    public abstract class BoundStatement
    {
        public abstract BoundNodeKind BoundNodeKind { get; }
        public abstract Type Type { get; }
    }
}