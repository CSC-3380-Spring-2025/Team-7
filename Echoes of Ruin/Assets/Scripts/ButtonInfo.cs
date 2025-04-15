using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{   public int ItemID;
    public TextMeshProUGUI CostTXT;
    public TextMeshProUGUI QuantTXT;
    public GameObject vend;
    
    void Start()
    {   CostTXT.text = "Price: \n" + vend.GetComponent<Vendor>().items[2, ItemID].ToString();
        QuantTXT.text = "Owned: \n" + vend.GetComponent<Vendor>().items[3, ItemID].ToString();
    }
    
}
