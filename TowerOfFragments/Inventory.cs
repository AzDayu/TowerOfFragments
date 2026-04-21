using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public void AddWeapon(WeaponData data)
        {
            Weapons.Add(new Weapon(data));
            Console.WriteLine($"[무기 획득] {data.Name}");
        }

        public void AddArmor(ArmorData data)
        {
            Armors.Add(new Armor(data));
            Console.WriteLine($"[방어구 획득] {data.Name}");
        }

        public void AddMaterial(MaterialsData data)
        {
            Materials.Add(data);
            Console.WriteLine($"[소재 획득] {data.Name}");
        }

        public void ShowInventory()
        {
            Console.WriteLine("\n========= [ 인벤토리 ] =========");

            Console.WriteLine("\n[무기]");
            if (Weapons.Count == 0) Console.WriteLine("- 비어 있음");
            foreach (var w in Weapons)
                Console.WriteLine($"- {w.Name} (공격력: {w.CurrentDamage}, 내구도: {w.CurrentDurability})");

            Console.WriteLine("\n[방어구]");
            if (Armors.Count == 0) Console.WriteLine("- 비어 있음");
            foreach (var a in Armors)
                Console.WriteLine($"- {a.Name} (방어력: {a.CurrentDefense}, 내구도: {a.CurrentDurability})");

            Console.WriteLine("\n[소재]");
            if (Materials.Count == 0) Console.WriteLine("- 비어 있음");
            foreach (var m in Materials)
                Console.WriteLine($"- {m.Name} (조합 시 공격력 +{m.ATK})");

            Console.WriteLine("================================");
        }
    }
}
