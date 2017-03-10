using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using PEResEnum.Native;

namespace PEResEnum.Accessors
{
    class PeResourceAccessor
    {
        private const int RootId = 0;
        private const int NoLink = -1;

        private readonly Dictionary<int, ResDirectory> _directories = new Dictionary<int, ResDirectory>();
        private readonly Dictionary<int, ResDirectoryEntry> _entries = new Dictionary<int, ResDirectoryEntry>();
        private readonly Dictionary<int, byte[]> _data = new Dictionary<int, byte[]>();


        public PeResourceAccessor(Stream peStream, long virtualAddress)
        {
            int nextId = RootId;
            long baseAddress = peStream.Position;
            ReadDirectory(peStream, baseAddress, virtualAddress, ref nextId);
        }


        private int ReadDirectory(Stream peStream, long baseAddress, long virtualAddress, ref int nextId)
        {
            int dirId = nextId++;
            var dirInfo = new ResDirectory();
            _directories.Add(dirId, dirInfo);

            dirInfo.directory = peStream.Read<IMAGE_RESOURCE_DIRECTORY>();

            int nChildren = dirInfo.directory.NumberOfIdEntries + dirInfo.directory.NumberOfNamedEntries;
            dirInfo.childMappings = new int[nChildren];

            for (int i = 0; i < nChildren; i++)
            {
                int entryId = nextId++;

                var dirEntry = new ResDirectoryEntry();

                _entries.Add(entryId, dirEntry);
                dirInfo.childMappings[i] = entryId;

                dirEntry.entry = peStream.Read<IMAGE_RESOURCE_DIRECTORY_ENTRY>();

                // preserve the current stream position while we read child data
                var curOffset = peStream.Position;

                // read the name
                if (dirEntry.entry.NameIsString)
                {
                    // note: offset from beginning of section.
                    peStream.Seek(baseAddress + dirEntry.entry.NameOffset, SeekOrigin.Begin);

                    int nChars = peStream.Read<short>();

                    // names are in "Unicode"
                    int nBytes = nChars * 2;

                    var nameBytes = new byte[nBytes];
                    peStream.Read(nameBytes, 0, nBytes);

                    dirEntry.name = Encoding.Unicode.GetString(nameBytes);
                }
                else
                {
                    dirEntry.name = dirEntry.entry.Id.ToString();
                }

                // read the child node(s)
                if (dirEntry.entry.DataIsDirectory)
                {
                    // note: offset from beginning of section.
                    peStream.Seek(baseAddress + dirEntry.entry.OffsetToDirectory, SeekOrigin.Begin);

                    dirEntry.child = ReadDirectory(peStream, baseAddress, virtualAddress, ref nextId);
                }
                else
                {
                    // note: offset from beginning of section.
                    peStream.Seek(baseAddress + dirEntry.entry.OffsetToData, SeekOrigin.Begin);

                    var dataEntry = peStream.Read<IMAGE_RESOURCE_DATA_ENTRY>();

                    // decode the RVA...
                    long realDataAddr = baseAddress + dataEntry.OffsetToData - virtualAddress;
                    peStream.Seek(realDataAddr, SeekOrigin.Begin);

                    var dataBytes = new byte[dataEntry.Size];
                    peStream.Read(dataBytes, 0, dataBytes.Length);

                    int dataId = nextId++;
                    _data.Add(dataId, dataBytes);
                    dirEntry.child = dataId;
                }

                peStream.Seek(curOffset, SeekOrigin.Begin);
            }

            return dirId;
        }


        public IEnumerable<byte[]> GetResourcesOfType(string type)
        {
            var root = _directories[RootId];

            // Note: we assume the convention here, that resources are organized into 3 tiers:
            //  - type
            //    - name
            //      - language
            if (root.directory.NumberOfNamedEntries > 0)
            {
                for (int i = 0; i < root.NumChildren; i++)
                {
                    var entry = _entries[root.childMappings[i]];

                    if (entry.IsNamed &&
                        StringComparer.OrdinalIgnoreCase.Equals(entry.name, type))
                    {
                        foreach (var data in GetAllChildDatas(entry))
                        {
                            yield return data;
                        }
                    }
                }
            }
        }

        IEnumerable<byte[]> GetAllChildDatas(ResDirectoryEntry entry)
        {
            if (entry.IsSubdirectory)
            {
                // subdirectory, recursively yield the data
                var dir = _directories[entry.child];

                for (int i = 0; i < dir.NumChildren; i++)
                {
                    var childEntry = _entries[dir.childMappings[i]];

                    foreach (var data in GetAllChildDatas(childEntry))
                    {
                        yield return data;
                    }
                }
            }
            else
            {
                // leaf node, just yield the data
                yield return _data[entry.child];
            }
        }


        private class ResDirectory
        {
            public IMAGE_RESOURCE_DIRECTORY directory;
            public int[] childMappings;

            public int NumChildren
            {
                get { return directory.NumberOfIdEntries + directory.NumberOfNamedEntries; }
            }
        }

        [DebuggerDisplay("Entry: {name}")]
        private class ResDirectoryEntry
        {
            public IMAGE_RESOURCE_DIRECTORY_ENTRY entry;
            public string name;
            public int child;

            public bool IsNamed
            {
                get { return entry.NameIsString; }
            }

            public bool IsSubdirectory
            {
                get { return entry.DataIsDirectory; }
            }
        }
    }
}
