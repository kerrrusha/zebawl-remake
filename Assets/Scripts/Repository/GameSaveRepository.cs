using Assets.Scripts.Entity;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;

namespace Assets.Scripts.Repository
{
    internal class GameSaveRepository
    {
        IDbConnection dbConnection;

        public GameSaveRepository()
        {
            string dbUri = "URI=file:saves.sqlite";
            dbConnection = new SqliteConnection(dbUri);
            dbConnection.Open();

            CreateTable();
        }
         
        private void CreateTable()
        {
            IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
            dbCommandCreateTable.CommandText = @"CREATE TABLE IF NOT EXISTS Game_Save (
                                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    Level_Name TEXT NOT NULL,
                                                    Created_At TEXT NOT NULL
                                                );";
            dbCommandCreateTable.ExecuteReader();
        }

        public void Add(GameSave gameSave)
        {
            IDbCommand dbCommandInsertValue = dbConnection.CreateCommand();
            dbCommandInsertValue.CommandText = $"INSERT INTO Game_Save(Level_Name, Created_At) VALUES ('{gameSave.LevelName}', '{gameSave.CreatedAt}')";
            dbCommandInsertValue.ExecuteNonQuery();
        }

        public GameSave GetLatestSave()
        {
            IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
            dbCommandReadValues.CommandText = "SELECT * FROM Game_Save order by id desc limit 1";
            IDataReader dataReader = dbCommandReadValues.ExecuteReader();

            if (dataReader.Read())
            {
                return MapToEntity(dataReader);
            }

            return null;
        }

        public List<GameSave> GetAll()
        {
            IDbCommand dbCommandReadValues = dbConnection.CreateCommand();
            dbCommandReadValues.CommandText = "SELECT * FROM Game_Save";
            IDataReader dataReader = dbCommandReadValues.ExecuteReader();

            List<GameSave> result = new List<GameSave>();

            while (dataReader.Read())
            {
                result.Add(MapToEntity(dataReader));
            }

            return result;
        }

        private GameSave MapToEntity(IDataReader dataReader)
        {
            int id = dataReader.GetInt32(0);
            string levelName = dataReader.GetString(1);

            string dateTimeStr = dataReader.GetString(2);
            DateTime createdAt = DateTime.MinValue;
            if (dateTimeStr != null && dateTimeStr.Trim().Length != 0)
            {
                createdAt = DateTime.Parse(dateTimeStr);
            }
            else
            {
                throw new InvalidCastException("Can't map 'game_save' entity: 'created_at' is null");
            }

            return new GameSave(id, levelName, createdAt);
        }

        public void ClearOldSaves(int allowedSize)
        {
            GameSave latestSave = GetLatestSave();

            if (latestSave.Id > allowedSize)
            {
                int deleteBeforeId = latestSave.Id - allowedSize;

                IDbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = $"delete from Game_Save where id <= {deleteBeforeId}";
                dbCommand.ExecuteNonQuery();
            }
        }

        ~GameSaveRepository()
        {
            dbConnection.Close();
        }
    }
}
