using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class TowerRotateArgs
    {
        public RenderWindow Window { get; set; }
        public TowerRotateArgs(RenderWindow win)
        {
            Window = win;
        }
    }
}
