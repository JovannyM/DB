using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EnemiesFolder
{
    class Enemies
    {
        public Sprite Sprite { get; protected set; }
        public Vector2f Position { get { return Sprite.Position; } protected set { Sprite.Position = value; } }
        public ETower[] ETowers { get; protected set; }

        protected RotationVaritableGroup RVG;
        public float Heath { get; protected set; }

        protected float Speed;

        public virtual void Move(Vector2f positionPlayer, Clock time) { }
        public virtual void Hit() { }
        //public virtual void Shot() { };
        public virtual void Draw(RenderWindow window) { }
    }
}
