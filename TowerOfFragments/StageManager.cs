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

        private List<RoomType> _currentRoomOptions = new List<RoomType>();

        public enum RoomType { NormalBattle, EliteBattle, Rest, RandomEvent }

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
            Console.WriteLine("==================================================");
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
                }
                else
                {
                    GenerateRoomOptions(); 
                    int choice = ChooseRoom();
                    ProcessRoom(choice);
                }

                if (_player.IsAlive)
                {
                    Console.WriteLine("\n[Enter] 키를 누르면 다음 층으로 이동합니다...");
                    Console.ReadLine();
                    _currentFloor++;
                }
            }

            if (_player.IsAlive && _currentFloor > MAX_FLOOR)
            {
                ShowVictoryEnding();
            }
        }

        private void GenerateRoomOptions()
        {
            _currentRoomOptions.Clear();
            Array values = Enum.GetValues(typeof(RoomType));

            for (int i = 0; i < 3; i++)
            {
                _currentRoomOptions.Add((RoomType)values.GetValue(_random.Next(values.Length)));
            }
        }

        private int ChooseRoom()
        {
            Console.WriteLine("앞에 세 갈래의 문이 나타났습니다. 어디로 향하시겠습니까?\n");

            for (int i = 0; i < _currentRoomOptions.Count; i++)
            {
                string roomName = "";
                string desc = "";

                switch (_currentRoomOptions[i])
                {
                    case RoomType.NormalBattle:
                        roomName = "[일반 전투]"; desc = "비교적 약한 몬스터의 기운이 느껴집니다."; break;
                    case RoomType.EliteBattle:
                        roomName = "[엘리트 전투]"; desc = "강력한 적의 압박감이 전해집니다. (고급 보상)"; break;
                    case RoomType.Rest:
                        roomName = "[휴식 공간]"; desc = "잠시 장비를 정비하고 숨을 돌릴 수 있습니다."; break;
                    case RoomType.RandomEvent:
                        roomName = "[미지의 방]"; desc = "무엇이 기다릴지 알 수 없는 기묘한 방입니다."; break;
                }
                Console.WriteLine($"{i + 1}. {roomName} - {desc}");
            }

            Console.Write("\n선택 (1~3) : ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
                return choice - 1;

            Console.Write("\n잘못 누르셨습니다 강제로 첫 번째 방으로 갑니다.");
            Thread.Sleep(1000);
            return 0; 
        }

        private void ProcessRoom(int index)
        {
            Console.Clear();
            RoomType selected = _currentRoomOptions[index];

            switch (selected)
            {
                case RoomType.NormalBattle: EnterBattleRoom(false); break;
                case RoomType.EliteBattle: EnterBattleRoom(true); break;
                case RoomType.Rest: EnterRestRoom(); break;
                case RoomType.RandomEvent: EnterRandomRoom(); break;
            }
        }

        private void EnterBattleRoom(bool isElite)
        {
            string battleType = isElite ? "엘리트 전투" : "일반 전투";
            Console.WriteLine($"[{battleType}] 문을 열자 적이 나타났습니다!\n");

            var allMonsters = GameDataManager.Instance.MonstersDataList.Values.ToList();
            List<MonstersData> pool = new List<MonstersData>();

            foreach (var m in allMonsters)
            {
                if (int.TryParse(m.Id.Replace("Mon_", ""), out int idNum))
                {
                    if (isElite)
                    {
                        if (idNum >= 17 && idNum <= 27) pool.Add(m);
                    }
                    else
                    {
                        if (idNum >= 1 && idNum <= 16) pool.Add(m);
                    }
                }
            }

            if (pool.Count == 0)
            {
                Console.WriteLine("해당하는 몬스터를 찾을 수 없습니다.");
                return;
            }

            Monster monster = CreateMonsterInstance(pool[_random.Next(pool.Count)]);
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
            Console.WriteLine("안전한 장소에 도착했습니다. 무엇을 하시겠습니까?");
            Console.WriteLine("1. 휴식 (HP 회복)");
            Console.WriteLine("2. 정비 (인벤토리 및 아이템 조합)");

            string input = Console.ReadLine();
            if (input == "1")
            {
                int healAmount = (int)(_player.MaxHP * 0.3f);
                _player.HP = Math.Min(_player.MaxHP, _player.HP + healAmount);

                Console.WriteLine($"체력을 {healAmount}만큼 회복했습니다! (현재 HP: {_player.HP}/{_player.MaxHP})");
                Thread.Sleep(1500);
            }
            else if (input == "2")
            {
                OpenInventory();
            }

            Console.Clear();
        }

        private void EnterRandomRoom()
        {
            _player.HP -= 10;
            Console.WriteLine($"함정 발동! 현재 체력: {_player.HP}");
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

        private void ShowVictoryEnding()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================");
            Console.WriteLine(" [축하합니다!] 파편의 왕을 쓰러뜨리고 탑을 정복했습니다! ");
            Console.WriteLine("==================================================");
            Console.WriteLine("\n플레이어 이름: " + _player.Name);
            Console.WriteLine("최종 도달 층수: " + (_currentFloor - 1) + "층");
            Console.WriteLine("\n당신의 전설은 파편의 탑에 영원히 기록될 것입니다.");
            Console.WriteLine("==================================================");
            Console.ResetColor();

            Console.WriteLine("\n[Enter] 키를 누르면 게임을 종료합니다.");
            Console.ReadLine();
        }

        private void OpenInventory()
        {
            _player.MyInventory.ShowInventory(
                _player.Job,
                _player.EquippedWeapon,
                 _player.EquippedArmor,
                (selectedWeapon) => {
                    _player.EquippedWeapon = selectedWeapon; 
                },
                (selectedArmo) => {
                    _player.EquippedArmor = selectedArmo;
                },
                () => {
                    CombinationManager combo = new CombinationManager();
                    combo.ShowCombinationMenu(_player.Job, _player.MyInventory);
                }
            );
        }
    }


}
