using Assets.Scripts.Entity;
using Assets.Scripts.Repository;
using System.Collections.Generic;

namespace Assets.Scripts.Service
{
    internal class GameSaveService
    {
        private const int ALLOWED_SIZE = 10;

        private GameSaveRepository gameSaveRepository;

        public GameSaveService()
        {
            gameSaveRepository = new GameSaveRepository();
        }

        public void Add(GameSave gameSave)
        {
            gameSaveRepository.Add(gameSave);
            gameSaveRepository.ClearOldSaves(ALLOWED_SIZE);
        }

        public GameSave GetLatestSave()
        {
            var latestSave = gameSaveRepository.GetLatestSave();

            if (latestSave == null)
            {
                throw new KeyNotFoundException("No game saves found.");
            }
            return latestSave;
        }

        public List<GameSave> GetAll()
        {
            return gameSaveRepository.GetAll();
        }

        public string GetInfo()
        {
            string response = string.Empty;
            
            List<GameSave> saves = GetAll();
            response += $"Found {saves.Count} saves:\n";
            
            foreach (GameSave save in saves)
            {
                response += $"{save}\n";
            }

            return response;
        }
    }
}
