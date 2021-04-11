using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    class Resurses
    {
        public static SoundBuffer[] SoundBuffersTankShot { get; private set; } = new SoundBuffer[3];
        public static SoundBuffer[] SoundBuffersDeathEnemy { get; private set; } = new SoundBuffer[4];
        public static SoundBuffer[] SoundBuffersSplashEnemy { get; private set; } = new SoundBuffer[2];
        public static Music[] Musics { get; private set; } = new Music[4];
        public static Texture TankTexture { get; private set; }
        public static Texture TowerTexture { get; private set; }
        public static Texture TorTexture { get; private set; }
        public static Texture ETowerTexture { get; private set; }
        public static Texture BackTexture { get; private set; }
        public static Texture PlayerBullet { get; private set; }
        public static Texture EnemyBullet { get; private set; }
        public static Texture SpeedEnemyTexture { get; private set; }
        public static Texture HealthBar { get; private set; }
        public static Texture Baraban { get; private set; }
        public static Texture CDbaraban { get; private set; }
        public static Texture BackCDbaraban { get; private set; }
        public static Texture ExitButtom { get; private set; }
        public static Texture SaveButtom { get; private set; }
        public static Texture[] RangTexture { get; private set; } = new Texture[33];
        public static Font Font;
        private const string LocationTexture = "Textures/Objects/";        
        private const string LocationSound = "Sounds/";        

        public static void Load()
        {
            TankTexture = new Texture(LocationTexture + "Tank.png");
            TankTexture.Smooth = true;
            TowerTexture = new Texture(LocationTexture + "Tower.png");
            TowerTexture.Smooth = true;
            TorTexture = new Texture(LocationTexture + "EnemyTank.png");
            TorTexture.Smooth = true;
            ETowerTexture = new Texture(LocationTexture + "EnemyTower.png");
            TowerTexture.Smooth = true;
            Baraban = new Texture(LocationTexture + "Baraban.png");
            Baraban.Smooth = true;
            CDbaraban = new Texture(LocationTexture + "CDbaraban.png");
            BackCDbaraban = new Texture(LocationTexture + "BackCDbaraban.png");
            BackTexture = new Texture(LocationTexture + "noname.png");
            PlayerBullet = new Texture(LocationTexture + "Bullet.png");
            PlayerBullet.Smooth = true;
            EnemyBullet = new Texture(LocationTexture + "EnemyBullet.png");
            EnemyBullet.Smooth = true;
            SpeedEnemyTexture = new Texture(LocationTexture + "SpeedEnemy.png");
            SpeedEnemyTexture.Smooth = true;
            HealthBar = new Texture(LocationTexture + "PiuHealth.png");
            Font = new Font("19655.otf");
            ExitButtom = new Texture(LocationTexture + "exit.png");
            SaveButtom = new Texture(LocationTexture + "save.png");
            for (int i = 1; i <= 33; i++)
            {
                Texture toss = new Texture(LocationTexture + "Rangs/Rang" + i.ToString() + ".png");
                RangTexture[i - 1] = toss;
            }
            SoundBuffersTankShot[0] = new SoundBuffer(LocationSound + "chirp06.ogg");
            SoundBuffersTankShot[1] = new SoundBuffer(LocationSound + "chirp03.ogg");
            SoundBuffersTankShot[2] = new SoundBuffer(LocationSound + "chirp10.ogg");
            SoundBuffersDeathEnemy[0] = new SoundBuffer(LocationSound + "pain01.ogg");
            SoundBuffersDeathEnemy[1] = new SoundBuffer(LocationSound + "pain02.ogg");
            SoundBuffersDeathEnemy[2] = new SoundBuffer(LocationSound + "pain03.ogg");
            SoundBuffersDeathEnemy[3] = new SoundBuffer(LocationSound + "pain04.ogg");
            SoundBuffersSplashEnemy[0] = new SoundBuffer(LocationSound + "launch01.ogg");
            SoundBuffersSplashEnemy[1] = new SoundBuffer(LocationSound + "launch07.ogg");
            Musics[0] = new Music(LocationSound + "menu.ogg");
            Musics[1] = new Music(LocationSound + "1.ogg");
            Musics[2] = new Music(LocationSound + "2.ogg");
            Musics[3] = new Music(LocationSound + "3.ogg");
        }
    }
}
