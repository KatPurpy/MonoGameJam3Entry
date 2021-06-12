using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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
            foreach (var levelThumbnail in levelThumbnail)
            {
                Game.ImGuiRenderer.UnbindTexture(levelThumbnail);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        enum Screen
        {
            MainMenu,
            SelectMode,
            Race_SelectTrack,

            Goodbye
            
        }

        bool butt(string text)
        {
            ImGui.Text("        ");
            ImGui.SameLine();
            return ImGui.Button(text);
        }

        Screen screen;
        public override void Draw(GameTime gameTime)
        {
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

                    }
                    ImGui.SameLine();
                    ImGui.ImageButton(levelThumbnail[1], new(180, 90));
                    ImGui.SameLine();
                    ImGui.ImageButton(levelThumbnail[2], new(180, 90));

                    ImGui.Text("");

                    var offset =  "                           "; // 28 spaces
                    var offset2 = "                    ";// ??? spaces
                    ImGui.Text(offset + offset2);
                    ImGui.SameLine();
                    if (ImGui.Button("<--")) screen = Screen.SelectMode;

                    ImGui.End();
                    break;
        }
            ImGui.GetBackgroundDrawList().AddText(new(0,720-18),Color.Black.PackedValue, "(c) Kat Purpy, 2021");
            ;
        }


    }
}
