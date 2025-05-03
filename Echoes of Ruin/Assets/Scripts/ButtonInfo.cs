using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public TMP_Text CostTXT;
    public TMP_Text QuantTXT;
    public GameObject vend;

    private Vendor cachedVendorScript;

    // Called once when the script instance is being loaded.
    void Awake()
    {
        bool referencesValid = true;
        if (CostTXT == null) { referencesValid = false; }
        if (QuantTXT == null) { referencesValid = false; }
        if (vend == null)
        {
            referencesValid = false;
        }
        else
        {
            cachedVendorScript = vend.GetComponent<Vendor>();
            if (cachedVendorScript == null) { referencesValid = false; }
        }

        if (!referencesValid)
        {
            this.enabled = false;
            if (CostTXT != null) CostTXT.text = "Price:\nERR";
            if (QuantTXT != null) QuantTXT.text = "Quantity:\nERR";
            return;
        }
    }

    // Called every frame.
    void Update()
    {
        UpdateCostText();
        UpdateQuantityText();
    }

    // Updates the UI text element for the item's cost.
    void UpdateCostText()
    {
        if (cachedVendorScript != null && cachedVendorScript.items != null)
        {
            if (ItemID >= 0 && ItemID < cachedVendorScript.items.GetLength(1))
            {
                CostTXT.text = $"Price:\n{cachedVendorScript.items[2, ItemID]}";
            }
            else
            {
                CostTXT.text = "Price:\n???";
            }
        }
        else
        {
            CostTXT.text = "Price:\nERR";
        }
    }

    // Updates the UI text element for the player's owned quantity of the item.
    void UpdateQuantityText()
    {
        int currentQuantity = 0;
        if (ShopQuantityManager.Instance != null)
        {
            currentQuantity = ShopQuantityManager.Instance.GetItemQuantity(ItemID);
        }
        QuantTXT.text = $"Quantity:\n{currentQuantity}";
    }
}