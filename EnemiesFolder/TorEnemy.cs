using DB.MyEventArgs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DB.EnemiesFolder
{
    class TorEnemy : Enemies
    {
        public TorEnemy(Vector2f position, Action<object, BulletSpawnArgs> shoot)
        {
            Sprite = new Sprite()
            {
                Texture = Resurses.TorTexture,
                TextureRect = new IntRect(0, 0, 158, 84),
                Origin = new Vector2f(79, 42),
                Position = position
            };
            Speed = 0.5f;
            Heath = 3f;
            ETowers = new ETower[1];
            ETowers[0] = new ETower(Resurses.ETowerTexture, Position, 2, shoot);
            RVG = new RotationVaritableGroup();
        }

        public override void Move(Vector2f positionPlayer, Clock time)
        {
            RVG.vector = new Vector(positionPlayer.X - Position.X, positionPlayer.Y - Position.Y);
            RVG.vector.Normalize();
            int newX = (int)Math.Round(Speed * RVG.vector.X * time.ElapsedTime.AsMilliseconds() / 10f);
            int newY = (int)Math.Round(Speed * RVG.vector.Y * time.ElapsedTime.AsMilliseconds() / 10f);
            Position += new Vector2f(newX, newY);

            RVG.speed = Speed * 200 * time.ElapsedTime.AsSeconds();
            RVG.end = Math.Round(Vector.AngleBetween(new Vector(1, 0), RVG.vector));
            RVG.rotation = Sprite.Rotation;
            if (Math.Abs(RVG.rotation) > 180) RVG.rotation += RVG.rotation < 0 ? 360 : -360;
            if (Math.Round(RVG.end) != RVG.rotation)
            {
                if (Math.Abs(RVG.rotation - RVG.end) > 180)
                    RVG.rotation += RVG.rotation < RVG.end ? -1 * RVG.speed : RVG.speed;
                else
                    RVG.rotation += RVG.rotation < RVG.end ? RVG.speed : -1 * RVG.speed;
            }
            if (Math.Abs(RVG.end - Sprite.Rotation) < RVG.speed)
            {
                Sprite.Rotation = (float)RVG.end;
            }
            else
            {
                Sprite.Rotation = RVG.rotation;
            }
            ETowers[0].Position = Position;
            ETowers[0].Rotate(positionPlayer);

        }

        public override void Hit()
        {
            Heath--;
        }

        public override void Draw(RenderWindow window)
        {            
            window.Draw(Sprite);
            ETowers[0].Draw(window);
        }
    }
}
