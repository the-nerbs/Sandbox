using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    public enum TokenType
    {
        HKeyClassesRoot,        // 'HKEY_CLASSES_ROOT' or 'HKCR
        HKeyCurrentUser,        // 'HKEY_CURRENT_USER' or 'HKCU'
        HKeyLocalMachine,       // 'HKEY_LOCAL_MACHINE' or 'HKLM'
        HKeyUsers,              // 'HKEY_USERS' or 'HKU'
        HKeyPerformanceData,    // 'HKEY_PERFORMANCE_DATA' or 'HKPD'
        HKeyDynData,            // 'HKEY_DYN_DATA' or 'HKDD'
        HKeyCurrentConfig,      // 'HKEY_CURRENT_CONFIG' or 'HKCC'

        Delete,                 // 'Delete'
        ForceRemove,            // 'ForceRemove'
        NoRemove,               // 'NoRemove'
        Val,                    // 'val'

        LBrace,                 // {
        RBrace,                 // }
        Equals,                 // =
        SQuote,                 // apostrophe/single quote (')
        Type_String,            // 's'
        Type_Dword,             // 'd'
        Type_MultiString,       // 'm'
        Type_Binary,            // 'b'

        Name,                   // [^\0 \t\v\f\r\n]+ | '[^\0 \t\v\f\r\n]+'
        Value,                  // '[^\0 \t\v\f\r\n]+'
    }

    class Token
    {
        public TokenType Type { get; }
        public SourceLocation Start { get; }
        public SourceLocation End { get; }

        public string Value { get; }

        public int Length
        {
            get { return End.RawLocation - Start.RawLocation; }
        }


        public Token(TokenType type, SourceLocation start, SourceLocation end, string value)
        {
            Type = type;
            Start = start;
            End = end;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type} @ {Start.RawLocation}:{End.RawLocation} = [{Value}]";
        }
    }
}
