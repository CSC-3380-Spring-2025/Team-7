using UnityEngine;
using UnityEngine.EventSystems;

public class NurtureUseScript : MonoBehaviour, IDropHandler
{   public void OnDrop(PointerEventData eventData) {
    Debug.Log("OnDrop");
    }
}
