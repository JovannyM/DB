using DB.MyEventArgs;
using DB.TankFolder;
using SFML.Graphics;
using SFML.System;
using System;

namespace DB
{
    class GameInterface
    {
        private Sprite HealthSprite;
        private CircleShape First;
        private CircleShape Second;
        private CircleShape Third;
        private RectangleShape CoolDownRect;
        private RectangleShape BackCD;
        private string TimeText;
        private string RangText;
        private Text CoolDown, FirstCoolDown, SecondCoolDown, ThirdCoolDown, Rang;

        private int LocalRang = 10;
        private Sprite RangSprite;

        public RectangleShape ExitButtom { get; private set; }
        public RectangleShape SaveButtom { get; private set; }

        public GameInterface(int tankHealth)
        {
            {
                First = new CircleShape()
                {
                    Origin = new Vector2f(75, 75),
                    Radius = 75f,
                    Texture = Resurses.Baraban
                };
                Second = new CircleShape()
                {
                    Origin = new Vector2f(75, 75),
                    Radius = 75f,
                    Texture = Resurses.Baraban
                };
                Third = new CircleShape()
                {
                    Origin = new Vector2f(75, 75),
                    Radius = 75f,
                    Texture = Resurses.Baraban
                };
                CoolDownRect = new RectangleShape(new Vector2f(77, 184))
                {
                    Origin = new Vector2f(77, 92),
                    Texture = Resurses.CDbaraban
                };
                BackCD = new RectangleShape(new Vector2f(300, 184))
                {
                    Origin = new Vector2f(300, 92),
                    Texture = Resurses.BackCDbaraban
                };
            }

            {
                CoolDown = new Text(TimeText, new Font("19655.otf"))
                {
                    Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 350),
                    CharacterSize = 14
                };
                ThirdCoolDown = new Text(TimeText, new Font("19655.otf"))
                {
                    Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 330),
                    CharacterSize = 14
                };
                SecondCoolDown = new Text(TimeText, new Font("19655.otf"))
                {
                    Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 310),
                    CharacterSize = 14
                };
                FirstCoolDown = new Text(TimeText, new Font("19655.otf"))
                {
                    Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 290),
                    CharacterSize = 14
                };
            }

            Rang = new Text(RangText, new Font("19655.otf"))
            {
                Position = new Vector2f(Game.MainView.Center.X - 50, Game.MainView.Center.Y + 200),
                CharacterSize = 14
            };
            RangSprite = new Sprite()
            {
                TextureRect = new IntRect(0, 0, 318, 110),
                Scale = new Vector2f((float)0.5, (float)0.5),
                Texture = Resurses.RangTexture[0]
            };

            {
                ExitButtom = new RectangleShape()
                {
                    Size = new Vector2f(157, 70),
                    Texture = Resurses.ExitButtom,
                    Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 350),
                };
                SaveButtom = new RectangleShape()
                {
                    Size = new Vector2f(215, 70),
                    Texture = Resurses.SaveButtom,
                    Position = new Vector2f(Game.MainView.Center.X - 500, Game.MainView.Center.Y - 350),
                };
            }

            HealthSprite = new Sprite()
            {
                TextureRect = new IntRect(0, 0, tankHealth, 20),
                Texture = Resurses.HealthBar,
                Position = new Vector2f(Game.MainView.Center.X - 50, Game.MainView.Center.Y + 55)
            };
        }

        public void SetInterface(Object sender, SetInterfaceArgs arg)
        {
            CoolDownRect.Position = new Vector2f(arg.CDVG.MCD * 100 / Tower.MainCoolDown + Game.MainView.Center.X - 600, Game.MainView.Center.Y);
            First.Position = new Vector2f(arg.CDVG.FCD * 100 / Tower.CoolDownFirstBullet + Game.MainView.Center.X - 600, Game.MainView.Center.Y - 60);
            Second.Position = new Vector2f(arg.CDVG.SCD * 100 / Tower.CoolDownSecondBullet + Game.MainView.Center.X - 600, Game.MainView.Center.Y);
            Third.Position = new Vector2f(arg.CDVG.TCD * 100 / Tower.CoolDownThirdBullet + Game.MainView.Center.X - 600, Game.MainView.Center.Y + 60);
            BackCD.Position = new Vector2f(Game.MainView.Center.X - 500, Game.MainView.Center.Y);

            ExitButtom.Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y - 350);
            SaveButtom.Position = new Vector2f(Game.MainView.Center.X - 470, Game.MainView.Center.Y - 350);

            TimeText = "КД между выстрелами = " + arg.CDVG.MCD + ";";
            CoolDown = new Text(TimeText, Resurses.Font)
            {
                Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y + 340),
                CharacterSize = 14
            };
            TimeText = "Третий снаряд заряжен на: " + (int)arg.CDVG.TCD + ";";
            ThirdCoolDown = new Text(TimeText, Resurses.Font)
            {
                Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y + 320),
                CharacterSize = 14
            };
            TimeText = "Второй снаряд заряжен на: " + (int)arg.CDVG.SCD + ";";
            SecondCoolDown = new Text(TimeText, Resurses.Font)
            {
                Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y + 300),
                CharacterSize = 14
            };
            TimeText = "Первый снаряд заряжен на: " + (int)arg.CDVG.FCD + ";";
            FirstCoolDown = new Text(TimeText, Resurses.Font)
            {
                Position = new Vector2f(Game.MainView.Center.X - 630, Game.MainView.Center.Y + 280),
                CharacterSize = 14
            };
            RangText = "Текущий ранг: " + arg.Rang + ";";
            Rang = new Text(RangText, Resurses.Font)
            {
                Position = new Vector2f(Game.MainView.Center.X - 50, Game.MainView.Center.Y + 200),
                CharacterSize = 14
            };
            SetRang(arg.Rang);
            HealthSprite.TextureRect = new IntRect(0, 0, arg.TankHealth * 10, 20);
            HealthSprite.Position = new Vector2f(Game.MainView.Center.X - 50, Game.MainView.Center.Y + 50);

        }

        private void SetRang(int thisrang)
        {
            if (LocalRang < 330 && thisrang >= LocalRang)
            {
                LocalRang += 10;
                RangSprite.Texture = Resurses.RangTexture[(LocalRang / 10) - 1];
            }
            RangSprite.Position = new Vector2f(Game.MainView.Center.X - 100, Game.MainView.Center.Y - 340);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(BackCD);
            window.Draw(First);
            window.Draw(Second);
            window.Draw(Third);
            window.Draw(CoolDownRect);
            window.Draw(ExitButtom);
            window.Draw(SaveButtom);
            window.Draw(HealthSprite);
            //window.Draw(CoolDown);
            //window.Draw(ThirdCoolDown);
            //window.Draw(SecondCoolDown);
            //window.Draw(FirstCoolDown);
            //window.Draw(Rang);
            window.Draw(RangSprite);
        }
    }
}
