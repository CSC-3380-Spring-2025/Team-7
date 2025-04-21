using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{   public int ItemID;
    public Text CostTXT;
    public Text QuantTXT;
    public GameObject vend;
    
    void Start()
    {   //CostTXT.text = "Cost: " + vend.GetComponent<Vendor>().BuyItem[2, ItemID].ToString();
        //QuantTXT.text = "Quantity: " + vend.GetComponent<Vendor>().BuyItem[3, ItemID].ToString();
    }
    
}
