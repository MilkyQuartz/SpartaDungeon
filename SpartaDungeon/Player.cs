using System.Text.Json;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

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


        //디버그
        public int potion = 4;



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

        // Level Up
        // 레벨업을 호출하는 경우. 
            // 전투 결과에서 출력하는 경우, 
            // 퀘스트 완료에서 출력하는 경우
                // 호출예시 : player.LevelUp();
        public void LevelUp()
        {
            // 출력예시 : [캐릭터 정보]
            // 출력예시 : Lv.1 Chad -> Lv2. Chad
            Console.WriteLine("[캐릭터 정보]");
            ConsoleUtility.PrintTextHighlightsNoLF("Lv.", Level.ToString(), Name);
            ConsoleUtility.PrintTextHighlightsNoLF("", " -> ");
            ConsoleUtility.PrintTextHighlightsNoLF("Lv.", (Level + 1).ToString(), Name);
            Level++;
            Exp = Exp - MaxExp; // 넘치는 Exp를 다음레벨 Exp에 이월
            MaxExp += 25 + 5 * (Level - 2);   // 필요경험치 상승폭은 5씩 커진다
            Atk += 0.5f;
            Def += 1f;
            MaxHp += 2f;
            MaxMp += 2f;
        }


        public string InputName()
        {
            bool isTrue = true;
            string? name = null;
            while (isTrue)
            {
                Console.WriteLine("이름을 입력하세요");
                name = Console.ReadLine();
                if (name == "")
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
        public string JobSelect(string? prompt = null)
        {
            string result = ""; // 기본적으로 빈 문자열로 초기화

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
            Console.WriteLine("                        PRESS ANYKEY TO ENTER THE VILLAGE                             ");

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
                    result = "Warrior"; // 전사 선택 시 "Warrior" 반환
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
                    result = "Mage"; // 마법사 선택 시 "Mage" 반환
                    break;
                default:
                    result = ""; // 기본적으로 빈 문자열 반환
                    break;
            }

            return result; // 결과 반환
        }

        // 이름 입력과 직업선택 함수
        public void CharacterMakingMenu(Player player, Action MainMenu)
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
                    RememberMyFace(player, MainMenu);
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

        private void RememberMyFace(Player player, Action MainMenu)
        {
            Console.WriteLine("경비병 : 당신의 이름이 뭐지?");
            Console.WriteLine();
            Console.WriteLine("이름을 입력하세요");
            string name = Console.ReadLine();

            // 주어진 이름을 가진 플레이어가 존재하는지 확인
            if (IsNameCheck(name))
            {
                // 이름이 존재하는 경우, 해당 플레이어 정보를 가져옴
                Player existingPlayer = GetPlayerByName(name);

                // 플레이어 정보 업데이트
                player.Name = existingPlayer.Name;
                player.Job = existingPlayer.Job;
                player.Level = existingPlayer.Level;
                player.Atk = existingPlayer.Atk;
                player.Def = existingPlayer.Def;
                player.Hp = existingPlayer.Hp;
                player.MaxHp = existingPlayer.MaxHp;
                player.Mp = existingPlayer.Mp;
                player.MaxMp = existingPlayer.MaxMp;
                player.Gold = existingPlayer.Gold;
                player.MaxExp = existingPlayer.MaxExp;
                player.Exp = existingPlayer.Exp;
                player.BonusAtk = existingPlayer.BonusAtk;
                player.BonusDef = existingPlayer.BonusDef;
                player.BonusHp = existingPlayer.BonusHp;

                // 잘가져오는지 확인용.. 플레이어 정보를 출력
                /*Console.WriteLine("입력된 플레이어 정보:");
                Console.WriteLine($"이름: {player.Name}");
                Console.WriteLine($"직업: {player.Job}");
                Console.WriteLine($"레벨: {player.Level}");
                Console.WriteLine($"공격력: {player.Atk}");
                Console.WriteLine($"방어력: {player.Def}");
                Console.WriteLine($"체력: {player.Hp}");
                Console.WriteLine($"최대 체력: {player.MaxHp}");
                Console.WriteLine($"마나: {player.Mp}");
                Console.WriteLine($"최대 마나: {player.MaxMp}");
                Console.WriteLine($"골드: {player.Gold}");
                Console.WriteLine($"최대 경험치: {player.MaxExp}");*/

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
                // 주어진 이름을 가진 플레이어가 존재하지 않는 경우
                Console.WriteLine($"경비병 : 에잇 넌 뭐야? 나가!");
                Environment.Exit(0);
            }
        }

        private bool IsNameCheck(string name) // 아이디 중복 확인용 메서드. TRUE, FALSE
        {
            // Json 파일이 존재하지 않으면 false 반환
            if (!File.Exists("Player.json"))
                return false;

            // Json 파일 읽어오기
            var json = File.ReadAllText("Player.json");

            // 파일이 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(json))
                return false;

            // 읽어온 Json 파일의 데이터를 Player 배열로 역직렬화
            var players = JsonSerializer.Deserialize<List<Player>>(json);

            // 플레이어 배열에서 이름이 일치하는 플레이어가 있는지 확인
            return players != null && players.Any(p => p.Name == name);
        }

        private Player GetPlayerByName(string name) //객체 읽어오는 용 메서드.. 근데 IsNameCheck메서드랑 합쳐도 될 것 같긴한데 일단은 던전 만들고 손대겠습니당.
        {
            // Json 파일이 존재하지 않으면 null 반환
            if (!File.Exists("Player.json"))
                return null;

            // Json 파일 읽어오기
            var json = File.ReadAllText("Player.json");

            // 파일이 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(json))
                return null;

            // 읽어온 Json 파일의 데이터를 Player 배열로 역직렬화
            var players = JsonSerializer.Deserialize<List<Player>>(json);

            // 플레이어 배열에서 이름이 일치하는 플레이어를 찾아서 반환
            return players?.FirstOrDefault(p => p.Name == name);
        }

        private void SavePlayer()
        {
            // Json 파일이 존재하지 않으면 빈 리스트 생성
            List<Player> players;
            if (!File.Exists("Player.json") || new FileInfo("Player.json").Length == 0)
            {
                players = new List<Player>();
            }
            else
            {
                // 기존 플레이어 정보 읽기
                var json = File.ReadAllText("Player.json");
                players = JsonSerializer.Deserialize<List<Player>>(json);
            }

            // 동일한 이름을 가진 플레이어 정보 업데이트 또는 추가
            bool playerExists = false;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Name == this.Name)
                {
                    players[i] = this; // 동일한 이름의 플레이어 정보 업데이트
                    playerExists = true;
                    break;
                }
            }

            // 동일한 이름을 가진 플레이어가 없으면 새로운 플레이어 정보 추가
            if (!playerExists)
            {
                players.Add(this);
            }

            // 리스트를 Json 형식으로 직렬화하여 파일에 저장
            var jsonToWrite = JsonSerializer.Serialize(players);
            File.WriteAllText("Player.json", jsonToWrite);
        }

        public void SavePlayerIndirectly()
        {
            SavePlayer(); // private 메서드 호출
        }

    }
}