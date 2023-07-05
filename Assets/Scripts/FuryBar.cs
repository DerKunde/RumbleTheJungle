using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuryBar : MonoBehaviour
{
    public Slider furybar;

    private float CalculateSliderPercentage(float currentfury, float maxfury)
    {
        return currentfury / maxfury;
    }

    public void OnPlayerFuryChanged(int newfury, int maxfury)
    {
        furybar.value = CalculateSliderPercentage(newfury,maxfury);
    }
    private void OnEnable()
    {
        UiEvents.OnFuryChanged+=OnPlayerFuryChanged;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
}
