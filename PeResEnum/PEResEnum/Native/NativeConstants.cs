using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEResEnum.Native
{
    internal static class NativeConstants
    {
        public const uint DosSignature = 0x5A4D;        // "MZ"
        public const uint NTSignature  = 0x00004550;    // "PE\0\0"

        public const int IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;

        public const int IMAGE_SIZEOF_SHORT_NAME = 8;

        public const int IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10B;
        public const int IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20B;
        public const int IMAGE_ROM_OPTIONAL_HDR_MAGIC  = 0x107;
    }

    internal enum SpecialSectionName
    {
        Debug,
        Drectve,
        EData,
        PData,
        Reloc,
        TLS,
        Rsrc,
        CorMeta,
        SxData,
    }

    public enum ImageFileMachine
    {
        Unknown   = 0,
        I386      = 0x014C,     // Intel 386.
        R3000     = 0x0162,     // MIPS little-endian, 0x160 big-endian
        R4000     = 0x0166,     // MIPS little-endian
        R10000    = 0x0168,     // MIPS little-endian
        WCEMIPSV2 = 0x0169,     // MIPS little-endian WCE v2
        Alpha     = 0x0184,     // Alpha_AXP
        SH3       = 0x01A2,     // SH3 little-endian
        SH3DSP    = 0x01A3,
        SH3E      = 0x01A4,     // SH3E little-endian
        SH4       = 0x01A6,     // SH4 little-endian
        SH5       = 0x01A8,     // SH5
        ARM       = 0x01C0,     // ARM Little-Endian
        THUMB     = 0x01C2,
        AM33      = 0x01D3,
        PowerPC   = 0x01F0,     // IBM PowerPC Little-Endian
        PowerPCFP = 0x01F1,
        IA64      = 0x0200,     // Intel 64
        MIPS16    = 0x0266,     // MIPS
        Alpha64   = 0x0284,     // ALPHA64
        MIPSFPU   = 0x0366,     // MIPS
        MIPSFPU16 = 0x0466,     // MIPS
        AXP64     = Alpha64,    
        TRICORE   = 0x0520,     // Infineon
        CEF       = 0x0CEF,
        EBC       = 0x0EBC,     // EFI Byte Code
        AMD64     = 0x8664,     // AMD64 (K8)
        M32R      = 0x9041,     // M32R little-endian
        CEE       = 0xC0EE,
    }

    internal enum ImageDirectoryEntry
    {
        Export         = 0,     // Export Directory
        Import         = 1,     // Import Directory
        Resource       = 2,     // Resource Directory
        Exception      = 3,     // Exception Directory
        Security       = 4,     // Security Directory
        BaseReloc      = 5,     // Base Relocation Table
        Debug          = 6,     // Debug Directory
        // Copyright   = 7,     // (X86 usage)
        Architecture   = 7,     // Architecture Specific Data
        GlobalPtr      = 8,     // RVA of GP
        TLS            = 9,     // TLS Directory
        LoadConfig     = 10,    // Load Configuration Directory
        BoundImport    = 11,    // Bound Import Directory in headers
        IAT            = 12,    // Import Address Table
        DelayImport    = 13,    // Delay Load Import Descriptors
        COMDescriptor  = 14,    // COM Runtime descriptor
    }
}
