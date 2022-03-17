using BepInEx.Configuration;
using System;
using UnityEngine;

namespace VoidQoL
{
    internal static class Config
    {
        internal static ConfigEntry<bool> voidFieldsIncreaseChargeOnKill;
        internal static ConfigEntry<bool> voidFieldsEnemyHasteOnSpawn;
        internal static ConfigEntry<bool> voidFieldsHealOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnArenaEnd;
        internal static ConfigEntry<float> voidFieldsHoldoutZoneRadiusMult;

        internal static ConfigEntry<bool> voidLocusIncreaseChargeOnKill;
        internal static ConfigEntry<bool> voidLocusPlayerFogHaste;
        internal static ConfigEntry<bool> voidLocusDecreaseRadiusIfEnemyInvades;
        internal static ConfigEntry<bool> voidLocusHoldoutZoneVerticalTube;
        internal static ConfigEntry<float> voidLocusHoldoutZoneRadiusExtra;
        internal static ConfigEntry<float> voidLocusHoldoutZoneAutoCharge;
        internal static ConfigEntry<float> voidLocusHoldoutZonePlayerScaling;
        internal static ConfigEntry<float> voidLocusHoldoutZoneDischargeRate;
        public static void Initialize()
        {
            voidFieldsIncreaseChargeOnKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Charge",
                "Charge On Kill",
                true,
                "Should the holdout zones gain charge from monster kills.");
            voidFieldsEnemyHasteOnSpawn =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Enemy Haste",
                "Hasten enemies",
                true,
                "Should monsters spawning get a speedboost when spawning.");
            voidFieldsHealOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Heal",
                "Round Heal",
                 true,
                "Should the players inside the Void Vent get healed when starting a new wave.");
            voidFieldsReviveOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Round Revive",
                "Round Revival",
                false,
                "Should dead players revive when starting a new wave.");
            voidFieldsReviveOnArenaEnd =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Arena End Revive",
                "Clear Revival",
                false,
                "Should dead players revive when the arena has been cleared.");
            voidFieldsHoldoutZoneRadiusMult =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields Zone Multiplicator",
                "Radius Multiplication",
                1f,
                "By what number should the Holdout Zone multiply its radius for.");

            voidLocusIncreaseChargeOnKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Charge",
                "Charge On Kill",
                false,
                "Should the holdout zones gain charge from monster kills.");
            voidLocusPlayerFogHaste =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Player Fog Haste",
                "Apply haste if inside fog",
                true,
                "If true, it will grant a speed boost to all players in the fog whenever a zone gets activated.");
            voidLocusDecreaseRadiusIfEnemyInvades =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Radius Decrease If Enemy Invades",
                "Enable",
                true,
                "If true, the radius of the charging zone will decrease if there's enemies inside.");
            voidLocusHoldoutZoneVerticalTube =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Zone Shape",
                "Vertical Tube",
                true,
                "If true, it will change the zones to be a vertical tube instead of a sphere.");
            voidLocusHoldoutZoneRadiusExtra =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Zone Size Increase",
                "Radius Increase",
                5f,
                "By what number should the Holdout Zone increase its radius. Vanilla has a base radius of 20m, and it cannot be smaller than 5m.");
            voidLocusHoldoutZoneAutoCharge =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Zone Auto Charge",
                "Auto charging percentage",
                0f,
                "What percentage should the Holdout Zone gain charge each second.");
            voidLocusHoldoutZonePlayerScaling =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Zone Player Scaling",
                "Player scale value",
                0.75f,
                "How much players affect the charging rate while inside the zone. Default is 1. Math operation is (PlayersInRadius / AlivePlayers) ^ this.");
            voidLocusHoldoutZoneDischargeRate =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus Zone Discharge Rate",
                "Discharge Rate",
                -0.005f,
                "By how much should the zone discharge when there's no players inside. Negative values add to the charge percentage.");

#if DEBUG
            Debug.Log("The config values are:\nvoidFieldsIncreaseChargeOnKill " + voidFieldsIncreaseChargeOnKill.Value +
                "\nvoidFieldsHealOnRoundStart " + voidFieldsHealOnRoundStart.Value +
                "\nvoidFieldsReviveOnRoundStart " + voidFieldsReviveOnRoundStart.Value +
                "\nvoidFieldsReviveOnArenaEnd" + voidFieldsReviveOnArenaEnd.Value +
                "\nvoidFieldsHoldoutZoneRadiusMult" + voidFieldsHoldoutZoneRadiusMult.Value +
                "\n\nvoidLocusIncreaseChargeOnKill " + voidLocusIncreaseChargeOnKill.Value +
                "\nvoidLocusPlayerFogHaste " + voidLocusPlayerFogHaste.Value +
                "\voidLocusDecreaseRadiusIfEnemyInvades " + voidLocusDecreaseRadiusIfEnemyInvades.Value +
                "\nvoidLocusHoldoutZoneVerticalTube " + voidLocusHoldoutZoneVerticalTube.Value +
                "\voidLocusHoldoutZoneRadiusExtra " + voidLocusHoldoutZoneRadiusExtra.Value +
                "\nvoidLocusHoldoutZoneAutoCharge" + voidLocusHoldoutZoneAutoCharge.Value +
                "\nvoidLocusHoldoutZonePlayerScaling " + voidLocusHoldoutZonePlayerScaling.Value
                );
#endif

        }
    }
}