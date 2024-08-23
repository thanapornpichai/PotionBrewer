using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : BaseTable, IHasProgress
{
    public event EventHandler<IHasProgress.OnPregressChangedEventArgs> OnPregressChange;

    public event EventHandler OnCut;

    [SerializeField]
    private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasIngredientObject())
        {
            if (player.HasIngredientObject())
            {
                if (HasRecipeWithInput(player.GetIngredientObject().GetIngredientObjectSO()))
                {
                    player.GetIngredientObject().SetIngredientObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

                    OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
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

    public override void InteractAlternate(Player player)
    {
        if (HasIngredientObject() && HasRecipeWithInput(GetIngredientObject().GetIngredientObjectSO()))
        {
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetIngredientObject().GetIngredientObjectSO());

            OnPregressChange?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                IngredientObjectSO outputIngredientObjectSO = GetOutputForInput(GetIngredientObject().GetIngredientObjectSO());

                GetIngredientObject().DestroySelf();
                IngredientObject.SpawnIngredientObject(outputIngredientObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(IngredientObjectSO inputIngredientObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputIngredientObjectSO);
        return cuttingRecipeSO != null;
    }

    private IngredientObjectSO GetOutputForInput(IngredientObjectSO inputIngredientObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputIngredientObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(IngredientObjectSO inputIngredientObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputIngredientObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
