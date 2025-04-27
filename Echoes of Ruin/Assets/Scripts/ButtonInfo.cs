using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInfo : MonoBehaviour
{   public int ItemID;
    public TMP_Text CostTXT;
    public TMP_Text QuantTXT;
    public GameObject vend;
    
    void Start()
    {   CostTXT.text = "Price:\n" + vend.GetComponent<Vendor>().items[2, ItemID].ToString();
        QuantTXT.text = "Quantity:\n" + vend.GetComponent<Vendor>().items[3, ItemID].ToString();
    }
    
}
