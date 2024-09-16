using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace RandomSeedXSkill;

public class ForestFortuneAbility
{
    public class ForestFortune : Ability
    {
        private List<ItemTreeSeed> seeds;
        public List<ItemTreeSeed> Seeds
        {
            get
            {
                if (seeds == null)
                {
                    seeds = new List<ItemTreeSeed>();

                    foreach (Item obj in RandomSeedXSkillModSystem.xleveling.Api.World.Items)
                    {
                        if (obj is ItemTreeSeed itemPlantableSeed)
                            seeds.Add(itemPlantableSeed);
                    }
                }
                return seeds;
            }
        }
        
        public ForestFortune() : base("forestfortune",
            "Forest Fortune", "Gives you a {0}% chance to get a random seed upon cutting down a tree.", 0, 3,
            new[] { 5, 10, 15 })
        {
        }
    }
}