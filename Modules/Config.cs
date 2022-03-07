using BepInEx.Configuration;
using System;

namespace VoidQoL
{
    internal static class Config
    {
        internal static ConfigEntry<bool> voidFieldsIncreaseChargeOnKill;
        internal static ConfigEntry<bool> voidFieldsHealOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnArenaEnd;
        internal static ConfigEntry<float> voidFieldsHoldoutZoneRadiusMult;

        public static void Initialize()
        {
            voidFieldsIncreaseChargeOnKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Charge",
                "Enabled",
                true,
                "Should the holdout zones gain charge from monster kills.");
            voidFieldsHealOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Heal",
                "Enabled",
                 true,
                "Should the players inside the Void Vent get healed when starting a new wave.");
            voidFieldsReviveOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Round Revive",
                "Enabled",
                false,
                "Should dead players revive when starting a new wave.");
            voidFieldsReviveOnArenaEnd =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Arena End Revive",
                "Enabled",
                false,
                "Should dead players revive when the arena has been cleared.");
            voidFieldsHoldoutZoneRadiusMult =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Zone Multiplicator",
                "Enabled",
                1f,
                "By what number should the Holdout Zone multiply its radius for.");
        }
    }
}