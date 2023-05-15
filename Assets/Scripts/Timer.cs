using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Timer :MonoBehaviour
{
    

    protected float _timer;
    private void Update()
    {
        Countdown();
    }


    public abstract void Countdown();
    


}
