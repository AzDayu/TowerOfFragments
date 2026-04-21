using System;

namespace TowerOfFragments
{
    public abstract class Player : ICombatant
    {
        public string Name { get; protected set; }
        public string Job { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }

        public int BaseATK { get; set; }
        public int BaseDEF { get; set; }
        public int ATK => BaseATK + (EquippedWeapon?.CurrentDamage ?? 0);
        public int DEF => BaseDEF + (EquippedArmor?.CurrentDefense ?? 0);

        public int CRI { get; set; }
        public Inventory MyInventory { get; private set; }
        public Weapon EquippedWeapon { get; set; }
        public Armor EquippedArmor { get; set; }
        public bool IsAlive => HP > 0;

        public Player(string name, CharacterData data, Inventory inventory)
        {
            Name = name;
            Job = data.Name;
            MaxHP = data.HP;
            HP = data.HP;
            BaseATK = data.ATK;
            BaseDEF = data.DEF;
            CRI = data.CRI;
            MyInventory = inventory;

            Console.WriteLine("[시스템] 랜덤 장비가 지급되었습니다.");
        }

        public void TakeDamage(int damage)
        {
            int finalDamage = Math.Max(1, damage - DEF);
            HP -= finalDamage;
            Console.WriteLine($"{Name}이(가) {finalDamage}의 피해를 입었습니다. (남은 HP: {HP})");
        }
    }

    public class Warrior : Player
    {
        public Warrior(string name, CharacterData data, Inventory inventory) : base(name, data, inventory)
        {
            
        }
    }

    public class Rogue : Player
    {
        public Rogue(string name, CharacterData data, Inventory inventory) : base(name, data, inventory)
        {

        }
    }

    public class Mage : Player
    {
        public Mage(string name, CharacterData data, Inventory inventory) : base(name, data, inventory)
        {

        }
    }

    public class Paladin : Player
    {
        public Paladin(string name, CharacterData data, Inventory inventory) : base(name, data, inventory)
        {

        }
    }

    public class Archer : Player
    {
        public Archer(string name, CharacterData data, Inventory inventory) : base(name, data, inventory)
        {

        }
    }
}