using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the visualisation and positioning of the pucks on the client side
/// </summary>
public class NetPukControllerScript : MonoBehaviour
{
    [Tooltip("Prefab of the Puk object, which is local ")]
    public GameObject netPukPrefab;

    [Tooltip("List containing puks this script controls")]
    public List<GameObject> pukList;

    [Tooltip("List containing references network objects from players")]
    public GameObject[] networkPlayers;

    [Tooltip("Host network object")]
    public GameObject hostPlayer;

    [Tooltip("Transform where puks should be spawned")]
    public Transform otherHighlightTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        networkPlayers = GameObject.FindGameObjectsWithTag("NetworkPlayer");

        if (networkPlayers.Length != pukList.Count)
            AdjustpukList(networkPlayers.Length);

        MovePuksToPosition();
    }

    /// <summary>
    /// Changes the puk list, including order
    /// </summary>
    /// <param name="newLength">new length of the list</param>
    public void AdjustpukList(int newLength)
    {
        // networkPlayers = SortNetworkList(networkPlayers);
        while (pukList.Count != newLength)
        {
            if(pukList.Count > newLength)
            {
                // remove Puk
                Destroy(pukList[pukList.Count - 1]);
                pukList.RemoveAt(pukList.Count - 1);
            }
            else
            {
                // Add Puk
                GameObject tmp = Instantiate(netPukPrefab, otherHighlightTransform);
                pukList.Add(tmp);
                
            }
        }
        for(int i = 0; i < networkPlayers.Length; i++)
        {
            pukList[i].GetComponent<PukNotationScript>().SetPukColor(networkPlayers[i].GetComponent<PlayerStorage>().netPukColor);
        }
    }

    /// <summary>
    /// Finds host network object in list of network object
    /// </summary>
    /// <param name="networkPlayers_"></param>
    /// <returns></returns>
    public GameObject FindHostInNetworkPlayers(GameObject[] networkPlayers_)
    {
        foreach(GameObject player in networkPlayers_)
        {
            if (player.GetComponent<NetworkObject>().HasStateAuthority)
                return player;
        }
        Debug.LogError("Did not find host");
        return networkPlayers[0];
    }

    public void MovePuksToPosition()
    {
        for(int i = 0; i < networkPlayers.Length; i++)
        {
            PlayerStorage playerStorage = networkPlayers[i].GetComponent<PlayerStorage>();
            Debug.Log("Player " + playerStorage.netLocalPosition + "; " + playerStorage.netLocalRotation);
            pukList[i].transform.localPosition = playerStorage.netLocalPosition;
            pukList[i].transform.localEulerAngles = playerStorage.netLocalRotation;
            pukList[i].GetComponent<PukNotationScript>().netIsOnObject = playerStorage.netIsOnObject;
            
        }
    }

    public GameObject[] SortNetworkList(GameObject[] list_)
    {
        GameObject mod = null;
        foreach (GameObject player in list_)
        {
            if (player.GetComponent<NetworkObject>().HasStateAuthority)
                mod = player;
        }
        
        GameObject[] tmpArray = new GameObject[list_.Length];
        int c = 1;
        foreach(GameObject player in list_)
        {
            if (player == mod)
            {
                tmpArray[0] = mod;
            }
            else
            {
                tmpArray[c] = player;
                c++;
            }
        }
        return tmpArray;
    }
}
