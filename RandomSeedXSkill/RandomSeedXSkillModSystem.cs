using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using XLib.XLeveling;

namespace RandomSeedXSkill
{
    public class RandomSeedXSkillModSystem : ModSystem
    {
        public static ICoreAPI capi; 
        public static XLeveling xleveling;
        public static int ForestFortuneAbilityId;
        
        // All skills and abilities should be registered in the StartPre method,
        //  since the Xlib needs to have all abilities already registered in its Start method.
        //  The XLeveling interface is initialized in its StartPre method, so your mod has to be loaded after it.
        //  So you should add xlib to the dependencies in your modinfo.json file.
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            capi = api;
            
            // The Instance method is just a static shortcut for "api.ModLoader.GetModSystem("XLib.XLeveling.XLeveling") as XLeveling".
            // So it gives us the current mod interface for XLeveling
            xleveling = XLeveling.Instance(api);
            ForestFortuneAbilityId = xleveling.GetSkill("forestry").AddAbility(new ForestFortuneAbility.ForestFortune());
            
            var harmony = new Harmony("randomseedxskill");

            var original = AccessTools.Method(typeof(ItemAxe), nameof(ItemAxe.OnBlockBrokenWith));
            var postfix = AccessTools.Method(typeof(ItemAxePatch), nameof(ItemAxePatch.OnBlockBrokenWith));

            harmony.Patch(original, new HarmonyMethod(postfix));
        }

        public override double ExecuteOrder()
        {
            return 10000;
        }
    }
}