using Unity.VisualScripting;
using UnityEngine;

public class textTrigger : MonoBehaviour
{
    public GameObject textBubble;
    public GameObject trigger;
    public GameObject exclaimation;
    void Start()
    {
        textBubble.SetActive(false);
        exclaimation.SetActive(true);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag =="PlayerCat")
        {
            textBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if(textBubble != null) {
            textBubble.SetActive(false);
        }
        if(exclaimation != null) {
            exclaimation.SetActive(false);
        }
            Destroy(trigger);
        
    }
}
