using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonoGameJam3Entry
{
    class MappedSpriteSheet
    {
        public Texture2D Texture;
        public Dictionary<string, MappedSprite> Sprites = new();
        public MappedSpriteSheet(Game game, string mapFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(
                "<root>" +
                File.ReadAllText(mapFile) +
                "</root>"
                );

            XmlElement root = xmlDoc.DocumentElement;
            XmlNode map = root.SelectSingleNode("map");

            Texture = Game.LoadTexture("IMAGES/"+root.SelectSingleNode("img").Attributes["src"].Value);
            

            foreach (var a in map.ChildNodes)
            {
                XmlNode node = a as XmlNode;

                if (node.NodeType == XmlNodeType.Comment) continue;


                var name = node.Attributes["href"].Value;
                var rect = node.Attributes["coords"].Value.Split(",").Select(s => int.Parse(s)).ToArray();
                MappedSprite spr = new MappedSprite();
                spr.texture = Texture;
                spr.rectangle = new(rect[0], rect[1], rect[2], rect[3]);

                Sprites.Add(name, spr);
            }

        }
        public MappedSprite this[string s] => Sprites[s];
    }
}
