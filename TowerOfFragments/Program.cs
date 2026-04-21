using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TowerOfFragments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameUtil.LoadFullData();

            Console.Write("당신의 이름을 입력하세요: ");
            string name = Console.ReadLine();

            CharacterSelector selector = new CharacterSelector();
            Player player = selector.SelectCharacter(name);
            

            Console.WriteLine($"\n[알림] {player.Job} '{player.Name}' 캐릭터가 생성되었습니다!");
            Thread.Sleep(2000);

            StageManager stage = new StageManager(player);
            stage.StartGame();
        }
    }

    
    public class CharacterSelector
    {
        Inventory newInventory = new Inventory();

        public Player SelectCharacter(string playerName)
        {
            var jobList = GameDataManager.Instance.CharacterDataList.Values
                .Where(d => d.Id.StartsWith("Job"))
                .ToList();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine($"         [ 파편의 탑 : 직업 선택 ]          ");
                Console.WriteLine($"         반갑습니다, {playerName}님.          ");
                Console.WriteLine("==================================================");
                Console.WriteLine("원하시는 직업의 번호를 입력해주세요.\n");

                for (int i = 0; i < jobList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {jobList[i].Name}");
                }
                Console.WriteLine("==================================================");
                Console.Write("선택 : ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= jobList.Count)
                {
                    var selectedData = jobList[choice - 1];

                    if (ShowJobDetails(selectedData))
                    {
                        Player player = CreatePlayerInstance(playerName, selectedData);

                        GiveRandomInitialItems(player);

                        return player;
                    }

                }
                else
                {
                    Console.WriteLine("\n[!] 잘못된 입력입니다. 다시 선택해주세요.");
                    System.Threading.Thread.Sleep(1000);
                }
            }

            

        }
        private bool ShowJobDetails(CharacterData data)
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine($"         [ 직업 정보 : {data.Name} ]          ");
            Console.WriteLine("==================================================");
            Console.WriteLine($"설명: {data.Description}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"- 체력: {data.HP}");
            Console.WriteLine($"- 공격: {data.ATK}");
            Console.WriteLine($"- 방어: {data.DEF}");
            Console.WriteLine($"- 치명: {data.CRI}%");
            Console.WriteLine($"\n▶ 패시브: {data.Passive}");
            Console.WriteLine("==================================================");
            Console.Write("\n이 직업으로 시작하시겠습니까? (Y/N) : ");

            string input = Console.ReadLine()?.ToUpper();
            return input == "Y";
        }

        private Player CreatePlayerInstance(string name, CharacterData data)
        {
            switch (data.Id)
            {
                case "Job_01":
                    return new Warrior(name, data, newInventory);
                case "Job_02":
                    return new Rogue(name, data, newInventory);
                case "Job_03":
                    return new Mage(name, data, newInventory);
                case "Job_04":
                    return new Paladin(name, data, newInventory);
                case "Job_05":
                    return new Archer(name, data, newInventory);
                default:
                    return new Warrior(name, data, newInventory);
            }
        }

        private void GiveRandomInitialItems(Player player)
        {
            Random rand = new Random();
            var dataManager = GameDataManager.Instance;

            var allWeapons = new List<WeaponData>(dataManager.WeaponDataList.Values);
            for (int i = 0; i < 3; i++)
            {
                if (allWeapons.Count > 0)
                {
                    var randomWeapon = allWeapons[rand.Next(allWeapons.Count)];
                    player.MyInventory.AddWeapon(randomWeapon);
                }
            }
            if (player.MyInventory.Weapons.Count > 0)
            {
                player.EquippedWeapon = player.MyInventory.Weapons[0];
            }

            var allArmors = new List<ArmorData>(dataManager.ArmorDataList.Values);
            for (int i = 0; i < 3; i++)
            {
                if (allArmors.Count > 0)
                {
                    var randomArmor = allArmors[rand.Next(allArmors.Count)];
                    player.MyInventory.AddArmor(randomArmor);
                }
            }
            if (player.MyInventory.Armors.Count > 0)
            {
                player.EquippedArmor = player.MyInventory.Armors[0];
            }

            var allMaterials = new List<MaterialsData>(dataManager.MaterialsDataList.Values);
            for (int i = 0; i < 3; i++)
            {
                if (allMaterials.Count > 0)
                {
                    var randomMat = allMaterials[rand.Next(allMaterials.Count)];
                    player.MyInventory.AddMaterial(randomMat);
                }
            }

            Console.WriteLine("\n[시스템] 모험에 필요한 기본 보급품이 지급되었습니다!");
            System.Threading.Thread.Sleep(1500);
        }

    }
}
