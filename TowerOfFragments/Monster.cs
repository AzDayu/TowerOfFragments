using System;

namespace TowerOfFragments
{
    public abstract class Monster : ICombatant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public string Description { get; set; }

        public bool IsAlive => HP > 0;

        protected Monster(MonstersData data)
        {
            Id = data.Id;
            Name = data.Name;
            MaxHP = data.HP;
            HP = data.HP;
            ATK = data.ATK;
            DEF = data.DEF;
            Description = data.Description;
        }

        public abstract void TakeTurn(ICombatant target);

        protected void NormalAttack(ICombatant target)
        {
            int damage = Math.Max(1, ATK - target.DEF);
            target.HP -= damage;
            Console.WriteLine($"{Name}의 공격! {target.Name}에게 {damage}의 피해를 입혔습니다.");
        }

        public void TakeDamage(int damage)
        {
            int finalDamage = Math.Max(1, damage - DEF);
            HP -= finalDamage;
            Console.WriteLine($"{Name}이(가) {finalDamage}의 피해를 입었습니다. (남은 HP: {HP})");
        }

    }

    public class NormalMonster : Monster
    {
        public NormalMonster(MonstersData data) : base(data) { }

        public override void TakeTurn(ICombatant target)
        {
            NormalAttack(target);
        }
    }

    public class VampireMonster : Monster
    {
        public VampireMonster(MonstersData data) : base(data) { }

        public override void TakeTurn(ICombatant target)
        {
            Random rand = new Random();
            if (rand.Next(0, 100) < 40)
            {
                int damage = Math.Max(1, ATK - target.DEF);
                target.HP -= damage;

                int healAmount = damage / 2;
                HP = Math.Min(MaxHP, HP + healAmount);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[흡혈] {Name}이(가) {target.Name}의 피를 흡수하여 {healAmount}만큼 회복했습니다!");
                Console.ResetColor();
            }
            else
            {
                NormalAttack(target);
            }
        }
    }

    public class HealerMonster : Monster
    {
        public HealerMonster(MonstersData data) : base(data) { }

        public override void TakeTurn(ICombatant target)
        {
            if (HP < (MaxHP * 0.3f))
            {
                int healAmount = 20;
                HP = Math.Min(MaxHP, HP + healAmount);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[치유] {Name}이(가) 마법을 사용하여 체력을 {healAmount}만큼 회복했습니다!");
                Console.ResetColor();
            }
            else
            {
                NormalAttack(target);
            }
        }
    }

    public class BossMonster : Monster
    {
        private int _turnCount = 0;

        public BossMonster(MonstersData data) : base(data) { }

        public override void TakeTurn(ICombatant target)
        {
            _turnCount++;

            if (_turnCount % 3 == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n[경고] {Name}이(가) 힘을 모아 치명적인 공격을 날립니다!");

                int heavyDamage = Math.Max(1, (int)(ATK * 1.5f) - target.DEF);
                target.HP -= heavyDamage;

                Console.WriteLine($"{target.Name}은(는) {heavyDamage}의 막대한 피해를 입었습니다!\n");
                Console.ResetColor();
            }
            else
            {
                NormalAttack(target);
            }
        }
    }
}
