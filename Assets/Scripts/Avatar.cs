using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _renderer;

    public void Flip(bool hastToFlip)
    {
        _renderer.flipX = hastToFlip;
    }
        public void FlipY(bool hastToFlip)
    {
        _renderer.flipY = hastToFlip;
    }
}
