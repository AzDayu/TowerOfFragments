using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfFragments
{
    public class CharacterData : IGameData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int CRI { get; set; }
        public string Passive { get; set; }
        public string Description { get; set; }
    }

    public class WeaponData : IGameData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int BaseDamage { get; set; }
        public int Durability { get; set; }
        public string Description { get; set; }
    }

    public class ArmorData : IGameData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Defense { get; set; }
        public int Durability { get; set; }
        public string Description { get; set; }
    }

    public class MaterialsData : IGameData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public int DUR { get; set; }
        public string SpecialEffect { get; set; }
        public string Description { get; set; }
    }
}
