using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class TankSpawnArgs
    {
        public Vector2f SpawnPoint;
        public TankSpawnArgs(Vector2f point)
        {
            SpawnPoint = point;
        }
    }
}
