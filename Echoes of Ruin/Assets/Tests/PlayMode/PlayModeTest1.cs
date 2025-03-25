using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest1
{
    // A Test behaves as an ordinary method
    //[Test]
    //public void PlayModeTest1SimplePasses()
    //{
        // Use the Assert class to test conditions
   // }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayModeTest1WithEnumeratorPasses()
    {
        
        yield return null;
    }
}
