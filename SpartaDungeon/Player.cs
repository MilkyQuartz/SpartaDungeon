using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using static SpartaDungeon.Casino_Blackjack;
using System.Text.Json;
using System.Threading;

namespace SpartaDungeon
{
    public class JobData
    {
        public string Job { get; set; }
        public int Level { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public float Mp { get; set; }
        public float MaxMp { get; set; }
        public int Gold { get; set; }
    }
    public enum JOB
    {
        Warrior = 1,
        Mage,
        Archer,
        Assassin,
        END = 10
    }
    internal class Player : ICritical,IDamage
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

        //public List<Skill> Skills { get; set; }

        public float BonusAtk { get; set; }
        public float BonusDef { get; set; }
        public float BonusHp { get; set; }
        public float BonusMp { get; set; }

        public int casinoCoin = 0;


        //디버그
        public int potion = 4;

        public Player(string name, string job, int level, float atk, float def, float hp, float maxHp, float mp, float maxMp, int gold, float maxExp = 10,  float exp = 0, float bonusAtk = 0, float bonusDef = 0, float bonusHp = 0)
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
            //Skills = new List<Skill>();
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
            ConsoleUtility.PrintTextHighlightsNoLF("Lv.", Level.ToString(), $" {Name}");
            ConsoleUtility.PrintTextHighlightsNoLF("", " -> ");
            ConsoleUtility.PrintTextHighlights("Lv.", (Level + 1).ToString(), $" {Name}");

            Level++;
            Exp = Exp - MaxExp; // 넘치는 Exp를 다음레벨 Exp에 이월
            MaxExp += 25 + 5 * (Level - 2);   // 필요경험치 상승폭은 5씩 커진다
            Atk += 0.5f;
            Def += 1f;
            MaxHp += 2f;
            MaxMp += 2f;
        }

        public static void GetEquipStat(Player player, InventoryManager inventoryManager)
        {
            List<Item> playerInventory = inventoryManager.GetInventory(player.Name);
            player.BonusAtk = playerInventory.Where(item => item.IsEquipped).Sum(item => item.Atk);
            player.BonusDef = playerInventory.Where(item => item.IsEquipped).Sum(item => item.Def);
            player.BonusHp = playerInventory.Where(item => item.IsEquipped).Sum(item => item.Hp);
            player.BonusMp = playerInventory.Where(item => item.IsEquipped).Sum(item => item.Mp);
        }

        public void CheckCritical(ref int attackDamage, ref bool isCritical)
        {

            int rand = new Random().Next(1, 21);
            if (rand == 4 || rand == 11 || rand == 18)
            {
                attackDamage = (int)(attackDamage * 1.6);
                Console.WriteLine("치명타 공격!");
                isCritical = true;
            }
        }

        public void TakeDamage(int damage, bool isCritical)
        {
            int rand = new Random().Next(1, 11);
            if (rand ==10 && isCritical == false)
            {
                ConsoleUtility.PrintTextHighlights("", "\"무우~빙!\"", "");
                Console.WriteLine("몬스터의 공격을 회피했다!");
                Console.WriteLine("");
            }
            else
            {
                Hp -= damage;
                Console.WriteLine($"{damage}만큼의 피해를 입었습니다.");
                Console.WriteLine("");
            }
        }



        public string InputName()
        {
            Console.WriteLine("");
            ConsoleUtility.ShowTitle("■ 이름 선택 ■");
            bool isTrue = true;
            string? name = null;
            while (isTrue)
            {
                Console.Write(">> ");
                name = Console.ReadLine();
                Console.WriteLine("");
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
                ConsoleUtility.PrintTextHighlights("경비병: 이름이 ", name, "이 맞나?");
                Console.WriteLine("[0] 예");
                Console.WriteLine("[1] 이름 다시짓기");

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
            Console.WriteLine("[1] 전사 - 체력");
            Console.WriteLine("[2] 마법사 - 마력");
            Console.WriteLine("[3] 궁수 - 공격력");
            Console.WriteLine("[4] 암살자 - 방어력");

            int keyinput = ConsoleUtility.PromptMenuChoice(0, 4);

            // Json 파일에서 직업 정보 불러오기
            List<JobData> jobDataList = LoadJobData("Job.json");

            switch ((JOB)keyinput)
            {
                case 0:
                    JobSelect("어딜 가려고?");
                    break;
                case JOB.Warrior: 
                    JobData warriorData = jobDataList.FirstOrDefault(j => j.Job == "Warrior");
                    SetPlayerStats(warriorData);
                    result = warriorData.Job;
                    break;
                case JOB.Mage: 
                    JobData mageData = jobDataList.FirstOrDefault(j => j.Job == "Mage");
                    SetPlayerStats(mageData);
                    result = mageData.Job;
                    break;
                case JOB.Archer:
                    JobData archerData = jobDataList.FirstOrDefault(j => j.Job == "Archer");
                    SetPlayerStats(archerData);
                    result = archerData.Job;
                    break;
                case JOB.Assassin: 
                    JobData assassinData = jobDataList.FirstOrDefault(j => j.Job == "Assassin");
                    SetPlayerStats(assassinData);
                    result = assassinData.Job;
                    break;
                default:
                    result = ""; // 기본적으로 빈 문자열 반환
                    break;
            }

            return result;
        }

        private List<JobData> LoadJobData(string jsonFilePath)
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            List<JobData> jobDataList = JsonSerializer.Deserialize<List<JobData>>(jsonString);
            return jobDataList;
        }

        // 플레이어 스탯 설정
        private void SetPlayerStats(JobData jobData)
        {
            if (jobData != null)
            {
                Job = jobData.Job;
                Level = jobData.Level;
                Atk = jobData.Atk;
                Def = jobData.Def;
                Hp = jobData.Hp;
                MaxHp = jobData.MaxHp;
                Mp = jobData.Mp;
                MaxMp = jobData.MaxMp;
                Gold = jobData.Gold;
            }
        }

        // 이름 입력과 직업선택 함수
        public void CharacterMakingMenu(Player player, Action MainMenu)
        {
            Console.WriteLine("경비병 : 거기 멈춰서라.");
            Console.WriteLine("경비병 : 우리 마을은 정체도 모르는 이방인을 안으로 들이지 않는다.");
            Console.WriteLine("");
            Console.WriteLine("[0] 새로운 모험을 떠나기 위해 왔다.         [1] 내 얼굴도 기억하지 못하냐.         \n[2] 나는 무적의 개발자다.(플레이어, 인벤토리 초기화)");

            switch (ConsoleUtility.PromptMenuChoice(0, 2))
            {
                case 0:
                    StartNewAdventure(MainMenu);
                    break;
                case 1:
                    RememberMyFace(player, MainMenu);
                    break;
                case 2:
                    DeveloperBtn(player, MainMenu);
                    break;
            }
        }

        private void DeveloperBtn(Player player, Action mainMenu)
        {
            Console.WriteLine("[1] 모두 초기화         [2] 플레이어 초기화         [3] 인벤토리 초기화");
            int keyinput = ConsoleUtility.PromptMenuChoice(0, 2);
            switch (keyinput)
            {
                case 1:
                    // 모든 파일 초기화
                    File.WriteAllText("Player.json", "");
                    File.WriteAllText("Inventory.json", "");
                    // 나머지 파일도 필요한 경우 추가

                    break;
                case 2:
                    // 플레이어 파일 초기화
                    File.WriteAllText("Player.json", "");
                    break;
                case 3:
                    // 인벤토리 파일 초기화
                    File.WriteAllText("Inventory.json", "");
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }

        }

        private void StartNewAdventure(Action MainMenu)
        {
            Console.WriteLine("");
            Console.WriteLine("경비병 : 당신의 이름이 뭐지?");
            string name = InputName();
            Console.WriteLine("");

            if (!string.IsNullOrEmpty(name))
            {
                if (!IsNameCheck(name))
                {
                    Console.WriteLine("경비병: 흠... 신입 모험가인가? 직업을 선택하도록.");
                    // ... 직업
                    JobSelect();
                    Console.WriteLine();
                    Console.WriteLine("경비병 : 이제 들어가도 좋다.");
                    Console.WriteLine("");

                    SavePlayer();

                    Console.WriteLine("[0] 마을로 들어가기");

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
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
            }
        }

        private void RememberMyFace(Player player, Action MainMenu)
        {
            Console.WriteLine("");
            Console.WriteLine("경비병 : 당신 이름이 뭐지?");
            Console.Write(">> ");
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

                Console.WriteLine("");
                Console.WriteLine($"경비병 : 몰라봬서 죄송합니다 {name}용사님, 들어가시면 됩니다!");
                Console.WriteLine("");
                Console.WriteLine("[0] 마을로 들어가기");
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

            var json = File.ReadAllText("Player.json");

            // 파일이 비어 있는지 확인
            if (string.IsNullOrWhiteSpace(json))
                return null;

            // 읽어온 Json 파일의 데이터를 Player 배열로 역직렬화
            var players = JsonSerializer.Deserialize<List<Player>>(json);

            // 플레이어 배열에서 이름이 일치하는 플레이어를 찾아서 반환
            return players?.FirstOrDefault(p => p.Name == name);
        }

        public void SavePlayer()
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
                    players[i] = this; 
                    playerExists = true;
                    break;
                }
            }

            // 동일한 이름을 가진 플레이어가 없으면 새로운 플레이어 정보 추가
            if (!playerExists)
            {
                players.Add(this);
                // 플레이어의 인벤토리 정보 저장
                InventoryManager inventoryManager = new InventoryManager();
                inventoryManager.SaveInventory();
            }

            // 리스트를 Json 형식으로 직렬화하여 파일에 저장
            var jsonToWrite = JsonSerializer.Serialize(players);
            File.WriteAllText("Player.json", jsonToWrite);


        }

        public void SavePlayerIndirectly()
        {
            SavePlayer(); 
        }

    }
}