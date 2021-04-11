using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.MyEventArgs
{
    class CoolDownShotArgs
    {
        public Clock FrameTime { get; set; }
        public CoolDownShotArgs(Clock time)
        {
            FrameTime = time;
        }
    }
}
