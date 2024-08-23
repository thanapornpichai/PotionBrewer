using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTable : BaseTable
{

    [SerializeField]
    private IngredientObjectSO ingredientObjectSO;

    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                player.GetIngredientObject().SetIngredientObjectParent(this);
            }
            else
            {
                //Player not carrying anything
            }
        }
        else
        {
            if (player.HasIngredientObject())
            {
                //Player is carrying something
            }
            else
            {
                GetIngredientObject().SetIngredientObjectParent(player);
            }
        }
    }

}
