namespace Compiler
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


