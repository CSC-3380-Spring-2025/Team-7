using UnityEngine;
using UnityEngine.UI;

public class NurtureSlider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float totalStat;
    public float playAmount;
    public float cleanAmount;
    public float hungerAmount;

    public int statBonus;

    //do connections to counters in other functions here
    public int stepCount;
    public int attackCount;
    public int damageCount;

    public Slider totalSlider;
    public Slider playSlider;
    public Slider cleanSlider;
    public Slider hungerSlider;

    public Text statText;
    StatsTracking tracking;

    void Start()
    {   tracking = GameObject.Find("HeartsAndCoinsOverlay").GetComponent<StatsTracking>();
        totalSlider.maxValue = 100.0f;
        playSlider.maxValue = 100.0f;
        cleanSlider.maxValue = 100.0f;
        hungerSlider.maxValue = 100.0f;

        totalSlider.value = 100.0f;
        playSlider.value = 100.0f;
        cleanSlider.value = 100.0f;
        hungerSlider.value = 100.0f;

        totalStat = 100.0f;
        playAmount = 100.0f;
        cleanAmount = 100.0f;
        hungerAmount = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {   stepCount = tracking.stepCount;
        attackCount = tracking.attackCount;
        
        if (stepCount != 0)
        {   cleanAmount = cleanAmount - (stepCount / 3);
            cleanAmount = Mathf.Clamp(cleanAmount, 0, 100);
            stepCount = 0;
            tracking.stepCount = 0;
        }

        if (damageCount != 0)
        {   playAmount = playAmount - (3 * damageCount);
            playAmount = Mathf.Clamp(playAmount, 0, 100);
            damageCount = 0;
            tracking.damageCount = 0;
        }

        if (attackCount != 0)
        {   hungerAmount = hungerAmount - attackCount;
            hungerAmount = Mathf.Clamp(hungerAmount, 0, 100);
            attackCount = 0;
            tracking.attackCount = 0;
        }

        totalStat = (playAmount + cleanAmount + hungerAmount) / 3; //average of the 3 bars

        if (totalStat >= 75) 
            { statBonus = 3; }
        else if (totalStat >= 50)
            { statBonus = 2; }
        else if (totalStat >= 25)
            { statBonus = 1; }
        else 
            { statBonus = 0; }

        totalSlider.value = totalStat;
        playSlider.value = playAmount;
        cleanSlider.value = cleanAmount;
        hungerSlider.value = hungerAmount;

        statText.text = "+" + statBonus.ToString();

    }
}
