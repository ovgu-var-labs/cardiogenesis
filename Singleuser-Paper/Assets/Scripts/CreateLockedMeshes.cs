using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class CreateLockedMeshes : MonoBehaviour
{
    public storageArt storage;
    [ConditionalEnumField("storage", (int)storageArt.FBX)]
    public GameObject prefab;
    public GameObject meshes;
    public Material material;
    [ConditionalEnumField("storage", (int)storageArt.Addressables)]
    public AssetReference assetReference;
    [ConditionalEnumField("storage", (int)storageArt.Addressables)]
    public AssetLabelReference assetLabelReference;
    AsyncOperationHandle<GameObject> opHandle;
    [ConditionalEnumField("storage", (int)storageArt.AssetBundles)]
    public string stageName;
    [ConditionalEnumField("storage", (int)storageArt.AssetBundles)]
    public int startNumber;
    [ConditionalEnumField("storage", (int)storageArt.AssetBundles)]
    public int endNumber;
    [ConditionalEnumField("storage", (int)storageArt.AssetBundles)]
    public int stageNumber;

    [Tooltip("Object which contains Mesh or FBX visualiser script")]
    public GameObject visualiser;
    private AssetBundle myLoadedAssetBundle;
    [Tooltip("Resets transform component from created heart objects")]
    [SerializeField] private bool resetTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        visualiser = FindAnyObjectByType<FBXVisualizer_AllInOne>().gameObject;
        if (visualiser == null)
        {
            Debug.LogError("No visualiser assigned");
        }

        if (storage == storageArt.Mesh)
            meshes = Instantiate(prefab);
        else if (storage == storageArt.FBX)
        {
            Debug.Log("Create fbx stuff");
            foreach (GameObject test in prefab.GetComponent<LockedFBXStorage>().fbxStorage)
            {
                var tmp = Instantiate(test, visualiser.transform);
                tmp.GetComponent<MeshRenderer>().material = material;
                tmp.SetActive(false);
                if (resetTransform)
                {
                    tmp.transform.localPosition = Vector3.zero;
                    tmp.transform.localEulerAngles = Vector3.zero;
                    tmp.transform.localScale = Vector3.one;
                }
                
            }
        }
        else if (storage == storageArt.Addressables)
        {
            // Load über Adressables 
            LoadAdressableByLabel();
            //GameObject lockedGameObject = LoadAdressable();


        }
        else if (storage == storageArt.AssetBundles)
        {
            StartCoroutine(InstantiateObjectsBundleMethod(stageName, startNumber, endNumber, stageNumber));
        }


        Resources.UnloadUnusedAssets();
    }

    private void OnDisable()
    {
        if (storage == storageArt.Addressables)
        {
            Addressables.Release(opHandle);
            while (visualiser.transform.childCount > 0)
            {
                Destroy(visualiser.transform.GetChild(0));
            }
        }
        else if (storage == storageArt.AssetBundles)
        {
            myLoadedAssetBundle.Unload(false);
        }
        else
        {
            Destroy(meshes);
        }
        DestroyAllChildren(visualiser.transform);
    }

    private GameObject LoadAdressable()
    {
        GameObject lockedGameObject = null;

        assetReference.LoadAssetAsync<GameObject>().Completed +=
            (asyncOperationHandle) =>
            {
                if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    lockedGameObject = asyncOperationHandle.Result;
                }
                else
                {
                    Debug.LogError("Failed to Load Asset");
                }
            };
        return lockedGameObject;
    }

    private GameObject LoadAdressableByLabel()
    {
        GameObject lockedGameObject = null;

        Addressables.LoadAssetsAsync<GameObject>(assetLabelReference, null).Completed +=
            (opHandle) =>
            {
                if (opHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (GameObject fbxModell in opHandle.Result)
                    {
                        Debug.Log(fbxModell.name + "Loaded");
                        GameObject tmp = Instantiate(fbxModell, visualiser.transform);
                        tmp.GetComponent<MeshRenderer>().material = material;
                        tmp.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError("Failed to Load Asset");
                }
            };

        return lockedGameObject;
    }

    IEnumerator InstantiateObjects(string stageName_, int startValue_, int endValue_, int stageNumber_)
    {
        Debug.Log("Second Method");
        GameObject heartObject = null;

        string url = Path.Combine(Application.persistentDataPath, stageName_);   // Damit das funktioniert müssen die AssetBundles vorher auf die Brille geladen werden
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(stageName_, 0);
        Debug.Log("Second Method Adnroid Path");
#if UNITY_EDITOR
        url = Path.Combine(Application.dataPath, "AssetBundles", stageName_);
        request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(stageName_, 0);
        Debug.Log("Second Method Editor Path");
#endif
        yield return request.Send();
        Debug.Log("Second Method Sind schon weiter");
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        for (int i = startValue_; i < endValue_; i++)
        {
            string fbxName = "Stage " + stageNumber_ + "_" + i;
            heartObject = bundle.LoadAsset<GameObject>(fbxName);
            GameObject tmp = Instantiate(heartObject, visualiser.transform);
        }

        // GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        GameObject test = Instantiate(heartObject);
        test.GetComponent<MeshRenderer>().material = material;
        // Instantiate(sprite);
    }

    IEnumerator InstantiateObjectsBundleMethod(string stageName_, int startValue_, int endValue_, int stageNumber_)
    {
        Debug.Log("Second Method");
        GameObject heartObject = null;

        string url = Path.Combine(Application.persistentDataPath, stageName_);   // Damit das funktioniert müssen die AssetBundles vorher auf die Brille geladen werden, dafür online gucken wo der Pfad hinführt
        Debug.Log("Second Method Adnroid Path");
#if UNITY_EDITOR
        url = Path.Combine(Application.dataPath, "AssetBundles", stageName_);
        Debug.Log("Second Method Editor Path");
#endif
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(url);
        yield return bundleRequest;
        Debug.Log("Second Method loaded budnle");
        myLoadedAssetBundle = bundleRequest.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            yield break;
        }

        Debug.Log("Second Method Sind schon weiter");

        AssetBundleRequest assetMaterialRequest = myLoadedAssetBundle.LoadAssetAsync<Material>("VertexColorMaterial");
        yield return assetMaterialRequest;
        material = assetMaterialRequest.asset as Material;


        for (int i = startValue_; i < endValue_; i++)
        {
            string fbxName = "Stage " + stageNumber_ + "_" + i;
            AssetBundleRequest assetRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(fbxName);
            yield return assetRequest;
            Debug.Log("Second Method loaded: " + fbxName);
            GameObject model = assetRequest.asset as GameObject;
            GameObject tmp = Instantiate(model, visualiser.transform);
            tmp.GetComponent<MeshRenderer>().material = material;
            tmp.SetActive(false);
        }

        // GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        // GameObject test = Instantiate(heartObject);
        // test.GetComponent<MeshRenderer>().material = material;
        // Instantiate(sprite);
    }

    private void DestroyAllChildren(Transform transform_)
    {
        foreach (Transform child in transform_)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    /*
    private void SortChildrenByName(GameObject gameObject_)
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            List<Transform> children = new List<Transform>();
            for (int i = obj.transform.childCount - 1; i >= 0; i--)
            {
                Transform child = obj.transform.GetChild(i);
                children.Add(child);
                child.parent = null;
            }
            children.Sort((Transform t1, Transform t2) => { return t1.name.CompareTo(t2.name); });
            foreach (Transform child in children)
            {
                child.parent = obj.transform;
            }
        }
    }*/
}


