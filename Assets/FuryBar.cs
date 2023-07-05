using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuryBar : MonoBehaviour
{
    public Slider furybar;
    public Playercontrols player;



    private float CalculateSliderPercentage(float currentfury, float maxfury)
    {
        return currentfury / maxfury;
    }

    public void OnPlayerFuryChanged(int newfury, int maxfury)
    {
        furybar.value = CalculateSliderPercentage(newfury,maxfury);
    }
    // Start is called before the first frame update
    void Start()
    {
        OnPlayerFuryChanged(player.Fury, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
