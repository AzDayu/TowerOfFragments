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

        public Player(string _name, CharacterData data)
        {
            Name = _name;
            Job = data.Name;
            MaxHP = data.HP;
            CurrentHP = data.HP;
            DEF = data.DEF;
            CRI = data.CRI;
            Passive = data.Passive;
            Description = data.Description;
        }
    }

    public class Warrior : Player
    {
        // 부모에게 데이터를 넘겨주고, 본인은 추가적인 작업만 수행
        public Warrior(string name, CharacterData data) : base(name, data)
        {
            // 여기에 워리어만 가질 수 있는 초기 아이템 지급 등을 넣을 수 있음
            Console.WriteLine("워리어 전용 방패가 지급되었습니다.");
        }
    }


    public class Rogue : Player
    {
        // 부모에게 데이터를 넘겨주고, 본인은 추가적인 작업만 수행
        public Rogue(string name, CharacterData data) : base(name, data)
        {
            // 여기에 워리어만 가질 수 있는 초기 아이템 지급 등을 넣을 수 있음
            Console.WriteLine("Rogue 전용 방패가 지급되었습니다.");
        }
    }

    public class Mage : Player
    {
        // 부모에게 데이터를 넘겨주고, 본인은 추가적인 작업만 수행
        public Mage(string name, CharacterData data) : base(name, data)
        {
            // 여기에 워리어만 가질 수 있는 초기 아이템 지급 등을 넣을 수 있음
            Console.WriteLine("Mage 전용 방패가 지급되었습니다.");
        }
    }


    public class Paladine : Player
    {
        // 부모에게 데이터를 넘겨주고, 본인은 추가적인 작업만 수행
        public Paladine(string name, CharacterData data) : base(name, data)
        {
            // 여기에 워리어만 가질 수 있는 초기 아이템 지급 등을 넣을 수 있음
            Console.WriteLine("Paladine 전용 방패가 지급되었습니다.");
        }
    }

    public class Archer : Player
    {
        // 부모에게 데이터를 넘겨주고, 본인은 추가적인 작업만 수행
        public Archer(string name, CharacterData data) : base(name, data)
        {
            // 여기에 워리어만 가질 수 있는 초기 아이템 지급 등을 넣을 수 있음
            Console.WriteLine("Archer 전용 방패가 지급되었습니다.");
        }
    }
}
