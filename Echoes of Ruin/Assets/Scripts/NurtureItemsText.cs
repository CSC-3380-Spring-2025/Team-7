using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NurtureItemsText : MonoBehaviour
{   public int ItemID;
    public TMP_Text QuantityTXT;
    public GameObject items;
    
    void Start() {
      items = GameObject.Find("HeartsAndCoinsOverlay");
    }

    void Update() {
       QuantityTXT.text = "x" + items.GetComponent<ItemTrack>().itemNum[1, ItemID].ToString();
    }
}
