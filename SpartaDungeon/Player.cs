using System.Text.Json;
using System.Xml.Linq;

namespace SpartaDungeon
{
    public enum JOB
    {
        Warrior,
        Mage,

        END = 10
    }

    internal class Player
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public int Level { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public float Mp { get; set; }
        public float MaxMp { get; set; }
        public int Gold { get; set; }
        public float MaxExp { get; set; }
        public float Exp { get; set; }
        public float BonusAtk { get; set; }
        public float BonusDef { get; set; }
        public float BonusHp { get; set; }
        public float BonusMp { get; set; }

        public Player(string name, string job, int level, float atk, float def, float hp, float maxHp, float mp, float maxMp, int gold, float maxExp = 10, float exp = 0, float bonusAtk = 0, float bonusDef = 0, float bonusHp = 0)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            MaxHp = maxHp;
            Mp = mp;
            MaxMp = maxMp;
            Gold = gold;
            MaxExp = maxExp;
        }

        public void LevelUp()
        {
            Level++;
            Atk += 1;
            Def += 1;
        }

        // 이름입력 함수
        public string InputName()
        {
            bool isTrue = true;
            string? name = null;
            while (isTrue)
            {
                Console.WriteLine("이름을 입력하세요");
                // 이름입력
                name = Console.ReadLine();
                if(name == "")
                {
                    Console.WriteLine("빈 칸은 안됩니다.");
                    continue;
                }
                else if (name[0].ToString() == " ")
                {
                    Console.WriteLine("첫자리에 빈 칸은 안됩니다.");
                    continue;
                }
                ConsoleUtility.PrintTextHighlights("당신의 이름은 ", name, "이 맞습니까?");
                Console.WriteLine("[0] 예         [1] 이름 다시짓기");

                switch (ConsoleUtility.PromptMenuChoice(0, 1))
                {
                    case 0: //예
                        isTrue = false;
                        Name = name; // 이름을 플레이어 객체의 속성에 할당
                        break;
                    case 1: //아니오 반복문

                        break;
                }
            }
            return name;
        }

        // 직업선택 함수
        public void JobSelect(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.WriteLine();
            ConsoleUtility.ShowTitle("■ 직업 선택 ■");
            Console.WriteLine("원하는 직업을 선택하세요.");
            Console.WriteLine("[1] 전사");
            Console.WriteLine("[2] 마법사");
            Console.WriteLine();
            Console.WriteLine("0. 마을로 들어가기");

            int keyinput = ConsoleUtility.PromptMenuChoice(0, 2);

            switch (keyinput)
            {
                case 0:
                    JobSelect("어딜 가려고?");
                    break;
                case 1: //전사
                    this.Job = "Warrior";
                    this.Level = 1;
                    this.Atk = 10;
                    this.Def = 5;
                    this.Hp = 100;
                    this.MaxHp = 100;
                    this.Mp = 20;
                    this.MaxMp = 20;
                    this.Gold = 2000;
                    break;
                case 2:
                    this.Job = "Mage";
                    this.Level = 1;
                    this.Atk = 13;
                    this.Def = 3;
                    this.Hp = 60;
                    this.MaxHp = 60;
                    this.Mp = 60;
                    this.MaxMp = 60;
                    this.Gold = 2000;
                    break;
                default:

                    break;
            }

        }

        //이름 입력과 직업선택 함수
        public void CharacterMakingMenu(Action MainMenu)
        {
            Console.WriteLine("경비병 : 거기 멈춰서라.");
            Console.WriteLine("경비병 : 우리 마을은 정체도 모르는 이방인을 안으로 들이지 않는다.");
            Console.WriteLine("[0] 새로운 모험을 떠나기 위해 왔다.         [1] 내 얼굴도 기억하지 못하냐.");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    StartNewAdventure(MainMenu);
                    break;
                case 1:
                    RememberMyFace(MainMenu);
                    break;
            }
        }
        private void StartNewAdventure(Action MainMenu)
        {
            Console.WriteLine("경비병 : 당신의 이름이 뭐지?");
            Console.WriteLine();
            string name = InputName();

            if (!string.IsNullOrEmpty(name))
            {
                if (!IsNameCheck(name))
                {
                    Console.WriteLine("경비병 : 직업은?");
                    // ... 직업
                    JobSelect();
                    Console.WriteLine();
                    Console.WriteLine("경비병 : 이제 들어가도 좋다.");
                    GameManager gameManager = new GameManager();
                    gameManager.PastePlayer(name);
                    Console.WriteLine("");

                    SavePlayer();

                    Console.WriteLine("0. 마을로 들어가기");
                    Console.WriteLine("");

                    switch (ConsoleUtility.PromptMenuChoice(0, 0))
                    {
                        case 0:
                            MainMenu();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("경비병 : 용사님을 사칭하지마라!");
                    Environment.Exit(0);
                }
            }
            
        }

        private void RememberMyFace(Action MainMenu)
        {
            Console.WriteLine("경비병 : 당신의 이름이 뭐지?");
            Console.WriteLine();
            Console.WriteLine("이름을 입력하세요");
            string name = Console.ReadLine();

            if (IsNameCheck(name))
            {
                // 이름이 존재하는 경우, 해당 플레이어 정보를 불러옴
                Console.WriteLine($"경비병 : {name} 용사님! 몰라봬서 죄송합니다. 들어가시면 됩니다!");

                GameManager gameManager = new GameManager();
                gameManager.PastePlayer(name);


                Console.WriteLine("0. 마을로 들어가기");
                Console.WriteLine("");

                switch (ConsoleUtility.PromptMenuChoice(0, 0))
                {
                    case 0:
                        MainMenu();
                        break;
                }
            }
        }

        private bool IsNameCheck(string name)
        {
            return JsonSerializer.Deserialize<Player[]>(File.ReadAllText("Player.json")).Any(p => p.Name == name);
        }

        private void SavePlayer()
        {
            // 기존 플레이어 정보 읽기
            var players = JsonSerializer.Deserialize<List<Player>>(File.ReadAllText("Player.json"));

            players.Add(this);
            var json = JsonSerializer.Serialize(players);
            File.WriteAllText("Player.json", json);
        }

    }
}