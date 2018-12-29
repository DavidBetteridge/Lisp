using System;

namespace MyLisp
{
    internal abstract class BoundStatement
    {
        public abstract BoundNodeKind BoundNodeKind { get; }
        public abstract Type Type { get; }
    }
}