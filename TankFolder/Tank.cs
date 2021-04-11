using DB.MyEventArgs;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Windows;
using System.Windows.Media;

namespace DB.TankFolder
{
    class Tank
    {
        public Sprite Sprite { get; set;  }
        public Sprite[] Bounds { get; set; } = new Sprite[5];
        public Vector2f Position
        {
            get { return Sprite.Position; }
            set
            {
                Sprite.Position = value;
                Tower.Position = new Vector2f(Position.X, Position.Y);
                float posX = (Sprite.TextureRect.Width / 2 - SizeRectBounds / 2) * 0.9f;
                float posY = (Sprite.TextureRect.Height / 2 - SizeRectBounds / 2) * 0.8f;
                Bounds[0].Position = Position;
                Bounds[1].Position = PositionOnVectorRotation(new Vector2f(-posX, -posY), value);
                Bounds[2].Position = PositionOnVectorRotation(new Vector2f(posX, -posY), value);
                Bounds[3].Position = PositionOnVectorRotation(new Vector2f(-posX, posY), value);
                Bounds[4].Position = PositionOnVectorRotation(new Vector2f(posX, posY), value);
            }
        }
        public float Speed { get; set; } = 3f;
        public int Health { get; set; } = 1;

        public Tower Tower { get; set; }

        private RotationVaritableGroup RVGTank;
        private Vector2f PositionBeforeCollision;
        private Vector2f NewDirection = new Vector2f(0, 0);
        private Vector2f MainDirectionTank;
        private const int SizeRectBounds = 40;

        public Tank(Vector2f position)
        {
            Sprite = new Sprite(Resurses.TankTexture, new IntRect(0, 0, 158, 84))
            {
                Origin = new Vector2f(79, 42),
                Position = position
            };
            Bounds[0] = new Sprite() { TextureRect = new IntRect(0, 0, 60, 60), Origin = new Vector2f(30, 30), Texture = new Texture("Textures\\Objects\\noname.png") };
            for (int i = 1; i < Bounds.Length; i++)
            {
                Bounds[i] = new Sprite()
                {
                    TextureRect = new IntRect(0, 0, SizeRectBounds, SizeRectBounds),
                    Origin = new Vector2f(SizeRectBounds / 2f, SizeRectBounds / 2f),
                    Texture = new Texture("Textures\\Objects\\noname.png")
                };
            }
            Tower = new Tower(Position);
            RVGTank = new RotationVaritableGroup();
        }

        public void SubstractHealth(Object sender, TankSubstructHealthArgs args)
        {
            Health -= args.Health;
        }

        public void Spawn(Object sender, TankSpawnArgs arg)
        {
            Position = arg.SpawnPoint;
        }

        public void KeyPress(object sender, KeyEventArgs arg)
        {
            if (arg.Code == Keyboard.Key.Space)
            {
                Tower.CDUP();
                
            }
            if (arg.Code == Keyboard.Key.W) NewDirection += new Vector2f(0, -1);
            if (arg.Code == Keyboard.Key.A) NewDirection += new Vector2f(-1, 0);
            if (arg.Code == Keyboard.Key.S) NewDirection += new Vector2f(0, 1);
            if (arg.Code == Keyboard.Key.D) NewDirection += new Vector2f(1, 0);
            if (NewDirection.X != 0 || NewDirection.Y != 0)
            {
                MainDirectionTank.X = (float)(NewDirection.X / Math.Sqrt(NewDirection.X * NewDirection.X + NewDirection.Y * NewDirection.Y));
                MainDirectionTank.Y = (float)(NewDirection.Y / Math.Sqrt(NewDirection.X * NewDirection.X + NewDirection.Y * NewDirection.Y));
            }
            else MainDirectionTank *= 0;
        }

        public void KeyReleas(object sender, KeyEventArgs arg)
        {
            if (arg.Code == Keyboard.Key.W) NewDirection += new Vector2f(0, 1);
            if (arg.Code == Keyboard.Key.A) NewDirection += new Vector2f(1, 0);
            if (arg.Code == Keyboard.Key.S) NewDirection += new Vector2f(0, -1);
            if (arg.Code == Keyboard.Key.D) NewDirection += new Vector2f(-1, 0);
            if (NewDirection.X != 0 || NewDirection.Y != 0)
            {
                MainDirectionTank.X = (float)(NewDirection.X / Math.Sqrt(NewDirection.X * NewDirection.X + NewDirection.Y * NewDirection.Y));
                MainDirectionTank.Y = (float)(NewDirection.Y / Math.Sqrt(NewDirection.X * NewDirection.X + NewDirection.Y * NewDirection.Y));
            }
            else MainDirectionTank *= 0;
        }

        public void Move(Object sender, TankMoveArgs arg)
        {
            PositionBeforeCollision = Position;
            int newX = (int)Math.Round(Speed * MainDirectionTank.X * arg.Time.ElapsedTime.AsMilliseconds() / 10f);
            int newY = (int)Math.Round(Speed * MainDirectionTank.Y * arg.Time.ElapsedTime.AsMilliseconds() / 10f);
            Position += new Vector2f(newX, newY);
            RotationBody(arg.Time);
            if (arg.Method(Bounds) != null)
            {
                Position += PositionBeforeCollision - Position;
                if (arg.Method(Bounds) != null) Position += arg.Method(Bounds).Rect;
            }
            arg.View.Center = Position;
        }

        private void RotationBody(Clock time)
        {
            if (MainDirectionTank.X != 0 || MainDirectionTank.Y != 0)
            {
                RVGTank.speed = Speed * 200 * time.ElapsedTime.AsSeconds();
                RVGTank.vector = new Vector(NewDirection.X, NewDirection.Y);
                RVGTank.end = Math.Round(Vector.AngleBetween(new Vector(1, 0), RVGTank.vector));
                RVGTank.rotation = Sprite.Rotation;
                if (Math.Abs(RVGTank.rotation) > 180) RVGTank.rotation += RVGTank.rotation < 0 ? 360 : -360;
                if (Math.Round(RVGTank.end) != RVGTank.rotation)
                {
                    if (Math.Abs(RVGTank.rotation - RVGTank.end) > 180)
                        RVGTank.rotation += RVGTank.rotation < RVGTank.end ? -1 * RVGTank.speed : RVGTank.speed;
                    else
                        RVGTank.rotation += RVGTank.rotation < RVGTank.end ? RVGTank.speed : -1 * RVGTank.speed;
                }
                if (Math.Abs(RVGTank.end - Sprite.Rotation) < RVGTank.speed) Sprite.Rotation = (float)RVGTank.end;
                else Sprite.Rotation = RVGTank.rotation;
            }
        }

        private Vector2f PositionOnVectorRotation(Vector2f position, Vector2f value)
        {
            Vector vec = new Vector(position.X, position.Y);
            Matrix matrix = new Matrix(Math.Cos(Sprite.Rotation / 57.3), Math.Sin(Sprite.Rotation / 57.3), -Math.Sin(Sprite.Rotation / 57.3), Math.Cos(Sprite.Rotation / 57.3), 0, 0);
            return new Vector2f((float)(value.X + Vector.Multiply(vec, matrix).X), (float)(value.Y + Vector.Multiply(vec, matrix).Y));
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(Sprite);
            Tower.Draw(window);
            for (int i = 0; i < Bounds.Length; i++)
            {
                window.Draw(Bounds[i]);
            }
        }
    }
}
