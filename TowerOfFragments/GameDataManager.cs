using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace TowerOfFragments
{
    public interface IGameData
    {
        string Id { get; }
    }
    internal class GameDataManager
    {
        private static GameDataManager _instance;
        public static GameDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameDataManager();
                }
                return _instance;
            }
        }

        public Dictionary<string, CharacterData> CharacterDataList { get; private set; }
        public Dictionary<string, WeaponData> WeaponDataList { get; private set; }
        public Dictionary<string, ArmorData> ArmorDataList { get; private set; }
        public Dictionary<string, MaterialsData> MaterialsDataList { get; private set; }
        public Dictionary<string, MonstersData> MonstersDataList { get; private set; }

        private GameDataManager()
        {
            CharacterDataList = new Dictionary<string, CharacterData>();
            WeaponDataList = new Dictionary<string, WeaponData>();
            ArmorDataList = new Dictionary<string, ArmorData>();
            MaterialsDataList = new Dictionary<string, MaterialsData>();
            MonstersDataList = new Dictionary<string, MonstersData>();
        }

        private Dictionary<string, T> LoadData<T>(string jsonPath) where T : IGameData
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"[Error] 파일을 찾을 수 없습니다: {jsonPath}");
                return new Dictionary<string, T>();
            }

            try
            {
                string jsonString = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dataList = JsonSerializer.Deserialize<List<T>>(jsonString, options);

                if (dataList != null)
                {
                    Console.WriteLine($"{typeof(T).Name} 데이터를 {dataList.Count}개 로드했습니다.");
                    return dataList.ToDictionary(item => item.Id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{typeof(T).Name} JSON 로드 오류] {ex.Message}");
            }

            return new Dictionary<string, T>();
        }


        public void LoadCharacterData(string jsonPath)
        {
            CharacterDataList = LoadData<CharacterData>(jsonPath);
        }

        public void LoadWeaponData(string jsonPath)
        {
            WeaponDataList = LoadData<WeaponData>(jsonPath);
        }

        public void LoadArmorData(string jsonPath)
        {
            ArmorDataList = LoadData<ArmorData>(jsonPath);
        }

        public void LoadMaterialsData(string jsonPath)
        {
            MaterialsDataList = LoadData<MaterialsData>(jsonPath);
        }

        public void LoadMonstersData(string jsonPath)
        {
            MonstersDataList = LoadData<MonstersData>(jsonPath);
        }

        public CharacterData GetCharacterData(string id)
        {
            if (CharacterDataList == null || string.IsNullOrEmpty(id)) return null;

            return CharacterDataList.TryGetValue(id, out var item) ? item : null;
        }

        public WeaponData GetWeaponData(string id)
        {
            if (WeaponDataList == null || string.IsNullOrEmpty(id)) return null;

            return WeaponDataList.TryGetValue(id, out var data) ? data : null;
        }

        public ArmorData GetArmorData(string id)
        {
            if (ArmorDataList == null || string.IsNullOrEmpty(id)) return null;

            return ArmorDataList.TryGetValue(id, out var data) ? data : null;
        }

        public MaterialsData GetMaterialsData(string id)
        {
            if (MaterialsDataList == null || string.IsNullOrEmpty(id)) return null;

            return MaterialsDataList.TryGetValue(id, out var data) ? data : null;
        }
        public MonstersData GetMonstersData(string id)
        {
            if (MonstersDataList == null || string.IsNullOrEmpty(id)) return null;

            return MonstersDataList.TryGetValue(id, out var data) ? data : null;
        }
    }
}
