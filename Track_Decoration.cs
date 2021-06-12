using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    public class Track_Decoration : Entity
    {
        Vector2 _pos;
        public override Vector2 VisualPosition { get => _pos; set => _pos=value; }
        int decType;
        static Random r = new();
        bool flip;
        public override void Draw(GameTime time)
        {
            if(decType == 0)
            {
                decType = r.Next((int)DecorationType.flower1, (int)DecorationType.flower8 + 1);
            }
            flip = (int)((VisualPosition.X + VisualPosition.Y)) % 2 == 0;
            Assets.Sprites.decorMap[(DecorationType)decType].Draw(VisualPosition,layerDepth:GetLayerDepth(),effects: flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
        }

        public override void IMGUI(GameTime time)
        {
            VisualPosition = ImGuiUtils.VecField("Visual Position", VisualPosition);
            var str = Enum.GetNames(typeof(DecorationType));
            ImGui.ListBox("DecorType",ref decType, str,str.Length,5);
        }

        public override void RestoreState(JsonElement state)
        {
            ReadVisualPosition(state);
            decType = state.GetProperty(nameof(decType)).GetInt32();
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            WriteVisualPosition(writer);
            writer.WriteNumber(nameof(decType),decType);
        }

        public override void Update(GameTime time){}


    }

    public enum DecorationType
    {
        palm,
        anthill,
        rock1,
        rock2,
        rock3,
        rock4,
        water,
        ground,
        grass1,
        grass2,
        grass3,
        grass4,
        flower1,
        flower2,
        flower3,
        flower4,
        flower5,
        flower6,
        flower7,
        flower8,
    }
}
