using System;

namespace MyLisp
{
    internal class DiagnosticBag
    {
        public DiagnosticBag()
        {
        }

        internal void ReportBadCharacter(int position, char current)
        {
            throw new NotImplementedException();
        }

        internal void ReportInvalidNumber(string v, string text, Type type)
        {
            throw new NotImplementedException();
        }
    }
}