using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    class Source
    {
        public const char EOF = unchecked((char)(-1));


        private readonly string _inputText;

        private int _index  = 0;
        private int _line   = 0;
        private int _column = 0;


        public SourceLocation Location
        {
            get { return new SourceLocation(_index, _line, _column); }
        }

        public bool IsAtEof
        {
            get { return _index >= _inputText.Length; }
        }


        public Source(string inputText)
        {
            _inputText = inputText;
        }


        public char LookAhead(int amount)
        {
            int realIndex = _index + amount;

            if (0 <= realIndex && realIndex < _inputText.Length)
            {
                return _inputText[realIndex];
            }

            return EOF;
        }

        public void Consume(int amount)
        {
            Debug.Assert(amount > 0);

            for (int i = 0;
                 i < amount && !IsEof(_index + i);
                 i++)
            {
                int loc = _index + i;

                if (!IsEof(loc+1) &&
                    _inputText[loc] == '\r' && _inputText[loc+1] == '\n')
                {
                    // Windows newline CRLF
                    if (i + 1 < amount)
                    {
                        // advance past it if we are not set to stop in the middle of the CRLF.
                        i++;
                    }

                    _line++;
                    _column = 0;
                }
                else if (!IsEof(loc-1) && i-1 >= 0 &&
                         _inputText[loc-1] == '\r' && _inputText[loc] == '\n')
                {
                    // Windows newline, but we started after the CR
                }
                else if (_inputText[loc] == '\r' || _inputText[loc] == '\n')
                {
                    // Other newlines.
                    _line++;
                    _column = 0;
                }
                else
                {
                    _column++;
                }
            }

            _index += amount;
        }

        private bool IsEof(int location)
        {
            return location < 0 || _inputText.Length <= location;
        }
    }
}
