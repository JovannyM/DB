using DB.Bullets;
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
    class ETower
    {
        public Sprite Sprite { get; private set; }
        public Vector2f Position { get { return Sprite.Position; } set { Sprite.Position = value; } }
        private RotationVaritableGroupForTower RVG;
        private float CD = 0;
        private float MainCD = 0;

        private Action<object, BulletSpawnArgs> Shoot;

        public ETower(Texture texture, Vector2f position, float mainCD, Action<object, BulletSpawnArgs> shoot)
        {
            Shoot = shoot;
            Sprite = new Sprite()
            {
                Position = position,
                TextureRect = new IntRect(0, 0, 132, 54),
                Origin = new Vector2f(27, 27),
                Texture = texture
            };
            RVG = new RotationVaritableGroupForTower();
            MainCD = mainCD;
        }

        public void Rotate(Vector2f positionPlayer)
        {
            RVG.pointX = positionPlayer.X;
            RVG.pointY = positionPlayer.Y;
            RVG.vector = new Vector(RVG.pointX, RVG.pointY) - new Vector(Sprite.Position.X, Sprite.Position.Y);
            RVG.end = Math.Round(Vector.AngleBetween(new Vector(1, 0), RVG.vector));
            RVG.rotation = Sprite.Rotation;
            if (Math.Abs(RVG.rotation) > 180) RVG.rotation += RVG.rotation < 0 ? 360 : -360;
            if (Math.Round(RVG.end) != RVG.rotation)
            {
                if (Math.Abs(RVG.rotation - RVG.end) > 180)
                    RVG.rotation += RVG.rotation < RVG.end ? -1 : 1;
                else
                    RVG.rotation += RVG.rotation < RVG.end ? 1 : -1;
            }
            Sprite.Rotation = RVG.rotation;
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
        }

        public void OnShoot(Vector2f position, Clock timeOneFrame)
        {            
            CD += timeOneFrame.ElapsedTime.AsSeconds();
            if (CD > MainCD)
            {
                CD = 0;
                Shoot(this, new BulletSpawnArgs(new Bullet(Position, Sprite.Rotation, Resurses.EnemyBullet, WhoIs._enemy)));
            }
        }
    }
}
