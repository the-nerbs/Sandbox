using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PEResEnum.Accessors;
using PEResEnum.Native;

namespace PEResEnum
{
    class PeFile
    {
        private readonly byte[] _peImage;
        private readonly MemoryStream _peStream;


        public string Path { get;}


        public PeFile(string path)
        {
            Path = path;

            _peImage = File.ReadAllBytes(path);
            _peStream = new MemoryStream(_peImage, writable: false);

            ParseHeaders();
        }


        private void ParseHeaders()
        {
            var dosHeader = _peStream.Read<IMAGE_DOS_HEADER>();
            if (dosHeader.e_magic != NativeConstants.DosSignature)
            {
                throw new BadImageFormatException("File does not have a valid DOS header.");
            }

            _peStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

            uint ntSignature = _peStream.Read<uint>();
            if (ntSignature != NativeConstants.NTSignature)
            {
                throw new BadImageFormatException("File does not have a valid NT header.");
            }

            var machineType = (ImageFileMachine)_peStream.Read<ushort>();

            // return to the beginning of the NT header for the accessors.
            _peStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

            IPeAccessor accessor;

            switch (machineType)
            {
                case ImageFileMachine.I386:
                    accessor = new Pe32Accessor(_peStream);
                    break;

                case ImageFileMachine.AMD64:
                    accessor = new Pe64Accessor(_peStream);
                    break;

                default:
                    throw new BadImageFormatException($"File is for unsupported machine type {machineType}.");
            }

            var dir = accessor.GetDirectoryHeader(ImageDirectoryEntry.Resource);
            uint fileOffset = dir.VirtualAddress;

            var resSectionHeader = accessor.GetSectionHeader(SpecialSectionName.Rsrc);

            //resSectionHeader.PointerToRawData
            _peStream.Seek(resSectionHeader.PointerToRawData, SeekOrigin.Begin);

            var resAccessor = new PeResourceAccessor(_peStream, resSectionHeader.VirtualAddress);

            var rgsScriptDatas = new List<byte[]>(resAccessor.GetResourcesOfType("REGISTRY"));
            var rgsScripts = rgsScriptDatas.Select(data => Encoding.ASCII.GetString(data));
        }
    }
}
