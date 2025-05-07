using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NurtureItemsText : MonoBehaviour
{   public int ItemID;
    public TMP_Text QuantityTXT;
    ItemTrack items;
    
    void Start() {
      items = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<ItemTrack>();
    }

    void Update() {
     
      switch(ItemID)
      { case(1):
         QuantityTXT.text = "x" + items.ball;
         break;
        case(3):
         QuantityTXT.text = "x" + items.bisc;
         break;
        case(2):
         QuantityTXT.text = "x" + items.brush;
         break;
      }
    }
}
