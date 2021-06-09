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

        public static unsafe T VecField<T>(string label, T vec) where T:unmanaged,IEquatable<T>
        {
            if(sizeof(T) == Unsafe.SizeOf<Vector2>()) ImGui.DragFloat2(label, ref Unsafe.As<T, System.Numerics.Vector2>(ref vec));
            if (sizeof(T) == Unsafe.SizeOf<Vector3>()) ImGui.DragFloat3(label, ref Unsafe.As<T, System.Numerics.Vector3>(ref vec));
            if (sizeof(T) == Unsafe.SizeOf<Vector4>()) ImGui.DragFloat4(label, ref Unsafe.As<T, System.Numerics.Vector4>(ref vec));
            return vec;
        }

        public static void PolylineEditor(List<Vector2> Positions, Camera camera, ref int dragging, ref int selected, Action Rebuild, Vector2 newPos)
        {
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
