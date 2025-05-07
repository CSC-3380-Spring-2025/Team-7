using UnityEngine;

public class GachaButtonHelper : MonoBehaviour
{
    public void TriggerGachaRoll()
    {
        if (GachaMachine.Instance != null)
        {
            GachaMachine.Instance.StartGachaSequence();
        }
    }
}