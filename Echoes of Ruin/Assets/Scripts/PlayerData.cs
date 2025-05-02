using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string PlayerId;
    public int Currency;
    public List<string> OwnedSkins = new List<string>();
    public string CurrentSkin;
    public int BallCount;
    public int BiscuitCount;
    public int BrushCount;
    public DateTime LastUpdated;
}