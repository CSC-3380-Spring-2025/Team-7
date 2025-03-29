using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   
    Transform afterDrag;
    Vector3 temp;

    public void OnBeginDrag(PointerEventData eventData)
    {   temp = transform.position;  //sets current position so it can snap back later
        afterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    
    }
    public void OnDrag(PointerEventData eventData)
    {   transform.position = Input.mousePosition; }

    public void OnEndDrag(PointerEventData eventData)
    {  transform.SetParent(afterDrag);
       transform.position = temp; //snaps back to original position
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   if (other.tag == "CatPlayer")
        { Debug.Log("over sprite"); }
    }
}
