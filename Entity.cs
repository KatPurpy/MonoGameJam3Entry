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
        public abstract Vector2 VisualPosition { get; set; }

        public bool NeedsToStart = true;
        public bool Dead = false;
        public bool ShowInInspector = true;

        public virtual void Start() { }
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime time);
        public abstract void IMGUI(GameTime time);

        public abstract void SerializeState(Utf8JsonWriter writer);
        public abstract void RestoreState(ref JsonDocument json);

        public virtual void OnPause(bool pause) { }
        public virtual void Destroy() {
            Dead = true;
        }

        protected void WritePosition(Utf8JsonWriter writer)
        {
            SerializeVector2(writer, nameof(VisualPosition), VisualPosition);
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
