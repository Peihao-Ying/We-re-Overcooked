using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingStation : BaseCounter
{
    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private Transform[] ingredientPoints;
    [SerializeField] private int maxIngredientCount = 4;
    [SerializeField] private float cookingTime = 5f;

    public static event EventHandler OnCooking;

    private List<KitchenObject> stackedIngredients = new List<KitchenObject>();
    private float cookingTimer = 0f;
    private bool isCooking = false;
    private RecipeSO currentRecipe = null;

    private void Update()
    {
        if (isCooking)
        {
            cookingTimer += Time.deltaTime;
            
            // If cooking is complete
            if (cookingTimer >= cookingTime)
            {
                CompleteCooking();
            }
        }
    }

    public override void Interact(Player player)
    {
        if (isCooking)
        {
            Debug.Log("Can't interact, cooking in progress!");
            return;
        }

        if (player.IsHaveKitchenObject())
        {
            Debug.Log("Player has an object: " + player.GetKitchenObject().name);

            if (stackedIngredients.Count < maxIngredientCount)
            {
                KitchenObject playerObject = player.GetKitchenObject();
                Debug.Log("➡ Transferring " + playerObject.name + " to CookingStation");

                player.ClearKitchenObject();
                AddIngredient(playerObject);
                UpdateIngredientsVisual();
                CheckForRecipe();
            }
            else
            {
                Debug.Log("Can't add more ingredients, stack is full!");
            }
        }
        else
        {
            if (IsHaveKitchenObject())
            {
                Debug.Log("Player takes completed dish: " + GetKitchenObject().name);
                TransferKitchenObject(this, player);
            }
            else if (stackedIngredients.Count > 0)
            {
                KitchenObject lastIngredient = stackedIngredients[stackedIngredients.Count - 1];
                stackedIngredients.RemoveAt(stackedIngredients.Count - 1);
                
                Debug.Log("Player takes back: " + lastIngredient.name);
                lastIngredient.transform.SetParent(player.GetHoldPoint());
                player.SetKitchenObject(lastIngredient);
                UpdateIngredientsVisual();
            }
            else
            {
                Debug.Log("Nothing to interact with!");
            }
        }
    }


    public override void InteractOperate(Player player)
    {
        if (isCooking || stackedIngredients.Count <= 0)
        {
            // Can't cook if already cooking or no ingredients
            return;
        }

        // Try to start cooking if we have a recipe match
        if (currentRecipe != null)
        {
            StartCooking();
        }
    }

    private void AddIngredient(KitchenObject kitchenObject)
    {
        // Add to our ingredients list
        stackedIngredients.Add(kitchenObject);
        
        // Position at the appropriate ingredient point
        int index = stackedIngredients.Count - 1;
        if (index < ingredientPoints.Length)
        {
            kitchenObject.transform.SetParent(ingredientPoints[index]);
            kitchenObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            // If we have more ingredients than positions, stack them on the last point
            kitchenObject.transform.SetParent(ingredientPoints[ingredientPoints.Length - 1]);
            kitchenObject.transform.localPosition = new Vector3(0, 0.1f * (index - ingredientPoints.Length + 1), 0);
        }
    }

    private void CheckForRecipe()
    {
        // If we have no ingredients or no recipe list, return
        if (stackedIngredients.Count == 0 || recipeListSO == null || recipeListSO.recipeSOList == null)
        {
            currentRecipe = null;
            return;
        }

        // Create list of KitchenObjectSOs from our stacked ingredients
        List<KitchenObjectSO> currentIngredients = new List<KitchenObjectSO>();
        foreach (KitchenObject kitchenObject in stackedIngredients)
        {
            currentIngredients.Add(kitchenObject.GetKitchenObjectSO());
        }

        // Check each recipe
        foreach (RecipeSO recipe in recipeListSO.recipeSOList)
        {
            if (recipe.kitchenObjectSOList.Count != currentIngredients.Count)
            {
                // Different ingredient count, not a match
                continue;
            }

            bool isMatch = true;
            
            // Create copies of the lists to modify during checking
            List<KitchenObjectSO> recipeIngredients = new List<KitchenObjectSO>(recipe.kitchenObjectSOList);
            List<KitchenObjectSO> availableIngredients = new List<KitchenObjectSO>(currentIngredients);

            // Check if every recipe ingredient is in our available ingredients
            foreach (KitchenObjectSO recipeIngredient in recipe.kitchenObjectSOList)
            {
                bool ingredientFound = false;
                
                for (int i = 0; i < availableIngredients.Count; i++)
                {
                    if (availableIngredients[i].name == recipeIngredient.name)
                    {
                        // Found this ingredient
                        availableIngredients.RemoveAt(i);
                        ingredientFound = true;
                        break;
                    }
                }
                
                if (!ingredientFound)
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                // We found a matching recipe!
                currentRecipe = recipe;
                return;
            }
        }

        // No matching recipe found
        currentRecipe = null;
    }

    private void StartCooking()
    {
        isCooking = true;
        cookingTimer = 0f;
        
        // Trigger cooking event
        OnCooking?.Invoke(this, EventArgs.Empty);
    }

    private void CompleteCooking()
    {
        isCooking = false;
        
        // Destroy all stacked ingredients
        foreach (KitchenObject ingredient in stackedIngredients)
        {
            Destroy(ingredient.gameObject);
        }
        
        // Clear the stack
        stackedIngredients.Clear();
        
        // Create the final dish
        if (currentRecipe != null && currentRecipe.kitchenObjectSOList.Count > 0)
        {
            // Find the recipe output (assuming first item in the recipe is the finished dish)
            KitchenObjectSO outputSO = currentRecipe.kitchenObjectSOList[0];
            CreateKitchenObject(outputSO.prefab);
            
            Debug.Log("Cooking complete! Created: " + currentRecipe.recipleName);
        }
        
        // Reset current recipe
        currentRecipe = null;
    }

    private void UpdateIngredientsVisual()
    {
        // Update visual presentation of stacked ingredients
        // Hide any unused ingredient points
        for (int i = 0; i < ingredientPoints.Length; i++)
        {
            if (i < stackedIngredients.Count)
            {
                ingredientPoints[i].gameObject.SetActive(true);
            }
            else
            {
                ingredientPoints[i].gameObject.SetActive(false);
            }
        }
    }

    // Clear static events when scene unloads
    public new static void ClearStaticData()
    {
        OnCooking = null;
    }
}