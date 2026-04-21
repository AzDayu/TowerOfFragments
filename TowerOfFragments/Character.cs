using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfFragments
{
    public abstract class Player
    {
        public string Name { get; protected set; }
        public string Job { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int CRI { get; set; }
        public string Passive { get; set; }
        public string Description { get; set; }

        public bool IsAlive => CurrentHP > 0;
        public Weapon EquippedWeapon { get; set; }

        public Inventory MyInventory { get; private set; } = new Inventory();

        public Player(string _name, CharacterData data)
        {
            Name = _name;
            Job = data.Name;
            MaxHP = data.HP;
            CurrentHP = data.HP;
            ATK = data.ATK;
            DEF = data.DEF;
            CRI = data.CRI;
            Passive = data.Passive;
            Description = data.Description;
        }
    }

    public class Warrior : Player
    {
        public Warrior(string name, CharacterData data) : base(name, data)
        {
            Console.WriteLine("[시스템] 워리어 전용 낡은 롱소드와 방패가 지급되었습니다.");
        }
    }

    public class Rogue : Player
    {
        public Rogue(string name, CharacterData data) : base(name, data)
        {
            Console.WriteLine("[시스템] 로그 전용 날렵한 쌍검이 지급되었습니다.");
        }
    }

    public class Mage : Player
    {
        public Mage(string name, CharacterData data) : base(name, data)
        {
            Console.WriteLine("[시스템] 메이지 전용 마력이 깃든 나무 지팡이가 지급되었습니다.");
        }
    }

    public class Paladin : Player
    {
        public Paladin(string name, CharacterData data) : base(name, data)
        {
            Console.WriteLine("[시스템] 팔라딘 전용 성스러운 둔기가 지급되었습니다.");
        }
    }

    public class Archer : Player
    {
        public Archer(string name, CharacterData data) : base(name, data)
        {
            Console.WriteLine("[시스템] 아처 전용 튼튼한 사냥활이 지급되었습니다.");
        }
    }
}