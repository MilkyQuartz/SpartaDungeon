using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Quest
    {
        public string Name { get; }
        public string Desc { get; }
        public int Require {  get; }
        public string RewardItem { get; }
        public int RewardGold { get; }
        

        public Quest(string name, string desc, int require, string rewardItem, int rewardGold)
        {
            Name = name;
            Desc = desc;
            Require = require;
            RewardItem = rewardItem;
            RewardGold = rewardGold;
        }


        //퀘스트 목록 보여주기

        //퀘스트 수락하기

        //퀘스트 완료하기,보상받기
    }
}
