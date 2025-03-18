using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CuttingRecipe
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingCountMax;
}
[CreateAssetMenu()]
public class CuttingRecipeListSO : ScriptableObject
{
    public List<CuttingRecipe> list;


    public KitchenObjectSO GetOutput( KitchenObjectSO input )
    {
        foreach(CuttingRecipe recipe in list)
        {
            if (recipe.input == input)
            {
                return recipe.output;
            }
        }
        return null;
    }
    public bool TryGetCuttingRecipe(KitchenObjectSO input, out CuttingRecipe cuttingRecipe)
    {
        if (list == null)
        {
            Debug.LogError("ERROR: Cutting Recipe List is NULL!");
            cuttingRecipe = null;
            return false;
        }

        if (input == null)
        {
            Debug.LogError("ERROR: Input KitchenObjectSO is NULL!");
            cuttingRecipe = null;
            return false;
        }

        Debug.Log("🔍 Checking recipe for: " + input.name);

        foreach (CuttingRecipe recipe in list)
        {
            if (recipe == null || recipe.input == null)
            {
                Debug.LogError("ERROR: A recipe or its input is NULL!");
                continue;
            }

            Debug.Log("Comparing: " + recipe.input.name + " with " + input.name);

            if (recipe.input.name == input.name)
            {
                Debug.Log("Recipe MATCH found for: " + input.name);
                cuttingRecipe = recipe;
                return true;
            }
        }

        Debug.LogWarning("No matching recipe found for: " + input.name);
        cuttingRecipe = null;
        return false;
    }
}
