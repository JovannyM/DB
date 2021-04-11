using DB.EnemiesFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class SpawnEnemyArgs
    {
        public Enemies Enemy { get; set; }
        public SpawnEnemyArgs(Enemies enemy)
        {
            Enemy = enemy;
        }
    }
}
