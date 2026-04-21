using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TowerOfFragments
{
    internal class BattleManager
    {
        private Player _player;
        private Monster _monster;
        private Random _random = new Random();

        public BattleManager(Player player, Monster monster)
        {
            _player = player;
            _monster = monster;
        }

        public bool StartBattle()
        {
            Console.Clear();
            Console.WriteLine($"[!] {_monster.Name}(이)가 나타났습니다!");
            Console.WriteLine($"{_monster.Description}");
            Thread.Sleep(1000);

            while (_player.CurrentHP > 0 && _monster.CurrentHP > 0)
            {
                ShowBattleStatus();

                PlayerTurn();
                if (_monster.CurrentHP <= 0) break;

                Thread.Sleep(800);

                MonsterTurn();
                if (_player.CurrentHP <= 0) break;

                Thread.Sleep(1000);
            }

            return EndBattle();
        }

        private void ShowBattleStatus()
        {
            Console.WriteLine("\n========================================");
            Console.WriteLine($"{_player.Name} (HP: {_player.CurrentHP}/{_player.MaxHP})");
            Console.WriteLine($"{_monster.Name} (HP: {_monster.CurrentHP}/{_monster.MaxHP})");
            Console.WriteLine("========================================\n");
        }

        private void PlayerTurn()
        {
            Console.WriteLine($"{_player.Name}의 턴! 어떤 행동을 하시겠습니까?");
            Console.WriteLine("1. 공격  2. 인벤토리 확인");
            string input = Console.ReadLine();

            if (input == "1")
            {
                int totalAtk = _player.ATK + (_player.EquippedWeapon?.CurrentDamage ?? 0);
                bool isCritical = _random.Next(0, 100) < _player.CRI;

                int damage = GameUtil.CalcCharacterFinalDamage(totalAtk, 0, isCritical);
                int finalDamage = Math.Max(1, damage - _monster.DEF);

                _monster.CurrentHP -= finalDamage;

                Console.ForegroundColor = isCritical ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine($"\n[공격] {_player.Name}이(가) {_monster.Name}에게 {finalDamage}의 데미지를 입혔습니다! {(isCritical ? "(크리티컬!)" : "")}");
                Console.ResetColor();

                if (_player.EquippedWeapon != null)
                {
                    _player.EquippedWeapon.CurrentDurability--;
                    if (_player.EquippedWeapon.CurrentDurability <= 0)
                    {
                        Console.WriteLine($"[!] 무기({_player.EquippedWeapon.Name})가 파괴되었습니다!");
                        _player.EquippedWeapon = null;
                    }
                }
            }
            else
            {
                _player.MyInventory.ShowInventory();
                PlayerTurn();
            }
        }

        private void MonsterTurn()
        {
            Console.WriteLine($"\n{_monster.Name}의 공격!");

            int damage = _monster.ATK;
            int finalDamage = Math.Max(1, damage - _player.DEF);

            _player.CurrentHP -= finalDamage;
            Console.WriteLine($"{_player.Name}은(는) {finalDamage}의 피해를 입었습니다.");
        }

        private bool EndBattle()
        {
            if (_player.CurrentHP <= 0)
            {
                Console.WriteLine("\n[GAME OVER] 당신은 차원 파편 속으로 사라졌습니다...");
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[승리!] {_monster.Name}을(를) 처치했습니다!");
                Console.ResetColor();

                GetReward();
                return true;
            }
        }

        private void GetReward()
        {
            var materialDataList = GameDataManager.Instance.MaterialsDataList.Values.ToList();
            var reward = materialDataList[_random.Next(materialDataList.Count)];

            Console.WriteLine($"[보상] 전리품으로 '{reward.Name}'을(를) 획득했습니다.");
            _player.MyInventory.AddMaterial(reward);
        }
    }
}
