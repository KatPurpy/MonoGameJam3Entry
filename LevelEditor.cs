using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGameJam3Entry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public sealed partial class GameScene : Scene
    {
        Type entityBrush;
        string fileName = "FILENAME";

        DebugView physicsDebugDraw;
        bool drawColliders;

        bool testMode;
        bool fileIOActive;
        bool fileIOSave;

        enum EditMode
        {
            Entity,
            Track
        };

        EditMode editMode = EditMode.Track;

        void InitEditor()
        {
            physicsDebugDraw = new DebugView(physicsWorld);

            physicsDebugDraw.Enabled = true;
            physicsDebugDraw.AppendFlags(DebugViewFlags.Shape);
            physicsDebugDraw.LoadContent(Game._.gdm.GraphicsDevice, Game._.Content);

            ImGuiRenderer = new ImGuiRenderer(Game._);
            ImGuiRenderer.RebuildFontAtlas();
        }

        private void EditorFunctions(GameTime time)
        {

            ImGui.Checkbox("Draw colliders", ref drawColliders);



            if (testMode)
            {
                if (fileIOActive == false)
                {
                    if (ImGui.Button("End Test"))
                    {
                        Load(fileName);
                        testMode = false;
                    }
                    return;
                }
            }
            else
            {
                if(ImGui.Button("Start Test"))
                {
                    fileIOActive = true;
                    fileIOSave = true;
                    testMode = true;
                }
            }

            

            

            

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Save"))
                    {
                        fileIOActive = true;
                        fileIOSave = true;
                    }
                    if (ImGui.MenuItem("Load"))
                    {
                        fileIOActive = true;
                        fileIOSave = false;
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            if (fileIOActive &&
                
                ImGui.Begin("SaveModal",ImGuiWindowFlags.NoTitleBar))
            {
                
                var a = FilePicker.GetFilePicker(this, "");
                if (a.Draw())
                {
                    fileName = (a.SelectedFile);
                    FilePicker.RemoveFilePicker(this);
                }
                if (fileName == null) fileName = "FILENAME";
                ImGui.PushItemWidth(400);
                ImGui.InputText("FileName", ref fileName, 512);
                ImGui.PopItemWidth();

                if (ImGui.Button(fileIOSave ? "SAVE" : "LOAD"))
                {
                    if (fileIOSave) Save(fileName);
                    else Load(fileName);
                    fileIOActive = false;
                }
                if (ImGui.Button("Cancel"))
                {
                    testMode = false;
                    fileIOActive = false;
                    
                }
                ImGui.EndPopup();
            }

            

            ImGui.Begin("ENTITY EDITOR");

            int mode = (int)editMode;

            ImGui.LabelText("", "Entity type");

            if (ImGui.RadioButton("Entities", ref mode, 0)) editMode = EditMode.Entity;
            if (ImGui.RadioButton("Track", ref mode, 1)) editMode = EditMode.Track;

            ImGui.Separator();

            switch (editMode)
            {

                case EditMode.Entity:
                    EntityEditor(time, "Entities", em, ref currentEntityID, ref focusedEntity, spawn_entity);
                    break;
                case EditMode.Track:
                    EntityEditor(time, "Entities", track, ref track_ThingID, ref focusedEntity, spawn_track_entity);
                    break;
            }
            ImGui.End();

  
        }

        private void EntityEditor(GameTime time, string label, EntityManager manager, ref int currentEntityID, ref Entity currentEntity, Dictionary<Type, Func<Game, World, EntityManager, Entity>> instancables)
        {

            if (ImGui.BeginMenu("Add Entity"))
            {
                foreach (Type t in instancables.Keys)
                {
                    if (ImGui.MenuItem(t.Name))
                    {
                        currentEntityID = AddEntity(manager, instancables, t, out _);
                    }
                }
                ImGui.EndMenu();
            }
            bool enableEntityBrush = currentEntity == null || currentEntity.GetType() != typeof(Track_PalmWall);
            if (enableEntityBrush)
            {
                if (ImGui.BeginMenu("Entity Brush: " + (entityBrush == null ? "NONE" : entityBrush.Name)))
                {
                    foreach (Type t in instancables.Keys)
                    {
                        if (ImGui.MenuItem(t.Name))
                        {
                            entityBrush = t;
                        }
                    }
                    ImGui.EndMenu();
                }
            }
           
            StringBuilder sb = new StringBuilder();
            if (ImGui.BeginChildFrame(1, new System.Numerics.Vector2(200, 200)))
            {
                for (int i = 0; i < manager.SerializableEntities.Count; i++)
                {
                    var ent = manager.SerializableEntities[i];
                    sb.Clear();
                    sb.Append(i);
                    sb.Append(' ');
                    sb.Append(ent.GetType().Name);
                    if (ImGui.Selectable(sb.ToString(), i == currentEntityID))
                    {
                        currentEntityID = i;
                        break;
                    }
                }
                ImGui.EndChildFrame();
            }

            

            //ImGui.ListBox("", ref currentEntityID, s.Select(s => s.ToString()).ToArray(), s.Count, 10);


            if (focusedEntity?.GetType() != typeof(Track_Waypoints) && ImGui.Button("Delete Entity"))
            {
                ImGui.OpenPopup("CONFIRM DELETION?");
            }


            if (ImGui.BeginPopup("CONFIRM DELETION?"))
            {
                if (ImGui.Button("YES"))
                {
                    manager.SerializableEntities[currentEntityID].Destroy();
                    ImGui.CloseCurrentPopup();
                }
                if (ImGui.Button("NO"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }


            if (-1 < currentEntityID && currentEntityID < manager.SerializableEntities.Count)
            {
                var pos = NumericToXNA.ConvertXNAToNumeric(Vector2.Transform(manager.SerializableEntities[currentEntityID].VisualPosition, camera.View()));

                currentEntity = manager.SerializableEntities[currentEntityID];

                ImGui.GetBackgroundDrawList().AddCircleFilled(pos, MathF.Abs(MathF.Sin(2 * (float)time.TotalGameTime.TotalSeconds)) * 20 + 5, (uint)(MathHelper.Lerp(0x55FFFF00, 0x55FF00FF, MathF.Abs(MathF.Sin(4 * (float)time.TotalGameTime.TotalSeconds)))));
            }
            var m = ImGui.GetMousePos();
            if (enableEntityBrush && ImGui.IsMouseClicked(ImGuiMouseButton.Right))
            {
                currentEntityID = AddEntity(manager, instancables, entityBrush, out Entity ent);
                if (currentEntityID != -1)
                {
                    ent.VisualPosition = camera.ScreenToWorld(m.X, m.Y);
                }
            }
        }

        private int AddEntity(EntityManager manager, Dictionary<Type, Func<Game, World, EntityManager, Entity>> instancables, Type entityType, out Entity ent)
        {
            ent = null;
            if (entityType == null) return -1;
            int currentEntityID;
            manager.AddEntity(ent = instancables[entityType](Game._, this.physicsWorld, manager));
            currentEntityID = manager.Entities.Count - 1;
            return currentEntityID;
        }

        void SaveEntity(Utf8JsonWriter writer,Entity ent)
        {
            writer.WriteStartObject();
            writer.WriteString("type", ent.GetType().Name);
            ent.SerializeState(writer);
            writer.WriteEndObject();
        }

        private void Save(string fileName)
        {
            using (var stream = File.Create(fileName)) {
                Utf8JsonWriter writer = new Utf8JsonWriter(stream,new JsonWriterOptions()
                {
                    Indented = true
                });
                writer.WriteStartObject();

                writer.WritePropertyName("waypoints");
                writer.WriteStartObject();
                wayPoints.SerializeState(writer);
                writer.WriteEndObject();

                writer.WriteStartArray("entities");
                foreach (var entity in em.SerializableEntities) SaveEntity(writer, entity);
                writer.WriteEndArray();

                writer.WriteStartArray("track");
                foreach (var entity in track.SerializableEntities) SaveEntity(writer, entity);
                writer.WriteEndArray();

                writer.WriteEndObject();
                writer.Flush();

                //string json = UTF8Encoding.UTF8.GetString(stream.ToArray());
                //Debug.WriteLine(json);
            }

            //test loading
            Load(fileName);
        }
        


        private void Load(string filename)
        {
            physicsWorld = new World(Vector2.Zero);
            physicsDebugDraw.Dispose();
            physicsDebugDraw = new DebugView(physicsWorld);
            physicsDebugDraw.LoadContent(Game._.GraphicsDevice, Game._.Content);
            em.Clear();
            track.Clear();
            track.AddEntity(wayPoints = new Track_Waypoints() { camera = camera });
            JsonDocument json = JsonDocument.Parse(File.ReadAllText(filename));
            Type typeByString(string s) => Type.GetType("MonoGameJam3Entry." + s);

            wayPoints.RestoreState(json.RootElement.GetProperty("waypoints"));

            foreach (var entityState in json.RootElement.GetProperty("entities").EnumerateArray())
            {
                var type_str = entityState.GetProperty("type").GetString();
                var type = typeByString(type_str);
                var ent = spawn_entity[type](Game._, physicsWorld, em);
                ent.RestoreState(entityState);
                em.AddEntity(ent);
            }

            foreach (var entityState in json.RootElement.GetProperty("track").EnumerateArray())
            {
                var type_str = entityState.GetProperty("type").GetString();
                var type = typeByString(type_str);
                var ent = spawn_track_entity[type](Game._, physicsWorld, track);
                ent.RestoreState(entityState);
                track.AddEntity(ent);
            }
        }
    }
}

