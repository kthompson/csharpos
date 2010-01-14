using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Translator
{
    public enum SectionType
    {
        Text,
        Data,
        Imports, // .idata
        Resource, // .rsrc
        Relocation, // .reloc
        ReadOnlyData, // .rdata
        Debug, // .debug
        ThreadLocalStorage, // .tls
    }
}
