using Dcrew.Camera;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using tainicom.Aether.Physics2D.Controllers;

namespace DSastR.Core
{
    public abstract class Entity
    {
        public Camera camera;
        public abstract Vector2 VisualPosition { get; set; }

        public bool NeedsToStart = true;
        public bool Dead = false;
        public bool ShowInInspector = true;

        public virtual void Start() { }
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time);
        public abstract void IMGUI(GameTime time);

        public abstract void SerializeState(Utf8JsonWriter writer);
        public abstract void RestoreState(JsonElement state);

        public virtual void OnPause(bool pause) { }
        public virtual void Destroy() {
            Dead = true;
        }

        protected float GetLayerDepth()
        {
            return Math.Clamp(Vector2.Transform(VisualPosition,camera.View()).Y / MonoGameJam3Entry.Game._.gdm.PreferredBackBufferHeight,0,1);
        }

        protected void WriteVisualPosition(Utf8JsonWriter writer)
        {
            SerializeVector2(writer, nameof(VisualPosition), VisualPosition);
        }
        /// <summary></summary>
        /// <param name="element">Object representing the entity</param>
        protected void ReadVisualPosition(JsonElement element)
        {
            VisualPosition = ReadVector2(element.GetProperty(nameof(VisualPosition)));
        }

        protected Vector2 ReadVector2(JsonElement element)
        {
            return new(element.GetProperty("X").GetSingle(), element.GetProperty("Y").GetSingle());
        }

        protected void SerializeVector2(Utf8JsonWriter writer, Vector2 vec)
        {
            writer.WriteStartObject();
            writer.WriteNumber("X", vec.X);
            writer.WriteNumber("Y", vec.Y);
            writer.WriteEndObject();
        }

        protected void SerializeVector2(Utf8JsonWriter writer, string propertyName, Vector2 vec)
        {
            writer.WritePropertyName(propertyName);
            SerializeVector2(writer, vec);
        }
    }
}
