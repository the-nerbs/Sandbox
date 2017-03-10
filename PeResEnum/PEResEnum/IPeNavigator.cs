using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PEResEnum.Native;

namespace PEResEnum
{
    internal interface IPeAccessor
    {
        // not bit-dependent:
        uint Signature { get; }
        IMAGE_FILE_HEADER FileHeader { get; }

        // bit dependent:

        // Optional header data
        ulong ImageBase { get; }
        uint SectionAlignment { get; }
        Version ImageVersion { get; }

        IMAGE_DATA_DIRECTORY GetDirectoryHeader(ImageDirectoryEntry type);

        IMAGE_SECTION_HEADER GetSectionHeader(string sectionName);
    }

    internal static class PeAccessorExtensions
    {
        public static IMAGE_SECTION_HEADER GetSectionHeader(this IPeAccessor accessor, SpecialSectionName section)
        {
            switch (section)
            {
                case SpecialSectionName.Debug:
                    return accessor.GetSectionHeader(".debug");

                case SpecialSectionName.Drectve:
                    return accessor.GetSectionHeader(".drectve");

                case SpecialSectionName.EData:
                    return accessor.GetSectionHeader(".edata");

                case SpecialSectionName.PData:
                    return accessor.GetSectionHeader(".pdata");

                case SpecialSectionName.Reloc:
                    return accessor.GetSectionHeader(".reloc");

                case SpecialSectionName.TLS:
                    return accessor.GetSectionHeader(".tls");

                case SpecialSectionName.Rsrc:
                    return accessor.GetSectionHeader(".rsrc");

                case SpecialSectionName.CorMeta:
                    return accessor.GetSectionHeader(".cormeta");

                case SpecialSectionName.SxData:
                    return accessor.GetSectionHeader(".sxdata");

                default:
                    throw new ArgumentException("Invalid section");
            }
        }
    }
}
