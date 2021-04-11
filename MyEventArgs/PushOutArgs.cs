using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class PushOutArgs
    {
        public Vector2f Rect { get; set; }
        public PushOutArgs(Vector2f rect)
        {
            Rect = rect;
        }
    }
}
