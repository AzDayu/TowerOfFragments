using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TowerOfFragments
{
    internal class StageManager
    {
        private Player _player;
        private int _currentFloor = 1;
        private const int MAX_FLOOR = 10;
        private Random _random = new Random();

        public StageManager(Player player)
        {
            _player = player;
        }
        public void StartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("          파편의 탑에 입장하셨습니다...           ");
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
            Thread.Sleep(1500);

            while (_currentFloor <= MAX_FLOOR && _player.IsAlive)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n▶▶▶ [ 현재 층수 : {_currentFloor} / {MAX_FLOOR} 층 ] ◀◀◀\n");
                Console.ResetColor();

                if (_currentFloor == MAX_FLOOR)
                {
                    EnterBossRoom();
                    break;
                }

                int choice = ChooseRoom();
                ProcessRoom(choice);

                if (_player.IsAlive)
                {
                    Console.WriteLine("\n[Enter] 키를 누르면 다음 층으로 이동합니다...");
                    Console.ReadLine();
                    _currentFloor++;
                }
            }

            if (_player.IsAlive)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==================================================");
                Console.WriteLine(" [축하합니다!] 파편의 왕을 쓰러뜨리고 탑을 정복했습니다! ");
                Console.WriteLine("==================================================");
                Console.ResetColor();
            }
        }

        private int ChooseRoom()
        {
            Console.WriteLine("앞에 세 갈래의 문이 있습니다. 어디로 가시겠습니까?\n");
            Console.WriteLine("1. [전투의 방] 몬스터의 기운이 느껴집니다. (보상 획득 가능)");
            Console.WriteLine("2. [휴식의 방] 안전해 보입니다. (체력 회복)");
            Console.WriteLine("3. [미지의 방] 무슨 일이 일어날지 모릅니다. (랜덤 이벤트)\n");
            Console.Write("선택 (1~3) : ");

            string input = Console.ReadLine();
            if (input == "1" || input == "2" || input == "3")
            {
                return int.Parse(input);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 강제로 전투의 방에 입장합니다.");
                return 1;
            }
        }

        private void ProcessRoom(int choice)
        {
            Console.Clear();
            switch (choice)
            {
                case 1:
                    EnterBattleRoom();
                    break;
                case 2:
                    EnterRestRoom();
                    break;
                case 3:
                    EnterRandomRoom();
                    break;
            }
        }

        private void EnterBattleRoom()
        {
            Console.WriteLine("문을 열자 몬스터가 달려듭니다!\n");

            var normalMonsters = GameDataManager.Instance.MonstersDataList.Values
                .Where(m => !m.Id.Contains("Boss") && m.Id.Contains("Mon"))
                .ToList();

            if (normalMonsters.Count == 0)
            {
                Console.WriteLine("몬스터 데이터가 없습니다.");
                return;
            }

            var randomData = normalMonsters[_random.Next(normalMonsters.Count)];
            Monster monster = CreateMonsterInstance(randomData);

            BattleManager battle = new BattleManager(_player, monster);
            battle.StartBattle();
        }

        private void EnterBossRoom()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("거대한 문맥 너머로 압도적인 기운이 느껴집니다...");
            Console.WriteLine("탑의 주인이 당신을 기다리고 있습니다!\n");
            Console.ResetColor();
            Thread.Sleep(2000);

            var bossData = GameDataManager.Instance.GetMonstersData("Mon_28");
            if (bossData == null)
            {
                Console.WriteLine("보스 데이터를 찾을 수 없습니다.");
                return;
            }

            Monster boss = CreateMonsterInstance(bossData);

            BattleManager battle = new BattleManager(_player, boss);
            battle.StartBattle();
        }

        private void EnterRestRoom()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("조용하고 따뜻한 방입니다. 모닥불 곁에서 휴식을 취합니다.");

            int healAmount = (int)(_player.MaxHP * 0.3f);
            _player.CurrentHP = Math.Min(_player.MaxHP, _player.CurrentHP + healAmount);

            Console.WriteLine($"체력을 {healAmount}만큼 회복했습니다! (현재 HP: {_player.CurrentHP}/{_player.MaxHP})");
            Console.ResetColor();
        }

        private void EnterRandomRoom()
        {
            int rand = _random.Next(0, 100);
            if (rand < 50)
            {
                Console.WriteLine("함정이었습니다! 체력을 조금 잃었습니다.");
                _player.CurrentHP -= 10;
            }
            else
            {
                Console.WriteLine("바닥에 떨어진 파편을 주웠습니다!");
                var materials = GameDataManager.Instance.MaterialsDataList.Values.ToList();
                if (materials.Count > 0)
                {
                    var mat = materials[_random.Next(materials.Count)];
                    _player.MyInventory.AddMaterial(mat);
                }
            }
        }

        private Monster CreateMonsterInstance(MonstersData data)
        {
            if (data.Id.Contains("Boss") || data.Id == "Mon_28" || data.Id == "Mon_20" || data.Id == "Mon_10")
            {
                return new BossMonster(data);
            }
            else if (data.Id == "Mon_03" || data.Id == "Mon_19")
            {
                return new VampireMonster(data);
            }
            else if (data.Id == "Mon_18")
            {
                return new HealerMonster(data);
            }
            else
            {
                return new NormalMonster(data);
            }
        }
    }
}
