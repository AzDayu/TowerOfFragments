using System;
using System.Threading;

namespace TowerOfFragments
{
    internal class BattleManager
    {
        private ICombatant _player;
        private ICombatant _monster;

        public BattleManager(ICombatant player, ICombatant monster)
        {
            _player = player;
            _monster = monster;
        }

        public void StartBattle()
        {
            while (_player.IsAlive && _monster.IsAlive)
            {
                bool playerTurnEnd = false;

                while (!playerTurnEnd && _player.IsAlive)
                {
                    Console.Clear();
                    Console.WriteLine($"=== 전투: {_monster.Name} (HP: {_monster.HP}/{_monster.MaxHP}) ===");
                    Console.WriteLine($"--- {_player.Name}의 HP: {_player.HP}/{_player.MaxHP} ---");
                    Console.WriteLine("\n[플레이어의 턴] 무엇을 하시겠습니까?");
                    Console.WriteLine("1. 공격");
                    Console.WriteLine("2. 인벤토리 (장비 교체)");
                    Console.Write("선택: ");

                    string input = Console.ReadLine();

                    if (input == "1")
                    {
                        Console.WriteLine($"\n{_player.Name}의 공격!");
                        _monster.TakeDamage(_player.ATK);
                        playerTurnEnd = true;
                        Thread.Sleep(1000);
                    }
                    else if (input == "2")
                    {
                        OpenBattleInventory();
                    }
                }

                if (!_monster.IsAlive)
                {
                    Console.WriteLine($"\\n🏆 {_monster.Name}을(를) 물리쳤습니다!");
                    break;
                }

                Console.WriteLine($"\n--- {_monster.Name}의 턴 ---");
                if (_monster is Monster m) m.TakeTurn(_player);
                Thread.Sleep(1500);
            }
        }

        private void OpenBattleInventory()
        {
            if (_player is Player p)
            {
                p.MyInventory.ShowInventory(
                p.Job,
                p.EquippedWeapon,
                 p.EquippedArmor,
                (selectedWeapon) =>
                {
                    p.EquippedWeapon = selectedWeapon;
                },
                (selectedArmo) =>
                {
                    p.EquippedArmor = selectedArmo;
                },
                () =>
                {
                    CombinationManager combo = new CombinationManager();
                    combo.ShowCombinationMenu(p.Job, p.MyInventory);
                }
            );
            }
        }
    }
    
}
