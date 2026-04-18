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

            var myHero = GameDataManager.Instance.GetItem("Job_02");

            if (myHero != null)
            {
                Console.WriteLine($"로드된 캐릭터 이름: {myHero.Name}");
            }

        }
    }
}
