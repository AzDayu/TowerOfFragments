using System;
using System.Threading;

namespace TowerOfFragments
{
    public interface ICombatant
    {
        string Name { get; }
        int HP { get; set; }
        int MaxHP { get; }
        int ATK { get; }
        int DEF { get; }
        bool IsAlive { get; }
        void TakeDamage(int damage);
    }

    internal class CombinationManager
    {
        public void ShowCombinationMenu(string jobName, Inventory inventory)
        {
            Console.Clear();
            Console.WriteLine("=== [ 조합 장인 ] ===");
            Console.WriteLine("\n[ 어떤 장비를 강화하시겠습니까? ]");
            Console.WriteLine("1. 무기   2. 방어구   0. 취소");
            Console.Write("선택: ");
            string targetType = Console.ReadLine();

            object targetItem = null;

            if (targetType == "1")
            {
                if (inventory.Weapons.Count == 0) { Console.WriteLine("보유 중인 무기가 없습니다."); Thread.Sleep(1000); return; }
                Console.WriteLine("\n[ 강화할 무기를 선택하세요 ]");
                for (int i = 0; i < inventory.Weapons.Count; i++)
                    Console.WriteLine($"{i + 1}. {inventory.Weapons[i].Name} (공격력: {inventory.Weapons[i].CurrentDamage})");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= inventory.Weapons.Count)
                    targetItem = inventory.Weapons[idx - 1];
                else return;
            }
            else if (targetType == "2")
            {
                if (inventory.Armors.Count == 0) { Console.WriteLine("보유 중인 방어구가 없습니다."); Thread.Sleep(1000); return; }
                Console.WriteLine("\n[ 강화할 방어구를 선택하세요 ]");
                for (int i = 0; i < inventory.Armors.Count; i++)
                    Console.WriteLine($"{i + 1}. {inventory.Armors[i].Name} (방어력: {inventory.Armors[i].CurrentDefense})");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= inventory.Armors.Count)
                    targetItem = inventory.Armors[idx - 1];
                else return;
            }
            else return;

            Console.WriteLine("\n[ 재료로 사용할 아이템의 종류는 무엇입니까? ]");
            Console.WriteLine("1. 무기   2. 방어구   3. 소재   0. 취소");
            Console.Write("선택: ");
            string materialType = Console.ReadLine();

            object materialItem = null;

            if (materialType == "1")
            {
                if (inventory.Weapons.Count == 0) { Console.WriteLine("재료로 쓸 무기가 없습니다."); Thread.Sleep(1000); return; }
                for (int i = 0; i < inventory.Weapons.Count; i++)
                    Console.WriteLine($"{i + 1}. {inventory.Weapons[i].Name}");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= inventory.Weapons.Count)
                    materialItem = inventory.Weapons[idx - 1];
                else return;
            }
            else if (materialType == "2")
            {
                if (inventory.Armors.Count == 0) { Console.WriteLine("재료로 쓸 방어구가 없습니다."); Thread.Sleep(1000); return; }
                for (int i = 0; i < inventory.Armors.Count; i++)
                    Console.WriteLine($"{i + 1}. {inventory.Armors[i].Name}");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= inventory.Armors.Count)
                    materialItem = inventory.Armors[idx - 1];
                else return;
            }
            else if (materialType == "3")
            {
                if (inventory.Materials.Count == 0) { Console.WriteLine("재료로 쓸 소재가 없습니다."); Thread.Sleep(1000); return; }
                for (int i = 0; i < inventory.Materials.Count; i++)
                    Console.WriteLine($"{i + 1}. {inventory.Materials[i].Name}");

                if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= inventory.Materials.Count)
                    materialItem = inventory.Materials[idx - 1];
                else return;
            }
            else return;

            if (ReferenceEquals(targetItem, materialItem))
            {
                Console.WriteLine("\n[경고] 강화할 장비 본인을 재료로 갈아 넣을 수 없습니다!");
                Thread.Sleep(1500);
                return;
            }

            ExecuteCombine(jobName, inventory, targetItem, materialItem);
        }

        private void ExecuteCombine(string jobName, Inventory inv, object target, object material)
        {
            int bonusValue = 0;
            string materialName = "";

            if (material is Weapon matWeapon)
            {
                bonusValue = Math.Max(1, matWeapon.CurrentDamage / 2);
                materialName = matWeapon.Name;
                inv.Weapons.Remove(matWeapon);
            }
            else if (material is Armor matArmor)
            {
                bonusValue = Math.Max(1, matArmor.CurrentDefense / 2);
                materialName = matArmor.Name;
                inv.Armors.Remove(matArmor); 
            }
            else if (material is MaterialsData matData)
            {
                if (target is Weapon) bonusValue = matData.ATK;
                else if (target is Armor) bonusValue = matData.DEF;

                materialName = matData.Name;
                inv.Materials.Remove(matData); 
            }

            if (target is Weapon targetWeapon)
            {
                targetWeapon.CurrentDamage += bonusValue;
                targetWeapon.Name = $"{materialName} {targetWeapon.Name}";
                Console.WriteLine($"\n[성공] 뚝딱뚝딱... {targetWeapon.Name} 완성! (공격력 +{bonusValue})");
            }
            else if (target is Armor targetArmor)
            {
                targetArmor.CurrentDefense += bonusValue;
                targetArmor.Name = $"{materialName} {targetArmor.Name}";
                Console.WriteLine($"\n[성공] 뚝딱뚝딱... {targetArmor.Name} 완성! (방어력 +{bonusValue})");
            }

            Thread.Sleep(1500);
        }
    }
}
