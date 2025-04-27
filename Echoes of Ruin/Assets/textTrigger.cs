using UnityEngine;

public class textTrigger : MonoBehaviour
{
    bool visited  = false;
    public GameObject trigger;
    public GameObject text;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D obj) 
    {
        if (obj.gameObject.tag == "PlayerCat")
        {
            visited = true;
            text.SetActive(true);

              
        } 
    }

    void OriggerExit2D(Collider2D obj)
    {
        text.SetActive(false);
        Destroy(text.gameObject);
    }
}
