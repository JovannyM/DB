using DB.Bullets;
using DB.EnemiesFolder;
using DB.MyEventArgs;
using DB.TankFolder;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace DB
{
    enum WhoIs
    {
        _player,
        _enemy
    }
    public struct RotationVaritableGroup
    {
        public float speed;
        public Vector vector;
        public double end;
        public float rotation;
    }
    public struct RotationVaritableGroupForTower
    {
        public float pointX;
        public float pointY;
        public Vector vector;
        public double end;
        public float rotation;
    }
    public struct Parametres
    {
        public double X1, Y1, X2, Y2, X3, Y3, X4, Y4;
    }

    class Game
    {
        public event EventHandler<TankMoveArgs> TankMoveEvent;
        public event EventHandler<TowerRotateArgs> TowerRotateEvent;
        public event EventHandler<TankSpawnArgs> TankSpawnEvent;
        public event EventHandler<TankSubstructHealthArgs> TankSubstractHealthEvent;
        public event EventHandler<CoolDownShotArgs> CoolDownShotEvent;
        public event EventHandler<SetInterfaceArgs> SetNewInterfaceEvent;
        public static View MainView { get; private set; }
        public Clock TimeOneFrame { get; }
        public static int Level { get; private set; } = 0;

        private readonly RenderWindow Window;
        private TMX TMXMap;
        private readonly string[] LevelName = new string[4] { "Level_0.tmx", "Level_1.tmx", "Level_2.tmx", "Level_3.tmx" };

        private readonly GameInterface GameInterface;
        private readonly Tank Tank;
        private List<Bullet> AllBullets;
        private List<Bullet> DeleteBullets;
        private List<Enemies> Enemies;
        private List<Enemies> DeleteEnemies;
        private Parametres situation;
        private int Rang = 0;

        private Sound DeathSound;
        private Sound SplashSound;
        private Random Rnd;

        public Game()
        {
            Window = new RenderWindow(new VideoMode(1280, 720), "DigitalBattle", Styles.Close);
            Window.SetFramerateLimit(60);
            Window.SetVerticalSyncEnabled(true);
            Window.SetKeyRepeatEnabled(false);
            MainView = new View(new Vector2f(640, 360), new Vector2f(1280, 720));
            Window.SetView(MainView);
            Resurses.Load();
            
            Tank = new Tank(new Vector2f(-250, -250));
            TimeOneFrame = new Clock();
            AllBullets = new List<Bullet>();
            DeleteBullets = new List<Bullet>();
            Enemies = new List<Enemies>();
            DeleteEnemies = new List<Enemies>();
            situation = new Parametres();

            GameInterface = new GameInterface(Tank.Health);

            TMXMap = new TMX();
            TMXMap.Load(LevelName[0], SpawnNewBullet);

            DeathSound = new Sound();
            SplashSound = new Sound();
            Rnd = new Random();

            Window.Closed += WindowClosed;
            Window.KeyPressed += KeyPress;
            Window.KeyPressed += Tank.KeyPress;
            Window.KeyReleased += Tank.KeyReleas;
            Window.MouseButtonPressed += Tank.Tower.Shot;
            Window.MouseButtonPressed += ChoseBottomForGame;
            TankMoveEvent += Tank.Move;
            Tank.Tower.BulletSpawnEvent += SpawnNewBullet;
            TankSpawnEvent += Tank.Spawn;
            TankSubstractHealthEvent += Tank.SubstractHealth;
            TowerRotateEvent += Tank.Tower.Rotation;
            TMXMap.SpawnEnemyEvent += SpawnEnemy;
            CoolDownShotEvent += Tank.Tower.CD;
            SetNewInterfaceEvent += GameInterface.SetInterface;

            Resurses.Musics[0].Loop = true;
            Resurses.Musics[0].Play();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Window.Clear();

                GamePlay();
                Draw();

                TimeOneFrame.Restart();
                Window.Display();
            }
        }

        private void GamePlay()
        {
            if (Level > 0)
            {
                CoolDownShotEvent.Invoke(this, new CoolDownShotArgs(TimeOneFrame));
                SetNewInterfaceEvent.Invoke(this, new SetInterfaceArgs(Tank.Tower.CDVG, Tank.Health, Rang));
                TankMoveEvent.Invoke(this, new TankMoveArgs(TimeOneFrame, MainView, TankNoCollisionWalls));
                TowerRotateEvent.Invoke(this, new TowerRotateArgs(Window));
                BulletsCollisionAndMove();
                CollisionEnemyAndTank();
                if (Level < LevelName.Length - 1 && TankIntersectWithDoors()) NextLevel();   
            }
        }

        private bool TankIntersectWithDoors()
        {
            foreach(var door in TMXMap.LevelDoors) 
                if (Intersects(Tank.Bounds[0], door)) return true;
            return false;
        }

        private void CollisionEnemyAndTank()
        {
            foreach (Enemies enemy in Enemies)
            {
                if (Intersects(enemy.Sprite, Tank.Bounds[0]))
                {
                    Rang++;
                    DeleteEnemies.Add(enemy);
                    TankSubstractHealthEvent.Invoke(this, new TankSubstructHealthArgs(1));
                    DeathSound.SoundBuffer = Resurses.SoundBuffersDeathEnemy[Rnd.Next(0, 4)];
                    DeathSound.Play();
                }
                else if (new Vector(enemy.Position.X - Tank.Position.X, enemy.Position.Y - Tank.Position.Y).Length <= 700 && !ChekVisibleTankAndEnemy(Tank.Position, enemy.Position))
                {
                    enemy.Move(Tank.Position, TimeOneFrame);
                    if (enemy is TorEnemy)
                    {
                        for (int i = 0; i < enemy.ETowers.Length; i++)
                        {
                            enemy.ETowers[i].OnShoot(Tank.Position, TimeOneFrame);
                            SplashSound.SoundBuffer = Resurses.SoundBuffersSplashEnemy[Rnd.Next(0, 2)];
                            SplashSound.Play();
                        }
                    }
                }
                        
            }
            foreach (Enemies enemy in DeleteEnemies) Enemies.Remove(enemy);
            DeleteEnemies.Clear();
            if (Tank.Health <= 0) { TankDestroy(); return; }
        }

        private void SpawnEnemy(object sender, SpawnEnemyArgs e) => Enemies.Add(e.Enemy);

        private void ChoseBottomForGame(object sender, MouseButtonEventArgs e)
        {
            float MousOfViewX = Mouse.GetPosition(Window).X + Window.GetView().Center.X - Window.GetView().Size.X / 2;
            float MousOfViewY = Mouse.GetPosition(Window).Y + Window.GetView().Center.Y - Window.GetView().Size.Y / 2;
            if (Level == 0 && e.Button == Mouse.Button.Left && MousOfViewX <= 880)
            {
                if (MousOfViewY <= 235 && MousOfViewX >= 347 && MousOfViewY >= 100) NextLevel();
                else if (MousOfViewY <= 430 && MousOfViewX >= 349 && MousOfViewY >= 290) XMLLoad();
                else if (MousOfViewY <= 620 && MousOfViewX >= 347 && MousOfViewY >= 481) Window.Close();
            }
            else if (PointOfRectangle(GameInterface.ExitButtom, MousOfViewX, MousOfViewY)) Window.Close();
            else if (PointOfRectangle(GameInterface.SaveButtom, MousOfViewX, MousOfViewY)) XMLSave();
        }

        private bool PointOfRectangle(RectangleShape rect, float mousX, float mousY)
        {
            return mousX <= rect.Position.X + rect.Size.X && mousX >= rect.Position.X && mousY <= rect.Position.Y + rect.Size.Y && mousY >= rect.Position.Y;
        }

        private void NextLevel()
        {
            Resurses.Musics[Level].Loop = false;
            Resurses.Musics[Level].Stop();
            Enemies.Clear();
            AllBullets.Clear();
            Level++;
            Resurses.Musics[Level].Loop = true;
            Resurses.Musics[Level].Play();
            TMXMap.Load(LevelName[Level], SpawnNewBullet);
            TankSpawnEvent.Invoke(this, new TankSpawnArgs(TMXMap.StartPositionForTank));
        }

        private void TankDestroy()
        {
            TankSubstractHealthEvent.Invoke(this, new TankSubstructHealthArgs(-10));
            Tank.Position = new Vector2f(-250, -250);
            Enemies.Clear();
            AllBullets.Clear();            
            Resurses.Musics[Level].Loop = false;
            Resurses.Musics[Level].Stop();
            Level = 0;
            Resurses.Musics[Level].Loop = true;
            Resurses.Musics[Level].Play();
            TMXMap.Load(LevelName[0], SpawnNewBullet);
            MainView = new View(new Vector2f(640, 360), new Vector2f(1280, 720));            
        }

        private void XMLLoad()
        {
            XmlDocument LoadGame = new XmlDocument();
            try
            {
                LoadGame.Load("SaveGame.xml");
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlElement MetaData = LoadGame.CreateElement("MetaData");
                XmlElement LevelData = LoadGame.CreateElement("Level");
                LevelData.InnerText = "1";
                XmlElement RangData = LoadGame.CreateElement("Rang");
                RangData.InnerText = "0";

                MetaData.AppendChild(LevelData);
                MetaData.AppendChild(RangData);
                LoadGame.AppendChild(MetaData);
                LoadGame.Save("SaveGame.xml");
                LoadGame.Load("SaveGame.xml");
            }
            Resurses.Musics[Level].Loop = false;
            Resurses.Musics[Level].Stop();
            XmlElement LoadData = LoadGame.DocumentElement;
            foreach (XmlNode Node in LoadData)
            {
                if (Node.Name == "Level") Level = Int32.Parse(Node.InnerText.ToString());
                if (Node.Name == "Rang") Rang = Int32.Parse(Node.InnerText.ToString());
            }
            Resurses.Musics[Level].Loop = true;
            Resurses.Musics[Level].Play();
            TMXMap.Load(LevelName[Level], SpawnNewBullet);
            TankSpawnEvent.Invoke(this, new TankSpawnArgs(TMXMap.StartPositionForTank));
        }

        private void XMLSave()
        {
            XmlDocument LoadGame = new XmlDocument();
            XmlElement MetaData = LoadGame.CreateElement("MetaData");
            XmlElement LevelData = LoadGame.CreateElement("Level");
            LevelData.InnerText = Level.ToString();
            XmlElement RangData = LoadGame.CreateElement("Rang");
            RangData.InnerText = Rang.ToString();

            MetaData.AppendChild(LevelData);
            MetaData.AppendChild(RangData);
            LoadGame.AppendChild(MetaData);
            LoadGame.Save("SaveGame.xml");
        }

        private void BulletsCollisionAndMove()
        {
            foreach (Bullet bullet in AllBullets)
            {
                foreach(Sprite wall in TMXMap.Walls) // стены
                {
                    if (Intersects(bullet.Sprite, wall)) DeleteBullets.Add(bullet);
                }
                if (!DeleteBullets.Contains(bullet) && bullet.WhoIs == WhoIs._enemy && Intersects(bullet.Sprite, Tank.Bounds[0])) //Игрок
                {
                    if (Tank.Health <= 0) TankDestroy();
                    TankSubstractHealthEvent.Invoke(this, new TankSubstructHealthArgs(5));
                    DeleteBullets.Add(bullet);
                }
                if(!DeleteBullets.Contains(bullet) && bullet.WhoIs == WhoIs._player) //Противник
                {
                    foreach (Enemies enemy in Enemies)
                    {
                        if (Intersects(bullet.Sprite, enemy.Sprite))
                        {
                            Rang++;
                            if (enemy.Heath <= 1)
                            {
                                DeleteEnemies.Add(enemy); 
                                DeleteBullets.Add(bullet);
                                DeathSound.SoundBuffer = Resurses.SoundBuffersDeathEnemy[Rnd.Next(0, 4)];
                                DeathSound.Play();
                            }                            
                            else
                            {
                                enemy.Hit();
                                DeleteBullets.Add(bullet);
                            }
                            
                        }
                    }
                }                
            }
            foreach (Bullet bullet in DeleteBullets) AllBullets.Remove(bullet);
            DeleteBullets.Clear();
            foreach (Enemies enemy in DeleteEnemies) Enemies.Remove(enemy);
            DeleteEnemies.Clear();
            foreach (Bullet bullet in AllBullets) bullet.Move(TimeOneFrame);
        }

        private PushOutArgs TankNoCollisionWalls(Sprite[] bounds)
        {
            foreach (Sprite wall in TMXMap.Walls)
            {
                for (int i = 0; i < bounds.Length; i++)
                {
                    if (wall.GetGlobalBounds().Intersects(bounds[i].GetGlobalBounds(), out FloatRect rect))
                    {
                        double RectX = rect.Left + rect.Width / 2;
                        double RectY = rect.Top + rect.Height / 2;
                        if (RectX < bounds[i].Position.X) //точно слева
                        {
                            if (RectY < bounds[i].Position.Y + bounds[i].Origin.Y && RectY > bounds[i].Position.Y - bounds[i].Origin.Y) 
                                return new PushOutArgs(new Vector2f(rect.Width, 0));
                        }
                        if (RectX > bounds[i].Position.X) //точно справа
                        {
                            if (RectY < bounds[i].Position.Y + bounds[i].Origin.Y && RectY > bounds[i].Position.Y - bounds[i].Origin.Y) 
                                return new PushOutArgs(new Vector2f(-rect.Width, 0));
                        }
                        if (RectY > bounds[i].Position.Y) //точно снизу
                        {
                            if (RectX < bounds[i].Position.X + bounds[i].Origin.X && RectX > bounds[i].Position.X - bounds[i].Origin.X)
                                return new PushOutArgs(new Vector2f(0, -rect.Height));
                        }
                        if (RectY < bounds[i].Position.Y) //точно сверху
                        {
                            if (RectX < bounds[i].Position.X + bounds[i].Origin.X && RectX > bounds[i].Position.X - bounds[i].Origin.X)
                                return new PushOutArgs(new Vector2f(0, rect.Height));
                        }
                    }
                }
            }
            return null;
        }

        private bool Intersects(Sprite first, Sprite second)
        {
            return first.GetGlobalBounds().Intersects(second.GetGlobalBounds());
        }       

        private void SpawnNewBullet(object sender, BulletSpawnArgs arg)
        {
            AllBullets.Add(arg.Bullet);
        }
        
        private bool ChekVisibleTankAndEnemy(Vector2f firstPos, Vector2f secondPos)
        {
            situation.X1 = firstPos.X;
            situation.Y1 = firstPos.Y;
            situation.X2 = secondPos.X;
            situation.Y2 = secondPos.Y;
            foreach (Sprite wall in TMXMap.Walls)
            {                
                situation.X3 = wall.Position.X;
                situation.Y3 = wall.Position.Y;
                situation.X4 = wall.Position.X;
                situation.Y4 = wall.Position.Y + wall.TextureRect.Height;
                if (SomeInterspect(situation)) return true;
                situation.X3 = wall.Position.X;
                situation.Y3 = wall.Position.Y + wall.TextureRect.Height;
                situation.X4 = wall.Position.X + wall.TextureRect.Width;
                situation.Y4 = wall.Position.Y + wall.TextureRect.Height;
                if (SomeInterspect(situation)) return true;
                situation.X3 = wall.Position.X + wall.TextureRect.Width;
                situation.Y3 = wall.Position.Y;
                situation.X4 = wall.Position.X + wall.TextureRect.Width;
                situation.Y4 = wall.Position.Y + wall.TextureRect.Height;
                if (SomeInterspect(situation)) return true;
                situation.X3 = wall.Position.X;
                situation.Y3 = wall.Position.Y;
                situation.X4 = wall.Position.X + wall.TextureRect.Width;
                situation.Y4 = wall.Position.Y;
                if (SomeInterspect(situation)) return true;
            }
            return false;
        }

        private bool SomeInterspect(Parametres param)
        {
            double firstVec = Vector.CrossProduct(new Vector(param.X4 - param.X3, param.Y4 - param.Y3), new Vector(param.X1 - param.X3, param.Y1 - param.Y3));
            double secodVec = Vector.CrossProduct(new Vector(param.X4 - param.X3, param.Y4 - param.Y3), new Vector(param.X2 - param.X3, param.Y2 - param.Y3));
            double third = Vector.CrossProduct(new Vector(param.X2 - param.X1, param.Y2 - param.Y1), new Vector(param.X3 - param.X1, param.Y3 - param.Y1));
            double fourth = Vector.CrossProduct(new Vector(param.X2 - param.X1, param.Y2 - param.Y1), new Vector(param.X4 - param.X1, param.Y4 - param.Y1));
            return (Math.Sign(firstVec) != Math.Sign(secodVec) && Math.Sign(third) != Math.Sign(fourth));
        }

        private void Draw()
        {
            Window.SetView(MainView);
            TMXMap.TMXDraw(Window);
            if (Level > 0)
            {
                Tank.Draw(Window);
                foreach (Enemies enemy in Enemies) enemy.Draw(Window);
                foreach (Bullet bullet in AllBullets) bullet.Draw(Window);
                GameInterface.Draw(Window);
            }
        }

        private void KeyPress(object sender, KeyEventArgs arg)
        {
            if (arg.Code == Keyboard.Key.Escape) Window.Close();
            //if (arg.Code == Keyboard.Key.Space && Rang <= 310) Rang += 10;
        }

        private void WindowClosed(object sender, EventArgs arg)
        {
            Window.Close();
        }
    }
}
