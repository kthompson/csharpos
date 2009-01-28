﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Indy.IL2CPU.IL
{
    public partial class ILReader
    {
        private readonly MethodBody mBody;
        private MethodDefinition mMethod;
        private ModuleDefinition mModule;
        public ILReader(MethodDefinition aMethod)
        {
            mBody = aMethod.Body;
            mMethod = aMethod;
            mModule = mMethod.DeclaringType.Module;
        }

        public bool HasOperand { get; private set; }
        public OpCodeEnum OpCode { get; private set; }


        public byte[] Operand { get; private set; }

        private string mOperandValueStr;
        public string OperandValueStr
        {
            get
            {
                if (mOperandValueStr == null)
                {
                    mOperandValueStr = mModule.ResolveString(OperandValueInt32);
                }
                return mOperandValueStr;
            }
        }

        private MethodDefinition mOperandValueMethod;
        public MethodDefinition OperandValueMethod
        {
            get
            {
                if (mOperandValueMethod == null)
                {
                    Type[] xTypeGenArgs = null;
                    Type[] xMethodGenArgs = null;
                    if (mMethod.DeclaringType.IsGenericType)
                    {
                        xTypeGenArgs = mMethod.DeclaringType.GetGenericArguments();
                    }
                    if (mMethod.IsGenericMethod)
                    {
                        xMethodGenArgs = mMethod.GetGenericArguments();
                    }
                    mOperandValueMethod = mModule.ResolveMethod(OperandValueInt32, xTypeGenArgs, xMethodGenArgs);
                }
                return mOperandValueMethod;
            }
        }

        private uint? mOperandValueBranchPosition;
        private bool mIsShortcut;
        public uint OperandValueBranchPosition
        {
            get
            {
                if (mOperandValueBranchPosition == null)
                {
                    //sbyte xTemp = (sbyte)mOperand;
                    //if (xTemp == mOperand) {
                    //    mOperandValueBranchPosition = NextPosition + xTemp;
                    //} else {
                    //						if (mStream.Length < (NextPosition + mOperand + 1)) {
                    //							mOperandValueBranchPosition = (uint)mOperand;
                    //						} else {
                    //							mOperandValueBranchPosition = (uint)(NextPosition + mOperand);
                    //						}
                    if (!mIsShortcut)
                    {
                        mOperandValueBranchPosition = (uint?)(NextPosition + OperandValueInt32);
                    }
                    else
                    {
                        mOperandValueBranchPosition = (uint?)(NextPosition + (sbyte)OperandValueInt32);
                    }
                    //}
                }
                return mOperandValueBranchPosition.Value;
            }
        }

        private FieldDefinition mOperandValueField;
        public FieldDefinition OperandValueField
        {
            get
            {
                if (mOperandValueField == null)
                {
                    try
                    {
                        Type[] xTypeGenArgs = null;
                        Type[] xMethodGenArgs = null;
                        if (mMethod.DeclaringType.IsGenericType)
                        {
                            xTypeGenArgs = mMethod.DeclaringType.GetGenericArguments();
                        }
                        if (mMethod.IsGenericMethod)
                        {
                            xMethodGenArgs = mMethod.GetGenericArguments();
                        }
                        mOperandValueField = mModule.ResolveField(OperandValueInt32,
                                                                  xTypeGenArgs,
                                                                  xMethodGenArgs);
                    }
                    catch
                    {
                    }
                }
                return mOperandValueField;
            }
        }

        private TypeReference mOperandValueType;
        public TypeReference OperandValueType
        {
            get
            {
                if (mOperandValueType == null)
                {
                    try
                    {
                        Type[] xTypeGenArgs = null;
                        Type[] xMethodGenArgs = null;
                        if (mMethod.DeclaringType.IsGenericType)
                        {
                            xTypeGenArgs = mMethod.DeclaringType.GetGenericArguments();
                        }
                        if (mMethod.IsGenericMethod)
                        {
                            xMethodGenArgs = mMethod.GetGenericArguments();
                        }
                        mOperandValueType = mModule.ResolveType(OperandValueInt32,
                                                                xTypeGenArgs,
                                                                xMethodGenArgs);
                    }
                    catch
                    {
                    }
                }
                return mOperandValueType;
            }
        }

        public uint[] OperandValueBranchLocations
        {
            get;
            private set;
        }

        private int? mOperandValueInt32;
        public int OperandValueInt32
        {
            get
            {
                if (!mIsShortcut)
                {
                    if (mOperandValueInt32 == null)
                    {
                        byte[] xData = new byte[4];
                        Array.Copy(Operand, xData, Math.Min(4, Operand.Length));
                        mOperandValueInt32 = BitConverter.ToInt32(xData, 0);
                    }
                }
                else
                {
                    sbyte xShortValue = (sbyte)Operand[0];
                    mOperandValueInt32 = xShortValue;
                }
                return mOperandValueInt32.Value;
            }
        }

        private Single? mOperandValueSingle;
        public Single OperandValueSingle
        {
            get
            {
                if (mOperandValueSingle == null)
                {
                    mOperandValueSingle = BitConverter.ToSingle(Operand, 0);
                }
                return mOperandValueSingle.Value;
            }
        }

        private Double? mOperandValueDouble;
        public Double OperandValueDouble
        {
            get
            {
                if (mOperandValueDouble == null)
                {
                    mOperandValueDouble = BitConverter.ToDouble(Operand, 0);
                }
                return mOperandValueDouble.Value;
            }
        }

        public bool Read()
        {
            Position = NextPosition;
            int xByteValueInt = mStream.ReadByte();
            OpCodeEnum xOpCode;
            if (xByteValueInt == -1)
            {
                return false;
            }
            byte xByteValue = (byte)xByteValueInt;
            if (xByteValue == 0xFE)
            {
                xByteValueInt = mStream.ReadByte();
                if (xByteValueInt == -1)
                {
                    return false;
                }
                xOpCode = (OpCodeEnum)(xByteValue << 8 | xByteValueInt);
            }
            else
            {
                xOpCode = (OpCodeEnum)xByteValue;
            }
            byte xOperandSize = GetOperandSize(xOpCode);
            this.Operand = null;
            mOperandValueStr = null;
            mOperandValueMethod = null;
            mOperandValueField = null;
            mOperandValueSingle = null;
            mOperandValueType = null;
            mOperandValueInt32 = null;
            mOperandValueBranchPosition = null;
            OperandValueBranchLocations = null;
            mOperandValueDouble = null;
            this.OpCode = GetNonShortcutOpCode(xOpCode);
            mIsShortcut = this.OpCode != xOpCode;
            this.HasOperand = xOperandSize > 0;
            if (this.HasOperand)
            {
                this.Operand = ReadOperand(xOperandSize);
                mOperandValueInt32 = GetInt32FromOperandByteArray(mOperand);
            }
            else
            {
                if (this.OpCode != xOpCode)
                {
                    long? xTempOperand = GetShortcutOperand(xOpCode);
                    if (xTempOperand != null)
                    {
                        this.HasOperand = true;
                        this.Operand = BitConverter.GetBytes(xTempOperand.Value);
                    }
                }
                if (this.OpCode == OpCodeEnum.Switch)
                {
                    int[] xBranchLocations1 = new int[ReadInt32()];
                    for (int i = 0; i < xBranchLocations1.Length; i++)
                    {
                        xBranchLocations1[i] = ReadInt32();
                    }
                    uint[] xResult = new uint[xBranchLocations1.Length];
                    for (int i = 0; i < xBranchLocations1.Length; i++)
                    {
                        if ((NextPosition + xBranchLocations1[i]) < 0)
                        {
                            xResult[i] = (uint)xBranchLocations1[i];
                        }
                        else
                        {
                            xResult[i] = (uint)(NextPosition + xBranchLocations1[i]);
                        }
                    }
                    OperandValueBranchLocations = xResult;
                }
            }
            return true;
        }


        private Int64 ReadInt64()
        {
            long xResult = 0;
            byte xOperandSize = 64;
            byte[] xBytes = new byte[xOperandSize / 8];
            while (xOperandSize > 0)
            {
                int xByteValueInt = mStream.ReadByte();
                if (xByteValueInt == -1)
                {
                    break;
                }
                xBytes[(xOperandSize / 8) - 1] = (byte)xByteValueInt;
                xOperandSize -= 8;
            }
            for (int i = 0; i < xBytes.Length; i++)
            {
                xResult = xResult << 8 | xBytes[i];
            }
            return xResult;
        }

        private byte[] ReadOperand(byte aOperandSize)
        {
            byte[] xBytes = new byte[aOperandSize / 8];
            int index = 0;
            while (aOperandSize > 0)
            {
                int xByteValueInt = mStream.ReadByte();
                if (xByteValueInt == -1)
                {
                    break;
                }
                xBytes[index] = (byte)xByteValueInt;
                index += 1;
                aOperandSize -= 8;
            }
            return xBytes;
        }

        private static Int32 GetInt32FromOperandByteArray(byte[] aData)
        {
            Int32 xResult = 0;
            for (int i = aData.Length - 1; i >= 0; i--)
            {
                xResult = xResult << 8 | aData[i];
            }
            return xResult;
        }

        private Int32 ReadInt32()
        {
            return GetInt32FromOperandByteArray(ReadOperand(32));
        }
    }
}