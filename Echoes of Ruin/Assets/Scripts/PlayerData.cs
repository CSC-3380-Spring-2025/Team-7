using System.Collections.Generic; 

[System.Serializable]
public class PlayerData{

    public string playerId; 
    public int currency;    
    public List<string> ownedSkins = new List<string>(); 
    public string currentSkin; 

}