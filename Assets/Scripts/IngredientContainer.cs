using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientContainer :BaseTable
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField]
    private IngredientObjectSO ingredientObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasIngredientObject())
        {
            IngredientObject.SpawnIngredientObject(ingredientObjectSO, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }           
    }
}
