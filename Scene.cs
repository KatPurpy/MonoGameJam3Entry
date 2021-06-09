using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSastR.Core
{
    public abstract class Scene
    {
        public Scene()
        {
        }

        public abstract void Enter();
        public abstract void Leave();

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
