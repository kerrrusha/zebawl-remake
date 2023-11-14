using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Model
{
    internal class LevelReader
    {
        public List<Level> Load()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, "levels.json");

            string jsonContent = File.ReadAllText(filePath);

            return JsonUtility.FromJson<LevelsContainer>(jsonContent).levels;
        }
    }
}