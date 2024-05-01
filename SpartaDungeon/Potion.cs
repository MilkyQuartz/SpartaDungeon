using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Potion
    {
        // 이 함수는 실행 Test되지 않았습니다.
        // 연결 작업이 필요한 함수입니다.
        // 어디에 UsePotion()만 복사 붙여넣기 하거나 Potion객체생성후 사용해야 합니다.
        // 어디에서 _howManyPotion을 선언할지 정해야 합니다.
        // _howManyPotion 포션 갯수의 설정이 필요합니다.
        //  위 작업이 완료되면 이 주석을 지워주세요.
        public static void HealMenu(Player player, int _howManyPotion, Action Menu,string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }
            Action prevMenu = Menu;

            Console.WriteLine();
            ConsoleUtility.ShowTitle("■ 회복 ■");
            Console.Write("포션을 사용하면 체력을 ");            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("30");
            Console.ResetColor();
            Console.Write(" 회복 할 수 있습니다.  ");
            ConsoleUtility.PrintTextHighlights("남은 포션 : ", _howManyPotion.ToString(), " )");
            ConsoleUtility.PrintTextHighlights("현재 체력 : ", (player.Hp).ToString(), " / ", (player.Hp + player.BonusHp).ToString(), player.BonusHp > 0 ? $" (+{player.BonusHp})" : "");
            Console.WriteLine();
            Console.WriteLine("1. 사용하기");
            Console.WriteLine("0. 돌아가기");
            Console.WriteLine();
            int keyinput = ConsoleUtility.PromptMenuChoice(0, 1);

            switch (keyinput)
            {
                case 0:
                    // 이전화면
                    Menu();
                    break;
                case 1: //포션사용 체력회복
                    if (_howManyPotion > 0)
                    {
                        if (player.Hp < player.MaxHp)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine(player.Hp.ToString() + " -> " + ((player.Hp + 50) > 100 ? (player.Hp = 100) : (player.Hp + 50)).ToString());
                            Console.ResetColor();
                            player.Hp = (player.Hp + 30) >= player.MaxHp ? (player.Hp = player.MaxHp) : (player.Hp + 30);  //HP 적용
                            --_howManyPotion;
                            HealMenu(player, _howManyPotion, prevMenu, "체력을 회복했습니다.");
                        }
                        else
                            HealMenu(player, _howManyPotion, prevMenu, "체력이 최대치입니다.");
                    }
                    else
                    {
                        HealMenu(player, _howManyPotion, prevMenu, "포션이 부족합니다.");
                    }
                    break;               
            }            
        }

        // 포션 사용
        // 호출되는 상황 예시 : 회복화면 진입없이 전투 중 포션사용
        // 호출 예시 : 조건충족시 UsePotion(player, _howManyPotion, Menu)
        public static void UsePotion(Player player, int _howManyPotion, Action Menu)
        {
            Action prevMenu = Menu;
            if (_howManyPotion > 0)
            {
                if (player.Hp < player.MaxHp)
                {   
                    // 출력예시 : {회복전Hp} -> {회복후Hp}
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(player.Hp.ToString() + " -> " + ((player.Hp + 50) > 100 ? (player.Hp = 100) : (player.Hp + 50)).ToString());
                    Console.ResetColor();                    
                    player.Hp = (player.Hp + 30) >= player.MaxHp ? (player.Hp = player.MaxHp) : (player.Hp + 30);
                    --_howManyPotion;
                    Thread.Sleep(1000);
                    prevMenu();

                }
                else
                {
                    Console.WriteLine("체력이 최대치입니다.");
                    Thread.Sleep(1000);
                    prevMenu();
                }
            }
            else
            {
                Console.WriteLine("체력이 최대치입니다.");
                Thread.Sleep(1000);
                prevMenu();
            }
        }
    }
}
