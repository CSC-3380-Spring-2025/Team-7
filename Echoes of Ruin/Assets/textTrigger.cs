using Unity.VisualScripting;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public GameObject textBubble; //refers to the text showing on screen
    public GameObject trigger; //refers to the trigger that player will start
    public GameObject exclaimation; //refers to the symbol that is supposed to guide players to trigger 
    void Start()
    {
        textBubble.SetActive(false); //will not show up on screen at first
        exclaimation.SetActive(true); //will show up on screen at first
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D obj) //when playercat enters into trigger area
    {
        if (obj.tag =="PlayerCat") //if it is playercat
        {
            textBubble.SetActive(true); //text will show up on screen
        }
    }

    void OnTriggerExit2D(Collider2D obj) //when playercat leaves the trigger area
    {
        if(textBubble != null) {
            textBubble.SetActive(false); //text will disappear
        }
        if(exclaimation != null) {
            exclaimation.SetActive(false); //exclaimation disappears
        }
            Destroy(trigger); //will destroy the trigger area so player cannot trigger text again for sake of it being a tutorial
        
    }
}
