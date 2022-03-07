using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace VoidQoL.Modules
{
    internal class VoidFieldsQoL
    {
        private static VoidFieldsQoLServerListener instance;

        [RoR2.SystemInitializer(new Type[]
        {
            typeof(RoR2.SceneCatalog)
        })]
        public static void Init()
        {
            ArenaMissionController.onInstanceChangedGlobal += onInstanceChangedGlobal;
            ArenaMissionController.onBeatArena += RemoveComponent;
        }

        private static void RemoveComponent()
        {
            if (NetworkServer.active && instance != null)
            {
                UnityEngine.Object.Destroy(instance);
            }
        }

        private static void onInstanceChangedGlobal()
        {
            if (instance == null && NetworkServer.active)
            {
                instance = ArenaMissionController.instance.gameObject.AddComponent<VoidFieldsQoLServerListener>();
            }
        }
    }

    internal class VoidFieldsQoLServerListener : MonoBehaviour
    {
        private float accumulatedCharge;
        private int nOfClearedRounds;
        private bool awaitActivation = false;
        private EntityStateMachine cachedMachineOfCurrentRound;

        private void OnEnable()
        {
            cachedMachineOfCurrentRound = ArenaMissionController.instance.nullWards[0].GetComponent<EntityStateMachine>();
            if (Config.voidFieldsIncreaseChargeOnKill.Value)
            {
                GlobalEventManager.onCharacterDeathGlobal += onCharacterDeathGlobal;
            }
            foreach (var item in ArenaMissionController.instance.nullWards)
            {
                HoldoutZoneController holdoutZoneController = item.GetComponent<HoldoutZoneController>();
                holdoutZoneController.calcAccumulatedCharge += calcAccumulatedCharge;
                holdoutZoneController.calcRadius += calcRadius;
            }
        }

        private void calcRadius(ref float radius)
        {
            if (Config.voidFieldsHoldoutZoneRadiusMult.Value != 1f)
            {
                radius *= Config.voidFieldsHoldoutZoneRadiusMult.Value;
            }
        }

        private void OnDisable()
        {
            GlobalEventManager.onCharacterDeathGlobal -= onCharacterDeathGlobal;
            foreach (GameObject item in ArenaMissionController.instance.nullWards)
            {
                HoldoutZoneController holdoutZoneController = item.GetComponent<HoldoutZoneController>();
                holdoutZoneController.calcAccumulatedCharge -= calcAccumulatedCharge;
            }
        }

        private void OnDestroy()
        {
            SphereZone sphereZone = ArenaMissionController.instance.nullWards[ArenaMissionController.instance.currentRound].GetComponent<SphereZone>();

            if (!Run.instance.isGameOverServer && Config.voidFieldsReviveOnArenaEnd.Value)
            {
                foreach (var item in PlayerCharacterMasterController.instances)
                {
                    CharacterMaster characterMaster = item.master;
                    if (item.isConnected && characterMaster.IsDeadAndOutOfLivesServer() && Config.voidFieldsReviveOnRoundStart.Value)
                    {
                        Vector3 vector = characterMaster.deathFootPosition;
                        if (sphereZone)
                        {
                            vector = (TeleportHelper.FindSafeTeleportDestination(sphereZone.transform.position, characterMaster.bodyPrefab.GetComponent<CharacterBody>(), RoR2Application.rng) ?? vector);
                        }
                        characterMaster.Respawn(vector, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
                        CharacterBody body = characterMaster.GetBody();
                        if (body)
                        {
                            body.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                            foreach (EntityStateMachine entityStateMachine in body.GetComponents<EntityStateMachine>())
                            {
                                entityStateMachine.initialStateType = entityStateMachine.mainStateType;
                            }
                        }
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (nOfClearedRounds != ArenaMissionController.instance.clearedRounds)
            {
                cachedMachineOfCurrentRound = ArenaMissionController.instance.nullWards[ArenaMissionController.instance.currentRound].GetComponent<EntityStateMachine>();
                nOfClearedRounds = ArenaMissionController.instance.clearedRounds;
                awaitActivation = true;
            }
            if (awaitActivation && cachedMachineOfCurrentRound.state is EntityStates.Missions.Arena.NullWard.Active ActiveNullWard)
            {
                awaitActivation = false;
                if (!Run.instance.isGameOverServer)
                {
                    for (int i = 0; i < PlayerCharacterMasterController.instances.Count; i++)
                    {
                        CharacterMaster characterMaster = PlayerCharacterMasterController.instances[i].master;
                        if (PlayerCharacterMasterController.instances[i].isConnected && characterMaster.IsDeadAndOutOfLivesServer() && Config.voidFieldsReviveOnRoundStart.Value)
                        {
                            Vector3 vector = characterMaster.deathFootPosition;
                            if (ActiveNullWard.sphereZone)
                            {
                                vector = (TeleportHelper.FindSafeTeleportDestination(ActiveNullWard.sphereZone.transform.position, characterMaster.bodyPrefab.GetComponent<CharacterBody>(), RoR2Application.rng) ?? vector);
                            }
                            characterMaster.Respawn(vector, Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f));
                            CharacterBody body = characterMaster.GetBody();
                            if (body)
                            {
                                body.AddTimedBuff(RoR2Content.Buffs.Immune, 3f);
                                foreach (EntityStateMachine entityStateMachine in body.GetComponents<EntityStateMachine>())
                                {
                                    entityStateMachine.initialStateType = entityStateMachine.mainStateType;
                                }
                            }
                        }

                        if (ActiveNullWard.sphereZone.IsInBounds(characterMaster.GetBody().transform.position) && Config.voidFieldsHealOnRoundStart.Value)
                        {
                            characterMaster.GetBody().healthComponent.HealFraction(0.75f, default(ProcChainMask));
                            EffectData effectData = new EffectData
                            {
                                origin = characterMaster.GetBody().transform.position
                            };
                            effectData.SetNetworkedObjectReference(characterMaster.GetBodyObject());
                            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MedkitHealEffect"), effectData, true);
                        }
                    }
                }
            }
        }

        private void calcAccumulatedCharge(ref float charge)
        {
            if (charge > 0.05) //If it's charging
            {
                charge += accumulatedCharge / 100f;
                accumulatedCharge = 0;
            }
        }

        private void onCharacterDeathGlobal(DamageReport obj)
        {
            if (TeamManager.IsTeamEnemy(obj.victimTeamIndex, TeamIndex.Player))
            {
                if (cachedMachineOfCurrentRound.state is EntityStates.Missions.Arena.NullWard.Active)
                {
                    accumulatedCharge += obj.victimIsChampion ? 5f : obj.victimBody.bestFitRadius / 2f;
                }
            }
        }
    }
}