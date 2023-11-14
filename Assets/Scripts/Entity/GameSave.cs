using System;
using Unity.VisualScripting.Dependencies.Sqlite;

namespace Assets.Scripts.Entity
{
    internal class GameSave
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string LevelName { get; set; }
        public DateTime CreatedAt { get; set; }

        public GameSave(string levelName, DateTime createdAt)
        {
            LevelName = levelName;
            CreatedAt = createdAt;
        }

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
