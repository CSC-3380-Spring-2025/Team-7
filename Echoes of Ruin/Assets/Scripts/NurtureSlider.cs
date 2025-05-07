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
        playAmount = tracking.playSave;
        cleanAmount = tracking.cleanSave;
        hungerAmount = tracking.hungerSave;
    }

    // Update is called once per frame
    void Update()
    {   stepCount = tracking.stepCount;
        attackCount = tracking.attackCount;
        damageCount = tracking.damageCount;
        
        if (stepCount != 0)
        {   cleanAmount = cleanAmount - (stepCount / 10); // divided just to make it slower drain
            cleanAmount = Mathf.Clamp(cleanAmount, 0, 100); //forces it to be between 0 and 100 for the sliders
            stepCount = 0; //resets counter so it doesn't keep adding up
            tracking.stepCount = 0; // ^ same as the above
        }

        if (damageCount != 0)
        {   playAmount = playAmount - (2 * damageCount);
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

        tracking.hungerSave = hungerAmount; //saves info otherwise it resets every time you reenter
        tracking.cleanSave = cleanAmount;
        tracking.playSave = playAmount;

        totalStat = (playAmount + cleanAmount + hungerAmount) / 3; //average of the 3 bars

        if (totalStat >= 75) //player stat gains
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

        tracking.statBonus = statBonus;

        statText.text = "+" + statBonus.ToString();

    }
}
