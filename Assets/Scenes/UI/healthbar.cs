using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart, halfHeart, emptyHeart;

    public void OnHealthChanged(int currenthealth, int maxhealth)
    {
        var heartCount = hearts.Length;
        var heartvalue = ((float)currenthealth) / maxhealth * heartCount;
        var fullhearts = (int)heartvalue;
        var hasHalfHeart = fullhearts < heartvalue;
        for (int i = 0; i < heartCount; i++)
        {
            if (i<fullhearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        if (hasHalfHeart)
        {
            hearts[fullhearts].sprite = halfHeart;
        }
    }
    private void OnEnable()
    {
        UiEvents.OnHealthChanged+=OnHealthChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    void Update() {

        if(health > numberOfHearts) {
            health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numberOfHearts)
            {
                hearts[i].enabled = true;
            } else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
