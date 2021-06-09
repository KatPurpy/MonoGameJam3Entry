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
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public class Track_Palm : Entity
    {
        public override Vector2 VisualPosition { set => body.Position = value / Game.PixelsPerMeter; get => body.Position * Game.PixelsPerMeter; }

        public MappedSprite texture;

        World world;
        Body body;
        Vector2 sprite_offset = new(40,75);
        public Track_Palm(World world)
        {
            this.world = world;
            body = world.CreateEllipse(32*1.5f/Game.PixelsPerMeter,16*1.5f/Game.PixelsPerMeter,8,1);
        }

        public override void IMGUI(GameTime time)
        {
            body.Position = ImGuiUtils.VecField("POSITION",body.Position);
            sprite_offset = ImGuiUtils.VecField("SpriteOffset",sprite_offset);
        }

        public override void Update(GameTime time)
        {
        }
        public override void Destroy()
        {
            base.Destroy();
            world.Remove(body);
        }

        public override void Draw(GameTime time)
        {
            texture.Draw(VisualPosition, origin: sprite_offset, scale:Vector2.One*2) ;
        }

        public override void SerializeState(Utf8JsonWriter writer)
        {
            WritePosition(writer);
        }

        public override void RestoreState(ref JsonDocument reader)
        {
            throw new NotImplementedException();
        }
    }
}
