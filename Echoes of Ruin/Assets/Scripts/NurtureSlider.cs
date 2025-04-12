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

    void Start()
    {   cleanAmount = 100 - (stepCount / 3);
        cleanAmount = Mathf.Clamp(cleanAmount, 0, 100);

        playAmount = 100 - (3 * damageCount);
        playAmount = Mathf.Clamp(playAmount, 0, 100);

        hungerAmount = 100 - attackCount;
        hungerAmount = Mathf.Clamp(hungerAmount, 0, 100);

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

    // Update is called once per frame
    void Update()
    {   

    }
}
