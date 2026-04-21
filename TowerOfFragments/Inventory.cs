using System;
using System.Collections.Generic;
using System.Threading;

namespace TowerOfFragments
{

    public abstract class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        protected Item(IGameData data)
        {
            Id = data.Id;
            Name = data.Name;
            Description = data.Description;
        }
    }
    public class Weapon : Item
    {
        public int CurrentDamage { get; set; }
        public int CurrentDurability { get; set; }

        public Weapon(WeaponData data) : base(data)
        {
            CurrentDamage = data.BaseDamage;
            CurrentDurability = data.Durability;
        }
    }

    public class Armor : Item
    {
        public int CurrentDefense { get; set; }
        public int CurrentDurability { get; set; }

        public Armor(ArmorData data) : base(data)
        {
            CurrentDefense = data.Defense;
            CurrentDurability = data.Durability;
        }
    }

    public class Inventory
    {
        public List<Weapon> Weapons { get; private set; } = new List<Weapon>();
        public List<Armor> Armors { get; private set; } = new List<Armor>();
        public List<MaterialsData> Materials { get; private set; } = new List<MaterialsData>();

        public void ShowInventory(string jobName, Weapon WeaponEquipped, Armor ArmorEquipped, Action<Weapon> onWeaponEquip, Action<Armor> onArmorEquip, Action onCombine)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== 인벤토리 (직업: {jobName}) ===");

                Console.WriteLine("\n[ 무기 목록 ]");
                if (Weapons.Count == 0) Console.WriteLine("보유 중인 무기가 없습니다.");
                else
                {
                    for (int i = 0; i < Weapons.Count; i++)
                    {
                        string equipStatus = (Weapons[i] == WeaponEquipped) ? "[장착중] " : "";
                        Console.WriteLine($"{i + 1}. {equipStatus}{Weapons[i].Name} (공격력: {Weapons[i].CurrentDamage})");
                    }
                }

                Console.WriteLine("\n[ 방어구 목록 ]");
                if (Armors.Count == 0) Console.WriteLine("보유 중인 방어구가 없습니다.");
                else
                {
                    for (int i = 0; i < Armors.Count; i++)
                    {
                        string equipStatus = (Armors[i] == ArmorEquipped) ? "[장착중] " : "";
                        Console.WriteLine($"{i + 1}. {equipStatus}{Armors[i].Name} (방어력: {Armors[i].CurrentDefense})");
                    }
                }

                Console.WriteLine("\n[ 보유 소재 ]");
                if (Materials.Count == 0)
                {
                    Console.WriteLine("보유 중인 소재가 없습니다.");
                }
                else
                {
                    foreach (var mat in Materials)
                    {
                        Console.WriteLine($"- {mat.Name} (ATK +{mat.ATK})");
                    }
                }

                Console.WriteLine("\n1. 무기 장착 2. 방어구 장착 3. 조합하기  0. 나가기");
                Console.Write("선택: ");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    Console.Write("장착할 무기 번호를 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= Weapons.Count)
                    {
                        WeaponEquipped = Weapons[idx - 1];
                        onWeaponEquip?.Invoke(WeaponEquipped);
                        Console.WriteLine($"\n[알림] {WeaponEquipped.Name}(으)로 교체되었습니다.");
                        Thread.Sleep(500);
                    }
                }
                else if (input == "2")
                {
                    Console.Write("장착할 방어구 번호를 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= Armors.Count)
                    {
                        ArmorEquipped = Armors[idx - 1];
                        onArmorEquip?.Invoke(ArmorEquipped);
                        Console.WriteLine($"\n[알림] {ArmorEquipped.Name}(으)로 교체되었습니다.");
                        Thread.Sleep(500);
                    }
                }
                else if (input == "3") { onCombine?.Invoke(); }
                else if (input == "0") break;
            }
        }

        public void AddWeapon(WeaponData data) => Weapons.Add(new Weapon(data));

        public void AddArmor(ArmorData data) => Armors.Add(new Armor(data));

        public void AddMaterial(MaterialsData data){Materials.Add(data);}

    }
}
