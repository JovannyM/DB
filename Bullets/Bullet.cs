using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Bullets
{
    class Bullet
    {
        public Sprite Sprite { get; }
        public Vector2f Position { get { return Sprite.Position; } set { if (Sprite != null) Sprite.Position = value; } }
        public WhoIs WhoIs;

        private Vector2f NewPostion;
        private Vector2f Direction;
        private float Speed = 7f;

        public Bullet(Vector2f startPosition, float angleParent, Texture textureBullet, WhoIs isShut)
        {
            WhoIs = isShut;
            Direction = new Vector2f((float)Math.Cos(angleParent / 180 * 3.14159), (float)Math.Sin(angleParent / 180 * 3.14159));
            Sprite = new Sprite(textureBullet, new IntRect(0, 0, 22, 22))
            {
                Origin = new Vector2f(11, 11),
                Position = new Vector2f(startPosition.X + Direction.X * 110, startPosition.Y + Direction.Y * 110),
                Rotation = angleParent
            };
        }

        public void Move(Clock mainTime)
        {
            NewPostion.X = Speed * Direction.X * mainTime.ElapsedTime.AsMilliseconds() / 10f;
            NewPostion.Y = Speed * Direction.Y * mainTime.ElapsedTime.AsMilliseconds() / 10f;
            Sprite.Position += NewPostion;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
        }
    }
}
