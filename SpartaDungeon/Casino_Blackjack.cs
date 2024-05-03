using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;

namespace SpartaDungeon
{
    internal static class Casino_Blackjack
    {
        public enum Suit { Hearts, Diamonds, Clubs, Spades }
        public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

        // 카드 한 장을 표현하는 클래스
        public class Card
        {
            public Suit Suit { get; private set; }
            public Rank Rank { get; private set; }

            public Card(Suit s, Rank r)
            {
                Suit = s;
                Rank = r;
            }

            public int GetValue()   // 활용 : card.GetValue();
            {
                if ((int)Rank <= 10)
                {
                    return (int)Rank;   // 숫자패
                }
                else if ((int)Rank <= 13)   // K,Q,J,10은 10점으로 계산한다.
                {
                    return 10;
                }
                else   // A는 일단 11로 계산한다. 1 or 11 선택은 GetTotalValue()에서 연산한다.
                {
                    return 11;
                }
            }

            public override string ToString()       // 출력에 활용하자 card.ToString(); 스트링을 돌려주는거다.
            {
                return $"{Rank} of {Suit}";
            }
        }

        // 덱을 표현하는 클래스
        public class Deck   // 활용예: Deck deck = new Deck()
        {
            public List<Card> cards;

            public Deck()   // 종류별 한장씩 가지는 카드덱 한 벌을 만들고 섞는다.
            {
                cards = new List<Card>();

                foreach (Suit s in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank r in Enum.GetValues(typeof(Rank)))
                    {
                        cards.Add(new Card(s, r));
                    }
                }

                Shuffle();
            }

            public void Shuffle()    //피셔-예이츠 셔플(Fisher-Yates shuffle) 알고리즘
            {
                Random rand = new Random();

                for (int i = 0; i < cards.Count; i++)   //Deck() 생성자를 통해 만들어진 카드덱List cards의 Count만큼 반복한다.
                {
                    int j = rand.Next(i, cards.Count);  // 무작위로 j번째의 카드를 
                    Card temp = cards[i];   // i번째의 카드를 temp에 담아둔다. 
                    cards[i] = cards[j];    // j번째의 카드를 i자리에 넣는다.
                    cards[j] = temp;        // j자리에 i자리에 있던 temp 카드를 넣는다.
                }
            }

            public Card DrawCard()
            {
                Console.WriteLine();
                if (cards.Count < 1)    //뽑을 카드가 없으면 새 카드 깐다!
                {
                    Console.WriteLine("새 카드의 포장을 뜯습니다.");
                    cards.Clear();
                    cards = new List<Card>();

                    foreach (Suit s in Enum.GetValues(typeof(Suit)))
                    {
                        foreach (Rank r in Enum.GetValues(typeof(Rank)))
                        {
                            cards.Add(new Card(s, r));
                        }
                    }

                    Shuffle();
                }
                Card card = cards[0];   // 덱의 맨 위에 있는 카드를 드로우카드로 정한다. 카드를 담아둔다.

                cards.RemoveAt(0);  // 덱 맨 위에 있는 카드를 제거한다.
                return card;    // 드로우카드를 return한다.
            }
        }

        // 패를 표현하는 클래스
        public class Hand
        {
            public List<Card> cards;   // 카드 리스트를 받는다

            public Hand()
            {
                cards = new List<Card>();   // 내 Hand로 쓸 리스트를 새로 생성
            }

            public void AddCard(Card card)  //활용. player.Hand.AddCard(DrawCardFromDeck(Deck deck))
            {
                cards.Add(card);
            }

            public int GetTotalValue()  // 카드숫자합
            {
                int total = 0;
                int aceCount = 0;

                foreach (Card card in cards)
                {
                    if (card.Rank == Rank.Ace)
                    {
                        aceCount++;     // 카드 중에 A가 있으면 A카운트를 올린다.
                    }
                    total += card.GetValue();       // 손패의 Value를 합한다.
                }

                while (total > 21 && aceCount > 0) //Ace카드를 1로 할지 14로 할지 정하는 로직. 합이 21이 넘을때 A가 Hand에 있다면 
                {
                    total -= 10;    // 밸류합에서 10을 빼는 방법으로 A 1을 구현한다. // 굳이 거슬러올라가서 A 11을 1로 바꾸지 않아도 된다.
                    aceCount--;
                }

                return total;
            }
        }

        // 플레이어를 표현하는 클래스
        public class Player
        {
            public Hand Hand { get; private set; }

            public Player()   //플레이어 생성자 -> Hand에 Hand()생성자 -> List<Card>()생성자 
            {
                Hand = new Hand();
            }

            public Card DrawCardFromDeck(Deck deck)  // 드로우한 카드
            {
                Card drawnCard = deck.DrawCard();   // 덱에서 DrawCard()를 실행
                Hand.AddCard(drawnCard);    //플레이어의 Hand에 DrawCard()한 drawnCard를 넣는다.
                return drawnCard;
            }
        }

        // 여기부터는 학습자가 작성
        // 딜러 클래스를 작성하고, 딜러의 행동 로직을 구현하세요.
        public class Dealer : Player
        {
            // 코드를 여기에 작성하세요
            public Hand Hand { get; private set; }

            public Dealer()
            {
                Hand = new Hand();
            }

            public Card DrawCardFromDeck(Deck deck)  // 카드 뽑을땐 이것만 호출하면 된다.
            {
                Card drawnCard = deck.DrawCard();   // 덱에서 DrawCard()를 실행
                Hand.AddCard(drawnCard);    //플레이어의 Hand에 DrawCard()한 drawnCard를 넣는다.
                return drawnCard;
            }


        }

        // 블랙잭 게임을 구현하세요. 
        public class Blackjack
        {
            // 코드를 여기에 작성하세요
            // 덱 한벌로 게임을 진행한다. 딜러 1명, 플레이어 1명이다.
            Deck deck; //= new Deck();
            Player dealer; // = new Dealer();
            Player player; // = new Player();

            bool stop = false;
            int playerWin = 0;
            int dealerWin = 0;



            public Blackjack()
            {
                deck = new Deck();
                dealer = new Dealer();
                player = new Player();
            }

            public void Play(int _bettedCoin)
            {
                Console.Clear(); 
                Console.WriteLine();
                //초기카드 패가 0장이면 카드 두장씩 받는다
                if (player.Hand.cards.Count == 0)
                {
                    player.DrawCardFromDeck(deck);
                    dealer.DrawCardFromDeck(deck);
                    player.DrawCardFromDeck(deck);
                    dealer.DrawCardFromDeck(deck);
                }
                Console.WriteLine("딜러 카드");
                // 딜러핸드 출력 if 플레이어 스탑 or 플레이어 버스트
                if (player.Hand.GetTotalValue() > 21 || stop) // 딜러의 두번째카드를 공개한다. 이때 카드총합밸류를 계산한다.
                {
                    foreach (var open in dealer.Hand.cards)
                    {
                        Console.Write("[" + open.ToString() + "]" + " ");
                    }
                    Console.WriteLine(" Dealer : " + dealer.Hand.GetTotalValue().ToString());
                }
                else Console.Write("[" + dealer.Hand.cards[0].ToString() + "]" + " " + "[ ? ]");
                Console.WriteLine(" Dealer : ?");

                Console.WriteLine("\n");
                ConsoleUtility.PrintTextHighlights("배팅한 코인 : ", _bettedCoin.ToString());
                Console.WriteLine("플레이어 카드");

                foreach (var open in player.Hand.cards)
                {
                    Console.Write("[" + open.ToString() + "]" + " ");
                }
                Console.WriteLine(" Player : " + player.Hand.GetTotalValue().ToString());
                //플레이어 카드합 평가
                if (21 < player.Hand.GetTotalValue())  //버스트
                {
                    Console.WriteLine();

                    ++dealerWin;
                    Console.WriteLine();
                    Console.WriteLine("Player Burst");
                    Game_Blackjack.backCoin = 0;
                    return;
                }
                // 카드합 평가 
                else if (stop)
                {
                    if (21 < dealer.Hand.GetTotalValue())  //버스트
                    {
                        ++playerWin;
                        Console.WriteLine();
                        Console.WriteLine("Dealer Burst");
                        Game_Blackjack.backCoin = _bettedCoin * 2;
                        return;
                    }
                    else if (21 == dealer.Hand.GetTotalValue())
                    {
                        ++dealerWin;
                        Console.WriteLine();
                        Console.WriteLine("Dealer Win");
                        Game_Blackjack.backCoin = 0;
                        return;
                    }
                    else if (17 <= dealer.Hand.GetTotalValue()) // 딜러 17이상 17이상이면 무조건 뽑지 않는다.
                    {
                        //비교
                        if (dealer.Hand.GetTotalValue() >= player.Hand.GetTotalValue())
                        {
                            ++dealerWin;
                            Console.WriteLine();
                            Console.WriteLine("Dealer Win");
                            Game_Blackjack.backCoin = 0;
                            return;
                        }
                        else
                        {
                            ++playerWin;                            
                            Console.WriteLine();
                            Console.WriteLine("Player Win");
                            Game_Blackjack.backCoin = _bettedCoin * 2;
                            return;
                        }
                    }
                }
                

                Console.WriteLine("\n");
                
                if(!stop)
                Console.WriteLine("0. 스탠드    1. 히트");

                if (!stop) // 플레이어 히트중
                {
                    switch (PromptMenuChoice(0, 1))
                    {
                        case 0: // 카드 안뽑음. 안뽑으면 계속 안뽑아야함. 딜러가 뽑기 시작함.
                            stop = true;
                            Play(_bettedCoin);
                            break;
                        case 1: // 카드 뽑음
                            player.DrawCardFromDeck(deck);
                            Play(_bettedCoin);
                            break;
                    }
                }
                else // 플레이어 스탑시 딜러 행동.
                {
                    dealer.DrawCardFromDeck(deck);
                    Thread.Sleep(1000);
                    Play(_bettedCoin);

                }

                static int PromptMenuChoice(int min, int max)
                {
                    while (true)
                    {
                        Console.Write("원하시는 번호를 입력해주세요: ");
                        if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                        {
                            return choice;
                        }
                        Console.WriteLine("잘못된 입력입니다. 다시 시도해주세요.");
                    }
                }
            }
        }



        public class Game_Blackjack
        {
            public static int backCoin {  get; set; }

            public static void StartBlackjack(int _bettedCoin)
            { 
                // 블랙잭 게임을 실행하세요
                Blackjack blackjack = new Blackjack();
                
                blackjack.Play(_bettedCoin);
            }
                       
        }
    }
}
