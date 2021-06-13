using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public class Track_Waypoints : Entity
    {
        public override Vector2 VisualPosition
        {
            get =>
Positions.Count == 0 ? new(Mouse.GetState().X - 120, Mouse.GetState().Y) :

Positions[Math.Min(selected, Positions.Count - 1)] * Game.PixelsPerMeter; set { }
        }
        public List<Vector2> Positions = new();
        int dragging, selected;

        void dummy() { }

        public override void Update(GameTime time){}
        public override void Draw(GameTime time){}

        public const float triggerradius = 42;
        public const float aiTriggerRadius = 22;
        public override void IMGUI(GameTime time)
        {
            ImGuiUtils.PolylineEditor(Positions, camera, ref dragging, ref selected, dummy,
                VisualPosition / Game.PixelsPerMeter
                );
            DrawWayPoints();
        }
        static int frame;
        public void DrawWayPoints()
        {
            foreach (var pos in Positions)
            {
                ImGui.GetBackgroundDrawList().AddCircle(
                    NumericToXNA.ConvertXNAToNumeric(Vector2.Transform(pos * Game.PixelsPerMeter, camera.View())),
                    triggerradius * Game.PixelsPerMeter * camera.Scale.X,
                    0xFF00FF00
                    );
                ImGui.GetBackgroundDrawList().AddCircle(
                    NumericToXNA.ConvertXNAToNumeric(Vector2.Transform(pos * Game.PixelsPerMeter, camera.View())),
                    aiTriggerRadius * Game.PixelsPerMeter * camera.Scale.X,
                    0xFFFF0000
                    );
            }

            if (Positions.Count > 1)
                for (int i = 0; i < Positions.Count; i++)
                {
                    var screenPos1 = Vector2.Transform(Positions[i] * Game.PixelsPerMeter, camera.View());
                    var screenPos2 = Vector2.Transform(Positions[(i + 1) % Positions.Count] * Game.PixelsPerMeter, camera.View());
                    ImGui.GetBackgroundDrawList().AddLine(NumericToXNA.ConvertXNAToNumeric(screenPos1), NumericToXNA.ConvertXNAToNumeric(screenPos2),
                        frame % 3 == 0 ? 0xFFFF0000 : 0xFF0000FF
                        );
                }
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            writer.WriteStartArray(nameof(Positions));
            foreach (var vec in Positions)
            {
                SerializeVector2(writer, vec);
            }
            writer.WriteEndArray();
        }

        public override void RestoreState(JsonElement state)
        {
            Positions.Clear();

            foreach (var val in state.GetProperty(nameof(Positions)).EnumerateArray())
            {
                Positions.Add(ReadVector2(val));
            }
        }
        public override void Destroy()
        {
            Dead = true;
        }
    }
}
