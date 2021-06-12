using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    class MainMenuScene : Scene
    {

        IntPtr[] levelThumbnail = new IntPtr[0];
        IntPtr levelBlocked;

        public override void Enter()
        {
            levelThumbnail = new[] { Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE1),
            Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE2),
            Game.ImGuiRenderer.BindTexture(Assets.Sprites.RACE3)};
            levelBlocked = Game.ImGuiRenderer.BindTexture(Assets.Sprites.@lock);

        }

        public override void Leave()
        {

        }

        ~MainMenuScene()
        {
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
            SpaceStoryIntro,
            SpaceStoryEnding
        }

        bool butt(string text)
        {
            ImGui.Text("        ");
            ImGui.SameLine();
            return ImGui.Button(text);
        }

        public Screen screen;
        public override void Draw(GameTime gameTime)
        {
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



            
            

            switch (screen) {
                case Screen.MainMenu: 
                    ImGuiUtils.BeginFixedWindow("Main Menu", 180, 158);
                    if (butt("Play")) screen = Screen.SelectMode;
                    if (butt("Settings"))
                    {
                        ImGui.OpenPopup("oof");
                    }
                    
                    if (ImGui.BeginPopupModal("oof"))
                    {
                        ImGui.Text("Sorry but we ran out of budget for this :(");
                          if (butt("Understood. Have a nice day")) ImGui.CloseCurrentPopup();
                        ImGui.EndPopup();
                    }

                    butt("Credits");
                    if(butt("Exit")) throw new NotImplementedException();

     

                    ImGui.End();
                    
         
                break;
                case Screen.SelectMode:
                    ImGuiUtils.BeginFixedWindow("Select mode", 180, 158);
                    if(butt("Race")) screen = Screen.Race_SelectTrack;
                    butt("Wheelball");
                    butt("Space brawl");
                    if(butt("<--")) screen = Screen.MainMenu;
                    break;
                case Screen.Race_SelectTrack:
                    ImGuiUtils.BeginFixedWindow("Select track",600-5,180+15);
                    
                    if(ImGui.ImageButton(levelThumbnail[0], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.Race2Unlock = true;
                        };
                        GameScene.StartLevel("LEVELS/RACE1");
                    }
                    ImGui.SameLine();
                    if (PlayerProfile.Race2Unlock && ImGui.ImageButton(levelThumbnail[1], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.Race3Unlock = true;
                        };
                        GameScene.StartLevel("LEVELS/RACE2");
                    }
                    ImGui.SameLine();
                    if (PlayerProfile.Race3Unlock && ImGui.ImageButton(levelThumbnail[2], new(180, 90)))
                    {
                        Game.DoAfterWin = () =>
                        {
                            PlayerProfile.FootballUnlock = true;
                        };
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
            }

            if(screen == Screen.Race_SelectTrack && PlayerProfile.FootballUnlock && !PlayerProfile.PlayedFootballStoryEnding)
            {
                Debug.WriteLine("MUST PLAY RACE ENDING CUTSCENE");
            }

            if (screen == Screen.Football && !PlayerProfile.PlayedFootballStoryIntro)
            {
                Debug.WriteLine("MUST PLAY FOOTBALL INTRO CUTSCENE");
            }

            if (screen == Screen.Football && PlayerProfile.SpaceUnlock && !PlayerProfile.PlayedFootballStoryEnding)
            {
                Debug.WriteLine("MUST PLAY FOOTBALL OUTRO CUTSCENE");
            }

            if(screen == Screen.SpaceArena && !PlayerProfile.PlayedRaceStoryIntro)
            {
                Debug.WriteLine("MUST PLAY SPACE ARENA INTRO CUTSCENE");
            }

            if(screen == Screen.SpaceArena && !PlayerProfile.SpaceComplete)
            {
                Debug.WriteLine("ENDING CUTSCENE MUST BE PLAYED");
            }

            ImGui.GetBackgroundDrawList().AddText(new(0,720-18),Color.Black.PackedValue, "(c) Kat Purpy, 2021");
            ;
        }


    }
}
