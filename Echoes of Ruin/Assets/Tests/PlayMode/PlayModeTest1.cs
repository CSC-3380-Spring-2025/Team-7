using System.Collections;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayModeTest1
{
    // A Test behaves as an ordinary method
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    private Health healthcheck;
    public int i;

    [SetUp]
    public void SetUp()
    {
        GameObject dummyPlayer = new GameObject();
        healthcheck = dummyPlayer.AddComponent<Health>();
    }
    
    [UnityTest]
    public IEnumerator PlayModeTest1WithEnumeratorPasses()
    {
        healthcheck.currentHearts = 3;
        healthcheck.Damage(20);
        int result = healthcheck.hearts[i].activeSelf;
        Assert.AreEqual(2, result);
        
        yield return null;
    }
}
