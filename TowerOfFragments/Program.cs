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

            string name;
            Console.WriteLine("플레이어님의 성함을 입력해주세요");
            name = Console.ReadLine();

            Console.WriteLine($"플레이어님의 성함은 {name} 맞습니까?");
            var data = GameDataManager.Instance.GetCharacterData("Job_02");
            Player player = new Rogue(name, data);

        }
    }

}
