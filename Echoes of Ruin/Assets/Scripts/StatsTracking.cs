using UnityEngine;
//using UnityEngine.SceneManagement;

public class StatsTracking : MonoBehaviour
{
    public int stepCount;
    public int attackCount;
    public int damageCount;


    void Start()
    {   stepCount = 0;
        attackCount = 0;
        damageCount = 0; 
    }

    void Update()
    { if (Input.GetKey("w"))
      { stepCount++; }
      else if (Input.GetKey("a"))
      { stepCount++; }
      else if (Input.GetKey("s"))
      { stepCount++; }
      else if (Input.GetKey("d"))
      { stepCount++; }
      else if (Input.GetKeyDown("q"))
      { attackCount++; }
      else if (Input.GetKeyDown("e"))
      { attackCount++; }
    }
}
