using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static PEResEnum.Native.NativeConstants;

namespace PEResEnum.Native
{
    [StructLayout(LayoutKind.Explicit, Pack = 4, Size = sizeof(uint))]
    internal struct Misc_t
    {
        [FieldOffset(0)]
        public uint PhysicalAddress;

        [FieldOffset(0)]
        public uint VirtualSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    internal struct IMAGE_SECTION_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = IMAGE_SIZEOF_SHORT_NAME)]
        public byte[] Name;

        public Misc_t Misc;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public uint PointerToRelocations;
        public uint PointerToLinenumbers;
        public ushort NumberOfRelocations;
        public ushort NumberOfLinenumbers;
        public uint Characteristics;


        public string NameStr
        {
            get { return Encoding.ASCII.GetString(Name).TrimEnd('\0'); }
        }
    }
}
