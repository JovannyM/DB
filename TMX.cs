using DB.EnemiesFolder;
using DB.MyEventArgs;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DB
{
    class TMX
    {
        public event EventHandler<SpawnEnemyArgs> SpawnEnemyEvent;
        public string LocationFile_tmx { get; private set; }
        public List<Sprite> Walls { get; private set; }
        public List<Sprite> LevelDoors { get; private set; }
        public Vector2f StartPositionForTank { get; private set; }
        private Sprite LevelImage;
        private List<Sprite> OtherImage;

        public void Load(string _location, Action<object, BulletSpawnArgs> shoot)
        {
            Walls = new List<Sprite>();
            OtherImage = new List<Sprite>();
            LevelDoors = new List<Sprite>();

            LocationFile_tmx = _location;
            XmlDocument document = new XmlDocument();
            document.Load(LocationFile_tmx);
            XmlElement Element = document.DocumentElement;

            foreach(XmlNode Node in Element)
            {
                foreach(XmlNode Child in Node.ChildNodes)
                {
                    if (Node.Name == "objectgroup" && Child.Name == "object")
                    {
                        if (Child.Attributes.Count == 4)
                        {
                            if (Child.Attributes.GetNamedItem("name").Value == "Start")
                            {
                                StartPositionForTank = PositionObj(Child, "x", "y");
                            }
                            else if (Child.Attributes.GetNamedItem("name").Value == "Boss")
                            {
                                //x = Convert.ToSingle(Child.Attributes.GetNamedItem("x").Value, CultureInfo.InvariantCulture);
                                //y = Convert.ToSingle(Child.Attributes.GetNamedItem("y").Value, CultureInfo.InvariantCulture);
                                //SpawnEnemy.Invoke(this, new SpawnEnemyArgs(x, y, 2));
                            }
                            else if (Child.Attributes.GetNamedItem("name").Value == "Tor")
                            {
                                float x = Convert.ToSingle(Child.Attributes.GetNamedItem("x").Value, CultureInfo.InvariantCulture);
                                float y = Convert.ToSingle(Child.Attributes.GetNamedItem("y").Value, CultureInfo.InvariantCulture);
                                SpawnEnemyEvent.Invoke(this, new SpawnEnemyArgs(new TorEnemy(new Vector2f(x, y), shoot)));
                            }
                            else if (Child.Attributes.GetNamedItem("name").Value == "Speed")
                            {
                                float x = Convert.ToSingle(Child.Attributes.GetNamedItem("x").Value, CultureInfo.InvariantCulture);
                                float y = Convert.ToSingle(Child.Attributes.GetNamedItem("y").Value, CultureInfo.InvariantCulture);
                                SpawnEnemyEvent.Invoke(this, new SpawnEnemyArgs(new SpeedEnemy(new Vector2f(x, y))));
                            }
                        }
                        else if (Child.Attributes.Count == 6)
                        {
                            if (Child.Attributes.GetNamedItem("name").Value == "Door")
                            {
                                LevelDoors.Add(SetSprite(Child));
                            }
                        }
                        else
                        {
                            Walls.Add(SetSprite(Child));
                        }
                    }
                    if (Node.Name == "imagelayer" && Child.Name == "image")
                    {
                        if (Node.Attributes.GetNamedItem("name").Value == "Level")
                        {
                            string LocationTextureTile = Child.Attributes.GetNamedItem("source").Value;
                            LevelImage = new Sprite()
                            {
                                Texture = new Texture(LocationTextureTile),
                                TextureRect = SizeObj(Child)                                
                            };
                        }
                        else if (Node.Attributes.Count > 2)
                        {
                            string LocationTextureTile = Child.Attributes.GetNamedItem("source").Value;
                            OtherImage.Add(new Sprite()
                            {
                                Position = PositionObj(Node, "offsetx", "offsety"),
                                TextureRect = SizeObj(Child),
                                Texture = new Texture(LocationTextureTile)
                            });
                        }
                    }
                }
            }
        }
        private Vector2f PositionObj(XmlNode Child, string name_x, string name_y)
        {
            float x = Convert.ToSingle(Child.Attributes.GetNamedItem(name_x).Value, CultureInfo.InvariantCulture);
            float y = Convert.ToSingle(Child.Attributes.GetNamedItem(name_y).Value, CultureInfo.InvariantCulture);
            return new Vector2f(x, y);
        }

        private IntRect SizeObj(XmlNode Child)
        {
            float RectangleWidth = Convert.ToSingle(Child.Attributes.GetNamedItem("width").Value, CultureInfo.InvariantCulture);
            float RectangleHeight = Convert.ToSingle(Child.Attributes.GetNamedItem("height").Value, CultureInfo.InvariantCulture);
            return new IntRect(0, 0, (int)RectangleWidth, (int)RectangleHeight);
        }

        private Sprite SetSprite(XmlNode Child)
        {
            return new Sprite() { Position = PositionObj(Child, "x", "y"), TextureRect = SizeObj(Child), Color = Color.Red };
        }

        public void TMXDraw(RenderWindow window)
        {
            foreach (Sprite image in OtherImage) window.Draw(image);
            foreach (Sprite wall in Walls) window.Draw(wall);
            window.Draw(LevelImage);
        }
    }
}
