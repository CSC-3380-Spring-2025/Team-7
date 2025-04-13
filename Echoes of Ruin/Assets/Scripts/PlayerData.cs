using System.Collections.Generic;
using System; 

[Serializable] 
public class PlayerData
{
    public string PlayerId;
    public int Currency;
    public List<string> OwnedSkins = new List<string>();
    public string CurrentSkin;
}