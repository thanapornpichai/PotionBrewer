using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTable : MonoBehaviour, IIngredientObjectParent
{
    [SerializeField]
    private Transform tableTopPoint;

    private IngredientObject ingredientObject;
    public virtual void Interact(Player player)
    {
        Debug.LogError("BaseTable.Interact();");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseTable.InteractAlternate();");
    }

    public Transform GetIngredientFollowTransform()
    {
        return tableTopPoint;
    }

    public void SetIngredientObject(IngredientObject ingredientObject)
    {
        this.ingredientObject = ingredientObject;
    }

    public IngredientObject GetIngredientObject()
    {
        return ingredientObject;
    }

    public void ClearIngredientObject()
    {
        ingredientObject = null;
    }

    public bool HasIngredientObject()
    {
        return ingredientObject != null;
    }
}
