using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class UiEvents
{
    public static UnityAction<int, int> OnHealthChanged;
    public static UnityAction<int, int> OnFuryChanged;
}
