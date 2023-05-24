using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript 
{
    
    public static IEnumerator TimeFreeze(float delay)
    {
        Time.timeScale = 0.0001f;
        for(float i = 0.0001f; i < delay; i += Time.unscaledDeltaTime)
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            Time.timeScale = i / delay;
        }
        Time.timeScale = 1;
    }
    
    
}
        
    