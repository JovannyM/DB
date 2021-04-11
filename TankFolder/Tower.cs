using DB.Bullets;
using DB.MyEventArgs;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DB.TankFolder
{
    public struct CoolDownVaritableGroup
    {
        public float MCD;
        public float FCD;
        public float SCD;
        public float TCD;
        public CoolDownVaritableGroup(float cds, float fcd, float scd, float tcd)
        {
            MCD = cds;
            FCD = fcd;
            SCD = scd;
            TCD = tcd;
        }
    }

    class Tower
    {
        public event EventHandler<BulletSpawnArgs> BulletSpawnEvent;
        public Sprite Sprite { get; set; }
        public Vector2f Position { get { return Sprite.Position; } set { Sprite.Position = value; } }        
        public CoolDownVaritableGroup CDVG { get; private set; }
        public static float MainCoolDown { get; private set; } = 1f;
        public static float CoolDownFirstBullet { get; private set; } = 1f;
        public static float CoolDownSecondBullet { get; private set; } = 2f;
        public static float CoolDownThirdBullet { get; private set; }= 3f;

        private Sound ShotSound;
        private Random Rnd;

        private RotationVaritableGroupForTower RVGTower;

        public Tower(Vector2f position)
        {
            CDVG = new CoolDownVaritableGroup(0, 0, 0, 0);
            Sprite = new Sprite(Resurses.TowerTexture, new IntRect(0, 0, 132, 54))
            {
                Origin = new Vector2f(27, 27),
                Position = position
            };
            ShotSound = new Sound();
            Rnd = new Random();
        }

        public void CDUP()
        {
            CDVG = new CoolDownVaritableGroup(1, 2, 4, 5);
        }

        public void Shot(object sender, MouseButtonEventArgs arg)
        {
            if (Game.Level > 0 && arg.Button == Mouse.Button.Left)
            {
                if (CDVG.MCD >= MainCoolDown)
                {
                    if(CDVG.FCD >= CoolDownFirstBullet)
                    {
                        BulletSpawnEvent.Invoke(this, new BulletSpawnArgs(new Bullet(Position, Sprite.Rotation, Resurses.PlayerBullet, WhoIs._player)));
                        CDVG = new CoolDownVaritableGroup(0, 0, CDVG.SCD, CDVG.TCD);
                        ShotSound.SoundBuffer = Resurses.SoundBuffersTankShot[Rnd.Next(0, 3)];
                        ShotSound.Play();
                    }
                    else if(CDVG.SCD >= CoolDownSecondBullet)
                    {
                        BulletSpawnEvent.Invoke(this, new BulletSpawnArgs(new Bullet(Position, Sprite.Rotation, Resurses.PlayerBullet, WhoIs._player)));
                        CDVG = new CoolDownVaritableGroup(0, 0, 0, CDVG.TCD);
                        ShotSound.SoundBuffer = Resurses.SoundBuffersTankShot[Rnd.Next(0, 3)];
                        ShotSound.Play();
                    }
                    else if(CDVG.TCD >= CoolDownThirdBullet)
                    {
                        BulletSpawnEvent.Invoke(this, new BulletSpawnArgs(new Bullet(Position, Sprite.Rotation, Resurses.PlayerBullet, WhoIs._player)));
                        CDVG = new CoolDownVaritableGroup(0, 0, 0, 0);
                        ShotSound.SoundBuffer = Resurses.SoundBuffersTankShot[Rnd.Next(0, 3)];
                        ShotSound.Play();
                    }
                }
            }
            //else if (Game.Level > 0 && arg.Button == Mouse.Button.Right)
            //{
            //    BulletSpawnEvent.Invoke(this, new BulletSpawnArgs(new Bullet(Position, Sprite.Rotation, Resurses.PlayerBullet, WhoIs._player)));
            //    ShotSound.SoundBuffer = Resurses.SoundBuffersTankShot[Rnd.Next(0, 3)];
            //    ShotSound.Play();
            //}
            //Это оставили что бы стрелять без перезарядки
        }

        public void CD(object sender, CoolDownShotArgs arg)
        {
            if (CDVG.MCD >= MainCoolDown)
            {
                CDVG = new CoolDownVaritableGroup(MainCoolDown, CDVG.FCD, CDVG.SCD, CDVG.TCD);
                if (CDVG.TCD >= CoolDownThirdBullet)
                {
                    CDVG = new CoolDownVaritableGroup(CDVG.MCD, CDVG.FCD, CDVG.SCD, CoolDownThirdBullet);
                    if (CDVG.SCD >= CoolDownSecondBullet)
                    {
                        CDVG = new CoolDownVaritableGroup(CDVG.MCD, CDVG.FCD, CoolDownSecondBullet, CDVG.TCD);
                        if (CDVG.FCD >= CoolDownFirstBullet)
                            CDVG = new CoolDownVaritableGroup(CDVG.MCD, CoolDownFirstBullet, CDVG.SCD, CDVG.TCD);
                        else CDVG = new CoolDownVaritableGroup(CDVG.MCD, CDVG.FCD + arg.FrameTime.ElapsedTime.AsSeconds(), CDVG.SCD, CDVG.TCD);
                    }
                    else CDVG = new CoolDownVaritableGroup(CDVG.MCD, CDVG.FCD, CDVG.SCD + arg.FrameTime.ElapsedTime.AsSeconds(), CDVG.TCD);
                }
                else CDVG = new CoolDownVaritableGroup(CDVG.MCD, CDVG.FCD, CDVG.SCD, CDVG.TCD + arg.FrameTime.ElapsedTime.AsSeconds());
            }
            else CDVG = new CoolDownVaritableGroup(CDVG.MCD + arg.FrameTime.ElapsedTime.AsSeconds(), CDVG.FCD, CDVG.SCD, CDVG.TCD);
        }

        public void Rotation(Object sender, TowerRotateArgs arg)
        {
            RVGTower.pointX = Mouse.GetPosition(arg.Window).X + arg.Window.GetView().Center.X - arg.Window.GetView().Size.X / 2;
            RVGTower.pointY = Mouse.GetPosition(arg.Window).Y + arg.Window.GetView().Center.Y - arg.Window.GetView().Size.Y / 2;
            RVGTower.vector = new Vector(RVGTower.pointX, RVGTower.pointY) - new Vector(Sprite.Position.X, Sprite.Position.Y);
            RVGTower.end = Math.Round(Vector.AngleBetween(new Vector(1, 0), RVGTower.vector));
            RVGTower.rotation = Sprite.Rotation;
            if (Math.Abs(RVGTower.rotation) > 180) RVGTower.rotation += RVGTower.rotation < 0 ? 360 : -360;
            if (Math.Round(RVGTower.end) != RVGTower.rotation)
            {
                if (Math.Abs(RVGTower.rotation - RVGTower.end) > 180) RVGTower.rotation += RVGTower.rotation < RVGTower.end ? -1 : 1;
                else RVGTower.rotation += RVGTower.rotation < RVGTower.end ? 1 : -1;
            }
            Sprite.Rotation = RVGTower.rotation;
        }
        
        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
        }
    }
}
