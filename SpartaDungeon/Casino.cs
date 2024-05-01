using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;

namespace SpartaDungeon
{
    internal class Casino
    {
        Player player;
        
        int bettedCoin;
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
            ConsoleUtility.PrintTextHighlights("COIN : ", player.casinoCoin.ToString());
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
                    Betting(Casino_Blackjack.Game_Blackjack.StartBlackjack, bettedCoin);                    
                    break;
                case 3:
                    Console.WriteLine("아직 준비가 안됐습니다.");
                    Thread.Sleep(1000);
                    CasinoMenu(MainMenu);
                    break;
            }
        }

        public void Betting(Action<int> _Game, int bettedCoin)
        {
            if( 0 < Game_Blackjack.backCoin)
            player.casinoCoin += Game_Blackjack.backCoin;
            Console.WriteLine();
            ConsoleUtility.ShowTitle("■ Betting ■");
            ConsoleUtility.PrintTextHighlights("","얼마나 베팅하시겠습니까?");
            Console.WriteLine();
            ConsoleUtility.ShowTitle("가지고 있는 COIN보다 크게 입력하면 ALL IN 합니다");
            ConsoleUtility.PrintTextHighlightsNoLF("배팅 가능 코인 : ","0"," ~ ");
            ConsoleUtility.PrintTextHighlights("", player.casinoCoin.ToString());
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine();
            int keyinput = ConsoleUtility.PromptMenuChoice(0, 99999);
            
            switch (keyinput)
            {
                case 0:
                    CasinoMenu(MainMenu);
                    break;
                default:
                    if(keyinput >= player.casinoCoin)
                    {
                        //올인
                        bettedCoin = player.casinoCoin;
                        player.casinoCoin = 0;
                        ConsoleUtility.PrintTextHighlights("", " ALL IN !!!");
                        Thread.Sleep(1000);
                        _Game(bettedCoin);
                        Betting(_Game, bettedCoin);


                    }
                    else
                    {
                        bettedCoin = keyinput;
                        player.casinoCoin -= keyinput;
                        ConsoleUtility.PrintTextHighlights("COIN ", keyinput.ToString(), "개를 베팅했습니다.");
                        Thread.Sleep(700);
                        _Game(bettedCoin);
                        Betting(_Game, bettedCoin);
                    }
                    break;
            }
        }

        void ExchangeStaion()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("■ 교환소 ■");
            Console.WriteLine("이곳에서 Gold와 COIN을 교환할 수 있습니다.");            
            Console.WriteLine();
            ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
            ConsoleUtility.PrintTextHighlights("COIN : ", player.casinoCoin.ToString());

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
                ConsoleUtility.ShowTitle("■ Gold -> COIN ■");
                Console.WriteLine();
                Console.WriteLine("100 Gold을 1 COIN으로 교환할 수 있습니다.");
                Console.WriteLine("몇 개 교환하시겠습니까?");
                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
                ConsoleUtility.PrintTextHighlights("COIN : ", player.casinoCoin.ToString());
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
                            player.casinoCoin += keyinput;
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
                ConsoleUtility.ShowTitle("■ COIN -> Gold ■");
                Console.WriteLine();
                Console.WriteLine("1 COIN을 100 Gold로 교환할 수 있습니다.");
                Console.WriteLine("몇 개 교환하시겠습니까?");
                Console.WriteLine();
                ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
                ConsoleUtility.PrintTextHighlights("COIN : ", player.casinoCoin.ToString());
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
                        if (keyinput <= player.casinoCoin)
                        {
                            //교환
                            player.Gold += keyinput * 100;
                            player.casinoCoin -= keyinput;
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
