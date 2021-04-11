using DB.TankFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class SetInterfaceArgs
    {
        public CoolDownVaritableGroup CDVG { get; set; }
        public int TankHealth { get; set; }
        public int Rang { get; set; }
        public SetInterfaceArgs(CoolDownVaritableGroup cdvg, int tankHeath, int rang)
        {
            CDVG = cdvg;
            TankHealth = tankHeath;
            Rang = rang;
        }
    }
}
