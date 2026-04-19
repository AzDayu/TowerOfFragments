using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfFragments
{
    internal class GameUtil
    {
        public static void LoadFullData()
        {
            GameDataManager.Instance.LoadCharacterData(GetFullDataPath("Character"));
            GameDataManager.Instance.LoadWeaponData(GetFullDataPath("Weapon"));
            GameDataManager.Instance.LoadArmorData(GetFullDataPath("Armor"));
            GameDataManager.Instance.LoadMaterialsData(GetFullDataPath("Materials"));
            GameDataManager.Instance.LoadMonstersData(GetFullDataPath("Monsters"));

        }

        public static string GetFullDataPath(string dataTableName)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Console.WriteLine("테이블 이름이 올바르지 않습니다!");
                return string.Empty;
            }

            string relativePath = $"../../../../TowerOfFragments/JsonConverter/JsonOutput/{dataTableName}.json";
            string fullPath = Path.GetFullPath(relativePath);
            return fullPath;
        }

        public static int CalcCharacterFinalDamage(int curCharacterLevel, int levelPerDamage, bool isCritical)
        {
            int damagePerLevel = (curCharacterLevel + levelPerDamage);
            int finalDamage = isCritical ? (damagePerLevel * 2) : damagePerLevel;
            return finalDamage;
        }
    }
}
