using UnityEngine;
using Mirror;
using System.Collections.Generic;

namespace ChonkerRaces.Networking
{
    public class NetworkManager : Mirror.NetworkManager
    {
        [Header("Game Settings")]
        [SerializeField] private int maxPlayers = 8;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;
        
        [Header("Race Settings")]
        [SerializeField] private float raceStartDelay = 5f;
        [SerializeField] private int totalLaps = 3;
        
        private List<NetworkConnection> connectedPlayers = new List<NetworkConnection>();
        private bool raceStarted = false;
        
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("Server started - Ready for players to connect");
        }
        
        public override void OnServerConnect(NetworkConnection conn)
        {
            base.OnServerConnect(conn);
            
            if (connectedPlayers.Count >= maxPlayers)
            {
                Debug.Log($"Server full! Rejecting connection from {conn.address}");
                conn.Disconnect();
                return;
            }
            
            connectedPlayers.Add(conn);
            Debug.Log($"Player connected: {conn.address}. Total players: {connectedPlayers.Count}");
            
            // Start race when we have enough players
            if (connectedPlayers.Count >= 2 && !raceStarted)
            {
                StartCoroutine(StartRaceAfterDelay());
            }
        }
        
        public override void OnServerDisconnect(NetworkConnection conn)
        {
            base.OnServerDisconnect(conn);
            connectedPlayers.Remove(conn);
            Debug.Log($"Player disconnected: {conn.address}. Total players: {connectedPlayers.Count}");
        }
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // Find an available spawn point
            Transform spawnPoint = GetAvailableSpawnPoint();
            
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
            
            Debug.Log($"Player spawned at {spawnPoint.name}");
        }
        
        private Transform GetAvailableSpawnPoint()
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                return transform; // Fallback to network manager position
            }
            
            // Simple round-robin spawn point selection
            int spawnIndex = connectedPlayers.Count % spawnPoints.Length;
            return spawnPoints[spawnIndex];
        }
        
        private System.Collections.IEnumerator StartRaceAfterDelay()
        {
            raceStarted = true;
            Debug.Log($"Starting race in {raceStartDelay} seconds...");
            
            yield return new WaitForSeconds(raceStartDelay);
            
            // Notify all clients to start the race
            RpcStartRace();
        }
        
        [ClientRpc]
        private void RpcStartRace()
        {
            Debug.Log("Race starting!");
            
            // Find the race manager and start the race
            RaceManager raceManager = FindObjectOfType<RaceManager>();
            if (raceManager != null)
            {
                // The race manager will handle the countdown and race start
            }
        }
        
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            Debug.Log("Connected to server!");
        }
        
        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);
            Debug.Log("Disconnected from server!");
        }
        
        // Public methods for UI
        public void StartHost()
        {
            StartHost();
        }
        
        public void StartClient(string serverAddress)
        {
            networkAddress = serverAddress;
            StartClient();
        }
        
        public void StopHost()
        {
            StopHost();
        }
        
        public void StopClient()
        {
            StopClient();
        }
        
        // Getters
        public int ConnectedPlayerCount => connectedPlayers.Count;
        public int MaxPlayerCount => maxPlayers;
        public bool IsRaceStarted => raceStarted;
    }
}
