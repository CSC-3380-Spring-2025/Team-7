using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string playerId; // Changed casing
    public int currency;     // Changed casing
    public List<string> ownedSkins = new List<string>(); // Changed casing
    public string currentSkin; // Changed casing
    public int ballCount;    // Changed casing
    public int biscuitCount; // Changed casing
    public int brushCount;   // Changed casing
    public string lastUpdated; // Changed casing AND type to string (see below)

    // Note: JsonUtility doesn't handle DateTime well by default.
    // It's easier to receive it as a string from the JSON
    // and parse it later if needed, or just keep it as a string.
    // Remove the default initialization if keeping as string.
    // public DateTime LastUpdated = DateTime.UtcNow; // Remove or comment out
}