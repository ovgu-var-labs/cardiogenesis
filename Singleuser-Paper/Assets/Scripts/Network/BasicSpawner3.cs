using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using TMPro;

public class BasicSpawner3 : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    public bool firstPress = false;
    public bool loadAsHost;
    public TextMeshPro textMesh;

    public TextMeshPro textMeshNumberPlayers;

    public Transform otherHighlight;

    public Color modColor;
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    //public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    //public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    // Methods which are necessary for photon, but are not used here
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    // Gets called to start game
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    /// <summary>
    /// Gets called on host when player joins the room
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            float tmp = UnityEngine.Random.Range(0.5f, 5f);
            // Create a unique position for the player
            Debug.Log("Spawning..");
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, new Vector3(0, tmp, 0), Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);


            // string textTMP = textMesh.text;
            // textMesh.text = runner.GameMode + " " + _spawnedCharacters.Count;
            textMeshNumberPlayers.text = _spawnedCharacters.Count.ToString();
            // Change color object on player objects
            switch (_spawnedCharacters.Count)
            {
                case 1:
                    Debug.Log("color 1");
                    networkPlayerObject.GetComponent<PlayerStorage>().netPukColor = modColor;
                    // networkPlayerObject.GetComponent<PlayerStorage>().netIsModerator = true;
                    break;
                case 2:
                    networkPlayerObject.GetComponent<PlayerStorage>().netPukColor = color1;
                    // networkPlayerObject.GetComponent<PlayerStorage>().netIsModerator = false;
                    break;
                case 3:
                    networkPlayerObject.GetComponent<PlayerStorage>().netPukColor = color2;
                    // networkPlayerObject.GetComponent<PlayerStorage>().netIsModerator = false;
                    break;
                case 4:
                    networkPlayerObject.GetComponent<PlayerStorage>().netPukColor = color3;
                    // networkPlayerObject.GetComponent<PlayerStorage>().netIsModerator = false;
                    break;
                default:
                    networkPlayerObject.GetComponent<PlayerStorage>().netPukColor = color4;
                    // networkPlayerObject.GetComponent<PlayerStorage>().netIsModerator = false;
                    break;
            }
            Debug.Log("Spawned");
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);

            textMeshNumberPlayers.text = _spawnedCharacters.Count.ToString();
        }
    }

    /// <summary>
    /// Used for dektop visualisation
    /// </summary>
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    
    private void Update()
    {
        // Currently used to decide if you want to be host or client
        if (firstPress == false)
        {
            bool AButton = OVRInput.Get(OVRInput.Button.One);
            bool BButton = OVRInput.Get(OVRInput.Button.Two);
            if (AButton)
            {
                StartGame(GameMode.Host);
                textMesh.text = "Host";
                firstPress = true;
                return;
            }
            if (BButton)
            {
                StartGame(GameMode.Client);
                textMesh.text = "Client";
                firstPress = true;
                var Buttons = FindAnyObjectByType<ButtonFunctionality>().gameObject;
                Buttons.transform.localScale = Vector3.zero;
                return;
            }
        }
        
    }

    private void Start()
    {
        //textMesh = Camera.main.GetComponentInChildren<TextMeshPro>();
        //textMesh.text = "A: Host\nB: Client";
        //textMeshNumberPlayers = GameObject.FindGameObjectWithTag("TMPNumberPlayers").GetComponent<TextMeshPro>();
        otherHighlight = GameObject.FindGameObjectWithTag("OtherHighlight").transform;
        loadAsHost = StaticClass.isHost;
        Debug.Log("Host:" + loadAsHost);
        if(loadAsHost)
        {
            StartGame(GameMode.Host);
            firstPress = true;
            textMeshNumberPlayers.text = "Connecting...";
        }
        else
        {
            Debug.Log("Start client");
            StartGame(GameMode.Client);
            firstPress = true;
            var Buttons = FindAnyObjectByType<ButtonFunctionality>().gameObject;
            Buttons.transform.localScale = Vector3.zero;
        }
        /*
        Debug.Log("Starting gamw");
        StartGame(GameMode.Client);
        textMesh.text = "Host";
        firstPress = true;
        */
    }
}