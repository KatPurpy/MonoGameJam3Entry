using Dcrew.Camera;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public static class ImGuiUtils
    {
        public static void StartList()
        {
        }

        public static void BeginFixedWindow(string title,int windWidth, int windHeight)
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(Game._.gdm.PreferredBackBufferWidth / 2 - windWidth / 2, Game._.gdm.PreferredBackBufferHeight / 2 - windHeight / 2), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(windWidth, windHeight), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowFocus();
            ImGui.Begin(title, ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);
        }

        public static void SetStyle()
        {
            System.Numerics.Vector4 ImVec4(float x, float y, float z, float w) => new(x, y, z, w);
            
            var colors = ImGui.GetStyle().Colors;
            colors[(int)ImGuiCol.Text] = ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = ImVec4(0.50f, 0.50f, 0.50f, 1.00f);
            colors[(int)ImGuiCol.WindowBg] = ImVec4(0.17f, 0.09f, 0.00f, 0.97f);
            colors[(int)ImGuiCol.ChildBg] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.PopupBg] = ImVec4(0.08f, 0.08f, 0.08f, 0.94f);
            colors[(int)ImGuiCol.Border] = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
            colors[(int)ImGuiCol.BorderShadow] = ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.FrameBg] = ImVec4(0.41f, 0.26f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.FrameBgHovered] = ImVec4(0.26f, 0.59f, 0.98f, 0.40f);
            colors[(int)ImGuiCol.FrameBgActive] = ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
            colors[(int)ImGuiCol.TitleBg] = ImVec4(0.04f, 0.04f, 0.04f, 1.00f);
            colors[(int)ImGuiCol.TitleBgActive] = ImVec4(0.53f, 0.33f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed] = ImVec4(0.00f, 0.00f, 0.00f, 0.51f);
            colors[(int)ImGuiCol.MenuBarBg] = ImVec4(0.14f, 0.14f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarBg] = ImVec4(0.02f, 0.02f, 0.02f, 0.53f);
            colors[(int)ImGuiCol.ScrollbarGrab] = ImVec4(0.31f, 0.31f, 0.31f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = ImVec4(0.51f, 0.51f, 0.51f, 1.00f);
            colors[(int)ImGuiCol.CheckMark] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = ImVec4(0.24f, 0.52f, 0.88f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Button] = ImVec4(0.40f, 0.29f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.ButtonHovered] = ImVec4(0.00f, 0.55f, 0.43f, 1.00f);
            colors[(int)ImGuiCol.ButtonActive] = ImVec4(0.55f, 0.18f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.Header] = ImVec4(0.26f, 0.59f, 0.98f, 0.31f);
            colors[(int)ImGuiCol.HeaderHovered] = ImVec4(0.26f, 0.59f, 0.98f, 0.80f);
            colors[(int)ImGuiCol.HeaderActive] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Separator] = ImVec4(0.43f, 0.43f, 0.50f, 0.50f);
            colors[(int)ImGuiCol.SeparatorHovered] = ImVec4(0.10f, 0.40f, 0.75f, 0.78f);
            colors[(int)ImGuiCol.SeparatorActive] = ImVec4(0.10f, 0.40f, 0.75f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip] = ImVec4(0.98f, 0.40f, 0.26f, 0.25f);
            colors[(int)ImGuiCol.ResizeGripHovered] = ImVec4(0.98f, 0.52f, 0.26f, 0.67f);
            colors[(int)ImGuiCol.ResizeGripActive] = ImVec4(0.98f, 0.69f, 0.26f, 0.95f);
            colors[(int)ImGuiCol.Tab] = ImVec4(0.58f, 0.43f, 0.18f, 0.86f);
            colors[(int)ImGuiCol.TabHovered] = ImVec4(0.74f, 0.56f, 0.00f, 0.80f);
            colors[(int)ImGuiCol.TabActive] = ImVec4(0.64f, 0.36f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused] = ImVec4(0.07f, 0.10f, 0.15f, 0.97f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = ImVec4(0.14f, 0.26f, 0.42f, 1.00f);
            colors[(int)ImGuiCol.DockingPreview] = ImVec4(0.26f, 0.59f, 0.98f, 0.70f);
            colors[(int)ImGuiCol.DockingEmptyBg] = ImVec4(0.20f, 0.20f, 0.20f, 1.00f);
            colors[(int)ImGuiCol.PlotLines] = ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget] = ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = ImVec4(0.80f, 0.80f, 0.80f, 0.35f);

            var style = ImGui.GetStyle();
            style.ChildRounding = 
            style.FrameRounding = 
            style.GrabRounding = 
            style.PopupRounding = 
            style.ScrollbarRounding = 
            style.TabRounding = 
            style.WindowRounding = 12;

            style.FrameRounding = 6;

            style.WindowTitleAlign = style.SelectableTextAlign = new(0.5f);
            style.FramePadding = new(4);
            style.ChildBorderSize = 
            style.FrameBorderSize = 
            style.PopupBorderSize = 
            style.TabBorderSize = 
            style.WindowBorderSize = 0;
            
            style.ScrollbarSize = 16;

            style.AntiAliasedFill = style.AntiAliasedLines = style.AntiAliasedLinesUseTex = false;
        }

        public static unsafe T VecField<T>(string label, T vec) where T:unmanaged,IEquatable<T>
        {
            if(sizeof(T) == Unsafe.SizeOf<Vector2>()) ImGui.DragFloat2(label, ref Unsafe.As<T, System.Numerics.Vector2>(ref vec));
            if (sizeof(T) == Unsafe.SizeOf<Vector3>()) ImGui.DragFloat3(label, ref Unsafe.As<T, System.Numerics.Vector3>(ref vec));
            if (sizeof(T) == Unsafe.SizeOf<Vector4>()) ImGui.DragFloat4(label, ref Unsafe.As<T, System.Numerics.Vector4>(ref vec));
            return vec;
        }
        static int frame;
        public static void PolylineEditor(List<Vector2> Positions, Camera camera, ref int dragging, ref int selected, Action Rebuild, Vector2 newPos)
        {
            frame++;
            if (dragging != -1 && ImGui.IsMouseDown(ImGuiMouseButton.Left))
            {
                selected = dragging;
                var m = ImGui.GetMousePos();
                Positions[dragging] = camera.ScreenToWorld(m.X, m.Y) / Game.PixelsPerMeter;
                
                var screenPos = new Vector2(m.X, m.Y);
                var a = new[] { NumericToXNA.ConvertXNAToNumeric(screenPos - Vector2.One * 10), NumericToXNA.ConvertXNAToNumeric(screenPos + Vector2.One * 10) };
                ImGui.GetBackgroundDrawList().AddRectFilled(a[0], a[1], 0xFF00FFFF);
            }
            else
            {
                if (dragging != -1)
                {
                    dragging = -1;
                    Rebuild();
                }
                if(Positions.Count > 1)
                for(int i = 0; i < Positions.Count-1; i++)
                {
                    var screenPos1 = Vector2.Transform(Positions[i] * Game.PixelsPerMeter, camera.View());
                    var screenPos2 = Vector2.Transform(Positions[i+1] * Game.PixelsPerMeter, camera.View());
                    ImGui.GetBackgroundDrawList().AddLine(NumericToXNA.ConvertXNAToNumeric(screenPos1), NumericToXNA.ConvertXNAToNumeric(screenPos2),
                        frame % 3 == 0 ? 0xFFFF0000 : 0xFF0000FF
                        );
                }

                for (int i = Positions.Count; i-- > 0;)
                {
                    //  var screenPosPain = camera.WorldToScreen(Positions[i] * Game.PixelsPerMeter);
                    var screenPos = Vector2.Transform(Positions[i] * Game.PixelsPerMeter, camera.View());

                    var a = new[] { NumericToXNA.ConvertXNAToNumeric(screenPos - Vector2.One * 10),
                        NumericToXNA.ConvertXNAToNumeric(screenPos + Vector2.One * 10) };

                    ImGui.GetBackgroundDrawList().AddRectFilled(a[0], a[1], 0xFF00FF00);
                    ImGui.GetBackgroundDrawList().AddCircleFilled(new(screenPos.X, screenPos.Y), 10, 0xFFFF0000);

                    if (ImGui.IsMouseHoveringRect(a[0], a[1], false)) {
                        if (ImGui.IsMouseDown(ImGuiMouseButton.Left))
                        {
                            dragging = i;
                            break;
                        }
                        if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
                        {
                            selected = i;
                            break;
                        }
                    }
                }
            }

            var garbageGenerator = Positions.Select(v => NumericToXNA.ConvertXNAToNumeric(v).ToString()).ToArray();
            ImGui.PushItemWidth(-1);
            ImGui.ListBox("", ref selected, garbageGenerator, garbageGenerator.Length, 5);
            ImGui.PopItemWidth();

            if (ImGui.Button("Add vertex"))
            {
                Positions.Add(newPos);
                Rebuild();
            }

            if(ImGui.Button("Remove vertex"))
            {
                Positions.RemoveAt(selected);
                Rebuild();
            }
        }
    }
}
