using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class TankMoveArgs
    {
        public Clock Time { get; set; }
        public View View { get; set; }
        public delegate PushOutArgs FuckingMethod(Sprite[] bounds);
        public FuckingMethod Method;
        public TankMoveArgs(Clock time, View view, FuckingMethod method)
        {
            Time = time;
            View = view;
            Method = method;           
        }

    }
}
