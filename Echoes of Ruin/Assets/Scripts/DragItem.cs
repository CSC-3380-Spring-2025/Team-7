using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{   public int itemID;
    Transform afterDrag;
    Vector3 temp;
    Vector3 mousePos;
    ItemTrack items;
    NurtureSlider sliders;

    void Start() 
    {   items = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<ItemTrack>();
        sliders = GameObject.Find("SliderCanvas").GetComponent<NurtureSlider>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {   temp = transform.position;  //sets current position so it can snap back later
        afterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    
    }
    public void OnDrag(PointerEventData eventData)
    {   transform.position = Input.mousePosition; }

    public void OnEndDrag(PointerEventData eventData)
    {   mousePos = Input.mousePosition;
        transform.SetParent(afterDrag);
        if (mousePos.x > 354 && mousePos.x < 700 && mousePos.y > 227 && mousePos.y < 600)
        {   switch(itemID)
            {   case(1):
                    if (items.ball > 0)
                    { items.ball--;
                      sliders.playAmount = sliders.playAmount + 10; 
                    }
                    break;
                case(2):
                    if (items.bisc > 0)
                    { items.bisc--; 
                      sliders.hungerAmount = sliders.hungerAmount + 10; 
                    }
                    break;
                case(3):
                    if (items.brush > 0)
                    { items.brush--; 
                      sliders.cleanAmount = sliders.cleanAmount + 10;
                    }
                    break;
            }
        }
        transform.position = temp; //snaps back to original position
    }

    //private void OnTriggerEnter2D(Collider2D other)
  //  {   if (other.tag == "CatPlayer")
    //    { Debug.Log("over sprite"); }
   // }
}
