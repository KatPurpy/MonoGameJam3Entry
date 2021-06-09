using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSastR.Core
{
    public class EntityManager
    {
        public EntityManager(Game gb)
        {
            this.game = gb;
        }
        Game game;
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
                    Entities.RemoveAt(i);
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

        public void Draw(GameTime time)
        {
            for(int i = Entities.Count - 1; i >= 0; i--){
                Entities[i].Draw(time);
            }
        }
    }
}
