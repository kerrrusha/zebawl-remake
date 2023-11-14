using Assets.Scripts.Model;
using System;
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

        public Level FindNext(Level current)
        {
            List<Level> levels = GetAll();
            int currentLevelIndex = levels.IndexOf(current);

            if (currentLevelIndex == -1)
            {
                throw new KeyNotFoundException($"Current level {current} not found in levels list.");
            }
            if (currentLevelIndex == levels.Count - 1)
            {
                throw new IndexOutOfRangeException($"Current level {current} is the last one.");
            }

            return levels[currentLevelIndex + 1];
        }
    }
}
