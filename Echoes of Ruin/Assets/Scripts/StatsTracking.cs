using UnityEngine;
//using UnityEngine.SceneManagement;

public class StatsTracking : MonoBehaviour
{
    public int stepCount;
    public int attackCount;
    public int damageCount;

    public float hungerSave;
    public float cleanSave;
    public float playSave;

    public int statBonus;

    void Start()
    {   stepCount = 0;
        attackCount = 0;
        damageCount = 0; 

        hungerSave = 100.00f;
        playSave = 100.00f;
        cleanSave = 100.00f;
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
