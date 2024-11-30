using System.Collections;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using SophisticatedStreamCam.Sceeps;
using UnityEngine;
using UnityEngine.UI;

namespace SophisticatedStreamCam
{
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		public static Plugin instance;
        public static AssetBundle bundle;
        public static GameObject Cam;
        public static string currentStreamKey;
        public static Streamered streamered;
        public string assetBundleName = "cam";
        public string parentName = "cam";
        public GameObject assetBundleParent;


        void Start()
		{
			GorillaTagger.OnPlayerSpawned(OnGameInitialized);
		}

		void OnGameInitialized()
		{
			instance = this;

			//This loads the asset bundle put in the AssetBundles folder
			bundle = LoadAssetBundle("SophisticatedStreamCam.Resources." + assetBundleName);

            //Spawn in Parent
            assetBundleParent = Instantiate(bundle.LoadAsset<GameObject>(parentName));

            //Set Parent Pos (DO NOT CHANGE)
            assetBundleParent.transform.position = new Vector3(-67.2225f, 11.57f, -82.611f);
	    Ppman3000();
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }
        RenderTexture FindRt(string name)
        {
            RenderTexture[] allRenderTextures = Resources.FindObjectsOfTypeAll<RenderTexture>();

            foreach (RenderTexture rt in allRenderTextures)
            {
                if (rt.name == name)
                {
                    return rt;
                }
            }
            return null;
        }
        private ConfigEntry<string> streamKey;
        private void Awake()
        {
            // Define a configuration entry for the stream key
             streamKey = Config.Bind(
                "Streaming",                    
                "StreamKey",                    
                "Stream-Key",
                "The Twitch stream key for streaming."
            );
            string currentStreamKey = streamKey.Value;
        }
        public void Ppman3000()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }
            GameObject clonedCameraObject = Instantiate(mainCamera.gameObject, mainCamera.transform.position, mainCamera.transform.rotation);
            Camera clonedCamera = clonedCameraObject.GetComponent<Camera>();
            clonedCamera.fieldOfView = 90;
            RenderTexture renderTexture = new RenderTexture(512, 256, 24)
            {
                antiAliasing = 1
            };
            renderTexture.Create();
            clonedCamera.targetTexture = renderTexture;
            Streamered.secondaryRenderTexture = renderTexture;
            RenderTexture ling = FindRt("ling");
            streamered = new Streamered(ling);
            Material material = new Material(Shader.Find("Universal Render Pipeline/Unlit"))
            {
                mainTexture = renderTexture
            };
            GameObject firstpObject = GameObject.Find("firstp");
            GameObject fpv = GameObject.Find("frist");
            if (firstpObject == null)
            {
                return;
            }
            Renderer renderer = firstpObject.GetComponent<Renderer>();
            RawImage raw = fpv.GetComponent<RawImage>();
            if (renderer != null)
            {
                renderer.material = material;
                raw.material = material;
            }

            GameObject swi = GameObject.Find("switch");
            GameObject swis = GameObject.Find("Starst");
            StreamButt.Stem = GameObject.Find("ind");
            Butt.fpp = GameObject.Find("firstp");
            Butt.bk = GameObject.Find("back");

			swi.AddComponent<Butt>();
            swis.AddComponent<StreamButt>();
        }
    }
}
