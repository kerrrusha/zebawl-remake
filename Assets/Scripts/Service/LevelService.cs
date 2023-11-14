using Assets.Scripts.Model;
using System.Collections.Generic;

namespace Assets.Scripts.Service
{
    internal class LevelService
    {
        private List<Level> levels;

        public LevelService() 
        {
            levels = new LevelReader().Load();
        }

        public List<Level> GetAll() 
        {
            return levels;
        }

        public Level FindByName(string levelName)
        {
            foreach (Level level in levels)
            {
                if (level.name.Equals(levelName))
                {
                    return level;
                }
            }
            throw new KeyNotFoundException($"There is not such level with levelName: {levelName}");
        }
    }
}
