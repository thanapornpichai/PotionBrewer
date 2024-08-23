using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BoilingRecipeSO : ScriptableObject
{
    public IngredientObjectSO input;
    public IngredientObjectSO output;
    public float boilingTimerMax;
}
