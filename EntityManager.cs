using Dcrew.Camera;
using Microsoft.Xna.Framework;
using MonoGameJam3Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSastR.Core
{
    public class EntityManager
    {
        public EntityManager(MonoGameJam3Entry.Game gb)
        {
            this.game = gb;
        }
        MonoGameJam3Entry.Game game;
        public List<Entity> Entities = new List<Entity>();
        public List<Entity> InspectableEntities
        {
            get
            {
                List<Entity> ent = new();
                foreach(var e in Entities)
                {
                    if (e.ShowInInspector) ent.Add(e);
                }
                return ent;
            }
        }

        public List<Entity> SerializableEntities
        {
            get
            {
                return InspectableEntities.Where(ent =>
                {
                    Type t = ent.GetType();
                    return t != typeof(Track_Waypoints);
                }).ToList();
            }
        }

        public int AddEntity(Entity obj)
        {
            obj.NeedsToStart = true;
            Entities.Add(obj);
            return Entities.Count;
        }

        public void Update(GameTime time)
        {
            for (int i = Entities.Count-1; i >= 0; i--)
            {
                var c = Entities[i];
                if (c.Dead)
                {
                    continue;
                }
                if (c.NeedsToStart)
                {
                    c.Start();
                    c.NeedsToStart = false;
                }
                c.Update(time);
            }
        }

        public void Draw(GameTime time,Camera camera)
        {
            for(int i = Entities.Count - 1; i >= 0; i--){
                if (Entities[i].Dead)
                {
                    Entities.RemoveAt(i);
                    continue;
                }
                Entities[i].camera = camera;
                Entities[i].Draw(time);
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i]?.Destroy();
            }
            Entities.Clear();
        }
    }
}
