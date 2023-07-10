using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        FadeManager.Fade(FadeType.Out, () =>
         {
             StartCoroutine(FadeChange()); 
         });

        IEnumerator FadeChange()
        {
            yield return new WaitForSeconds(2);
            FadeManager.Fade(FadeType.In,() =>{ });
        }
    }
}
