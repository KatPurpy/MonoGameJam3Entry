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
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public sealed partial class GameScene : Scene
    {
        Type entityBrush;
        string fileName = "FILENAME";
        bool saveMenuActive;
        private void EditorFunctions(GameTime time)
        {
            ImGuiRenderer.BeforeLayout(time);

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Save"))
                    {
                        saveMenuActive = true;
                    }
                    ImGui.EndMenu();
                }


                ImGui.EndMainMenuBar();
            }

            if (saveMenuActive && ImGui.Begin("SaveModal"))
            {
                ImGui.InputText("FileName", ref fileName, 64);
                if (ImGui.Button("SAVE"))
                {
                    Save(fileName);
                    saveMenuActive = false;
                }
                if (ImGui.Button("Cancel"))
                {
   
                    saveMenuActive = false;
                    
                }
                ImGui.EndPopup();
            }

            ImGui.Checkbox("Draw colliders", ref drawColliders);

            ImGui.Begin("ENTITY EDITOR");

            int mode = (int)editMode;

            ImGui.LabelText("", "Entity type");

            if (ImGui.RadioButton("Entities", ref mode, 0)) editMode = EditMode.Entity;
            if (ImGui.RadioButton("Track", ref mode, 1)) editMode = EditMode.Track;

            ImGui.Separator();

            switch (editMode)
            {

                case EditMode.Entity:
                    EntityEditor(time, "Entities", em, ref currentEntityID, ref focusedEntity, instancableEntities);
                    break;
                case EditMode.Track:
                    EntityEditor(time, "Entities", track, ref track_ThingID, ref focusedEntity, backgroudnEntities);
                    break;
            }
            ImGui.End();
            ImGui.Begin("Entity Properties");
            try
            {
                focusedEntity?.IMGUI(time);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            ImGui.End();
            ImGuiRenderer.AfterLayout();
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

            if (ImGui.BeginMenu("Set Entity Brush"))
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

            ImGui.LabelText("Selected Entity","Selected entity type: " + entityBrush);

            //var a = manager.Entities.Where(e => e.ShowInInspector).Select(a => manager.Entities.IndexOf(a).ToString() + " (" + a.GetType().Name + ")").ToArray();


            List<StringBuilder> s = new List<StringBuilder>();
            for (int i = 0; i < manager.InspectableEntities.Count; i++)
            {
                var ent = manager.InspectableEntities[i];


                if (ent.ShowInInspector)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(i);
                    sb.Append(" ");
                    sb.Append(ent.GetType().Name);
                    s.Add(sb);
                }

            }

            ImGui.ListBox("", ref currentEntityID, s.Select(s => s.ToString()).ToArray(), s.Count, 10);


            if (ImGui.Button("Delete Entity"))
            {
                ImGui.OpenPopup("CONFIRM DELETION?");
            }


            if (ImGui.BeginPopup("CONFIRM DELETION?"))
            {
                if (ImGui.Button("YES"))
                {
                    manager.Entities[currentEntityID].Destroy();
                    ImGui.CloseCurrentPopup();
                }
                if (ImGui.Button("NO"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }


            if (-1 < currentEntityID && currentEntityID < manager.InspectableEntities.Count)
            {
                var pos = NumericToXNA.ConvertXNAToNumeric(Vector2.Transform(manager.InspectableEntities[currentEntityID].VisualPosition, camera.View()));

                currentEntity = manager.InspectableEntities[currentEntityID];

                ImGui.GetBackgroundDrawList().AddCircleFilled(pos, MathF.Abs(MathF.Sin(2 * (float)time.TotalGameTime.TotalSeconds)) * 20 + 5, (uint)(MathHelper.Lerp(0x55FFFF00, 0x55FF00FF, MathF.Abs(MathF.Sin(4 * (float)time.TotalGameTime.TotalSeconds)))));
            }
            var m = ImGui.GetMousePos();
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right))
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
            manager.AddEntity(ent = instancables[entityType](Game._, this.world, manager));
            currentEntityID = manager.Entities.Count - 1;
            return currentEntityID;
        }

        private void Save(string fileName)
        {
            using (var stream = new MemoryStream()) {
                Utf8JsonWriter writer = new Utf8JsonWriter(stream);
                writer.WriteStartObject();

                track.InspectableEntities[0].SerializeState(writer);

                writer.WriteEndObject();
                writer.Flush();

                Debug.WriteLine(UTF8Encoding.UTF8.GetString(stream.ToArray()));

                var asww = JsonDocument.Parse(UTF8Encoding.UTF8.GetString(stream.ToArray()));

                track.InspectableEntities[0].RestoreState(ref asww);
            }

            

            /*var strWriter = new StringWriter();

            var xmlSerializer = new XmlSerializer(track.InspectableEntities[0].GetType());
            
            xmlSerializer.Serialize(strWriter, track.InspectableEntities[0]);
            */
            //Debug.WriteLine(strWriter.ToString());
            // File.WriteAllText(fileName,);
        }
    }
}

