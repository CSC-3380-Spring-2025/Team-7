using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System; 

public class MongoDBSaveLoadManager : MonoBehaviour
{
    // Base URL for the game data API endpoint.
    public string ApiBaseUrl = "http://localhost:5007/api/GameData";
    // Identifier for the current player's data.
    public string CurrentPlayerId = "player123";

    [Header("Persistent References")]
    // Reference to the Currency script component.
    public Currency CurrencyScript;
    // Reference to the ItemTrack script component.
    public ItemTrack ItemTrackScript;

    // Stores the list of owned skins names fetched during the last load operation.
    private List<string> lastLoadedOwnedSkins = null;

    // Singleton instance for easy access.
    public static MongoDBSaveLoadManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Gathers current player data from various game components into a PlayerData object.
    private PlayerData GetCurrentPlayerData()
    {
        PlayerData data = new PlayerData();
        data.playerId = this.CurrentPlayerId;

        if (CurrencyScript != null) { data.currency = CurrencyScript.coin; }
        else { data.currency = 0; }

        if (GachaMachine.Instance != null)
        { data.ownedSkins = new List<string>(GachaMachine.Instance.mySkins); }
        else
        { data.ownedSkins = new List<string>(); }

        PlayerSkinApplierFromSave skinApplier = GameObject.FindFirstObjectByType<PlayerSkinApplierFromSave>();
        if (skinApplier != null) { data.currentSkin = skinApplier.GetCurrentSkinName(); }
        else { data.currentSkin = "Default"; }

        if (ItemTrackScript != null)
        {
            data.ballCount = ItemTrackScript.ball;
            data.biscuitCount = ItemTrackScript.bisc;
            data.brushCount = ItemTrackScript.brush;
        }
        else
        { data.ballCount = 0; data.biscuitCount = 0; data.brushCount = 0; }

        data.lastUpdated = DateTime.UtcNow.ToString("o");
        return data;
    }

    // Applies the loaded PlayerData values back to the relevant game components.
    private void ApplyLoadedData(PlayerData loadedData)
    {
        if (loadedData == null) { return; }

        this.CurrentPlayerId = loadedData.playerId;

        if (CurrencyScript != null)
        { CurrencyScript.coin = loadedData.currency; }

        if (loadedData.ownedSkins != null)
        { lastLoadedOwnedSkins = new List<string>(loadedData.ownedSkins); }
        else
        { lastLoadedOwnedSkins = new List<string>(); }

        PlayerSkinApplierFromSave skinApplier = GameObject.FindFirstObjectByType<PlayerSkinApplierFromSave>();
        if (skinApplier != null)
        {
            string skinToApply = string.IsNullOrEmpty(loadedData.currentSkin) ? "Default" : loadedData.currentSkin;
            skinApplier.SetCurrentSkin(skinToApply);
        }

        if (ItemTrackScript != null)
        {
            ItemTrackScript.ball = loadedData.ballCount;
            ItemTrackScript.bisc = loadedData.biscuitCount;
            ItemTrackScript.brush = loadedData.brushCount;
        }

        GachaMachine.Instance?.TryApplyLoadedSkins();
    }

    // Gets the last loaded list of owned skin names.
    public List<string> GetLoadedOwnedSkins()
    {
        return lastLoadedOwnedSkins;
    }

    // Initiates the game saving process.
    public void SaveGame()
    {
        PlayerData dataToSave = GetCurrentPlayerData();
        if (dataToSave != null)
        { StartCoroutine(SaveRequestCoroutine(dataToSave)); }
    }

    // Initiates the game loading process.
    public void LoadGame()
    {
        StartCoroutine(LoadRequestCoroutine(CurrentPlayerId));
    }

    // Coroutine to handle the asynchronous save request via HTTP PUT.
    private IEnumerator SaveRequestCoroutine(PlayerData dataToSave)
    {
        string jsonData = JsonUtility.ToJson(dataToSave);
        string correctUrl = $"{ApiBaseUrl}/{this.CurrentPlayerId}";

        using (UnityWebRequest request = new UnityWebRequest(correctUrl, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Save Request Failed: {request.error} (Code: {request.responseCode})");
            }
            else { Debug.Log("Save Successful."); } 
        }
    }

    // Coroutine to handle the asynchronous load request via HTTP GET.
    private IEnumerator LoadRequestCoroutine(string playerIdToLoad)
    {
        string url = $"{ApiBaseUrl}/{playerIdToLoad}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    try
                    {
                        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                        if (loadedData != null)
                        {
                            if (string.IsNullOrEmpty(loadedData.playerId)) { loadedData.playerId = playerIdToLoad; }
                            ApplyLoadedData(loadedData);
                        }
                         else
                        {
                             Debug.LogWarning($"Load Request: Failed to deserialize JSON response from {url}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"Load Request: Exception during JSON parsing from {url}. Error: {ex.Message}");
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Load Request Failed: {request.error} (Code: {request.responseCode})");
            }
        }
    }
}