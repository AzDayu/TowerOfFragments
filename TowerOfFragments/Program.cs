using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfFragments
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameUtil.LoadFullData();

            Console.WriteLine("플레이어님의 성함을 입력해주세요:");
            string name = Console.ReadLine();

            var jobData = GameDataManager.Instance.GetCharacterData("Job_02");
            Player player = new Rogue(name, jobData);

            StageManager stageManager = new StageManager(player);
            stageManager.StartGame();
        }
    }
}
