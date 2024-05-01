using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Casino
    {
        Player player;

        bool firstVisit;
        int playerCoin;

        Action MainMenu;


        public Casino(Player _player)
        {
            player = _player;
        }


        //최초 카지노메뉴 호출: 
        public void CasinoMenu(Action Menu)
        {
            MainMenu = Menu;
            Console.Clear();
            ConsoleUtility.ShowTitle("■ Casino ■");
            ConsoleUtility.PrintTextHighlights("","길드가 운영하는 안전한 카지노입니다. ");
            ConsoleUtility.PrintTextHighlights("웨이트리스 : ", "교환소", "에 가시면 Gold와 COIN을 교환하실 수 있어요.");

            ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
            ConsoleUtility.PrintTextHighlights("COIN : ", playerCoin.ToString());
            Console.WriteLine("");

            Console.WriteLine("1. 교환소");
            Console.WriteLine("2. 블랙잭");
            Console.WriteLine("3. 홀짝");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 3))
            { 
                case 0:
                    MainMenu();
                    break;
                case 1:
                    ExchangeStaion();
                    break;
                case 2:
                    //블랙잭
                    Console.WriteLine("아직 준비가 안됐습니다.");
                    Thread.Sleep(1000);
                    CasinoMenu(MainMenu);
                    break;
                case 3:
                    Console.WriteLine("아직 준비가 안됐습니다.");
                    Thread.Sleep(1000);
                    CasinoMenu(MainMenu);
                    break;
            }

        }

        public void ExchangeStaion()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("■ 교환소 ■");
            Console.WriteLine("이곳에서 Gold와 COIN을 교환할 수 있습니다.");            
            Console.WriteLine();
            ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
            ConsoleUtility.PrintTextHighlights("COIN : ", playerCoin.ToString());

            Console.WriteLine("1. COIN으로 교환");
            Console.WriteLine("2. Gold로 교환");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 2))
            {
                case 0:
                    CasinoMenu(MainMenu);
                    break;
                case 1:
                    ExchangeCoin();
                    break;
                case 2:
                    ExchangeGold();
                    break;
            }

            void ExchangeCoin()
            {
                Console.Clear();
                ConsoleUtility.ShowTitle("Gold -> COIN");
                Console.WriteLine();
                Console.WriteLine("100 Gold을 1 COIN으로 교환할 수 있습니다.");
                Console.WriteLine("몇 개 교환하시겠습니까?");
                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
                ConsoleUtility.PrintTextHighlights("COIN : ", playerCoin.ToString());
                Console.WriteLine();
                Console.WriteLine("안내 : 교환 1회당 최대 100 COIN 교환 가능합니다.");
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine();
                int keyinput = ConsoleUtility.PromptMenuChoice(0, 100);
                switch (keyinput)
                {
                    case 0:
                        ExchangeStaion();
                        break;
                    default:
                        if(keyinput * 100 <= player.Gold)
                        {
                            //교환
                            player.Gold -= keyinput * 100;
                            playerCoin += keyinput;
                            ConsoleUtility.PrintTextHighlights("COIN ", keyinput.ToString(), "개를 교환했습니다.");
                            Thread.Sleep(1000);
                            ExchangeCoin();
                        }
                        else
                        {
                            //부족함
                            Console.WriteLine("Gold가 부족합니다.");
                            Thread.Sleep(1000);
                            ExchangeCoin();
                        }
                        break;
                }
            }
            void ExchangeGold()
            {
                Console.Clear();
                ConsoleUtility.ShowTitle("COIN -> Gold");
                Console.WriteLine();
                Console.WriteLine("1 COIN을 100 Gold로 교환할 수 있습니다.");
                Console.WriteLine("몇 개 교환하시겠습니까?");
                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
                ConsoleUtility.PrintTextHighlights("COIN : ", playerCoin.ToString());
                Console.WriteLine();
                Console.WriteLine("안내 : 교환 1회당 최대 100 COIN 교환 가능합니다. 0을 입력하면 돌아갑니다.");
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine();
                int keyinput = ConsoleUtility.PromptMenuChoice(0, 100);
                switch (keyinput)
                {
                    case 0:
                        ExchangeStaion();
                        break;
                    default:
                        if (keyinput <= playerCoin)
                        {
                            //교환
                            player.Gold += keyinput * 100;
                            playerCoin -= keyinput;
                            ConsoleUtility.PrintTextHighlights("COIN ", keyinput.ToString(), "개를 교환했습니다.");
                            Thread.Sleep(1000);
                            ExchangeGold();
                        }
                        else
                        {
                            //부족함
                            Console.WriteLine("COIN이 부족합니다.");
                            Thread.Sleep(1000);
                            ExchangeGold();
                        }
                        break;
                }
            }
        }









    }
}
