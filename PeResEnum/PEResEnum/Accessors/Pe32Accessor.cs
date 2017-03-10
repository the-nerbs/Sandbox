using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PEResEnum.Native;

namespace PEResEnum.Accessors
{
    internal class Pe32Accessor : IPeAccessor
    {
        private IMAGE_NT_HEADERS32 _ntHeader;
        private IMAGE_SECTION_HEADER[] _sectionHeaders;


        public uint Signature
        {
            get { return _ntHeader.Signature; }
        }

        public IMAGE_FILE_HEADER FileHeader
        {
            get { return _ntHeader.FileHeader; }
        }

        public ulong ImageBase
        {
            get { return _ntHeader.OptionalHeader.ImageBase; }
        }

        public uint SectionAlignment
        {
            get { return _ntHeader.OptionalHeader.SectionAlignment; }
        }

        public Version ImageVersion
        {
            get
            {
                return new Version(
                    _ntHeader.OptionalHeader.MajorImageVersion,
                    _ntHeader.OptionalHeader.MinorImageVersion
                );
            }
        }


        public Pe32Accessor(Stream peStream)
        {
            _ntHeader = peStream.Read<IMAGE_NT_HEADERS32>();

            uint nSections = _ntHeader.OptionalHeader.NumberOfRvaAndSizes;
            _sectionHeaders = new IMAGE_SECTION_HEADER[nSections];

            for (uint i = 0; i < nSections; i++)
            {
                _sectionHeaders[i] = peStream.Read<IMAGE_SECTION_HEADER>();
            }
        }


        public IMAGE_DATA_DIRECTORY GetDirectoryHeader(ImageDirectoryEntry type)
        {
            int index = (int)type;

            if (index < 0 || index >= _ntHeader.OptionalHeader.NumberOfRvaAndSizes)
            {
                throw new ArgumentOutOfRangeException($"Directory entry for {type} does not exist.");
            }

            return _ntHeader.OptionalHeader.DataDirectory[index];
        }


        public IMAGE_SECTION_HEADER GetSectionHeader(string sectionName)
        {
            for (int i = 0; i < _sectionHeaders.Length; i++)
            {
                if (_sectionHeaders[i].NameStr == sectionName)
                {
                    return _sectionHeaders[i];
                }
            }

            throw new KeyNotFoundException($"PE does not contain a section named '{sectionName}'.");
        }
    }
}
