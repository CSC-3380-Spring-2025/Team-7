using UnityEngine;

[System.Serializable]
public class LootItem {
    //Item to be Dropped and Chance
    public GameObject ItemPrefab;
    [Range(0,100)] public float DropChance;
}
