
# types used in templates

module Cecil

    class FieldWorker # abstract
    
        attr_reader(:field_name)
        attr_reader(:property_name)
        attr_reader(:type)
        attr_reader(:ns)
        attr_reader(:size)
        attr_reader(:target)

        def initialize(name, type, target)
            @property_name = name
            @target = target
            @field_name = to_field(name)
            
            @objtype = type
            
            ar = type.name.split(".")
            @type = ar.pop()
            @ns = ar.join(".")
            
            @size = type.size
        end

        def to_field(name)
            name = String.new(name)
            name[0] = name[0].chr.downcase
            return "m_" + name
        end
    
        def read_binary(inst)
            return case @type
                when "byte"
                    inst + ".ReadByte()"
                when "ushort"
                    inst + ".ReadUInt16()";
                when "short"
                    inst + ".ReadInt16()";
                when "uint"
                    inst + ".ReadUInt32()";
                when "int"
                    inst + ".ReadInt32()";
                when "RVA"
                    "new RVA(" + inst + ".ReadUInt32())"
                when "DataDirectory"
                    "new DataDirectory(\n                new RVA(" + inst + ".ReadUInt32()),\n                " + inst + ".ReadUInt32())"
                else
                    "(" + @objtype.name + ")" + case @objtype.underlying
                        when "ushort"
                            inst + ".ReadUInt16()";
                        when "uint"
                            inst + ".ReadUInt32()";
                    end
            end
        end
    end
    
    class Type
    
        attr_reader(:name)
        attr_reader(:size)
        attr_reader(:underlying)
        
        def initialize(name, size, underlying = nil)
            @name = name
            @size = size
            @underlying = underlying
        end
    end
    
    class Table
        
        attr_reader(:ref_ns)
        attr_reader(:rid)
        attr_reader(:name)
        attr_reader(:table_name)
        attr_reader(:row_name)
        attr_reader(:columns)
        
        def initialize(name, rid)
            @name = name
            @table_name = name + "Table"
            @row_name = name + "Row"
            @rid = rid
            @ref_ns = Array.new
            @columns = Array.new
        end
        
        def add_column(col)
            if (!@ref_ns.include?(col.ns) && col.ns.length != 0 && col.ns != "Cecil.Metadata") then
                @ref_ns.push(col.ns)
                @ref_ns.sort!()
            end
            @columns.push(col)
        end
        
        def row_size()
            size = 0
            @columns.each { |col|
                size += col.size
            }
            return size
        end
    end
    
    class Column < FieldWorker
        
        def initialize(name, type, target)
            super(name, type, target)
        end
        
    end
    
    class Collection
    
        attr_reader(:type)
        attr_reader(:intf)
        attr_reader(:name)
        attr_reader(:container)
        attr_reader(:visitable)
        attr_reader(:visitor)
        
        def initialize(type, container, visit)
            @type = type
            @intf = type + "Collection"
            @name = @intf[1..@intf.length]
            @container = container
            @visitable = visit + "Visitable"
            @visitor = visit + "Visitor"
        end
    end
    
    class OpCode
    
        attr_reader(:name)
        attr_reader(:field_name)
        attr_reader(:op1)
        attr_reader(:op2)
        attr_reader(:size)
        attr_reader(:flowcontrol)
        attr_reader(:opcodetype)
        attr_reader(:operandtype)
        attr_reader(:stackbehaviourpop)
        attr_reader(:stackbehaviourpush)
        
        def initialize(name, op1, op2, flowcontrol, opcodetype, operandtype, stackbehaviourpop, stackbehaviourpush)
            @name = name
            @field_name = name_to_prop(name)
            @op1 = op1 ; @op2 = op2
            @size = @op1 == "0xff" ? 1 : 2
            @flowcontrol = "FlowControl." + flowcontrol
            @opcodetype = "OpCodeType." + opcodetype
            @operandtype = "OperandType." + operandtype
            @stackbehaviourpop = "StackBehaviour." + stackbehaviourpop
            @stackbehaviourpush = "StackBehaviour." + stackbehaviourpush
        end
        
        def name_to_prop(name)
            field = ""
            ar = name.split(".")
            ar.each { |part|
                field += part.capitalize()
                field += "_" unless ar.last == part
            }
            return field
        end
    end
    
    class Header
    
        attr_reader(:name)
        attr_reader(:fields)
        
        def initialize(name)
            @name = name
            @fields = Array.new
        end
        
        def add_field(field)
            @fields.push(field)
        end
    end

    class Field < FieldWorker
    
        attr_reader(:default)
        
        def initialize(name, type, default)
            super(name, type, nil)
            @default = default
        end
    end
    
    class CodedIndexTable
        
        attr_reader(:name)
        attr_reader(:tag)
        
        def initialize(name, tag)
            @name = name
            @tag = tag
        end
    end
    
    class CodedIndex
    
        attr_reader(:name)
        attr_reader(:size)
        attr_reader(:tables)
        
        def initialize(name, size)
            @name = name
            @size = size
            @tables = Array.new
        end
        
        def add_table(name, tag)
            @tables.push(CodedIndexTable.new(name, tag))
        end
    end
    
end
