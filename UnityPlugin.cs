using BepInEx;

namespace VoidQoL
{
    [BepInPlugin(ModGuid, ModIdentifier, ModVer)]
    public class UnityPlugin : BaseUnityPlugin
    {
        internal const string ModVer =
#if DEBUG
            "9999." +
#endif
            "1.1.5";

        internal const string ModIdentifier = "VoidQoL";
        internal const string ModGuid = "com.Anreol." + ModIdentifier;
        public static UnityPlugin instance;

        public void Awake()
        {
            instance = this;
            VoidQoL.Config.Initialize();
        }
    }
}
