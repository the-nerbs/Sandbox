using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEResEnum
{
    class Program
    {
        static void Main(string[] args)
        {
            var pe = new PeFile(@"..\..\..\bin\x64_Debug\Dll.dll");
        }
    }
}
