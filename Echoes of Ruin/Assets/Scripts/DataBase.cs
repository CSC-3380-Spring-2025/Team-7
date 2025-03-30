using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class MongoDBSaveLoadManager : MonoBehaviour
{
    // API URL - Will be uncommented and configured later when the API is ready
    // public string apiBaseUrl = "http://localhost:3000/api/gamedata";

    [Header("Configuration")]
    public string currentPlayerId = "player123"; // Unique ID for the save file

    [Header("Script References")]
    public Currency currencyScript; // Assign GameObject with Currency script
    public GachaMachine gachaMachineScript; // Assign GameObject with GachaMachine script

   
    public void SaveGame()
    {
        Debug.Log("Gathering current game data for saving...");
        PlayerData dataToSave = GetCurrentPlayerData();

        
        string jsonData = JsonUtility.ToJson(dataToSave, true); 
        Debug.Log($"Data to be saved (JSON):\n{jsonData}");

        // Start the save process (API call is currently disabled)
        Debug.Log("Starting save coroutine (API call disabled)...");
        StartCoroutine(SaveRequestCoroutine(dataToSave));
    }

    public void LoadGame()
    {
        Debug.Log($"Attempting to load game data for player: {currentPlayerId}...");

        // Start the load process (API call is currently disabled)
        Debug.Log("Starting load coroutine (API call disabled)...");
        StartCoroutine(LoadRequestCoroutine(currentPlayerId));
    }


    private PlayerData GetCurrentPlayerData()
    {
        PlayerData data = new PlayerData();

        data.playerId = this.currentPlayerId;

        // Get Currency
        if (currencyScript != null)
        {
            data.currency = currencyScript.coin;
        }
        else { Debug.LogWarning("Currency script reference not set in SaveLoadManager."); }

        // Get Skins
        if (gachaMachineScript != null)
        {
            data.ownedSkins = new List<string>(gachaMachineScript.mySkins);
            data.currentSkin = "Default"; // Placeholder
        }
        else { Debug.LogWarning("GachaMachine script reference not set in SaveLoadManager."); }

        Debug.Log("Finished gathering data.");
        return data;
    } 

    private void ApplyLoadedData(PlayerData loadedData)
    {
        Debug.Log("Executing ApplyLoadedData...");
        if (loadedData == null)
        {
            Debug.LogError("ApplyLoadedData received null data. Cannot apply.");
            return;
        }

        // Although we load by currentPlayerId, it's good practice if the loaded data contains the ID too
        this.currentPlayerId = loadedData.playerId;
        Debug.Log($"Applying data for Player ID: {loadedData.playerId}");

        // Apply Currency
        if (currencyScript != null)
        {
            currencyScript.coin = loadedData.currency;
            Debug.Log($"Applied Currency: {loadedData.currency}");
        }
        else { Debug.LogWarning("Currency script reference not set. Cannot apply currency."); }

        // Apply Skins
        if (gachaMachineScript != null)
        {
            gachaMachineScript.mySkins = new HashSet<string>(loadedData.ownedSkins);
            Debug.Log($"Applied Owned Skins Count: {loadedData.ownedSkins.Count}");
            Debug.Log($"Applied Current Skin: {loadedData.currentSkin} (Implement visual change!)");
        }
        else { Debug.LogWarning("GachaMachine script reference not set. Cannot apply skins."); }

        Debug.Log("Finished applying loaded data.");
    }

    //Coroutines (API Calls Disabled)

    private IEnumerator SaveRequestCoroutine(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        // string url = $"{apiBaseUrl}/{data.playerId}"; // URL construction for when API is ready

        //API CALL DISABLED
        /*
        Debug.Log($"API CALL (DISABLED): Sending PUT request to: {url}");
        using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPUT))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            // request.SetRequestHeader("Authorization", "Bearer YOUR_TOKEN"); // Add if needed later

            yield return request.SendWebRequest();

            
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            #else
            if (request.isNetworkError || request.isHttpError)
            #endif
            {
                Debug.LogError($"API ERROR Saving Data: {request.error} - Code: {request.responseCode}");
            }
            else
            {
                Debug.Log($"API SAVE Successful! Code: {request.responseCode}");
            }
        }
        */
        // END OF API CALL DISABLED

        yield return null;
        Debug.Log("Save request coroutine finished (API call was disabled).");
    }

    private IEnumerator LoadRequestCoroutine(string playerIdToLoad)
    {
        // string url = $"{apiBaseUrl}/{playerIdToLoad}"; // URL construction for when API is ready

        //API CALL DISABLED
        /*
        Debug.Log($"API CALL (DISABLED): Sending GET request to: {url}");
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // request.SetRequestHeader("Authorization", "Bearer YOUR_TOKEN"); // Add if needed later

            yield return request.SendWebRequest();

            #if UNITY_2020_2_OR_NEWER
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            #else
            if (request.isNetworkError || request.isHttpError)
            #endif
            {
                 Debug.LogError($"API ERROR Loading Data: {request.error} - Code: {request.responseCode}");
                 if(request.responseCode == 404) {
                     Debug.LogWarning($"API: No save data found for player ID: {playerIdToLoad}.");
                 }
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"API LOAD Successful! Code: {request.responseCode}");
                if (!string.IsNullOrEmpty(jsonResponse)) {
                    try {
                        PlayerData loadedData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                        if (loadedData != null) {
                            ApplyLoadedData(loadedData); // Apply data received from API
                        } else {
                            Debug.LogError("API Failed to deserialize JSON response into PlayerData.");
                        }
                    } catch (System.Exception ex) {
                        Debug.LogError($"API Error deserializing JSON: {ex.Message}\nJSON: {jsonResponse}");
                    }
                } else {
                     Debug.LogWarning("API received empty response body.");
                }
            }
        }
        // If the API call *successfully loaded data*, you might want to skip the test data below.
        // Add a boolean flag or check if data was already applied.
        */
        //END OF API CALL DISABLED


        yield return null;
        Debug.Log("Load request coroutine finished (API call disabled, using test data).");
    }
}