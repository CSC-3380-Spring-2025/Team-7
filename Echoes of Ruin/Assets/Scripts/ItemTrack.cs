using UnityEngine;

public class ItemTrack : MonoBehaviour
{
    public int[,] itemNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { itemNum[1, 1] = 0;
      itemNum[1, 2] = 0;
      itemNum[1, 3] = 0;
        
    }
}
