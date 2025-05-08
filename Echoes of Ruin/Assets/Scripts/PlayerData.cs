using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string playerId;
    public int currency; 
    public List<string> ownedSkins = new List<string>(); 
    public string currentSkin; 
    public int ballCount;   
    public int biscuitCount; 
    public int brushCount;   
    public string lastUpdated; 
}