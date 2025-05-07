using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTest
{
    // A Test behaves as an ordinary method
    private Health healthcheck; 
    private PlayerHealth healthcheck2;
    int i;
    [SetUp] //establishes the same setup for multiple tests of same class
    public void SetUp()
    {
        GameObject dummyPlayer = new GameObject(); //this gameobject will act as standin for player
        healthcheck = dummyPlayer.AddComponent<Health>(); //attach dummyplayer to Health class 
        GameObject dummyPlayer2 = new GameObject();
        healthcheck2 = dummyPlayer2.AddComponent<PlayerHealth>();
    }

    [Test]
    public void CheckForNegativeDamageResponse() //check if response occurs correctly when damage negative
    {
        Assert.Throws<System.ArgumentOutOfRangeException>(() => healthcheck.Damage(-2));
        // Use the Assert class to test conditions
    }

    [Test]
     public void CheckForNegativeHealingResponse() //check if response occurs correctly when healing negative
    {
        
        Assert.Throws<System.ArgumentOutOfRangeException>(() => healthcheck.Heal(-2));
        // Use the Assert class to test conditions
    }

    [Test]
     public void CurrentHeartsCorrectlyCalculated() //check if currenthearts calculated correctly after damage
    {
        
        healthcheck.currentHearts = 5; //set conditions for current number of hearts
        healthcheck.Damage(20); //set damage
        Assert.AreEqual(4, healthcheck.currentHearts); //check expected vs. results
        // Use the Assert class to test conditions
    }

    [Test]
     public void CheckIfPlayerHPisZero() //check if playerHP is zero
    {     
        healthcheck.currentHearts = 1; //set conditions for current number of hearts
        healthcheck.Damage(20); //set damage
        Assert.AreEqual(0, healthcheck2.playerHP); //check expected vs results 
        // Use the Assert class to test conditions
    }

    [Test]
     public void WhatHappensToHPIfHeartIsOne() //check if playerHP calculated correctly after damage
    {     
        healthcheck.currentHearts = 2; //set conditions for current number of hearts
        healthcheck.Damage(20); //set damage
        Assert.AreEqual(20, healthcheck.playerHP); //check
        //RESULT = DOESNT PASS THE TEST BC PLAYERHP IS NEVER UPDATED WHEN HEART = 1 
        // Use the Assert class to test conditions
    }

    [Test]
     public void WhatHappensToHPIfHeartIsOnePt2() //check if playerHP updates the hearts array
    {   healthcheck.currentHearts = 2;  
        healthcheck.playerHP = 40; 
        healthcheck.Damage(20);
        Assert.AreEqual(1, healthcheck.hearts[i]); //check
        
    }
    [Test]
    public void WhatHappensWhenHealingHappens() //check if playerHP updates the hearts array
    {   healthcheck.currentHearts = 2;   
        healthcheck.Heal(20);
        Assert.AreEqual(3, healthcheck.currentHearts); //check
        
    }

    [Test]
    public void WhatHappensWhenCurrentHeartsMoreThanMaxHearts() //check if playerHP updates the hearts array
    {   healthcheck.currentHearts = 5;   
        healthcheck.Heal(20);
        Assert.AreEqual(5, healthcheck.currentHearts); //check
        
    }
    //end of testing Health = ALL TESTS PASSED EXCEPT ONE : SEE TEST NUMBER SIX
    
    
    
 
    


}

