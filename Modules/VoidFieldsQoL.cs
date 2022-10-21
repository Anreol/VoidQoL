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
            if (instance != null)
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
        private float totalAccumulatedChargeInCurrentRound;
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
            if (Config.voidFieldsEnemyHasteOnSpawn.Value)
            {
                CharacterBody.onBodyStartGlobal += onBodyStartGlobal;
            }
            foreach (var item in ArenaMissionController.instance.nullWards)
            {
                HoldoutZoneController holdoutZoneController = item.GetComponent<HoldoutZoneController>();
                holdoutZoneController.calcAccumulatedCharge += calcAccumulatedCharge;
                holdoutZoneController.calcRadius += calcRadius;
            }
        }

        private void onBodyStartGlobal(CharacterBody obj)
        {
            if (TeamManager.IsTeamEnemy(obj.teamComponent.teamIndex, TeamIndex.Player))
            {
                obj.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, Config.voidFieldsEnemyHasteDuration.Value - obj.moveSpeed);
                obj.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.75f); //To avoid chain killing due to procs... hehehe
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
            Debug.LogWarning("Destroyed!");
            SphereZone sphereZone = cachedMachineOfCurrentRound.gameObject.GetComponent<SphereZone>();
            if (!Run.instance.isGameOverServer && Config.voidFieldsReviveOnArenaEnd.Value)
            {
                Debug.LogWarning("Reviving players at round end!");
                foreach (var item in PlayerCharacterMasterController.instances)
                {
                    Debug.LogWarning("Checking for revival " + item.GetDisplayName());
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
                Debug.LogWarning("Awaiting new Round!");
                Debug.LogWarning("Amount of charge accumulated in the previous round by kills: " + totalAccumulatedChargeInCurrentRound);
                totalAccumulatedChargeInCurrentRound = 0;
                cachedMachineOfCurrentRound = ArenaMissionController.instance.nullWards[ArenaMissionController.instance.currentRound].GetComponent<EntityStateMachine>();
                nOfClearedRounds = ArenaMissionController.instance.clearedRounds;
                awaitActivation = true;
            }
            if (awaitActivation && cachedMachineOfCurrentRound.state is EntityStates.Missions.Arena.NullWard.Active ActiveNullWard)
            {
                Debug.LogWarning("New Round!");
                awaitActivation = false;
                if (!Run.instance.isGameOverServer)
                {
                    for (int i = 0; i < PlayerCharacterMasterController.instances.Count; i++)
                    {
                        Debug.LogWarning("Checking for healing & Revival: " + PlayerCharacterMasterController.instances[i].GetDisplayName());
                        CharacterMaster characterMaster = PlayerCharacterMasterController.instances[i].master;
                        if (PlayerCharacterMasterController.instances[i].isConnected && characterMaster.IsDeadAndOutOfLivesServer() && Config.voidFieldsReviveOnRoundStart.Value)
                        {
                            Vector3 vector = characterMaster.deathFootPosition;
                            if (ActiveNullWard.sphereZone)
                            {
                                vector = (TeleportHelper.FindSafeTeleportDestination(ActiveNullWard.sphereZone.transform.position, characterMaster.bodyPrefab.GetComponent<CharacterBody>(), RoR2Application.rng) ?? vector);
                            }
                            Debug.LogWarning("Respawning " + PlayerCharacterMasterController.instances[i].GetDisplayName());
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
                        CharacterBody characterBody = characterMaster.GetBody();
                        if (Config.voidFieldsHealOnRoundStart.Value)
                        {
                            Debug.LogWarning("Healing " + characterMaster.GetBody().GetDisplayName());

                            characterBody.healthComponent.HealFraction(0.05f, default(ProcChainMask));
                            EffectData effectData = new EffectData
                            {
                                origin = characterBody.transform.position
                            };
                            effectData.SetNetworkedObjectReference(characterBody.gameObject);
                            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/MedkitHealEffect"), effectData, true);
                            bool isCloseEnough = Vector3.Distance(characterBody.transform.position, ActiveNullWard.transform.position) <= 5f;
                            if (ActiveNullWard.sphereZone.IsInBounds(characterBody.transform.position) || isCloseEnough)
                            {
                                Debug.LogWarning("Healing more since was inside void cell " + characterMaster.GetBody().GetDisplayName());
                                characterBody.healthComponent.HealFraction(0.45f, default(ProcChainMask));
                            }
                        }
                    }
                }
            }
        }

        private void calcAccumulatedCharge(ref float charge)
        {
            if (charge >= 0.05f) //If it's charging
            {
                charge += accumulatedCharge / 100f;
                accumulatedCharge = 0;
            }
        }

        private void onCharacterDeathGlobal(DamageReport obj)
        {
            if (TeamManager.IsTeamEnemy(obj.victimTeamIndex, TeamIndex.Player))
            {
                if (cachedMachineOfCurrentRound.state is EntityStates.Missions.Arena.NullWard.Active || !awaitActivation)
                {
                    if (Config.voidFieldsIncreaseChargeBasedOnSize.Value)
                    {
                        accumulatedCharge += obj.victimIsChampion ? 5f : obj.victimBody.bestFitRadius / 1.5f;
                        totalAccumulatedChargeInCurrentRound += obj.victimIsChampion ? 5f : obj.victimBody.bestFitRadius / 1.5f;
                    }
                    if (Config.voidFieldsIncreaseChargePercentagePerKill.Value > 0f)
                    {
                        accumulatedCharge += Config.voidFieldsIncreaseChargePercentagePerKill.Value;
                        totalAccumulatedChargeInCurrentRound += Config.voidFieldsIncreaseChargePercentagePerKill.Value;
                    }
                }
            }
        }
    }
}