using System.Collections;
using System.IO;
using UnityEngine;

public class LoadExternalAssetBundle : MonoBehaviour
{
    public Material vertexmaterial;

    private GameObject visualiser;

    public string stageName;
    public int startNumber;
    public int endNumber;
    public int stageNumber;

    IEnumerator Start()
    {
        visualiser = FindAnyObjectByType<FBXVisualizer_AllInOne>().gameObject;

        StartCoroutine(InstantiateObject());

        string bundlePath = Path.Combine(Application.dataPath, "/AssetBundles/mybundle");
        Debug.Log("Loading AssetBundle from: " + Application.dataPath);
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return bundleRequest;

        AssetBundle myLoadedAssetBundle = bundleRequest.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            yield break;
        }

        AssetBundleRequest assetRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("CubePrefab");
        yield return assetRequest;

        GameObject model = assetRequest.asset as GameObject;
        if (model != null)
        {
            Instantiate(model);
        }
        else
        {
            Debug.LogError("Failed to load asset from AssetBundle!");
        }

        
    }

    IEnumerator InstantiateObject()
    {
        Debug.Log("Second Method");
        string url = "file:///D:/Jonas/Unity/AssetBundle/FBX/fbxmodelle";
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.Send();
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        GameObject cube = bundle.LoadAsset<GameObject>("Stage 1_2");
        // GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        GameObject test = Instantiate(cube);
        test.GetComponent<MeshRenderer>().material = vertexmaterial;
        // Instantiate(sprite);
    }

    IEnumerator InstantiateObjects(string stageName_, int startValue_, int endValue_, int stageNumber_)
    {
        Debug.Log("Second Method");
        GameObject heartObject = null;

        string url = Path.Combine(Application.persistentDataPath, stageName_);   // Damit das funktioniert müssen die AssetBundles vorher auf die Brille geladen werden
        var request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(stageName_, 0);

#if UNITY_EDITOR
        url = Path.Combine(Application.dataPath, "AssetBundles",  stageName_);
        request
            = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(stageName_, 0);
#endif
        yield return request.Send();

        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        for(int i = startValue_; i < endValue_; i++)
        {
            string fbxName = "Stage " + stageNumber_ + "_" + i;
            heartObject = bundle.LoadAsset<GameObject>(fbxName);
            GameObject tmp = Instantiate(heartObject, visualiser.transform);
        }
        
        // GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        GameObject test = Instantiate(heartObject);
        test.GetComponent<MeshRenderer>().material = vertexmaterial;
        // Instantiate(sprite);
    }
}
