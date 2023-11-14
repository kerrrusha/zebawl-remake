using System;
using System.Diagnostics;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Assets.Scripts.Entity
{
    internal class GameSave
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string LevelName { get; set; }
        public DateTime CreatedAt { get; set; }

        public GameSave(int id, string levelName, DateTime createdAt)
        {
            Id = id;
            LevelName = levelName;
            CreatedAt = createdAt;
        }

        public override string ToString()
        {
            return $"{{id={Id} level_name={LevelName}, created_at={CreatedAt}}}";
        }
    }
}
