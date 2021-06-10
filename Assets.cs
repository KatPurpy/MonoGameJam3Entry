using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public static class Assets
    {
        public static class Sprites {
            public static Texture2D basecart;
            public static Texture2D chr_monkey;

            public static MappedSpriteSheet jungle;

            public static MappedSprite palm => jungle[nameof(palm)];
            public static MappedSprite anthill => jungle[nameof(anthill)];
            public static MappedSprite rock1 => jungle[nameof(rock1)];
            public static MappedSprite rock2 => jungle[nameof(rock2)];
            public static MappedSprite rock3 => jungle[nameof(rock3)];
            public static MappedSprite rock4 => jungle[nameof(rock4)];
            public static MappedSprite water => jungle[nameof(water)];
            public static MappedSprite ground => jungle[nameof(ground)];
            public static MappedSprite grass1 => jungle[nameof(grass1)];
            public static MappedSprite grass2 => jungle[nameof(grass2)];
            public static MappedSprite grass3 => jungle[nameof(grass3)];
            public static MappedSprite grass4 => jungle[nameof(grass4)];
            public static MappedSprite lower1 => jungle[nameof(lower1)];
            public static MappedSprite flower2 => jungle[nameof(flower2)];
            public static MappedSprite flower3 => jungle[nameof(flower3)];
            public static MappedSprite flower4 => jungle[nameof(flower4)];
            public static MappedSprite flower5 => jungle[nameof(flower5)];
            public static MappedSprite flower6 => jungle[nameof(flower6)];
            public static MappedSprite flower7 => jungle[nameof(flower7)];
            public static MappedSprite flower8 => jungle[nameof(flower8)];
        }
        public static class Sounds {
            public static SoundEffect click;

            public static SoundEffect sfx_wroom;
            public static SoundEffect sfx_car_engine;
            public static SoundEffect sfx_car_bonk;
        }

        public static void Load(Game game)
        {
            Sprites.jungle = new MappedSpriteSheet(game, "jungle.map");
            Sprites.basecart = Game.LoadTexture("IMAGES/basecart.bmp");
            Sprites.chr_monkey = Game.LoadTexture("IMAGES/chr_monkey.bmp");
        }
    }
}
