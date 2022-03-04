using BepInEx;
using System;
using UnityEngine;

namespace VoidQoL
{
    [BepInPlugin(ModGuid, ModIdentifier, ModVer)]
    public class UnityPlugin : BaseUnityPlugin
    {
        internal const string ModVer =
#if DEBUG
            "9999." +
#endif
            "1.0.0";

        internal const string ModIdentifier = "VoidQoL";
        internal const string ModGuid = "com.Anreol." + ModIdentifier;
        public static UnityPlugin instance;

        public void Awake()
        {
            instance = this;
#if DEBUG
            Debug.LogWarning("Running " + ModGuid + " DEBUG build. PANIC!");
            //you can connect to yourself with a second instance of the game by hosting a private game with one and opening the console on the other and typing connect localhost:7777
            Debug.LogWarning("Setting up localhost:7777");
            On.RoR2.Networking.GameNetworkManager.OnClientConnect += (self, user, t) => { };
#endif
        }
    }
}
