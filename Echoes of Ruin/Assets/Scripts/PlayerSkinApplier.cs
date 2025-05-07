using UnityEngine;
using System.Collections.Generic;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("Player Skin GameObjects")]
    public GameObject defaultPlayerObject;
    public GameObject comBlackCatObject;
    public GameObject comWhiteCatObject;
    public GameObject rareSiameseCatObject;
    public GameObject rareAshortCatObject;
    public GameObject legCalicoCatObject;

    private Dictionary<string, GameObject> skinObjectMap;
    private GameObject currentlyActiveSkinObject;

    void Awake()
    {
        skinObjectMap = new Dictionary<string, GameObject>();

        if (defaultPlayerObject != null) {
             skinObjectMap.Add("Default", defaultPlayerObject);
        }
        if (comBlackCatObject != null) {
            skinObjectMap.Add("BlackCat", comBlackCatObject);
        }
        if (comWhiteCatObject != null) {
             skinObjectMap.Add("WhiteCat", comWhiteCatObject);
        }
        if (rareSiameseCatObject != null) {
             skinObjectMap.Add("SiameseCat", rareSiameseCatObject);
        }
        if (rareAshortCatObject != null) {
             skinObjectMap.Add("ShortCat", rareAshortCatObject);
        }
        if (legCalicoCatObject != null) {
             skinObjectMap.Add("CalicoCat", legCalicoCatObject);
        }

        DeactivateAllSkins();
    }

    private void DeactivateAllSkins()
    {
        if (skinObjectMap == null) return;

        foreach (var pair in skinObjectMap)
        {
            if (pair.Value != null)
            {
                pair.Value.SetActive(false);
            }
        }
        currentlyActiveSkinObject = null;
    }

    public bool ActivateSkin(string skinName)
    {
        if (skinObjectMap == null) { return false; }

        if (skinObjectMap.TryGetValue(skinName, out GameObject objectToActivate))
        {
            if (objectToActivate != null)
            {
                if (currentlyActiveSkinObject == objectToActivate) {
                     return true;
                }

                if (currentlyActiveSkinObject != null)
                {
                    currentlyActiveSkinObject.SetActive(false);
                }

                objectToActivate.SetActive(true);
                currentlyActiveSkinObject = objectToActivate;
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }
}