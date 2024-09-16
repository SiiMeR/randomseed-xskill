using System;
using System.Linq;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace RandomSeedXSkill;

public class ItemAxePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EntityAgent), "OnBlockBrokenWith")]
    public static void OnBlockBrokenWith(ItemAxe __instance, IWorldAccessor world,
        Entity byEntity,
        ItemSlot itemslot,
        BlockSelection blockSel,
        float dropQuantityMultiplier = 1f)
    {
        var groupCode = blockSel.Block.Attributes["treeFellingGroupCode"].AsString("");
        if (string.IsNullOrEmpty(groupCode))
        {
            return;
        }

        if (blockSel.Block.BlockMaterial != EnumBlockMaterial.Wood)
        {
            return;
        }

        if (byEntity is not EntityPlayer playerEntity)
        {
            return;
        }

        var playerSkillSet = (byEntity).GetBehavior<PlayerSkillSet>();
        var skill = playerSkillSet?.FindSkill("Forestry", true);
        if (skill == null)
        {
            return;
        }
        
        var ability = skill.PlayerAbilities[RandomSeedXSkillModSystem.ForestFortuneAbilityId];
        if (ability != null)
        {
            var chance = ability.Value(0, 0);
            // random roll
            if (Random.Shared.Next(0, 100) < chance)
            {
                var seeds = (ability.Ability as ForestFortuneAbility.ForestFortune)?.Seeds;
                if (seeds == null)
                {
                    return;
                }
                
                var randomSeed = seeds[Random.Shared.Next(0, seeds.Count)];
                ItemStack itemStack = new ItemStack(world.GetItem(new AssetLocation(randomSeed.Code.Domain, randomSeed.Code.Path)), 1);

                playerEntity.TryGiveItemStack(itemStack);
            }
        }
    }
}