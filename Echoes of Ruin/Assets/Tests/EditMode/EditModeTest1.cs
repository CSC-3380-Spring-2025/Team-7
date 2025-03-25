using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTest
{
    // A Test behaves as an ordinary method
    private Health healthcheck;
    [SetUp]
    public void SetUp()
    {
        GameObject dummyPlayer = new GameObject();
        healthcheck = dummyPlayer.AddComponent<Health>();
    }

    [Test]
    public void CheckForNegativeDamageResponse() //check if response occurs correctly when damage negative
    {
        var playerH = new Health();
        Assert.Throws<System.ArgumentOutOfRangeException>(() => playerH.Damage(-2));
        // Use the Assert class to test conditions
    }

    [Test]
     public void CheckForNegativeHealingResponse() //check if response occurs correctly when healing negative
    {
        var playerH = new Health();
        Assert.Throws<System.ArgumentOutOfRangeException>(() => playerH.Heal(-2));
        // Use the Assert class to test conditions
    }

    [Test]
     public void currentHeartsCorrectlyCalculated() //check if currenthearts calculated correctly after damage
    {
        healthcheck.currentHearts = 5;
        healthcheck.Damage(20);
        Assert.AreEqual(4, healthcheck.currentHearts);
        // Use the Assert class to test conditions
    }

}

