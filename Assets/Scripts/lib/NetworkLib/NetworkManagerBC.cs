using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
public class NetworkManagerBC : NetworkManager
{

    [SerializeField] private int minPlayers = 2;
    [Scene] [SerializeField] private string menuScene = string.Empty;

    [Header("Maps")]
    [SerializeField] private int numberOfRounds = 1;
    [SerializeField] private MapSet mapSet = null;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerBC roomPlayerPrefab = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerBC gamePlayerPrefab = null;
    [SerializeField] private GameObject playerSpawnSystem = null;
    [SerializeField] private GameObject roundSystem = null;

    private MapHandler mapHandler;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<NetworkRoomPlayerBC> RoomPlayers { get; } = new List<NetworkRoomPlayerBC>();
    public List<NetworkGamePlayerBC> GamePlayers { get; } = new List<NetworkGamePlayerBC>();

    public override void OnStartServer()
    {

        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            ClientScene.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if (SceneManager.GetActiveScene().path != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0; // se for o primeiro player  a entrar na sala define ele como lider

            NetworkRoomPlayerBC roomPlayerInstance = Instantiate(roomPlayerPrefab); //Instancia o player da sala

            roomPlayerInstance.IsLeader = isLeader; // Passa o status de lider para o player instanciado

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject); // Adiciona o player instanciado para conexão
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerBC>();//Pega o componente da sala

            RoomPlayers.Remove(player);// Kicka o player da sala

            NotifyPlayersOfReadyState();//Notifica para os players o atual status
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState() // Notifica se todos os players estão prontos
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()// Notifica se esta pronto para começar
    {
        if (numPlayers < minPlayers) { return false; }

        foreach (var player in RoomPlayers)
        {
            if (!player.IsReady) { return false; }
        }

        return true;
    }

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().path == menuScene)
        {//Verifica se esta na scene de menu
            if (!IsReadyToStart()) { return; } // Verifica se esta pronto para começar

            mapHandler = new MapHandler(mapSet, numberOfRounds); // Pega um novo mapa

            ServerChangeScene(mapHandler.NextMap);//Altera a scene para o novo mapa
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        Debug.Log(newSceneName);
        // From menu to game
        if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Assets/Scenes/Scene_Map"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;//Busca todos player conectados
                var gameplayerInstance = Instantiate(gamePlayerPrefab);//Instancia os players
                gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);//Determina o nome dos players

                NetworkServer.Destroy(conn.identity.gameObject);//Destroy os players antigos

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);//Cria no novo mapa os players.
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("Assets/Scenes/Scene_Map"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);

            GameObject roundSystemInstance = Instantiate(roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}
