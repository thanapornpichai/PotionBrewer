using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTable : BaseTable
{
    public override void Interact(Player player)
    {
        if (player.HasIngredientObject())
        {
            player.GetIngredientObject().DestroySelf();
        }
    }
}
