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
            public static Texture2D RACE1;
            public static Texture2D RACE2;
            public static Texture2D RACE3;

            public static Texture2D space;

            public static Texture2D[] CUTSCENE_RACE_INTRO;
            public static Texture2D[] CUTSCENE_RACE_ENDING;

            public static Texture2D[] CUTSCENE_FOOTBALL_INTRO;
            public static Texture2D[] CUTSCENE_FOOTBALL_ENDING;

            public static Texture2D circle;

            public static Texture2D[] CUTSCENE_SPACE_ENDING;

            public static Texture2D basecart;
            public static Texture2D chr_monkey;
            public static Texture2D chr_crocodile;
            public static Texture2D chr_rhino;
            public static Texture2D chr_panther;
            public static Texture2D chr_alien;

            public static Texture2D checkerboard;
            public static Texture2D @lock;

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
            public static MappedSprite flower1 => jungle[nameof(flower1)];
            public static MappedSprite flower2 => jungle[nameof(flower2)];
            public static MappedSprite flower3 => jungle[nameof(flower3)];
            public static MappedSprite flower4 => jungle[nameof(flower4)];
            public static MappedSprite flower5 => jungle[nameof(flower5)];
            public static MappedSprite flower6 => jungle[nameof(flower6)];
            public static MappedSprite flower7 => jungle[nameof(flower7)];
            public static MappedSprite flower8 => jungle[nameof(flower8)];

            public static Dictionary<DecorationType, MappedSprite> decorMap;
        }
        public static class Sounds {

            public static SoundEffect lap;
            public static SoundEffect yay;
            public static SoundEffect ubad;
            public static SoundEffect countdown;
            public static SoundEffect kaboom;
            public static SoundEffect enemygoal;
        }

        public static void Load(Game game)
        {
            Sprites.circle = Game.LoadTexture("IMAGES/circle.bmp");
            Sprites.jungle = new MappedSpriteSheet(game, "jungle.map");
            Sprites.basecart = Game.LoadTexture("IMAGES/basecart.bmp");
            Sprites.chr_monkey = Game.LoadTexture("IMAGES/chr_monkey.bmp");
            Sprites.chr_crocodile = Game.LoadTexture("IMAGES/chr_crocodile.bmp");
            Sprites.chr_rhino = Game.LoadTexture("IMAGES/chr_rhino.bmp");
            Sprites.chr_panther = Game.LoadTexture("IMAGES/chr_panther.bmp");
            Sprites.chr_alien = Game.LoadTexture("IMAGES/chr_alien.bmp");
            Sprites.checkerboard = Game.LoadTexture("IMAGES/checkerboard.bmp");
            Sprites.@lock = Game.LoadTexture("IMAGES/lock.bmp");
            Sprites.RACE1 = Game.LoadTexture("IMAGES/RACE1.bmp");
            Sprites.RACE2 = Game.LoadTexture("IMAGES/RACE2.bmp");
            Sprites.RACE3 = Game.LoadTexture("IMAGES/RACE3.bmp");
            Sprites.space = Game.LoadTexture("IMAGES/space.bmp");

            Sprites.CUTSCENE_RACE_INTRO = new[]
            {
                Game.LoadTexture("IMAGES/RACE_INTRO_0.bmp"),
                Game.LoadTexture("IMAGES/RACE_INTRO_1.bmp"),
                Game.LoadTexture("IMAGES/RACE_INTRO_2.bmp"),
            };


            Sprites.CUTSCENE_RACE_ENDING = new[]
            {
                Game.LoadTexture("IMAGES/RACE_ENDING_0.bmp"),
                Game.LoadTexture("IMAGES/RACE_ENDING_1.bmp"),
                Game.LoadTexture("IMAGES/RACE_ENDING_2.bmp"),
            };

            Sprites.CUTSCENE_FOOTBALL_INTRO = new[]
            {
                Game.LoadTexture("IMAGES/FOOTBALL_INTRO_0.bmp"),
                Game.LoadTexture("IMAGES/FOOTBALL_INTRO_1.bmp"),
            };

            Sprites.CUTSCENE_FOOTBALL_ENDING = new[]
            {
                Game.LoadTexture("IMAGES/FOOTBALL_ENDING_0.bmp"),
                Game.LoadTexture("IMAGES/FOOTBALL_ENDING_1.bmp")
            };

            Sprites.CUTSCENE_SPACE_ENDING = new[]
            {
                Game.LoadTexture("IMAGES/SPACE_ENDING_0.bmp"),
                Game.LoadTexture("IMAGES/SPACE_ENDING_1.bmp"),
                Game.LoadTexture("IMAGES/SPACE_ENDING_2.bmp"),
            };

            

            Sprites.decorMap = new()
            {
                { DecorationType.palm, Sprites.palm },
                { DecorationType.anthill, Sprites.anthill },
                { DecorationType.rock1, Sprites.rock1 },
                { DecorationType.rock2, Sprites.rock2 },
                { DecorationType.rock3, Sprites.rock3 },
                { DecorationType.rock4, Sprites.rock4 },
                { DecorationType.water, Sprites.water },
                { DecorationType.ground, Sprites.ground },
                { DecorationType.grass1, Sprites.grass1 },
                { DecorationType.grass2, Sprites.grass2 },
                { DecorationType.grass3, Sprites.grass3 },
                { DecorationType.grass4, Sprites.grass4 },
                { DecorationType.flower1, Sprites.flower1 },
                { DecorationType.flower2, Sprites.flower2 },
                { DecorationType.flower3, Sprites.flower3 },
                { DecorationType.flower4, Sprites.flower4 },
                { DecorationType.flower5, Sprites.flower5 },
                { DecorationType.flower6, Sprites.flower6 },
                { DecorationType.flower7, Sprites.flower7 },
                { DecorationType.flower8, Sprites.flower8 }
            };

            Sounds.lap = Game.LoadSound("SOUNDS/lap.ogg");
            Sounds.yay = Game.LoadSound("SOUNDS/yay.ogg");
            Sounds.ubad = Game.LoadSound("SOUNDS/ubad.ogg");
            Sounds.countdown = Game.LoadSound("SOUNDS/countdown.ogg");
            Sounds.kaboom = Game.LoadSound("SOUNDS/kaboom.ogg");
            Sounds.enemygoal = Game.LoadSound("SOUNDS/enemygoal.ogg");
        }
    }
}
