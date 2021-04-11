using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class TankSubstructHealthArgs
    {
        public int Health { get; set; }
        public TankSubstructHealthArgs(int health)
        {
            Health = health;
        }
    }
}
