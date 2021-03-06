﻿namespace MyLisp
{
    public enum NodeKind
    {
        Literal = 0,
        PlusCommand = 1,
        MinusCommand = 2,
        MultiplyCommand = 3,
        DivideCommand = 4,
        OnePlusCommand = 5,
        OneMinusCommand = 6,
        DividendDivisorCommand = 7,
        ModCommand = 8,
        DefVarCommand = 9,
        Identifier = 10,
        DefFunCommand = 11,
        FunctionCall = 12,
        Empty = 13,
        IfCommand = 14,
        String = 15,
        Integer = 16,
        FloatingPoint = 17
    }
}