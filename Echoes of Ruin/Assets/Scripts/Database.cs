using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;

public class MongoDBSaveLoadManager : MonoBehaviour
{
    public string apiBaseUrl = "http://localhost:5007/api/GameData";

    [Header("Configuration")]
    public string currentPlayerId = "player123";

    [Header("Script References")]
    public Currency currencyScript;
    public GachaMachine gachaMachineScript;
    public SkinChanger skinChangerScript; 

    public void SaveGame()
    {
        Debug.Log("Gathering current game data for saving...");
        PlayerData dataToSave = GetCurrentPlayerData();

        string jsonData = JsonUtility.ToJson(dataToSave, true);
        Debug.Log($"Data to be saved (JSON):\n{jsonData}");

        Debug.Log("Starting save coroutine (API call ENABLED)...");
        StartCoroutine(SaveRequestCoroutine(dataToSave));
    }

    public void LoadGame()
    {
        Debug.Log($"Attempting to load game data for player: {currentPlayerId}...");
        Debug.Log("Starting load coroutine (API call ENABLED)...");
        StartCoroutine(LoadRequestCoroutine(currentPlayerId));
    }


    private PlayerData GetCurrentPlayerData()
    {
        PlayerData data = new PlayerData();
        data.PlayerId = this.currentPlayerId;

        // Get Currency
        if (currencyScript != null) { data.Currency = currencyScript.coin; }
        else { Debug.LogWarning("Currency script reference not set."); }

        // Get Owned Skins from GachaMachine
        if (gachaMachineScript != null) { data.OwnedSkins = new List<string>(gachaMachineScript.mySkins); }
        else { Debug.LogWarning("GachaMachine script reference not set."); }

        
        if (skinChangerScript != null) { data.CurrentSkin = skinChangerScript.GetCurrentSkinName(); } 
        else { data.CurrentSkin = "Default"; Debug.LogWarning("SkinChanger script reference not set. Saving 'Default' skin."); }

        Debug.Log("Finished gathering data.");
        return data;
    }

    private void ApplyLoadedData(PlayerData loadedData)
    {
        Debug.Log("Executing ApplyLoadedData...");
        if (loadedData == null) { Debug.LogError("ApplyLoadedData received null data."); return; }

        this.currentPlayerId = loadedData.PlayerId;
        Debug.Log($"Applying data for Player ID: {loadedData.PlayerId}");

        // Apply Currency
        if (currencyScript != null) { currencyScript.coin = loadedData.Currency; Debug.Log($"Applied Currency: {loadedData.Currency}"); }
        else { Debug.LogWarning("Currency script ref not set. Cannot apply currency."); }

        // Apply Owned Skins to GachaMachine
        if (gachaMachineScript != null) { gachaMachineScript.mySkins = new HashSet<string>(loadedData.OwnedSkins); Debug.Log($"Applied Owned Skins Count: {loadedData.OwnedSkins.Count}"); }
        else { Debug.LogWarning("GachaMachine script ref not set. Cannot apply skins."); }

        if (skinChangerScript != null)
        {
            skinChangerScript.SetCurrentSkin(loadedData.CurrentSkin);
            Debug.Log($"Attempted to apply loaded current skin via SkinChanger: {loadedData.CurrentSkin}");
        }
        else { Debug.LogWarning("SkinChanger script ref not set. Cannot apply loaded current skin visuals."); }


        Debug.Log("Finished applying loaded data.");
    }

    private IEnumerator SaveRequestCoroutine(PlayerData dataToSave)
    {
        string jsonData = JsonUtility.ToJson(dataToSave); 
        string url = $"{apiBaseUrl}/{dataToSave.PlayerId}"; 

        Debug.Log($"API CALL (ENABLED): Sending PUT request to: {url}");
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            #if UNITY_2020_2_OR_NEWER
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            #else
            if (request.isNetworkError || request.isHttpError)
            #endif
            { Debug.LogError($"API ERROR Saving Data: {request.error} - Code: {request.responseCode}\nResponse: {request.downloadHandler.text}"); }
            else { Debug.Log($"API SAVE Successful! Code: {request.responseCode}"); }
        }
    }

    private IEnumerator LoadRequestCoroutine(string playerIdToLoad)
    {
        string url = $"{apiBaseUrl}/{playerIdToLoad}";
        Debug.Log($"API CALL (ENABLED): Sending GET request to: {url}");
         using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            #if UNITY_2020_2_OR_NEWER
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            #else
            if (request.isNetworkError || request.isHttpError)
            #endif
            {
                 Debug.LogError($"API ERROR Loading Data: {request.error} - Code: {request.responseCode}");
                 if(request.responseCode == 404) { Debug.LogWarning($"API: No save data found for player ID: {playerIdToLoad}."); }
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"API LOAD Successful! Code: {request.responseCode}");
                if (!string.IsNullOrEmpty(jsonResponse)) {
                    try {
                        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                        if (loadedData != null) { ApplyLoadedData(loadedData); }
                        else { Debug.LogError("API Failed to deserialize JSON response into PlayerData."); }
                    } catch (Exception ex) { Debug.LogError($"API Error deserializing JSON: {ex.Message}\nJSON: {jsonResponse}"); }
                } else { Debug.LogWarning("API received empty response body."); }
            }
        }
    }
}