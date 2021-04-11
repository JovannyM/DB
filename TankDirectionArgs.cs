using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EventArgs
{
    class TankDirectionArgs
    {
        public int DX { get; set; }
        public int DY { get; set; }
        public TankDirectionArgs(int dx, int dy)
        {
            DX = dx;
            DY = dy;
        }
    }
}
