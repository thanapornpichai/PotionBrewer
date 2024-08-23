using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour
{
    [SerializeField]
    private IngredientObjectSO ingredientObjectSO;

    private IIngredientObjectParent ingredientObjectParent;

    public IngredientObjectSO GetIngredientObjectSO()
    {
        return ingredientObjectSO;
    }

    public void SetIngredientObjectParent(IIngredientObjectParent ingredientObjectParent)
    {
        if(this.ingredientObjectParent != null)
        {
            this.ingredientObjectParent.ClearIngredientObject();
        }

        this.ingredientObjectParent = ingredientObjectParent;

        if (ingredientObjectParent.HasIngredientObject())
        {
            Debug.Log("IngredientObjectParent already has an Ingredient Objects!");
        }
        ingredientObjectParent.SetIngredientObject(this);

        transform.parent = ingredientObjectParent.GetIngredientFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IIngredientObjectParent GetIngredientObjectParent()
    {
        return ingredientObjectParent;
    }

    public void DestroySelf()
    {
        ingredientObjectParent.ClearIngredientObject();
        Destroy(gameObject);
    }

    public static IngredientObject SpawnIngredientObject(IngredientObjectSO ingredientObjectSO, IIngredientObjectParent ingredientObjectParent)
    {
        Transform ingredientObjectTransform = Instantiate(ingredientObjectSO.prefab);

        IngredientObject ingredientObject = ingredientObjectTransform.GetComponent<IngredientObject>();
        
        ingredientObject.SetIngredientObjectParent(ingredientObjectParent);

        return ingredientObject;
    }
}
