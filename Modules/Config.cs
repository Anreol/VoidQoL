﻿using BepInEx.Configuration;
using System;
using UnityEngine;

namespace VoidQoL
{
    internal static class Config
    {
        internal static ConfigEntry<bool> voidFieldsIncreaseChargeOnKill;
        internal static ConfigEntry<bool> voidFieldsIncreaseChargeBasedOnSize;
        internal static ConfigEntry<float> voidFieldsIncreaseChargePercentagePerKill;
        internal static ConfigEntry<bool> voidFieldsEnemyHasteOnSpawn;
        internal static ConfigEntry<float> voidFieldsEnemyHasteDuration;
        internal static ConfigEntry<bool> voidFieldsHealOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnRoundStart;
        internal static ConfigEntry<bool> voidFieldsReviveOnArenaEnd;
        internal static ConfigEntry<float> voidFieldsHoldoutZoneRadiusMult;

        internal static ConfigEntry<bool> voidLocusIncreaseChargeOnKill;
        internal static ConfigEntry<bool> voidLocusPlayerFogHaste;
        internal static ConfigEntry<bool> voidLocusSupressNPCEntry;
        internal static ConfigEntry<bool> voidLocusDecreaseRadiusIfEnemyInvades;
        internal static ConfigEntry<bool> voidLocusVoidMonsterNoVoidItem;
        internal static ConfigEntry<bool> voidLocusHoldoutZoneVerticalTube;
        internal static ConfigEntry<float> voidLocusHoldoutZoneRadiusExtra;
        internal static ConfigEntry<float> voidLocusHoldoutZoneAutoCharge;
        internal static ConfigEntry<float> voidLocusHoldoutZonePlayerScaling;
        internal static ConfigEntry<float> voidLocusHoldoutZoneDischargeRate;
        public static void Initialize()
        {
            voidFieldsIncreaseChargeOnKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Charge On Kill : Active",
                true,
                "Should the holdout zones gain charge from monster kills.");
            voidFieldsIncreaseChargeBasedOnSize =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Charge On Kill : Gain based on enemy Size",
                true,
                "Should the holdout zones gain charge from monster kills based on their size divided by 1.5. Champions always give flat 5%. Needs: Charge On Kill : Active");
            voidFieldsIncreaseChargePercentagePerKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Charge On Kill : Flat gain per skill",
                0f,
                "Flat amount of charge the holdout zones will gain with each kill. Needs: Charge On Kill : Active");
            voidFieldsEnemyHasteOnSpawn =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Hasten Enemies : Active",
                true,
                "Should monsters spawning get a speedboost when spawning.");
            voidFieldsEnemyHasteDuration =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Hasten Enemies : Buff Duration",
                16f,
                "Base amount in seconds which the cloak speed boost buff will last in enemies spawning. Substracted by their current speed, so fast enemies / enemies with items don't go overdrive. Needs: Hasten Enemies : Active");
            voidFieldsHealOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Round Heal",
                 true,
                "Should the players inside the Void Vent get healed when starting a new wave.");
            voidFieldsReviveOnRoundStart =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Round Revival",
                false,
                "Should dead players revive when starting a new wave.");
            voidFieldsReviveOnArenaEnd =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Clear Revival",
                false,
                "Should dead players revive when the arena has been cleared.");
            voidFieldsHoldoutZoneRadiusMult =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Fields",
                "Radius Multiplication",
                1f,
                "By what number should the Holdout Zone multiply its radius for.");

            voidLocusIncreaseChargeOnKill =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Charge On Kill",
                false,
                "Should the holdout zones gain charge from monster kills.");
            voidLocusPlayerFogHaste =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Apply haste if inside fog on new round start.",
                true,
                "If true, it will grant a speed boost to all players in the fog whenever a zone gets activated.");
            voidLocusSupressNPCEntry =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Supress NPC entry.",
                false,
                "If true, NPCs like drones won't be allowed in the stage. Bazaar between Time functions in the same way.");
            voidLocusDecreaseRadiusIfEnemyInvades =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Decrease If Enemy Invades",
                true,
                "If true, the radius of the charging zone will decrease if there's enemies inside.");
            voidLocusVoidMonsterNoVoidItem =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Remove void items from void enemies",
                true,
                "If true, any void enemies that spawn will get their void items taken away.");
            voidLocusHoldoutZoneVerticalTube =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Vertical Tube",
                true,
                "If true, it will change the zones to be a vertical tube instead of a sphere.");
            voidLocusHoldoutZoneRadiusExtra =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Radius Increase",
                5f,
                "By what number should the Holdout Zone increase its radius. Vanilla has a base radius of 20m, and it cannot be smaller than 5m.");
            voidLocusHoldoutZoneAutoCharge =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Auto charging percentage",
                0f,
                "What percentage should the Holdout Zone gain charge each second.");
            voidLocusHoldoutZonePlayerScaling =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
                "Player scale value",
                0.75f,
                "How much players affect the charging rate while inside the zone. Default is 1. Math operation is (PlayersInRadius / AlivePlayers) ^ this.");
            voidLocusHoldoutZoneDischargeRate =
                UnityPlugin.instance.Config.Bind("VoidQoL :: Void Locus",
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