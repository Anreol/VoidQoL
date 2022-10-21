using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace VoidQoL.Modules
{
    public class VoidLocusQoL
    {
        public static VoidLocusQoLMissionControllerListener instance;

        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.SceneCatalog)
        })]
        public static void Init()
        {
            Stage.onStageStartGlobal += onStageStartGlobal;
            CharacterBody.onBodyStartGlobal += onBodyStartGlobal;
            SceneCatalog.GetSceneDefFromSceneName("voidstage").suppressNpcEntry = Config.voidLocusSupressNPCEntry.Value;
        }

        private static void onBodyStartGlobal(CharacterBody obj)
        {
            if (NetworkServer.active && instance != null)
            {
                if (Config.voidLocusVoidMonsterNoVoidItem.Value && obj.teamComponent.teamIndex == TeamIndex.Void)
                {
                    List<ItemIndex> voidShit = new List<ItemIndex>();
                    foreach (var item in obj.inventory.itemAcquisitionOrder)
                    {
                        ItemDef shit = ItemCatalog.GetItemDef(item);
                        if (shit.tier == ItemTier.VoidTier1 || shit.tier == ItemTier.VoidTier2 || shit.tier == ItemTier.VoidTier3 || shit.tier == ItemTier.VoidBoss)
                        {
                            voidShit.Add(item);
                        }
                    }
                    foreach (var item in voidShit)
                    {
                        obj.inventory.ResetItem(item);
                    }
                }
            }
        }

        /*private static void EditVoidSignalPrefab()
        {
            var asynoperation = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/DLC1/DeepVoidPortalBattery/iscDeepVoidPortalBattery.asset");
            var asset = asynoperation.WaitForCompletion();
            asset.prefab.GetComponent<HoldoutZoneController>().holdoutZoneShape = HoldoutZoneController.HoldoutZoneShape.VerticalTube;
        }*/

        public static void onStageStartGlobal(Stage obj)
        {
            if (VoidStageMissionController.instance && instance == null)
            {
                instance = VoidStageMissionController.instance.gameObject.AddComponent<VoidLocusQoLMissionControllerListener>();
            }
        }
    }

    public class VoidLocusQoLMissionControllerListener : MonoBehaviour
    {
        //FIXME: NOT NETWORKED
        private int cachedFogCount; //if this changes, it means that there's a new battery active

        private void FixedUpdate()
        {
            //FIXME: NOT NETWORKED
            /*if (VoidStageMissionController.instance.fogDamageController)
            {
                if (VoidStageMissionController.instance.fogRefCount > cachedFogCount)
                {
                    ScanForHoldoutZones();
                }
                cachedFogCount = VoidStageMissionController.instance.fogRefCount;
            }*/
            ScanForHoldoutZones();
        }

        private void ScanForHoldoutZones()
        {
            List<HoldoutZoneController> listZone = InstanceTracker.GetInstancesList<HoldoutZoneController>();
            foreach (var item in listZone)
            {
                if (!item.gameObject.GetComponent<VoidLocusQoLHoldoutZoneController>())
                {
#if DEBUG
                    Debug.LogWarning("Gave " + item + " its own VoidLocusQoL controller");
#endif
                    item.gameObject.AddComponent<VoidLocusQoLHoldoutZoneController>();
                    if (Config.voidLocusPlayerFogHaste.Value && NetworkServer.active)
                    {
#if DEBUG
                        Debug.Log("Applying haste!");
#endif
                        IEnumerable<CharacterBody> affectedBodiesOnTeam = VoidStageMissionController.instance.fogDamageController.GetAffectedBodiesOnTeam(TeamIndex.Player);
                        foreach (var body in affectedBodiesOnTeam)
                        {
                            body.AddTimedBuff(DLC1Content.Buffs.KillMoveSpeed, 6f);
                            body.AddTimedBuff(DLC1Content.Buffs.KillMoveSpeed, 6f);
                            body.AddTimedBuff(DLC1Content.Buffs.KillMoveSpeed, 6f);
                            body.AddTimedBuff(DLC1Content.Buffs.KillMoveSpeed, 6f);
                            body.AddTimedBuff(DLC1Content.Buffs.KillMoveSpeed, 6f);
#if DEBUG
                            Debug.Log("Applied haste to " + body);
#endif
                        }
                    }
                }
            }
        }
    }

    internal class VoidLocusQoLHoldoutZoneController : MonoBehaviour
    {
        private HoldoutZoneController disThing;
        private SphereSearch sphereSearch;
        private List<HurtBox> enemyHurtboxList;
        private TeamMask voidTeam;
        private float chargeFromKilling;
        private float stopwatch;

        private void OnEnable()
        {
            InstanceTracker.Add<VoidLocusQoLHoldoutZoneController>(this);
            disThing = gameObject.GetComponent<HoldoutZoneController>();
            if (disThing == null)
            {
                Debug.LogError("Destroying self, theres no HoldoutZoneController to attach ourselves to!");
                Destroy(this);
                return;
            }
            if (Config.voidLocusHoldoutZoneVerticalTube.Value)
            {
#if DEBUG
                Debug.Log("Tubifying the zone...");
#endif
                disThing.radiusIndicator.gameObject.transform.parent.transform.localScale = new Vector3(1, 50, 1);
                disThing.holdoutZoneShape = HoldoutZoneController.HoldoutZoneShape.VerticalTube;
            }
            if (NetworkServer.active)
            {
                voidTeam = new TeamMask();
                voidTeam.AddTeam(TeamIndex.Void);
                if (Config.voidLocusDecreaseRadiusIfEnemyInvades.Value)
                {
#if DEBUG
                    Debug.Log("Initializing sphere search...");
#endif
                    enemyHurtboxList = new List<HurtBox>();
                    sphereSearch = new RoR2.SphereSearch()
                    {
                        queryTriggerInteraction = UnityEngine.QueryTriggerInteraction.Collide,
                        mask = LayerIndex.entityPrecise.mask,
                        origin = this.transform.position,
                    };
                }
                GlobalEventManager.onCharacterDeathGlobal += onCharacterDeathGlobal;
                disThing.calcAccumulatedCharge += calcAccumulatedCharge;
                disThing.calcRadius += calcRadius;
                disThing.playerCountScaling = Config.voidLocusHoldoutZonePlayerScaling.Value;
                disThing.dischargeRate = Config.voidLocusHoldoutZoneDischargeRate.Value;
            }
        }

        private void FixedUpdate()
        {
            if (disThing.wasCharged)
            {
                Destroy(this);
            }
        }

        private void OnDisable()
        {
            InstanceTracker.Remove<VoidLocusQoLHoldoutZoneController>(this);
        }

        private void onCharacterDeathGlobal(DamageReport obj)
        {
            if (TeamManager.IsTeamEnemy(obj.victimTeamIndex, TeamIndex.Player))
            {
                chargeFromKilling += obj.victimIsChampion ? 5f : obj.victimBody.bestFitRadius / 5f;
            }
        }

        private void calcRadius(ref float radius)
        {
            float finalRadius = 0;
            if (Config.voidLocusDecreaseRadiusIfEnemyInvades.Value && sphereSearch != null)
            {
                sphereSearch.radius = disThing.currentRadius;
                enemyHurtboxList.Clear();
                sphereSearch.RefreshCandidates().FilterCandidatesByDistinctHurtBoxEntities().FilterCandidatesByHurtBoxTeam(voidTeam).GetHurtBoxes(enemyHurtboxList);
                foreach (HurtBox item in enemyHurtboxList)
                {
                    if (!item) //HOW?
                        continue;
                    if (!item.healthComponent)
                        continue;
                    if (!item.healthComponent.body)
                        continue;
                    finalRadius -= item.healthComponent.body.bestFitRadius > 1f ? 1f : item.healthComponent.body.bestFitRadius;
                }
            }
            finalRadius += Config.voidLocusHoldoutZoneRadiusExtra.Value;
            radius += finalRadius;
        }

        private void calcAccumulatedCharge(ref float charge)
        {
            if (charge >= 0.01f)
            {
                stopwatch += Time.fixedDeltaTime;
                if (stopwatch >= 1f)
                {
                    stopwatch = 0;
                    charge += Config.voidLocusHoldoutZoneAutoCharge.Value;
                }
                if (Config.voidLocusIncreaseChargeOnKill.Value)
                {
                    charge += chargeFromKilling / 100;
                    chargeFromKilling = 0;
                }
            }
        }
    }
}