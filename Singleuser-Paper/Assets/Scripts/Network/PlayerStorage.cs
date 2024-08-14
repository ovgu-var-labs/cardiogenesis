using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using TMPro;
/// <summary>
/// Is on the network player objects
/// </summary>
public class PlayerStorage : NetworkBehaviour
{

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public int netActiveStage { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public float netProgress { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public bool netAnnotationsEnabled { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public bool netMarkingIsLocked { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public bool netRotatingIsLocked { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public float netAngleCameraHeart { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public float netRotationAroundX { get; set; }


    [Networked, /*OnChangedRender(nameof(ChangeNumberPlayers))*/]
    public int netPlayersIngame { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public Vector3 netLocalPosition { get; set; }

    [Networked, OnChangedRender(nameof(ChangeTMP))]
    public Vector3 netLocalRotation { get; set; }

    [Networked]
    public bool netIsOnObject { get; set; }

    [Networked]
    public Color netPukColor { get; set; }

    [Networked]
    public bool netIsModerator { get; set; }

    [Networked, Capacity(10)]
    public NetworkArray<Vector3> netPositonArray { get; } = MakeInitializer(new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero });

    [Networked, Capacity(10)]
    public NetworkArray<Vector3> netRotationArray { get; } = MakeInitializer(new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero });

    public GameObject pukPrefab;

    public List<GameObject> otherPuks;

    public Transform otherHighligt;

    public GameObject[] networkPlayers;

    public StageManager stageManager;

    public NetworkRunner networkRunner;

    public MudRendererCommunication communication;

    public GameObject localPuk;
    public T5Wand t5Wand;

    public TextMeshPro textMeshPro;

    public Vector3[] rotationList;
    public Vector3[] positionList;

    // Start is called before the first frame update
    void Start()
    {
        if (!stageManager)
        {
            stageManager = FindAnyObjectByType<StageManager>();
        }
        if (!networkRunner)
        {
            networkRunner = FindAnyObjectByType<NetworkRunner>();
        }
        communication = FindAnyObjectByType<MudRendererCommunication>();
        if (Object.HasInputAuthority)
        {
            localPuk = GameObject.FindGameObjectWithTag("Puk");
            
        }
        textMeshPro = GetComponentInChildren<TextMeshPro>();

        // this.transform.parent = GameObject.FindGameObjectWithTag("OtherHighlight").transform;
    }

    // Update is called once per frame
    void Update()
    {

        // Executed by everyone

        //Check if important scripts are connected
        if (!otherHighligt)
            otherHighligt = GameObject.FindGameObjectWithTag("OtherHighlight").transform;
        if(!t5Wand)
            t5Wand = FindAnyObjectByType<T5Wand>();
        

        // Debug.Log("MP.");
        // Check if object has input authority -> Executed only on user which owns the network object
        if (Object.HasInputAuthority == false)
        {
            return;
        }

        // Debug.Log("MP..");

        DebugToTextField();

        RPC_SendPosVector(localPuk.transform.localPosition);
        RPC_SendRotVector(localPuk.transform.localEulerAngles);
        RPC_SendNetIsOnObject(localPuk.GetComponent<PukNotationScript>().isOnObject);

        this.gameObject.transform.position = localPuk.transform.position;

        if (Object.StateAuthority == Runner.LocalPlayer || networkRunner.IsServer)
        {
            // Is executed by host

            netPositonArray.Set(0, localPuk.transform.localPosition);
            netRotationArray.Set(0, localPuk.transform.localEulerAngles);

            var buttons = FindAnyObjectByType<ButtonFunctionality>();
            networkPlayers = GameObject.FindGameObjectsWithTag("NetworkPlayer");

            foreach(GameObject player in networkPlayers)
            {
                if(player.TryGetComponent<PlayerStorage>(out var playerStorage))
                {
                    
                    playerStorage.RPC_Change_netProgress(communication.animationProgress);
                    playerStorage.RPC_Change_netAnnotationsEnabled(stageManager.GetAnnontationsEnabled());
                    playerStorage.RPC_Change_netAngleCameraHeart(communication.angleCameraHeart);
                    playerStorage.RPC_Change_netRotationAroundX(communication.rotationAroundX);
                    playerStorage.RPC_Change_netMarkingIsLocked(communication.markingIsLocked);
                    playerStorage.RPC_Change_netRotatingIsLocked(communication.rotationIsLocked);
                    playerStorage.RPC_Change_netActiveStage(stageManager.currentStage);
                    // playerStorage.RPC_Change_PukPositionAndRotation();
                    // playerStorage.RPC_Change_netArrays(netPositonArray.ToArray(), netRotationArray.ToArray());
                    playerStorage.RPC_Change_netPlayersIngame(networkPlayers.Length);
                }
            }
        }
        else
        {
            // Is executedd by client
        }

        // Debug.Log("MP..." + networkRunner.IsServer.ToString());
    }

    /// <summary>
    /// Debug field
    /// </summary>
    public void DebugToTextField()
    {
        textMeshPro.text = "Progress " + netProgress + "\n" +
            "Players " + netPlayersIngame + "\n" +
            "Annotations " + netAnnotationsEnabled + "\n" +
            "Forced Perspective " + netRotatingIsLocked + " Angle " + netAngleCameraHeart + "\n";
    }

    /// <summary>
    /// Gets called as soon as values which use this function are changed
    /// </summary>
    public void ChangeTMP()
    {

        // TODO stage wechsel einbauen

        Debug.Log("Changed Something");
        stageManager.SetAnimationProgress(netProgress);
        stageManager.SetAnnotationsEnabled(netAnnotationsEnabled);

        communication.animationProgress = netProgress;
        communication.annotationsEnabled = netAnnotationsEnabled;
        communication.markingIsLocked = netMarkingIsLocked;
        
        if(stageManager.currentStage != netActiveStage)
        {
            // Müssen stage wechseln
            stageManager.SetLoadStage(netActiveStage);
        }

        if(!networkRunner.IsServer)
        {
            if (netRotatingIsLocked)
            {
                communication.NetForceRotateHeartNew(netAngleCameraHeart, netRotationAroundX, communication.ModeratorMudRender);
            }
            
        }
    }

    public void ChangeNumberPlayers()
    {
        Debug.Log("Change Number Players()");
        // Add or delete puk prefabs under other highlight
        if(otherPuks.Count < netPlayersIngame)
        {
            // Add puk
            GameObject tmp = Instantiate(pukPrefab, otherHighligt);
            tmp.transform.tag = "Untagged";
            otherPuks.Add(tmp);
        }
        else
        {
            // Delete puk
            Destroy(otherPuks[otherPuks.Count - 1]);
            otherPuks.RemoveAt(otherPuks.Count - 1);
        }

    }

    public void ChangePosAndRotPuks()
    {
        while(otherPuks.Count != netPlayersIngame)
        {
            ChangeNumberPlayers();
        }
        for(int i = 0; i < netPlayersIngame; i++)
        {
            GameObject tmp = otherPuks[i];
            tmp.transform.localPosition = netPositonArray[i];
            tmp.transform.localEulerAngles = netRotationArray[i];
        }
    }

    /// <summary>
    /// Sets the progress value on this network object
    /// </summary>
    /// <param name="input">new progress value</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netProgress(float input)
    {
        // Debug.Log("Funktion aufgerufen");
        netProgress = input;
    }

    /// <summary>
    /// Sets the active stage on this network object
    /// </summary>
    /// <param name="input">new active stage</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netActiveStage(int input)
    {
        // Debug.Log("Funktion aufgerufen");
        netActiveStage = input;
    }

    /// <summary>
    /// Sets the bool value if noations are enabled on the network object
    /// </summary>
    /// <param name="input">if notations are enabled</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netAnnotationsEnabled(bool input)
    {
        if (Object.StateAuthority == Runner.LocalPlayer)
            return;
        // Debug.Log("Funktion aufgerufen");
        netAnnotationsEnabled = input;
    }

    /// <summary>
    /// Sets if marking is locked on this network object
    /// </summary>
    /// <param name="input">if marking is locked</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netMarkingIsLocked(bool input)
    {
        if (Object.StateAuthority == Runner.LocalPlayer)
            return;
        // Debug.Log("Funktion aufgerufen");
        netMarkingIsLocked = input;
    }

    /// <summary>
    /// Sets if rotating is locked on this network object
    /// </summary>
    /// <param name="input">if rotating is locked</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netRotatingIsLocked(bool input)
    {
        // Debug.Log("Funktion aufgerufen");
        netRotatingIsLocked = input;
    }

    /// <summary>
    /// Sets the angle between the heart and the camera
    /// </summary>
    /// <param name="input">new angle</param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netAngleCameraHeart(float input)
    {
        // Debug.Log("Funktion aufgerufen");
        netAngleCameraHeart = input;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netRotationAroundX(float input)
    {
        // Debug.Log("Funktion aufgerufen");
        netRotationAroundX = input;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netPlayersIngame(int input)
    {
        // Debug.Log("Funktion aufgerufen");
        netPlayersIngame = input;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_netArrays(Vector3 [] posArray, Vector3[] rotArray)
    {
        for(int i = 0; i < posArray.Length; i++)
        {
            netPositonArray.Set(i, posArray[i]);
            netRotationArray.Set(i, rotArray[i]);

        }
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Change_PukPositionAndRotation()
    {
        netLocalPosition = localPuk.transform.localPosition;
        netLocalRotation = localPuk.transform.localEulerAngles;
    }
    
    [Rpc(RpcSources.All, RpcTargets.StateAuthority , HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendPositionToHost(Vector3 vector3Pos, RpcInfo info = default)
    {
        // Host should input this knowledge into the netArray Position
        Debug.Log("MP Sending info from " + info.Source.AsIndex);

        netPositonArray.Set(info.Source.AsIndex - 1, vector3Pos);
        RPC_RelayPositionToOtherPlayes(vector3Pos, info.Source.AsIndex - 1);
        RPC_ChangePosLocally(vector3Pos, info.Source);
        // RPC_RelayPosition(vector3Pos, info.Source);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendRotationToHost(Vector3 vector3Rot, RpcInfo info = default)
    {
        // Host should input this knowledge into the netArray Rotation
        netRotationArray.Set(info.Source.AsIndex - 1, vector3Rot);
        RPC_RelayRotationToOtherPlayes(vector3Rot, info.Source.AsIndex - 1);
        RPC_ChangeRotLocally(vector3Rot, info.Source);
        // RPC_RelayRotation(vector3Rot, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayPositionToOtherPlayes(Vector3 vector3Rot, int playerNumber)
    {
        netPositonArray.Set(playerNumber, vector3Rot);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayRotationToOtherPlayes(Vector3 vector3Rot, int playerNumber)
    {
        netRotationArray.Set(playerNumber, vector3Rot);
    }




    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_ChangePosLocally(Vector3 pos, PlayerRef posSource)
    {
        if(posSource == Runner.LocalPlayer)
        {
            netLocalPosition = pos;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_ChangeRotLocally(Vector3 rot, PlayerRef posSource)
    {
        if (posSource == Runner.LocalPlayer)
        {
            netLocalRotation = rot;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SendPosVector(Vector3 vector3Pos, RpcInfo info = default)
    {
        // Debug.Log("Vector Sending info from " + (info.Source.AsIndex - 1) + " " + vector3Pos);
        int placeInArray = info.Source.AsIndex - 1;
        if (placeInArray < 0)
            placeInArray = 0;
        netLocalPosition = vector3Pos;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SendRotVector(Vector3 vector3Pos, RpcInfo info = default)
    {
        // Debug.Log("Vector Sending info from " + (info.Source.AsIndex - 1) + " " + vector3Pos);
        int placeInArray = info.Source.AsIndex - 1;
        if (placeInArray < 0)
            placeInArray = 0;
        netLocalRotation = vector3Pos;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SendNetIsOnObject(bool input)
    {
        netIsOnObject = input;
    }
    /*
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayRotation(Vector3 vector3Rot, PlayerRef messageSource)
    {
        if (messageSource == Runner.LocalPlayer)
        {
            netLocalRotation = vector3Rot;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayPosition(Vector3 vector3Pos, PlayerRef messageSource)
    {
        if (messageSource == Runner.LocalPlayer)
        {
            netLocalPosition = vector3Pos;
        }
    }
    */
}
