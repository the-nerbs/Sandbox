using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    class Lexer
    {
        private readonly IReadOnlyDictionary<TokenType, string[]> TokenTexts =
            new Dictionary<TokenType, string[]>
            {
                [TokenType.HKeyClassesRoot]     = new[] { "HKEY_CLASSES_ROOT", "HKCR" },
                [TokenType.HKeyCurrentUser]     = new[] { "HKEY_CURRENT_USER", "HKCU" },
                [TokenType.HKeyLocalMachine]    = new[] { "HKEY_LOCAL_MACHINE", "HKLM" },
                [TokenType.HKeyUsers]           = new[] { "HKEY_USERS", "HKU" },
                [TokenType.HKeyPerformanceData] = new[] { "HKEY_PERFORMANCE_DATA", "HKPD" },
                [TokenType.HKeyDynData]         = new[] { "HKEY_DYN_DATA", "HKDD" },
                [TokenType.HKeyCurrentConfig]   = new[] { "HKEY_CURRENT_CONFIG", "HKCC" },

                [TokenType.Delete]              = new[] { "Delete" },
                [TokenType.ForceRemove]         = new[] { "ForceRemove" },
                [TokenType.NoRemove]            = new[] { "NoRemove" },
                [TokenType.Val]                 = new[] { "val" },

                [TokenType.LBrace]              = new[] { "{" },
                [TokenType.RBrace]              = new[] { "}" },
                [TokenType.Equals]              = new[] { "=" },
                [TokenType.SQuote]              = new[] { "'" },

                [TokenType.Type_String]         = new[] { "s" },
                [TokenType.Type_Dword]          = new[] { "d" },
                [TokenType.Type_MultiString]    = new[] { "m" },
                [TokenType.Type_Binary]         = new[] { "b" },
            };


        private readonly Source _input;


        public Lexer(Source input)
        {
            _input = input;
        }


        public Token Accept(TokenType type)
        {
            AdvanceToNextNonWhiteSpace();

            Token tok = null;

            string matched;
            if (TryMatch(type, out matched))
            {
                SourceLocation start = _input.Location;
                _input.Consume(matched.Length);
                tok = new Token(type, start, _input.Location, matched);
            }

            return tok;
        }

        public Token AcceptAny(params TokenType[] types)
        {
            AdvanceToNextNonWhiteSpace();

            TokenType? bestType = null;
            string best = null;

            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                string match;

                if (TryMatch(type, out match))
                {
                    if (best == null ||
                        best.Length < match.Length)
                    {
                        bestType = type;
                        best = match;
                    }
                }
            }

            if (best != null)
            {
                SourceLocation start = _input.Location;
                _input.Consume(best.Length);
                return new Token(bestType.Value, start, _input.Location, best);
            }

            return null;
        }

        private bool TryMatch(TokenType type, out string matched)
        {
            string[] possiblilities;
            if (TokenTexts.TryGetValue(type, out possiblilities))
            {
                SourceLocation start = _input.Location;

                if (MatchAny(possiblilities, out matched))
                {
                    return true;
                }
            }
            else if (type == TokenType.Value)
            {
                SourceLocation start = _input.Location;
                int len = 0;
                char ch = _input.LookAhead(len);
                var match = new StringBuilder();

                if (ch == '\'')
                {
                    match.Append('\'');
                    len++;
                    ch = _input.LookAhead(len);

                    while (ch != Source.EOF &&
                           ch != '\'')
                    {
                        match.Append(ch);
                        len++;
                        ch = _input.LookAhead(len);
                    }

                    if (ch == '\'')
                    {
                        match.Append('\'');
                        len++;
                    }
                    else
                    {
                        // did not match.
                        len = 0;
                    }
                }

                if (len > 0)
                {
                    matched = match.ToString();
                    return true;
                }
            }
            else if (type == TokenType.Name)
            {
                SourceLocation start = _input.Location;
                int len = 0;
                char ch = _input.LookAhead(len);
                var match = new StringBuilder();

                if (ch != '\'')
                {
                    // only accept until next whitespace
                    while (ch != Source.EOF &&
                           !char.IsWhiteSpace(ch))
                    {
                        match.Append(ch);
                        len++;
                        ch = _input.LookAhead(len);
                    }
                }
                else
                {
                    match.Append('\'');
                    len++;
                    ch = _input.LookAhead(len);

                    // accept until next SQuote
                    while (ch != Source.EOF &&
                           ch != '\'')
                    {
                        match.Append(ch);
                        len++;
                        ch = _input.LookAhead(len);
                    }

                    if (ch == '\'')
                    {
                        len++;
                        match.Append('\'');
                    }
                }

                if (len > 0)
                {
                    matched = match.ToString();
                    return true;
                }
            }

            // nothing matched
            matched = null;
            return false;
        }

        private void AdvanceToNextNonWhiteSpace()
        {
            while (char.IsWhiteSpace(_input.LookAhead(0)))
            {
                _input.Consume(1);
            }
        }

        private bool MatchAny(string[] texts, out string matched)
        {
            string longest = null;

            foreach (var item in texts)
            {
                if (NextIs(item))
                {
                    if (longest == null ||
                        longest.Length < item.Length)
                    {
                        longest = item;
                    }
                }
            }

            matched = longest;
            return (longest != null);
        }

        private bool NextIs(string text)
        {
            Debug.Assert(!string.IsNullOrEmpty(text));

            for (int i = 0; i < text.Length; i++)
            {
                if (_input.LookAhead(i) != text[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
