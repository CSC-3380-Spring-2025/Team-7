using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System; // Keep for Exception

public class MongoDBSaveLoadManager : MonoBehaviour
{
    // --- Existing Variables ---
    public string apiBaseUrl = "http://localhost:5007/api/GameData";
    public string currentPlayerId = "player123";

    [Header("Persistent References")]
    public Currency currencyScript;
    public ItemTrack itemTrackScript;

    // --- ADDED ---
    // Variable to hold the list of owned skins after loading
    private List<string> _lastLoadedOwnedSkins = null;
    // --- END ADDED ---

    // Singleton Pattern (Optional but useful for easy access)
    public static MongoDBSaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        // --- Singleton Implementation ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // --- End Singleton ---

        // Make this persist between scenes
        DontDestroyOnLoad(gameObject);
    }

   private PlayerData GetCurrentPlayerData()
    {
        PlayerData data = new PlayerData();
        data.playerId = this.currentPlayerId;

        // Get Currency
        if (currencyScript != null) { data.currency = currencyScript.coin; }
        else { Debug.LogWarning("Currency script ref not set."); data.currency = 0; }

        // --- MODIFIED Gacha Machine Logic ---
        // Try to get skins from the GachaMachine Singleton Instance if it exists
        if (GachaMachine.Instance != null)
        {
            data.ownedSkins = new List<string>(GachaMachine.Instance.mySkins);
        }
        else // Otherwise, create an empty list
        {
             data.ownedSkins = new List<string>();
             // Don't warn here, GachaMachine might just not be loaded yet, which is okay for saving
             // Debug.LogWarning("GachaMachine Instance not found during GetCurrentPlayerData.");
        }
        // --- END MODIFIED Gacha Machine Logic ---

        // Find PlayerSkinApplier in current scene
        PlayerSkinApplierFromSave skinApplier = GameObject.FindFirstObjectByType<PlayerSkinApplierFromSave>();
        if (skinApplier != null) { data.currentSkin = skinApplier.GetCurrentSkinName(); }
        else { data.currentSkin = "Default"; Debug.LogWarning("PlayerSkinApplierFromSave not found during GetCurrentPlayerData."); }

        // Get Items
        if (itemTrackScript != null)
        {
            data.ballCount = itemTrackScript.ball;
            data.biscuitCount = itemTrackScript.bisc;
            data.brushCount = itemTrackScript.brush;
        }
        else
        {
            Debug.LogWarning("ItemTrack script ref not set.");
            data.ballCount = 0; data.biscuitCount = 0; data.brushCount = 0;
        }

        data.lastUpdated = DateTime.UtcNow.ToString("o");
        Debug.Log($"Collected data: {JsonUtility.ToJson(data, true)}");
        return data;
    }

    private void ApplyLoadedData(PlayerData loadedData)
    {
        if (loadedData == null) { Debug.LogError("ApplyLoadedData received null data."); return; }

        Debug.Log($"Starting ApplyLoadedData for Player: {loadedData.playerId}");

        // Apply PlayerId
        this.currentPlayerId = loadedData.playerId;
        Debug.Log($" > Applied PlayerId: {this.currentPlayerId}");

        // Apply Currency
        if (currencyScript != null)
        {
            Debug.Log($" > Applying Currency: {loadedData.currency} (Current: {currencyScript.coin})");
            currencyScript.coin = loadedData.currency;
            Debug.Log($" > Currency script coin set to: {currencyScript.coin}");
            // Add currency UI update if needed: currencyScript.UpdateDisplay?.Invoke();
        }
        else { Debug.LogWarning(" > Currency script ref missing during ApplyLoadedData."); }


        // --- MODIFIED Gacha Machine Logic ---
        // Store the loaded owned skins list locally instead of applying directly
        if (loadedData.ownedSkins != null)
        {
            // Store a copy
            _lastLoadedOwnedSkins = new List<string>(loadedData.ownedSkins);
            Debug.Log($" > Stored {_lastLoadedOwnedSkins.Count} owned skins locally in SaveLoadManager.");
        }
        else
        {
            // Store empty list if null in data (safer than leaving _lastLoadedOwnedSkins null)
            _lastLoadedOwnedSkins = new List<string>();
            Debug.LogWarning(" > loadedData.ownedSkins was null, storing empty list in SaveLoadManager.");
        }
        // We no longer try to apply directly here, GachaMachine will ask for it.
        // --- END MODIFIED Gacha Machine Logic ---


        // Find and apply to SkinApplier if in current scene
        PlayerSkinApplierFromSave skinApplier = GameObject.FindFirstObjectByType<PlayerSkinApplierFromSave>();
        if (skinApplier != null)
        {
            string skinToApply = string.IsNullOrEmpty(loadedData.currentSkin) ? "Default" : loadedData.currentSkin;
            Debug.Log($" > Applying Current Skin: '{skinToApply}' to PlayerSkinApplier.");
            skinApplier.SetCurrentSkin(skinToApply);
        }
        else { Debug.LogWarning(" > PlayerSkinApplierFromSave not found during ApplyLoadedData."); }

        // Apply Items
        if (itemTrackScript != null)
        {
            Debug.Log($" > Applying Items: B={loadedData.ballCount}, Bsc={loadedData.biscuitCount}, Br={loadedData.brushCount}");
            itemTrackScript.ball = loadedData.ballCount;
            itemTrackScript.bisc = loadedData.biscuitCount;
            itemTrackScript.brush = loadedData.brushCount;
            Debug.Log($" > ItemTrack set: B={itemTrackScript.ball}, Bsc={itemTrackScript.bisc}, Br={itemTrackScript.brush}");
            // Call ItemTrack UI update
            //itemTrackScript.UpdateItemDisplay(); // Ensure this call exists
        }
        else { Debug.LogWarning(" > ItemTrack script ref missing during ApplyLoadedData."); }

        Debug.Log($" > Data Last Updated (from server): {loadedData.lastUpdated}");
        Debug.Log("Finished applying loaded data in SaveLoadManager.");

        // --- ADDED ---
        // After applying basic data, notify GachaMachine if it exists already
        // This helps if Load is called AFTER GachaMachine scene is already loaded
        GachaMachine.Instance?.TryApplyLoadedSkins();
        // --- END ADDED ---
    }

    // --- ADDED ---
    // Public method for GachaMachine (or others) to get the loaded skins
    public List<string> GetLoadedOwnedSkins()
    {
        return _lastLoadedOwnedSkins;
    }
    // --- END ADDED ---


    // --- Public methods to trigger Save/Load ---
    public void SaveGame()
    {
        Debug.Log("Gathering current game data for saving...");
        PlayerData dataToSave = GetCurrentPlayerData();
        if (dataToSave != null) // Add null check before starting coroutine
        {
             StartCoroutine(SaveRequestCoroutine(dataToSave));
        }
        else { Debug.LogError("Failed to gather PlayerData for saving!"); }

    }

    public void LoadGame()
    {
        Debug.Log($"Attempting to load game data for player: {currentPlayerId}...");
        StartCoroutine(LoadRequestCoroutine(currentPlayerId));
    }
    // --- End Public methods ---


    // --- Coroutines for Web Requests (Unchanged structure) ---
    private IEnumerator SaveRequestCoroutine(PlayerData dataToSave)
    {
        string jsonData = JsonUtility.ToJson(dataToSave);
        string correctUrl = $"{apiBaseUrl}/{this.currentPlayerId}";

        Debug.Log($"Attempting to save data to: {correctUrl}");
        Debug.Log($"JSON being sent: {jsonData}");

        using (UnityWebRequest request = new UnityWebRequest(correctUrl, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Save Error Details: URL={correctUrl}, Code={request.responseCode}, Error={request.error}, Response={request.downloadHandler.text}");
                Debug.LogError($"Data Attempted: {jsonData}");
            }
            else { Debug.Log($"Save Successful! Code: {request.responseCode}"); }
        }
    }

     private IEnumerator LoadRequestCoroutine(string playerIdToLoad)
    {
        string url = $"{apiBaseUrl}/{playerIdToLoad}";
        Debug.Log($"API CALL: Sending GET request to: {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                 Debug.LogError($"Load Error Details: URL={url}, Code={request.responseCode}, Error={request.error}, Response={request.downloadHandler.text}");
                 if(request.responseCode == 404) { Debug.LogWarning($"No save data found for player ID: {playerIdToLoad}"); }
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Load Successful! Response: {jsonResponse}");

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    try
                    {
                        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                        if (loadedData != null)
                        {
                             if(string.IsNullOrEmpty(loadedData.playerId)) { loadedData.playerId = playerIdToLoad; }
                            ApplyLoadedData(loadedData); // Apply the data
                        }
                        else { Debug.LogError("Failed to deserialize JSON (result was null)."); }
                    }
                    catch (ArgumentException argEx)
                    { Debug.LogError($"Error deserializing JSON (ArgumentException): {argEx.Message}\nJSON: {jsonResponse}\n{argEx.StackTrace}"); }
                    catch (Exception ex)
                    { Debug.LogError($"Generic error during JSON processing: {ex.Message}\nJSON: {jsonResponse}\n{ex.StackTrace}"); }
                }
                else { Debug.LogWarning("Received empty response body."); }
            }
        }
    }
    // --- End Coroutines ---
}