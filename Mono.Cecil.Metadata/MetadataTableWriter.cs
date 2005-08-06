/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sat Aug 06 02:22:42 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	using System;
	using System.Collections;
	using System.IO;

	internal sealed class MetadataTableWriter : IMetadataTableVisitor {

		private MetadataRoot m_metadataRoot;
		private TablesHeap m_heap;
		private MetadataRowWriter m_mrrw;
		private BinaryWriter m_binaryWriter;

		public MetadataTableWriter (MetadataWriter mrv, BinaryWriter writer)
		{
			m_metadataRoot = mrv.GetMetadataRoot ();
			m_heap = m_metadataRoot.Streams.TablesHeap;
			m_mrrw = new MetadataRowWriter (this);
			m_binaryWriter = writer;
		}

		public MetadataRoot GetMetadataRoot ()
		{
			return m_metadataRoot;
		}

		public IMetadataRowVisitor GetRowVisitor ()
		{
			return m_mrrw;
		}

		public BinaryWriter GetWriter ()
		{
			return m_binaryWriter;
		}

		public AssemblyTable GetAssemblyTable ()
		{
			Type tt = typeof (AssemblyTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyTable;

			AssemblyTable table = new AssemblyTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public AssemblyOSTable GetAssemblyOSTable ()
		{
			Type tt = typeof (AssemblyOSTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyOSTable;

			AssemblyOSTable table = new AssemblyOSTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public AssemblyProcessorTable GetAssemblyProcessorTable ()
		{
			Type tt = typeof (AssemblyProcessorTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyProcessorTable;

			AssemblyProcessorTable table = new AssemblyProcessorTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public AssemblyRefTable GetAssemblyRefTable ()
		{
			Type tt = typeof (AssemblyRefTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyRefTable;

			AssemblyRefTable table = new AssemblyRefTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public AssemblyRefOSTable GetAssemblyRefOSTable ()
		{
			Type tt = typeof (AssemblyRefOSTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyRefOSTable;

			AssemblyRefOSTable table = new AssemblyRefOSTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public AssemblyRefProcessorTable GetAssemblyRefProcessorTable ()
		{
			Type tt = typeof (AssemblyRefProcessorTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as AssemblyRefProcessorTable;

			AssemblyRefProcessorTable table = new AssemblyRefProcessorTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ClassLayoutTable GetClassLayoutTable ()
		{
			Type tt = typeof (ClassLayoutTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ClassLayoutTable;

			ClassLayoutTable table = new ClassLayoutTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ConstantTable GetConstantTable ()
		{
			Type tt = typeof (ConstantTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ConstantTable;

			ConstantTable table = new ConstantTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public CustomAttributeTable GetCustomAttributeTable ()
		{
			Type tt = typeof (CustomAttributeTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as CustomAttributeTable;

			CustomAttributeTable table = new CustomAttributeTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public DeclSecurityTable GetDeclSecurityTable ()
		{
			Type tt = typeof (DeclSecurityTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as DeclSecurityTable;

			DeclSecurityTable table = new DeclSecurityTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public EventTable GetEventTable ()
		{
			Type tt = typeof (EventTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as EventTable;

			EventTable table = new EventTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public EventMapTable GetEventMapTable ()
		{
			Type tt = typeof (EventMapTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as EventMapTable;

			EventMapTable table = new EventMapTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ExportedTypeTable GetExportedTypeTable ()
		{
			Type tt = typeof (ExportedTypeTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ExportedTypeTable;

			ExportedTypeTable table = new ExportedTypeTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public FieldTable GetFieldTable ()
		{
			Type tt = typeof (FieldTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as FieldTable;

			FieldTable table = new FieldTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public FieldLayoutTable GetFieldLayoutTable ()
		{
			Type tt = typeof (FieldLayoutTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as FieldLayoutTable;

			FieldLayoutTable table = new FieldLayoutTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public FieldMarshalTable GetFieldMarshalTable ()
		{
			Type tt = typeof (FieldMarshalTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as FieldMarshalTable;

			FieldMarshalTable table = new FieldMarshalTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public FieldRVATable GetFieldRVATable ()
		{
			Type tt = typeof (FieldRVATable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as FieldRVATable;

			FieldRVATable table = new FieldRVATable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public FileTable GetFileTable ()
		{
			Type tt = typeof (FileTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as FileTable;

			FileTable table = new FileTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public GenericParamTable GetGenericParamTable ()
		{
			Type tt = typeof (GenericParamTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as GenericParamTable;

			GenericParamTable table = new GenericParamTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public GenericParamConstraintTable GetGenericParamConstraintTable ()
		{
			Type tt = typeof (GenericParamConstraintTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as GenericParamConstraintTable;

			GenericParamConstraintTable table = new GenericParamConstraintTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ImplMapTable GetImplMapTable ()
		{
			Type tt = typeof (ImplMapTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ImplMapTable;

			ImplMapTable table = new ImplMapTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public InterfaceImplTable GetInterfaceImplTable ()
		{
			Type tt = typeof (InterfaceImplTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as InterfaceImplTable;

			InterfaceImplTable table = new InterfaceImplTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ManifestResourceTable GetManifestResourceTable ()
		{
			Type tt = typeof (ManifestResourceTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ManifestResourceTable;

			ManifestResourceTable table = new ManifestResourceTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public MemberRefTable GetMemberRefTable ()
		{
			Type tt = typeof (MemberRefTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as MemberRefTable;

			MemberRefTable table = new MemberRefTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public MethodTable GetMethodTable ()
		{
			Type tt = typeof (MethodTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as MethodTable;

			MethodTable table = new MethodTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public MethodImplTable GetMethodImplTable ()
		{
			Type tt = typeof (MethodImplTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as MethodImplTable;

			MethodImplTable table = new MethodImplTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public MethodSemanticsTable GetMethodSemanticsTable ()
		{
			Type tt = typeof (MethodSemanticsTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as MethodSemanticsTable;

			MethodSemanticsTable table = new MethodSemanticsTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public MethodSpecTable GetMethodSpecTable ()
		{
			Type tt = typeof (MethodSpecTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as MethodSpecTable;

			MethodSpecTable table = new MethodSpecTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ModuleTable GetModuleTable ()
		{
			Type tt = typeof (ModuleTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ModuleTable;

			ModuleTable table = new ModuleTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ModuleRefTable GetModuleRefTable ()
		{
			Type tt = typeof (ModuleRefTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ModuleRefTable;

			ModuleRefTable table = new ModuleRefTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public NestedClassTable GetNestedClassTable ()
		{
			Type tt = typeof (NestedClassTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as NestedClassTable;

			NestedClassTable table = new NestedClassTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public ParamTable GetParamTable ()
		{
			Type tt = typeof (ParamTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as ParamTable;

			ParamTable table = new ParamTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public PropertyTable GetPropertyTable ()
		{
			Type tt = typeof (PropertyTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as PropertyTable;

			PropertyTable table = new PropertyTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public PropertyMapTable GetPropertyMapTable ()
		{
			Type tt = typeof (PropertyMapTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as PropertyMapTable;

			PropertyMapTable table = new PropertyMapTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public StandAloneSigTable GetStandAloneSigTable ()
		{
			Type tt = typeof (StandAloneSigTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as StandAloneSigTable;

			StandAloneSigTable table = new StandAloneSigTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public TypeDefTable GetTypeDefTable ()
		{
			Type tt = typeof (TypeDefTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as TypeDefTable;

			TypeDefTable table = new TypeDefTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public TypeRefTable GetTypeRefTable ()
		{
			Type tt = typeof (TypeRefTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as TypeRefTable;

			TypeRefTable table = new TypeRefTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public TypeSpecTable GetTypeSpecTable ()
		{
			Type tt = typeof (TypeSpecTable);
			if (m_heap.HasTable (tt))
				return m_heap [tt] as TypeSpecTable;

			TypeSpecTable table = new TypeSpecTable ();
			m_heap.Valid |= 1L << TablesHeap.GetTableId (tt);
			return table;
		}

		public void Visit (TableCollection coll)
		{
			// sort table by tid
		}

		public void Visit (AssemblyTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (AssemblyOSTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (AssemblyProcessorTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (AssemblyRefTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (AssemblyRefOSTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (AssemblyRefProcessorTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ClassLayoutTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ConstantTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (CustomAttributeTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (DeclSecurityTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (EventTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (EventMapTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ExportedTypeTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (FieldTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (FieldLayoutTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (FieldMarshalTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (FieldRVATable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (FileTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (GenericParamTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (GenericParamConstraintTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ImplMapTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (InterfaceImplTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ManifestResourceTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (MemberRefTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (MethodTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (MethodImplTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (MethodSemanticsTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (MethodSpecTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ModuleTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ModuleRefTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (NestedClassTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (ParamTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (PropertyTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (PropertyMapTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (StandAloneSigTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (TypeDefTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (TypeRefTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Visit (TypeSpecTable table)
		{
			m_binaryWriter.Write (table.Rows.Count);
		}

		public void Terminate(TableCollection coll)
		{
		}
	}
}
