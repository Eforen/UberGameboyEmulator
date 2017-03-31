using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberGameboyEmulator
{
    class Register16Bit
    {
        DataBus16Bit busIn;
        DataBus16Bit busOut;

        protected bool _read;
        protected bool _write;

        public BitArray data = new BitArray(16);
        public Register16Bit(DataBus16Bit busIn, DataBus16Bit busOut)
        {
            this.busIn = busIn;
            this.busOut = busOut;
        }

        public void clock()
        {
            if (_read)
            {
                //set data onto the out bus
                for (int i = 0; i < 16; i++)
                {
                    busOut.bus.Set(i, data.Get(i));
                }
            }
            if (_write)
            {
                //set data from the in bus
                for (int i = 0; i < 16; i++)
                {
                    busIn.bus.Set(i, data.Get(i));
                }
            }
        }

        public bool enable
        {
            get
            {
                return _read;
            }
            set
            {
                _read = value;
            }
        }
        public bool set
        {
            get
            {
                return _write;
            }
            set
            {
                _write = value;
            }
        }
    }
}
