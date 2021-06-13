using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    class MainMenuScene : Scene
    {
        IntPtr logo, gamelogo,background;
        IntPtr[] levelThumbnail = new IntPtr[0];
        int cutscenePage = 0;

        IntPtr[] currentCutScene = new IntPtr[0];
        IntPtr levelBlocked;

        string credits = File.ReadAllText("CREDITS.TXT");

        public override void Enter()
        {
            levelThumbnail = new[] { Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE1),
            Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE2),
            Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE3)};
            logo = Game.ImGuiRenderer.BindTexture(Assets.Sprites.LOGO);
            gamelogo = Game.ImGuiRenderer.BindTexture(Assets.Sprites.TITLE);
            background = Game.ImGuiRenderer.BindTexture(Assets.Sprites.MENUBG);
        }

        public override void Leave()
        {

        }

        ~MainMenuScene()
        {
            Game.ImGuiRenderer.UnbindTexture(logo);
            Game.ImGuiRenderer.UnbindTexture(gamelogo);
            Game.ImGuiRenderer.UnbindTexture(background);
            foreach (var levelThumbnail in levelThumbnail)
            {
                Game.ImGuiRenderer.UnbindTexture(levelThumbnail);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public enum Screen
        {
            MainMenu,
            SelectMode,
            Race_SelectTrack,
            Football,
            SpaceArena,
            Cutscene,
            Goodbye
            
        }

        public enum CutsceneType
        {
            RaceStoryIntro,
            RaceStoryEnding,
            FootballStoryIntro,
            FootballStoryEnding,
            SpaceStoryEnding
        }

        bool butt(string text)
        {
            ImGui.Text("        ");
            ImGui.SameLine();
            return ImGui.Button(text);
        }

        public static Screen screen;
        public static Screen prevScreen;
        void PrepareCutscene(CutsceneType cutscene)
        {
            prevScreen = screen == Screen.Cutscene ? Screen.MainMenu : screen ;
            screen = Screen.Cutscene;
            cutscenePage = 0;

            Texture2D[] frames;
            switch (cutscene)
            {
                case CutsceneType.RaceStoryIntro:
                    frames = Assets.Sprites.CUTSCENE_RACE_INTRO;
                    break;
                case CutsceneType.RaceStoryEnding:
                    frames = Assets.Sprites.CUTSCENE_RACE_ENDING;
                    break;
                case CutsceneType.FootballStoryIntro:
                    frames = Assets.Sprites.CUTSCENE_FOOTBALL_INTRO;
                    break;
                case CutsceneType.FootballStoryEnding:
                    frames = Assets.Sprites.CUTSCENE_FOOTBALL_ENDING;
                    break;
                case CutsceneType.SpaceStoryEnding:
                    frames = Assets.Sprites.CUTSCENE_SPACE_ENDING;
                    break;
                default: throw new Exception("wtf?");
            }

            currentCutScene = frames.Select(t => Game.ImGuiRenderer.BindTexture(t)).ToArray();

        }
        bool dummy1;
        public override void Draw(GameTime gameTime)
        {
            {
                System.Numerics.Vector2 logoSize = new System.Numerics.Vector2(128, 128) / 2;
                System.Numerics.Vector2 logoPos = new(0.5f, 0.5f);
                ImGui.GetBackgroundDrawList().AddImage(
    logo,
    ImGui.GetIO().DisplaySize * logoPos - logoSize,
    ImGui.GetIO().DisplaySize * logoPos + logoSize
    );
                ImGui.GetBackgroundDrawList().AddText(ImGui.GetIO().DisplaySize * logoPos - System.Numerics.Vector2.UnitX * 121/2 + System.Numerics.Vector2.UnitY * 60, 0xFFFFFFFF,"Kat Purpy presents");
            }
            if (gameTime.TotalGameTime.TotalSeconds < 3) return;
            //int a = 0;
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.Race1Unlock                     );

            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.Race2Unlock                     );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.Race3Unlock                     );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.FootballUnlock                  );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.SpaceUnlock                     );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.PlayedRaceStoryIntro            );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.PlayedRaceStoryEnding           );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.PlayedFootballStoryIntro        );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.PlayedFootballStoryEnding       );
            //ImGui.Checkbox((a++).ToString(),ref    PlayerProfile.PlayedSpaceStoryIntro           );
            //ImGui.Checkbox((a++).ToString(),ref PlayerProfile.PlayedSpaceStoryEnding);

            //ImGui.Text(PlayerProfile.GetPacked().ToString());

            //foreach(var b in PlayerProfile.DebugGetFlags())
            //{
            //    ImGui.Text(b.ToString());
            //    ImGui.SameLine();
            //}

            //if(ImGui.ArrowButton("Test Save",ImGuiDir.Down))
            //{
            //    PlayerProfile.Save();
            //}return;


            // ImGui.SetWindowFontScale(1);
            ImGui.GetBackgroundDrawList().AddImage(
     background,
     new(0, 0),
     ImGui.GetIO().DisplaySize
     );
            {
                System.Numerics.Vector2 gamelogoSize = new System.Numerics.Vector2(512, 128) / 2;
                System.Numerics.Vector2 gameLogoPos = new(0.5f, 0.15f);
                ImGui.GetBackgroundDrawList().AddImage(
    gamelogo,
    ImGui.GetIO().DisplaySize * gameLogoPos - gamelogoSize,
    ImGui.GetIO().DisplaySize * gameLogoPos + gamelogoSize
    );
            }


            switch (screen) {
                case Screen.Cutscene:

                    ImGui.GetBackgroundDrawList().AddImage(
                        currentCutScene[cutscenePage],
                        new(0, 0),
                        ImGui.GetIO().DisplaySize
                        );

                    ImGui.SetNextWindowPos(new(640-142/2,720 - 50),ImGuiCond.Always);
                    //ImGui.SetNextWindowSize()
                    
                    ImGui.Begin("bookthing", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoSavedSettings);
                    ImGui.SetWindowFontScale(1.5f);
                    if (ImGui.Button("<--"))
                    {
                        if (--cutscenePage < 0)
                        {
                            cutscenePage = 0;
                            break;
                        }
                    }
                    ImGui.SameLine();
                    ImGui.Text(string.Format("{0}/{1}",cutscenePage+1,currentCutScene.Length));
                    ImGui.SameLine();
                    if (ImGui.Button("-->"))
                    {
                        if (++cutscenePage == currentCutScene.Length)
                        {
                            screen = prevScreen;
                            break;
                        }
                    }
                    ImGui.End();

                    break;

                case Screen.MainMenu: 
                    ImGuiUtils.BeginFixedWindow("Main Menu", 180, 158);
                    if (butt("Play")) screen = Screen.SelectMode;
                    if (butt("Settings"))
                    {
                        ImGui.OpenPopup("Settings");
                    }
                    dummy1 = true;
                    if (ImGui.BeginPopupModal("Settings",ref dummy1, ImGuiWindowFlags.NoSavedSettings| ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
                    {
                        ImGui.Text("Sorry but we ran out of budget for this :(\nIf you want to erase progress just delete PLRSAV file, get it?");
                          if (butt("Understood. Have a nice day")) ImGui.CloseCurrentPopup();
                        ImGui.EndPopup();
                    }

                    if (butt("Credits"))
                    {
                        ImGui.OpenPopup("CREDITS");
                    }

                    if (ImGui.BeginPopupModal("CREDITS", ref dummy1, ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse))
                    {
                        ImGui.Text(credits);
                        ImGui.EndPopup();
                    }


                    if (butt("Exit"))
                    {
                        screen = Screen.Goodbye;
                        Game._.Exit();
                    }
     

                    ImGui.End();
                    
         
                break;
                case Screen.SelectMode:
                    ImGuiUtils.BeginFixedWindow("Select mode", 180, 158);
                    if(butt("Race")) screen = Screen.Race_SelectTrack;
                    if(PlayerProfile.FootballUnlock && butt("Wheelball"))
                    {
                        screen = Screen.Football;
                    }
                    if(PlayerProfile.SpaceUnlock && butt("Space brawl"))
                    {
                        screen = Screen.SpaceArena;
                    }
                    if(butt("<--")) screen = Screen.MainMenu;
                    break;
                case Screen.Football:
                    ImGuiUtils.BeginFixedWindow("Wheelball",370,158);
                    ImGui.Text("Rules am s1mple: the put ball the 0n enemy g0a1.\n3 goals.\nGood luck.");
                    if (ImGui.Button("<--")) screen = Screen.SelectMode;
                    ImGui.SameLine(300);
                    if (ImGui.Button("Proceed."))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.SpaceUnlock = true;
                        };
                        Game.LoadMusic("SOUNDS/football.ogg").Play();
                        GameScene.StartLevel("LEVELS/FOOTBALL");
                    }
                    ImGui.End();
                    break;
                case Screen.SpaceArena:
                    ImGuiUtils.BeginFixedWindow("TH0SE AL1ENS11", 410, 158);
                    ImGui.Text("0hh the notn't! al1ens in need 0f to kill us.\nSh0wn them tHat per50n the 1s b0ss around th1s plAcE!\nTask: Survive and push away others. There can be only one.");
                    if (ImGui.Button("<--")) screen = Screen.SelectMode;
                    ImGui.SameLine(300);
                    if (ImGui.Button("Proceed."))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.SpaceComplete = true;
                        };
                        Game.LoadMusic("SOUNDS/space.ogg").Play();
                        GameScene.StartLevel("LEVELS/SPACE");
                    }
                    ImGui.End();
                    break;
                case Screen.Race_SelectTrack:
                    ImGuiUtils.BeginFixedWindow("Select track",600-5,180+15);
                    
                    if(ImGui.ImageButton(levelThumbnail[0], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.Race2Unlock = true;
                        };
                        Game.LoadMusic("SOUNDS/race1.ogg").Play();
                        GameScene.StartLevel("LEVELS/RACE1");
                    }
                    ImGui.SameLine();
                    if (PlayerProfile.Race2Unlock && ImGui.ImageButton(levelThumbnail[1], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.Race3Unlock = true;
                        };
                        Game.LoadMusic("SOUNDS/race2.ogg").Play();
                        GameScene.StartLevel("LEVELS/RACE2");
                    }
                    ImGui.SameLine();
                    if (PlayerProfile.Race3Unlock && ImGui.ImageButton(levelThumbnail[2], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.FootballUnlock = true;
                        };
                        Game.LoadMusic("SOUNDS/race3.ogg").Play();
                        GameScene.StartLevel("LEVELS/RACE3");
                    }

                    ImGui.Text("");

                    var offset =  "                           "; // 28 spaces
                    var offset2 = "                    ";// ??? spaces
                    ImGui.Text(offset + offset2);
                    ImGui.SameLine();
                    if (ImGui.Button("<--")) screen = Screen.SelectMode;

                    ImGui.End();
                    break;
        }

            if (screen == Screen.Race_SelectTrack && !PlayerProfile.PlayedRaceStoryIntro)
            {
                Debug.WriteLine("MUST PLAY RACE INTRO CUTSCENE");
                PlayerProfile.PlayedRaceStoryIntro = true;
                PrepareCutscene(CutsceneType.RaceStoryIntro);
                PlayerProfile.Save();
            }

            if (screen == Screen.Race_SelectTrack && PlayerProfile.FootballUnlock && !PlayerProfile.PlayedRaceStoryEnding)
            {
                Debug.WriteLine("MUST PLAY RACE ENDING CUTSCENE");
                PrepareCutscene(CutsceneType.RaceStoryEnding);
                PlayerProfile.PlayedRaceStoryEnding = true; PlayerProfile.Save();
            }

            if (screen == Screen.Football && !PlayerProfile.PlayedFootballStoryIntro)
            {
                Debug.WriteLine("MUST PLAY FOOTBALL INTRO CUTSCENE");
                PrepareCutscene(CutsceneType.FootballStoryIntro);
                PlayerProfile.PlayedFootballStoryIntro = true; PlayerProfile.Save();
            }

            if (screen == Screen.Football && PlayerProfile.SpaceUnlock && !PlayerProfile.PlayedFootballStoryEnding)
            {
                Debug.WriteLine("MUST PLAY FOOTBALL OUTRO CUTSCENE");
                PrepareCutscene(CutsceneType.FootballStoryEnding);
                Game.LoadMusic("SOUNDS/ohshit.ogg").Play();
                PlayerProfile.PlayedFootballStoryEnding = true; PlayerProfile.Save();
            }

            if(screen == Screen.SpaceArena && !PlayerProfile.PlayedSpaceStoryEnding && PlayerProfile.SpaceComplete)
            {
                Debug.WriteLine("ENDING CUTSCENE MUST BE PLAYED");
                PrepareCutscene(CutsceneType.SpaceStoryEnding);
                Game.LoadMusic("SOUNDS/ending.ogg").Play();
                PlayerProfile.PlayedSpaceStoryEnding = true; PlayerProfile.Save();
            }

            ImGui.GetBackgroundDrawList().AddText(new(0,720-18),Color.White.PackedValue, "(c) Kat Purpy, 2021");
            ;
        }


    }
}
