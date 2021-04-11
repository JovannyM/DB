using DB.Bullets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class BulletSpawnArgs
    {
        public Bullet Bullet { get; private set; }
        public BulletSpawnArgs(Bullet bullet)
        {
            Bullet = bullet;
        }
    }
}
