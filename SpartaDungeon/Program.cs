using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace SpartaDungeon
{

    public class GameManager
    {
        private static GameManager instance;
        private Player player;
        private List<Item> inventory;
        private List<Item> storeInventory;
        private List<Monster> monsters;
        private Dictionary<ItemType, int> compareDic;
        private List<Quest> questList;
        private List<Quest> myQuest;
        private List<Quest> completeQuest;
        private Casino casino;

        public GameManager()
        {
            InitializeGame();
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        private void InitializeGame()
        {
            player = new Player(name: "", job: "", level: 1, atk: 10, def: 5, hp: 100, maxHp: 100, mp: 20, maxMp: 20, gold: 10000, maxExp: 10);
            compareDic = new Dictionary<ItemType, int>();
            inventory = new List<Item>();
            storeInventory = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText("StoreInventory.json"));
            monsters = JsonSerializer.Deserialize<List<Monster>>(File.ReadAllText("Monster.json"));

            questList = new List<Quest>();
            questList.Add( new Quest("몬스터 사냥", "몬스터를 사냥하세요", "몬스터", 100));
            questList.Add( new Quest("레벨 업", "레벨을 올려보자", "레벨", 100));
            questList.Add( new Quest("몬스터 사냥", "몬스터를 사냥하세요", "몬스터", 100));
            myQuest = new List<Quest>();
            completeQuest = new List<Quest>();
            casino = new Casino(player);
        }

        public void StartGame()
        {
            Console.Clear();
            ConsoleUtility.PrintGameHeader();
            Console.Clear();
            player.CharacterMakingMenu(player, MainMenu);
        }

        private void MainMenu()
        {
            // 구성
            // 0. 화면 정리
            Console.Clear();

            // 1. 선택 멘트를 줌
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
            Console.WriteLine("");

            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상    점");
            Console.WriteLine("4. 던    전");
            Console.WriteLine("5. 주    점");
            Console.WriteLine("6. 길    드");
            Console.WriteLine("7. 퀘스트 완료시키기(테스트용)");
            Console.WriteLine("8 . 게임종료");
            Console.WriteLine("9 . $$카지노$$");
            Console.WriteLine("");

            // 2. 선택한 결과를 검증함
            int choice = ConsoleUtility.PromptMenuChoice(1, 9);

            // 3. 선택한 결과에 따라 보내줌
            switch (choice)
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    StoreMenu();
                    break;
                case 4:
                    DungeonMenu();
                    break;
                case 5:
                    BarMenu();
                    break;
                case 6:
                    GuildMenu();
                    break;
                case 7:
                    TestQuest();
                    break;
                case 8:
                    GameOverMenu();
                    break;
                case 9:                    
                    casino.CasinoMenu(MainMenu);
                    
                    break;
            }
            MainMenu();
        }

        private void StatusMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상태보기 ■");
            Console.WriteLine("캐릭터의 정보가 표기됩니다.");

            ConsoleUtility.PrintTextHighlights("Lv. ", player.Level.ToString("00"));
            ConsoleUtility.PrintTextHighlights("경험치 : ", (player.Exp).ToString("00"), " / ", player.MaxExp.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine($"{player.Name} ({player.Job})");

            // TODO : 능력치 강화분을 표현하도록 변경

            player.BonusAtk = inventory.Select(item => item.IsEquipped ? item.Atk : 0).Sum();
            player.BonusDef = inventory.Select(item => item.IsEquipped ? item.Def : 0).Sum();
            player.BonusHp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();
            player.BonusMp = inventory.Select(item => item.IsEquipped ? item.Hp : 0).Sum();

            ConsoleUtility.PrintTextHighlights("공격력 : ", (player.Atk + player.BonusAtk).ToString(), player.BonusAtk > 0 ? $" (+{player.BonusAtk})" : "");
            ConsoleUtility.PrintTextHighlights("방어력 : ", (player.Def + player.BonusDef).ToString(), player.BonusDef > 0 ? $" (+{player.BonusDef})" : "");
            ConsoleUtility.PrintTextHighlights("체 력 : ", (player.Hp).ToString(), " / ", (player.MaxHp + player.BonusHp).ToString(), player.BonusHp > 0 ? $" (+{player.BonusHp})" : "");
            ConsoleUtility.PrintTextHighlights("마 력 : ", (player.Mp).ToString(), " / ", (player.MaxMp + player.BonusMp).ToString(), player.BonusMp > 0 ? $" (+{player.BonusMp})" : "");

            ConsoleUtility.PrintTextHighlights("Gold : ", player.Gold.ToString());
            Console.WriteLine("");

            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))//0))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    //아래테스트영역 삭제가능
                    Potion.HealMenu(player, player.potion, StatusMenu);

                    break;
                    //위 테스트영역 삭제가능
            }
        }

        private void InventoryMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription();
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;
            }
        }

        private void EquipMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 인벤토리 - 장착 관리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            if (inventory.Count == 1 && inventory[0] == null)
            {
                Console.WriteLine("보유 중인 아이템이 존재하지 않습니다.");
            }
            else
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    inventory[i].PrintItemStatDescription(true, i + 1); // 나가기가 0번 고정, 나머지가 1번부터 배정
                }
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");

            int KeyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count);

            switch (KeyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default: // 추가요소 장비교체
                    // 같은 아이템 선택하면 장비해제로 가고 같은타입이면 기존장비 해제 후 그 장비 착용
                    // null이면 착용, null이 아니면 키값Type 비교, 같으면 해제 후 착용, 같아도 Value Name이 같으면 장비해제 
                    if (!compareDic.ContainsKey(inventory[KeyInput - 1].Type))
                    {
                        inventory[KeyInput - 1].ToggleEquipStatus();
                        compareDic.Add(inventory[KeyInput - 1].Type, KeyInput - 1);
                    }
                    else // 같은 자리에 장비를 끼고있다 == 선택된 장비와 타입이 같다 같은 타입이면서 다른 장비이면 바꿔끼기.
                    {
                        foreach (KeyValuePair<ItemType, int> dic in compareDic)
                        {
                            if (!(dic.Value == KeyInput - 1)) //dic에 저장된 장비가 선택한 장비와 같은 장비인지 비교 다르면
                            {
                                inventory[dic.Value].ToggleEquipStatus(); // 기존장비 해제
                                compareDic.Remove(dic.Key); // 기존장비 삭제     

                                inventory[KeyInput - 1].ToggleEquipStatus(); //골랐던 장비 착용
                                compareDic.Add(inventory[KeyInput - 1].Type, KeyInput - 1);
                                break;
                            }
                            //같은 타입이면서 같은 장비이면 장비해제.
                            else
                            {
                                inventory[KeyInput - 1].ToggleEquipStatus();
                                compareDic.Remove(dic.Key); // 기존장비 삭제 
                                break;
                            }
                        }
                    }
                    // 인벤토리 파일 업데이트
                    string inventoryJson = JsonSerializer.Serialize(inventory);
                    File.WriteAllText("Inventory.json", inventoryJson);

                    EquipMenu();
                    break;
            }
        }


        private void StoreMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            switch (ConsoleUtility.PromptMenuChoice(0, 2))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    PurchaseMenu();
                    break;
                case 2:
                    SellMenu();
                    break;
            }
        }

        private void PurchaseMenu(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < storeInventory.Count; i++)
            {
                storeInventory[i].PrintStoreItemDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            int keyInput = ConsoleUtility.PromptMenuChoice(0, storeInventory.Count);

            switch (keyInput)
            {
                case 0:
                    StoreMenu();
                    break;
                default:
                    // 1 : 이미 구매한 경우
                    if (storeInventory[keyInput - 1].IsPurchased) // index 맞추기
                    {
                        PurchaseMenu("이미 구매한 아이템입니다.");
                    }
                    // 2 : 돈이 충분해서 살 수 있는 경우
                    else if (player.Gold >= storeInventory[keyInput - 1].Price)
                    {
                        player.Gold -= storeInventory[keyInput - 1].Price;
                        storeInventory[keyInput - 1].Purchase();
                        inventory.Add(storeInventory[keyInput - 1]);

                        // 
                        string inventoryJson = JsonSerializer.Serialize(inventory);
                        File.WriteAllText("Inventory.json", inventoryJson);


                        PurchaseMenu();
                    }
                    // 3 : 돈이 모자라는 경우
                    else
                    {
                        PurchaseMenu("Gold가 부족합니다.");
                    }
                    break;
            }
        }


        private void SellMenu(string? prompt = null) // 추가요소 상점 판매
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 상점 ■");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            ConsoleUtility.PrintTextHighlights("", player.Gold.ToString(), " G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inventory.Count; i++)
            {
                inventory[i].PrintItemStatDescription(true, i + 1);
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, inventory.Count);

            void Sell(int keyInput)
            {
                player.Gold += (int)(inventory[keyInput - 1].Price * 0.85f);
                foreach (var item in storeInventory)
                {
                    if (item.Name == inventory[keyInput - 1].Name)
                    {
                        item.Refund();
                        break;
                    }
                }
                inventory.Remove(inventory[keyInput - 1]);
                string inventoryJson = JsonSerializer.Serialize(inventory);
                File.WriteAllText("Inventory.json", inventoryJson);
            }

            switch (keyInput)
            {
                case 0:
                    StoreMenu();
                    break;
                default:
                    // 1 : 장비한 아이템인 경우
                    if (inventory[keyInput - 1].IsEquipped) // index 맞추기
                    {
                        Console.WriteLine("정말로 장비한 아이템을 파시겠습니까?");
                        Console.WriteLine("0. 아니오     1. 예");
                        int keyInput2 = ConsoleUtility.PromptMenuChoice(0, 1);
                        switch (keyInput2)
                        {
                            case 0:
                                SellMenu();
                                break;
                            case 1:
                                // 장비해제
                                inventory[keyInput - 1].ToggleEquipStatus();
                                // 판매
                                Sell(keyInput);
                                break;
                        }
                    }
                    // 2 : 장비하지 않은 아이템인 경우
                    else
                    {
                        Sell(keyInput);
                        SellMenu();
                    }
                    break;
            }
        }
        public void DungeonMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 던전탐색 ■");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("");

            Console.WriteLine("");
            Console.WriteLine("1. 쉬운 던전");
            Console.WriteLine("2. 일반 던전");
            Console.WriteLine("3. 어려운 던전");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            if (player.Hp < 1)
            {
                Console.WriteLine("체력이 부족해 던전에 입장할 수 없습니다. 휴식을 취한 후 시도해주세요.");
                Thread.Sleep(1000);
                MainMenu();
                return;
            }

            switch (ConsoleUtility.PromptMenuChoice(0, 3))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    StartDungeon(1);
                    break;
                case 2:
                    StartDungeon(2);
                    break;
                case 3:
                    StartDungeon(3);
                    break;
            }
        }

        public void StartDungeon(int difficulty)
        {
            // 해당 난이도에 맞는 몬스터들을 가져옴
            List<Monster> dungeonMonsters = GetDungeonMonsters(difficulty);

            // 전투 시작
            Battle(dungeonMonsters);
        }
        private List<Monster> GetDungeonMonsters(int difficulty)
        {
            List<Monster> filteredMonsters = new List<Monster>();
            Random rand = new Random();

            int minLevel = 1;
            int maxLevel = 10;

            switch (difficulty)
            {
                case 1: // 쉬움
                    minLevel = 1;
                    maxLevel = 3;
                    break;
                case 2: // 보통
                    minLevel = 4;
                    maxLevel = 7;
                    break;
                case 3: // 어려움
                    minLevel = 8;
                    maxLevel = 10;
                    break;
                default:
                    break;
            }

            // 필터링된 몬스터 선택
            filteredMonsters = monsters.Where(monster => monster.Level >= minLevel && monster.Level <= maxLevel).ToList();

            // 3마리 랜덤으로 선택
            List<Monster> selectedMonsters = new List<Monster>();
            for (int i = 0; i < 3; i++)
            {
                int index = rand.Next(filteredMonsters.Count);
                Monster originalMonster = filteredMonsters[index]; // 원본 ... 복사했을때 참조형식으로 복사가 돼서 다른 독립적인 객체로 만들기 위해 new로 생성

                // 몬스터 스킬 복사
                List<MonsterSkill> copiedSkills = new List<MonsterSkill>();
                foreach (var originalSkill in originalMonster.MonsterSkills)
                {
                    MonsterSkill copiedSkill = new MonsterSkill(originalSkill.MonsterSkillName, originalSkill.MonsterDamage);
                    copiedSkills.Add(copiedSkill);
                }

                Monster copiedMonster = new Monster(originalMonster.Name, originalMonster.Level, originalMonster.Atk, originalMonster.Def, originalMonster.Hp, originalMonster.MaxHp, originalMonster.Price);
                copiedMonster.MonsterSkills = copiedSkills; // 복사본 생성

                selectedMonsters.Add(copiedMonster);
            }
            selectedMonsters = selectedMonsters.OrderBy(monster => monster.Level).ToList(); // 보기좋게 오름차순으로 정렬해서 반환하기
            return selectedMonsters;
        }
        internal void Battle(List<Monster> monsters)
        {
            bool gameOver = false;
            int totalGold = 0;

            while (!gameOver)
            {
                Console.WriteLine("[Battle!!]\n");

                // 몬스터 정보 출력
                for (int i = 0; i < monsters.Count; i++)
                {
                    var monster = monsters[i];
                    if (monster.Hp > 0)
                        Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP: {monster.Hp}");
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP: Dead");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine($"\n[내정보]\nLv. {player.Level} {player.Name} ({player.Job})");
                Console.WriteLine($"HP: {player.Hp}/{player.MaxHp}");
                Console.WriteLine("");

                Console.WriteLine("1. 공격");
                Console.WriteLine("0. 도망가기");
                Console.WriteLine("");

                int choice = ConsoleUtility.PromptMenuChoice(0, 1);

                switch (choice)
                {
                    case 0:
                        Console.WriteLine("쫄?");
                        MainMenu();
                        break;
                    case 1:
                        Console.WriteLine("[My turn!]\n");
                        Console.WriteLine("공격할 대상을 선택하세요.\n");
                        for (int i = 0; i < monsters.Count; i++)
                        {
                            var monster = monsters[i];
                            if (monster.Hp > 0)
                                Console.WriteLine($"{i + 1}. Lv.{monster.Level} {monster.Name} HP: {monster.Hp}");
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"{i + 1}. Lv.{monster.Level} {monster.Name} HP: Dead");
                                Console.ResetColor();
                            }
                        }
                        Console.WriteLine("");
                        int targetChoice = ConsoleUtility.PromptMenuChoice(1, monsters.Count) - 1;
                        Monster targetMonster = monsters[targetChoice];
                        if (targetMonster.Hp > 0)
                        {
                            int minAttack = (int)(player.Atk * 0.9f);
                            int maxAttack = (int)(player.Atk * 1.1f);
                            int attackDamage = new Random().Next(minAttack, maxAttack + 1);
                            targetMonster.Hp -= attackDamage;
                            if (targetMonster.Hp <= 0)
                            {
                                targetMonster.Hp = 0;
                                totalGold += targetMonster.Price;
                            }
                            Console.WriteLine($"당신은 {targetMonster.Name}에게 {attackDamage}의 피해를 입혔습니다.");
                            Console.WriteLine("");
                            if (targetMonster.Hp == 0)
                            {
                                Console.WriteLine($"{targetMonster.Name}이(가) 죽었습니다.");
                                Console.WriteLine("");
                            }
                        }
                        else
                        {
                            Console.WriteLine("이미 죽은 몬스터입니다.");
                            Console.WriteLine("");
                        }
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine("");
                        break;
                }

                Console.WriteLine("[Monster Turn!]"); // 아직 몬스터 스킬 수정 필요합니당.. 스킬당 데미지 확률로 바꾸려구용~~
                 Random rand = new Random();

                foreach (var monster in monsters)
                {
                    MonsterSkill randomSkill = monster.MonsterSkills[rand.Next(monster.MonsterSkills.Count)]; // 스킬 랜덤으로 사용

                    if (monster.Hp > 0)
                    {
                        int minAttack = (int)(monster.Atk * 0.9f);
                        int maxAttack = (int)(monster.Atk * 1.1f);
                        int attackDamage = new Random().Next(minAttack, maxAttack + 1);
                       
                        // 몬스터의 스킬 사용
                        if (attackDamage >= 0)
                        {
                            player.Hp -= attackDamage;
                            if (player.Hp <= 0)
                            {
                                player.Hp = 0;
                            }
                            Console.WriteLine($">> {monster.Name}(이)가 [{randomSkill.MonsterSkillName}] 스킬을 사용했습니다! [데미지: {attackDamage}]");
                        }
                        else
                        {
                            // 회복량 표시
                            int healAmount = Math.Abs(attackDamage);
                            player.Hp += healAmount;
                            Console.WriteLine($"{monster.Name}(이)가 {healAmount}만큼 회복되었습니다.");
                            Console.WriteLine("");
                        }

                    }
                }

                gameOver = CheckGameOver(monsters, totalGold);
            }
        }

        private bool CheckGameOver(List<Monster> monsters, int totalGold)
        {
            bool allMonstersDead = monsters.All(monster => monster.Hp <= 0);
            bool playerDead = player.Hp <= 0;

            if (allMonstersDead)
            {
                Console.WriteLine("던전 공략에 성공하셨습니다.");
                Console.WriteLine($"총 획득 골드: {totalGold}");
                player.Gold += totalGold;
                Console.WriteLine("");
                Console.WriteLine("1. 다시하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                switch (ConsoleUtility.PromptMenuChoice(0, 2))
                {
                    case 0:
                        MainMenu();
                        break;
                    case 1:
                        DungeonMenu();
                        break;
                }
            }
            else if (playerDead)
            {
                Console.WriteLine("던전 공략에 실패하셨습니다.");
                Console.WriteLine("");
                Console.WriteLine("1. 다시하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                switch (ConsoleUtility.PromptMenuChoice(0, 2))
                {
                    case 0:
                        MainMenu();
                        break;
                    case 1:
                        DungeonMenu();
                        break;
                }
            }

            return false;
        }


        public void BarMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 주점입장 ■");
            Console.WriteLine("이곳에서 체력을 회복할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine($"현재 체력:{player.Hp} | 현재 골드: {player.Gold}");
            Console.WriteLine("");

            Console.WriteLine("");
            Console.WriteLine(" 1. [카스테라주] 자신의 보유 체력의 50%를 채워준다.(체력 50일때 +25)     - 가격 : 100G");
            Console.WriteLine(" 2. [복분자주] 정읍의 자랑, 100을 기준으로 체력을 50% 채워준다.          - 가격 : 300G");
            Console.WriteLine(" 3. [조니왔다] 유명 위스키, 100을 기준으로 체력을 100% 채워준다.         - 가격 : 500G");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            while (true)
            {
                switch (ConsoleUtility.PromptMenuChoice(0, 3))
                {
                    case 0:
                        MainMenu();
                        break;
                    case 1:
                        if (player.Gold < 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\t  \"키야~ 역시 한국인이라면 이 맥주를 마셔줘야지!\"");
                        player.Hp += player.Hp / 2;
                        if (player.Hp > 100)
                        {
                            player.Hp = 100;
                        }
                        player.Gold -= 100;

                        player.SavePlayerIndirectly();
                        break;
                    case 2:
                        if (player.Gold < 300)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\"아차차~ 이런 술은 난생 처음 마셔봤네! 너무 맛있다! 자주 사먹어야겠는걸?\"");
                        player.Hp += 50;
                        if (player.Hp > 100)
                        {
                            player.Hp = 100;
                        }
                        player.Gold -= 300;

                        player.SavePlayerIndirectly();
                        break;
                    case 3:
                        if (player.Gold < 500)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\t\t\t \"비싸구만.. \"");
                        player.Hp += 100;
                        if (player.Hp > 100)
                        {
                            player.Hp = 100;
                        }
                        player.Gold -= 500;

                        player.SavePlayerIndirectly();
                        break;
                }
            }
        }

        public void GuildMenu()
        {
            Console.Clear();
            ConsoleUtility.ShowTitle("■ 길드입장 ■");
            Console.WriteLine("");
            Console.WriteLine("길드에 들어서자 접수원이 말을 건다.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\"어서오세요! 길드 카지노 입니다.\"");
            Console.WriteLine("\"무엇을 도와드릴까요?\"");
            Console.ResetColor();
            Console.WriteLine("");

            Console.WriteLine("1. \"괜찮은 일거리 좀 있나?\"");
            Console.WriteLine("2. \"내가 뭘 해야 하더라...?\"");
            Console.WriteLine("3. \"보상을 받으러 왔는데\"");
            Console.WriteLine("");
            Console.WriteLine("0. 발을 돌려 나간다.");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 3))
            {
                case 0:
                    MainMenu();
                    break;
                case 1:
                    QuestMenu();
                    break;
                case 2:
                    MyQuestMenu();
                    break;
                case 3:
                    RewardMenu();
                    break;
            }

        }

        public void QuestMenu(string? prompt = null)
        {
            if (prompt != null)
            {
                // 1초간 메시지를 띄운 다음에 다시 진행
                Console.Clear();
                ConsoleUtility.ShowTitle(prompt);
                Thread.Sleep(1000);
            }

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\"현재 게시된 의뢰 목록들이에요.\"");
            Console.ResetColor();
            Console.WriteLine("");

            for (int i = 0; i<questList.Count; i++)
            {
                questList[i].PrintQuestList(i + 1);
            }

            Console.WriteLine("");
            Console.WriteLine("0. \"별거 없군...\"");
            Console.WriteLine("");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\"관심이 가는 의뢰가 있나요?\"");
            Console.ResetColor();
            Console.WriteLine("");
            int choice = ConsoleUtility.PromptMenuChoice(0, questList.Count);

            switch (choice)
            {
                case 0:
                    GuildMenu();
                    break;
                default:
                    if(questList[choice - 1].IsAccept == false)
                    {
                        QuestInfo(choice - 1);
                        QuestMenu("\"탁월한 선택이에요!\"");
                    }
                    else
                    {                       
                        QuestMenu("\"어머! 이건 이미 수락하셨는걸요?\"");
                    }
                    break;
            }

        }

        public void QuestInfo(int choice)
        {
            Console.Clear();
            questList[choice].PrintQuestInfo();
            Console.WriteLine("");

            Console.WriteLine("1. 수락");
            Console.WriteLine("0. 돌아가기");

            switch(ConsoleUtility.PromptMenuChoice(0, 1))
            {
                case 0:
                    QuestMenu();
                    break;
                case 1:
                    myQuest.Add(questList[choice]);
                    questList[choice].AcceptQuest();
                    break;
            }
        }

        public void MyQuestMenu()
        {
            Console.Clear();
            Console.WriteLine("\"현재 수령하신 의뢰 목록들이에요.");
            Console.WriteLine("");

            int i = 0;
            foreach(var quest in myQuest)
            {
                quest.PrintMyQuestList(i + 1);
                i++;
            }

            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            switch (ConsoleUtility.PromptMenuChoice(0, 0))
            {
                case 0:
                    GuildMenu();
                    break;
            }
        }

        public void RewardMenu()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\"어떤 의뢰를 완료하셨나요?\"");
            Console.ResetColor();
            Console.WriteLine("");

            int i = 0;
            foreach (var quest in completeQuest)
            {
                quest.PrintMyQuestList(i + 1);
                i++;
            }

            Console.WriteLine("");
            Console.WriteLine("0. \"다음에 다시 오지\"");
            Console.WriteLine("");

            int choice = ConsoleUtility.PromptMenuChoice(0, completeQuest.Count);

            switch (choice)
            {
                case 0:
                    GuildMenu();
                    break;
                default:
                    player.Gold += completeQuest[choice - 1].RewardGold;
                    completeQuest.RemoveAt(choice - 1);
                    RewardMenu();
                    break;
            }
        }

        public void TestQuest()
        {
            Console.Clear();

            int i = 0;
            foreach (var quest in myQuest)
            {
                quest.PrintMyQuestList(i + 1);
                i++;
            }

            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int choice = ConsoleUtility.PromptMenuChoice(0, myQuest.Count);

            switch (choice)
            {
                case 0:
                    MainMenu();
                    break;
                default:
                    myQuest[choice - 1].ClearQuest();
                    completeQuest.Add(myQuest[choice - 1]);
                    myQuest.RemoveAt(choice - 1);
                    TestQuest();
                    break;
            }

        }

        public void GameOverMenu()
        {
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 게임종료 ■");
            Console.WriteLine("다음에 다시 만나요.");
            Console.WriteLine("");

            Console.Write(">> ENTER");
            Console.ReadKey();

            Environment.Exit(0);
        }







    }

    public class Program
    {
        public static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();
            gameManager.StartGame();
        }
    }

}