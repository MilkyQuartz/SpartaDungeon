using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    public enum QuestType
    {
        levelUp,
        hunt,
        equip
    }
    internal class Quest
    {
        public string Name { get; }
        public string Desc { get; }
        public string Require { get; }
        public int Achievement { get; }
        public int Progress { get; set; }
        //public string RewardItem { get; }
        public int RewardGold { get; }
        public bool IsAccept { get; private set; }
        public bool IsClear { get; private set; }
        public QuestType Type { get; }


        public Quest(string name, string desc, string require, int achievement, int progress, int rewardGold, QuestType type, bool isAccept = false, bool isClear = false)
        {
            Name = name;
            Desc = desc;
            Require = require;
            Achievement = achievement;
            Progress = progress;
            //RewardItem = rewardItem;
            RewardGold = rewardGold;
            Type = type;
            IsAccept = isAccept;
            IsClear = isClear;
        }


        public void PrintQuestList(int idx)  //퀘스트 목록 보여주기
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{idx}. ");
            Console.ResetColor();

            if (IsAccept)
            {
                Console.Write(Name);
                Console.WriteLine(" - 수령 완료");
            }
            else
            {
                Console.WriteLine(Name);
            }

        }

        public void PrintQuestInfo()  // 퀘스트 정보 보여주기
        {
            Console.WriteLine(Name);
            Console.WriteLine("");

            Console.WriteLine(Desc);
        }

        public void PrintMyQuestList(int idx)  // 수령한 퀘스트 목록 보여주기
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{idx}. ");
            Console.WriteLine(Name);
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine(Desc);
            Console.WriteLine("");

            if (Type == QuestType.levelUp)
            {
                Console.WriteLine($"{Require} 달성 : {Progress} / {Achievement}");
                Console.WriteLine("");
            }
            else if (Type == QuestType.hunt)
            {
                Console.WriteLine($"{Name} : {Progress} / {Achievement}");
                Console.WriteLine("");
            }

        }

        public void AcceptQuest()  //퀘스트 수락처리
        {
            IsAccept = true;
            IsClear = false;
        }

        public void ClearQuest()  //퀘스트 완료처리
        {
            IsClear = true;
        }

    }

}