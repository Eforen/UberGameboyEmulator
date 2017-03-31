using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberGameboyCore
{
    class CPU
    {
        Register16Bit AF; //Accumulator & Flags
        Register16Bit BC;
        Register16Bit DE;
        Register16Bit HL;

        /// <summary>
        /// Stack Pointer
        /// </summary>
        Register16Bit _SP;

        /// <summary>
        /// Program Counter/Pointer
        /// </summary>
        Register16Bit _PC;

        #region Accessiors
        #region Registers
        #region Accumulator & Flags
        public bool[] A
        {
            get
            {
                return new bool[] { AF.data.Get(8), AF.data.Get(9), AF.data.Get(10), AF.data.Get(11), AF.data.Get(12), AF.data.Get(13), AF.data.Get(14), AF.data.Get(15) };
            }
            set
            {
                AF.data.Set(8, value[8]);
                AF.data.Set(9, value[9]);
                AF.data.Set(10, value[10]);
                AF.data.Set(11, value[11]);
                AF.data.Set(12, value[12]);
                AF.data.Set(13, value[13]);
                AF.data.Set(14, value[14]);
                AF.data.Set(15, value[15]);
            }
        }

        /// <summary>
        /// The Zero Flag (Z)
        /// This bit becomes set(1) if the result of an operation has been zero(0). Used for conditional jumps.
        /// </summary>
        public bool zf
        {
            get
            {
                return AF.data.Get(7);
            }
            set
            {
                AF.data.Set(7, value);
            }
        }

        /// <summary>
        /// The BCD Flags (N, H)
        /// These flags are(rarely) used for the DAA instruction only, N Indicates whether the previous instruction has been an addition or subtraction, and H indicates carry for lower 4bits of the result, also for DAA, the C flag must indicate carry for upper 8bits.
        /// After adding/subtracting two BCD numbers, DAA is intended to convert the result into BCD format; BCD numbers are ranged from 00h to 99h rather than 00h to FFh.
        /// Because C and H flags must contain carry-outs for each digit, DAA cannot be used for 16bit operations(which have 4 digits), or for INC/DEC operations(which do not affect C-flag).
        /// </summary>
        public bool n
        {
            get
            {
                return AF.data.Get(6);
            }
            set
            {
                AF.data.Set(6, value);
            }
        }
        
        /// <summary>
        /// The BCD Flags (N, H)
        /// These flags are(rarely) used for the DAA instruction only, N Indicates whether the previous instruction has been an addition or subtraction, and H indicates carry for lower 4bits of the result, also for DAA, the C flag must indicate carry for upper 8bits.
        /// After adding/subtracting two BCD numbers, DAA is intended to convert the result into BCD format; BCD numbers are ranged from 00h to 99h rather than 00h to FFh.
        /// Because C and H flags must contain carry-outs for each digit, DAA cannot be used for 16bit operations(which have 4 digits), or for INC/DEC operations(which do not affect C-flag).
        /// </summary>
        public bool h
        {
            get
            {
                return AF.data.Get(5);
            }
            set
            {
                AF.data.Set(5, value);
            }
        }

        /// <summary>
        /// The Carry Flag (C, or Cy)
        /// Becomes set when the result of an addition became bigger than FFh(8bit) or FFFFh(16bit). Or when the result of a subtraction or comparision became less than zero(much as for Z80 and 80x86 CPUs, but unlike as for 65XX and ARM CPUs). Also the flag becomes set when a rotate/shift operation has shifted-out a "1"-bit.
        /// Used for conditional jumps, and for instructions such like ADC, SBC, RL, RLA, etc.
        /// </summary>
        public bool cy
        {
            get
            {
                return AF.data.Get(4);
            }
            set
            {
                AF.data.Set(4, value);
            }
        }
        #endregion //Accumulator & Flags
        #region BC

        public bool[] B //High
        {
            get
            {
                return new bool[] { BC.data.Get(8), BC.data.Get(9), BC.data.Get(10), BC.data.Get(11), BC.data.Get(12), BC.data.Get(13), BC.data.Get(14), BC.data.Get(15) };
            }
            set
            {
                BC.data.Set(8, value[8]);
                BC.data.Set(9, value[9]);
                BC.data.Set(10, value[10]);
                BC.data.Set(11, value[11]);
                BC.data.Set(12, value[12]);
                BC.data.Set(13, value[13]);
                BC.data.Set(14, value[14]);
                BC.data.Set(15, value[15]);
            }
        }

        public bool[] C //Low
        {
            get
            {
                return new bool[] { BC.data.Get(0), BC.data.Get(1), BC.data.Get(2), BC.data.Get(3), BC.data.Get(4), BC.data.Get(5), BC.data.Get(6), BC.data.Get(7) };
            }
            set
            {
                BC.data.Set(0, value[0]);
                BC.data.Set(1, value[1]);
                BC.data.Set(2, value[2]);
                BC.data.Set(3, value[3]);
                BC.data.Set(4, value[4]);
                BC.data.Set(5, value[5]);
                BC.data.Set(6, value[6]);
                BC.data.Set(7, value[7]);
            }
        }

        #endregion //BC
        #region DE

        public bool[] D //High
        {
            get
            {
                return new bool[] { DE.data.Get(8), DE.data.Get(9), DE.data.Get(10), DE.data.Get(11), DE.data.Get(12), DE.data.Get(13), DE.data.Get(14), DE.data.Get(15) };
            }
            set
            {
                DE.data.Set(8, value[8]);
                DE.data.Set(9, value[9]);
                DE.data.Set(10, value[10]);
                DE.data.Set(11, value[11]);
                DE.data.Set(12, value[12]);
                DE.data.Set(13, value[13]);
                DE.data.Set(14, value[14]);
                DE.data.Set(15, value[15]);
            }
        }

        public bool[] E //Low
        {
            get
            {
                return new bool[] { DE.data.Get(0), DE.data.Get(1), DE.data.Get(2), DE.data.Get(3), DE.data.Get(4), DE.data.Get(5), DE.data.Get(6), DE.data.Get(7) };
            }
            set
            {
                DE.data.Set(0, value[0]);
                DE.data.Set(1, value[1]);
                DE.data.Set(2, value[2]);
                DE.data.Set(3, value[3]);
                DE.data.Set(4, value[4]);
                DE.data.Set(5, value[5]);
                DE.data.Set(6, value[6]);
                DE.data.Set(7, value[7]);
            }
        }

        #endregion //DE
        #region HL

        public bool[] H //High
        {
            get
            {
                return new bool[] { HL.data.Get(8), HL.data.Get(9), HL.data.Get(10), HL.data.Get(11), HL.data.Get(12), HL.data.Get(13), HL.data.Get(14), HL.data.Get(15) };
            }
            set
            {
                HL.data.Set(8, value[8]);
                HL.data.Set(9, value[9]);
                HL.data.Set(10, value[10]);
                HL.data.Set(11, value[11]);
                HL.data.Set(12, value[12]);
                HL.data.Set(13, value[13]);
                HL.data.Set(14, value[14]);
                HL.data.Set(15, value[15]);
            }
        }

        public bool[] L //Low
        {
            get
            {
                return new bool[] { HL.data.Get(0), HL.data.Get(1), HL.data.Get(2), HL.data.Get(3), HL.data.Get(4), HL.data.Get(5), HL.data.Get(6), HL.data.Get(7) };
            }
            set
            {
                HL.data.Set(0, value[0]);
                HL.data.Set(1, value[1]);
                HL.data.Set(2, value[2]);
                HL.data.Set(3, value[3]);
                HL.data.Set(4, value[4]);
                HL.data.Set(5, value[5]);
                HL.data.Set(6, value[6]);
                HL.data.Set(7, value[7]);
            }
        }

        #endregion //HL
        #region SP

        /// <summary>
        /// Full 16bit register for the Stack Pointer
        /// </summary>
        public bool[] SP
        {
            get
            {
                return new bool[] { _SP.data.Get(0), _SP.data.Get(1), _SP.data.Get(2), _SP.data.Get(3), _SP.data.Get(4), _SP.data.Get(5), _SP.data.Get(6), _SP.data.Get(7), _SP.data.Get(8), _SP.data.Get(9), _SP.data.Get(10), _SP.data.Get(11), _SP.data.Get(12), _SP.data.Get(13), _SP.data.Get(14), _SP.data.Get(15) };
                //return _SP.data;
            }
            set
            {
                _SP.data.Set(0, value[0]);
                _SP.data.Set(1, value[1]);
                _SP.data.Set(2, value[2]);
                _SP.data.Set(3, value[3]);
                _SP.data.Set(4, value[4]);
                _SP.data.Set(5, value[5]);
                _SP.data.Set(6, value[6]);
                _SP.data.Set(7, value[7]);
                _SP.data.Set(8, value[8]);
                _SP.data.Set(9, value[9]);
                _SP.data.Set(10, value[10]);
                _SP.data.Set(11, value[11]);
                _SP.data.Set(12, value[12]);
                _SP.data.Set(13, value[13]);
                _SP.data.Set(14, value[14]);
                _SP.data.Set(15, value[15]);
            }
        }

        #endregion //SP
        #region PC

        /// <summary>
        /// Full 16bit register for the Program Counter/Pointer
        /// </summary>
        public bool[] PC
        {
            get
            {
                return new bool[] { _PC.data.Get(0), _PC.data.Get(1), _PC.data.Get(2), _PC.data.Get(3), _PC.data.Get(4), _PC.data.Get(5), _PC.data.Get(6), _PC.data.Get(7), _PC.data.Get(8), _PC.data.Get(9), _PC.data.Get(10), _PC.data.Get(11), _PC.data.Get(12), _PC.data.Get(13), _PC.data.Get(14), _PC.data.Get(15) };
                //return _PC.data;
            }
            set
            {
                _PC.data.Set(0, value[0]);
                _PC.data.Set(1, value[1]);
                _PC.data.Set(2, value[2]);
                _PC.data.Set(3, value[3]);
                _PC.data.Set(4, value[4]);
                _PC.data.Set(5, value[5]);
                _PC.data.Set(6, value[6]);
                _PC.data.Set(7, value[7]);
                _PC.data.Set(8, value[8]);
                _PC.data.Set(9, value[9]);
                _PC.data.Set(10, value[10]);
                _PC.data.Set(11, value[11]);
                _PC.data.Set(12, value[12]);
                _PC.data.Set(13, value[13]);
                _PC.data.Set(14, value[14]);
                _PC.data.Set(15, value[15]);
            }
        }

        #endregion //PC
        #endregion //Registers
        #endregion //Accessiors
    }
}
