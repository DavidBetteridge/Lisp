﻿using System.Collections.Generic;

namespace MyLisp
{
    public class Error
    {
        public string Message { get;  }

        public Error(string message)
        {
            Message = message;
        }
    }
}