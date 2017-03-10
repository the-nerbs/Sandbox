using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PEResEnum.Native
{
    // Note: the original definition is a little more complex than I made it here...
    // typedef struct _IMAGE_RESOURCE_DIRECTORY_ENTRY {
    //     union {
    //         struct {
    //             DWORD NameOffset:31;
    //             DWORD NameIsString:1;
    //         } DUMMYSTRUCTNAME;
    //         DWORD   Name;
    //         WORD    Id;
    //     } DUMMYUNIONNAME;
    //     union {
    //         DWORD   OffsetToData;
    //         struct {
    //             DWORD   OffsetToDirectory:31;
    //             DWORD   DataIsDirectory:1;
    //         } DUMMYSTRUCTNAME2;
    //     } DUMMYUNIONNAME2;
    // } IMAGE_RESOURCE_DIRECTORY_ENTRY;

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    internal struct IMAGE_RESOURCE_DIRECTORY_ENTRY
    {
        private uint word0;
        private uint word1;


        public uint NameOffset
        {
            get { return (word0 & 0x7FFFFFFF); }
        }

        public bool NameIsString
        {
            get
            {
                return (word0 & 0x80000000) != 0;
            }
        }

        public uint Name
        {
            get { return word0; }
        }

        public ushort Id
        {
            get { return unchecked((ushort)word0); }
        }


        public uint OffsetToData
        {
            get { return word1; }
        }

        public uint OffsetToDirectory
        {
            get { return (word1 & 0x7FFFFFFF); }
        }

        public bool DataIsDirectory
        {
            get { return (word1 & 0x80000000) != 0; }
        }
    }
}
