using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSastR.Core
{
    class SceneManager
    {
        Game gameBase;
        public Scene Current;

        public SceneManager(Game gb)
        {
            gameBase = gb;
        }

        public void SwitchScene(Scene newScene)
        {
            Current?.Leave();
            Current = newScene;
            Current.Enter();
        }

        public void Update(GameTime time)
        {
            Current.Update(time);
        }

        public void Draw(GameTime time)
        {
            Current.Draw(time);
        }
    }
}
