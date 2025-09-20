using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace ChonkerRaces
{
    public class RaceManager : MonoBehaviour
    {
        [Header("Race Settings")]
        [SerializeField] private int totalLaps = 3;
        [SerializeField] private float countdownTime = 3f;
        [SerializeField] private bool raceStarted = false;
        [SerializeField] private bool raceFinished = false;
        
        [Header("UI References")]
        [SerializeField] private Text countdownText;
        [SerializeField] private Text lapCounterText;
        [SerializeField] private Text positionText;
        [SerializeField] private Text speedText;
        [SerializeField] private GameObject raceStartUI;
        [SerializeField] private GameObject raceFinishUI;
        [SerializeField] private Text raceResultText;
        
        [Header("Race Track")]
        [SerializeField] private Transform[] checkpoints;
        [SerializeField] private Transform finishLine;
        
        private List<CarController> cars = new List<CarController>();
        private Dictionary<CarController, RaceData> raceData = new Dictionary<CarController, RaceData>();
        private CarController playerCar;
        private float raceStartTime;
        private float countdownTimer;
        private bool countdownActive = false;
        
        private class RaceData
        {
            public int currentLap = 0;
            public int currentCheckpoint = 0;
            public float raceTime = 0f;
            public int position = 1;
            public bool finished = false;
        }
        
        public static RaceManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InitializeRace();
            StartCountdown();
        }
        
        private void Update()
        {
            if (countdownActive)
            {
                UpdateCountdown();
            }
            else if (raceStarted && !raceFinished)
            {
                UpdateRace();
            }
        }
        
        private void InitializeRace()
        {
            // Find all cars in the scene
            CarController[] foundCars = FindObjectsOfType<CarController>();
            cars.AddRange(foundCars);
            
            // Find player car (you might want to set this differently based on your multiplayer setup)
            if (cars.Count > 0)
            {
                playerCar = cars[0]; // For now, assume first car is player
            }
            
            // Initialize race data for each car
            foreach (var car in cars)
            {
                raceData[car] = new RaceData();
            }
            
            // Setup UI
            if (lapCounterText != null)
            {
                lapCounterText.text = $"Lap: 0/{totalLaps}";
            }
            
            if (raceStartUI != null)
            {
                raceStartUI.SetActive(true);
            }
            
            if (raceFinishUI != null)
            {
                raceFinishUI.SetActive(false);
            }
        }
        
        private void StartCountdown()
        {
            countdownActive = true;
            countdownTimer = countdownTime;
            
            // Disable car controls during countdown
            foreach (var car in cars)
            {
                car.enabled = false;
            }
        }
        
        private void UpdateCountdown()
        {
            countdownTimer -= Time.deltaTime;
            
            if (countdownText != null)
            {
                if (countdownTimer > 0)
                {
                    countdownText.text = Mathf.Ceil(countdownTimer).ToString();
                }
                else
                {
                    countdownText.text = "GO!";
                }
            }
            
            if (countdownTimer <= 0)
            {
                StartRace();
            }
        }
        
        private void StartRace()
        {
            countdownActive = false;
            raceStarted = true;
            raceStartTime = Time.time;
            
            // Enable car controls
            foreach (var car in cars)
            {
                car.enabled = true;
            }
            
            // Hide countdown UI
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
            }
            
            if (raceStartUI != null)
            {
                raceStartUI.SetActive(false);
            }
        }
        
        private void UpdateRace()
        {
            // Update race data for each car
            foreach (var car in cars)
            {
                if (raceData.ContainsKey(car) && !raceData[car].finished)
                {
                    raceData[car].raceTime = Time.time - raceStartTime;
                }
            }
            
            // Update positions
            UpdatePositions();
            
            // Update UI
            UpdateUI();
            
            // Check for race finish
            CheckRaceFinish();
        }
        
        private void UpdatePositions()
        {
            // Sort cars by race progress
            var sortedCars = cars.OrderByDescending(car => 
            {
                var data = raceData[car];
                return data.currentLap * 1000 + data.currentCheckpoint;
            }).ToList();
            
            // Update positions
            for (int i = 0; i < sortedCars.Count; i++)
            {
                raceData[sortedCars[i]].position = i + 1;
            }
        }
        
        private void UpdateUI()
        {
            if (playerCar != null && raceData.ContainsKey(playerCar))
            {
                var data = raceData[playerCar];
                
                // Update lap counter
                if (lapCounterText != null)
                {
                    lapCounterText.text = $"Lap: {data.currentLap}/{totalLaps}";
                }
                
                // Update position
                if (positionText != null)
                {
                    string positionSuffix = GetPositionSuffix(data.position);
                    positionText.text = $"{data.position}{positionSuffix}";
                }
                
                // Update speed
                if (speedText != null)
                {
                    speedText.text = $"{playerCar.Speed:F0} km/h";
                }
            }
        }
        
        private string GetPositionSuffix(int position)
        {
            switch (position)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return "th";
            }
        }
        
        private void CheckRaceFinish()
        {
            bool allFinished = true;
            
            foreach (var car in cars)
            {
                if (raceData.ContainsKey(car) && !raceData[car].finished)
                {
                    if (raceData[car].currentLap >= totalLaps)
                    {
                        raceData[car].finished = true;
                    }
                    else
                    {
                        allFinished = false;
                    }
                }
            }
            
            if (allFinished && !raceFinished)
            {
                FinishRace();
            }
        }
        
        private void FinishRace()
        {
            raceFinished = true;
            
            // Disable car controls
            foreach (var car in cars)
            {
                car.enabled = false;
            }
            
            // Show race results
            ShowRaceResults();
        }
        
        private void ShowRaceResults()
        {
            if (raceFinishUI != null)
            {
                raceFinishUI.SetActive(true);
            }
            
            if (raceResultText != null)
            {
                string results = "Race Results:\n\n";
                
                var sortedResults = raceData.OrderBy(kvp => kvp.Value.position).ToList();
                
                for (int i = 0; i < sortedResults.Count; i++)
                {
                    var car = sortedResults[i].Key;
                    var data = sortedResults[i].Value;
                    string positionSuffix = GetPositionSuffix(data.position);
                    
                    results += $"{data.position}{positionSuffix} - {car.name} - {FormatTime(data.raceTime)}\n";
                }
                
                raceResultText.text = results;
            }
        }
        
        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time % 1) * 100);
            
            return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        }
        
        public void OnCheckpointPassed(CarController car, int checkpointIndex)
        {
            if (raceData.ContainsKey(car))
            {
                var data = raceData[car];
                
                if (checkpointIndex == data.currentCheckpoint + 1)
                {
                    data.currentCheckpoint = checkpointIndex;
                    
                    // Check if lap is completed
                    if (checkpointIndex >= checkpoints.Length)
                    {
                        data.currentLap++;
                        data.currentCheckpoint = 0;
                        
                        Debug.Log($"{car.name} completed lap {data.currentLap}");
                    }
                }
            }
        }
        
        public void RestartRace()
        {
            // Reset all cars
            foreach (var car in cars)
            {
                car.ResetCar();
            }
            
            // Reset race data
            foreach (var data in raceData.Values)
            {
                data.currentLap = 0;
                data.currentCheckpoint = 0;
                data.raceTime = 0f;
                data.position = 1;
                data.finished = false;
            }
            
            // Reset race state
            raceStarted = false;
            raceFinished = false;
            countdownActive = false;
            
            // Hide finish UI
            if (raceFinishUI != null)
            {
                raceFinishUI.SetActive(false);
            }
            
            // Restart countdown
            StartCountdown();
        }
        
        // Public getters
        public bool IsRaceStarted => raceStarted;
        public bool IsRaceFinished => raceFinished;
        public int GetPlayerPosition()
        {
            if (playerCar != null && raceData.ContainsKey(playerCar))
            {
                return raceData[playerCar].position;
            }
            return 1;
        }
        
        public int GetPlayerLap()
        {
            if (playerCar != null && raceData.ContainsKey(playerCar))
            {
                return raceData[playerCar].currentLap;
            }
            return 0;
        }
    }
}
