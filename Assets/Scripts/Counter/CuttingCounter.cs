using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class CuttingCounter : BaseCounter
{
    public static event EventHandler OnCut;

    [SerializeField] private CuttingRecipeListSO cuttingRecipeList;

    [SerializeField] private ProgressBarUI progressBarUI;

    [SerializeField] private CuttingCounterVisual cuttingCounterVisual;

    private int cuttingCount = 0;

    private void Awake()
    {
        if (cuttingRecipeList == null)
        {
            Debug.LogError("cuttingRecipeList is NULL in Awake! Assigning manually...");
            cuttingRecipeList = Resources.Load<CuttingRecipeListSO>("CuttingRecipeList");

            if (cuttingRecipeList == null)
            {
                Debug.LogError("Could not find CuttingRecipeListSO in Resources!");
            }
            else
            {
                Debug.Log("Successfully loaded CuttingRecipeListSO: " + cuttingRecipeList.name);
            }
        }
    }

    private void Start()
    {
        if (cuttingRecipeList == null)
        {
            Debug.LogError("cuttingRecipeList is NULL at runtime! Assigning manually...");
            cuttingRecipeList = Resources.Load<CuttingRecipeListSO>("CuttingRecipeList");

            if (cuttingRecipeList == null)
            {
                Debug.LogError("Could not find CuttingRecipeListSO in Resources!");
            }
            else
            {
                Debug.Log("Successfully loaded CuttingRecipeListSO: " + cuttingRecipeList.name);
            }
        }
        else
        {
            Debug.Log("cuttingRecipeList is assigned: " + cuttingRecipeList.name);
        }
    }


    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {
            if (IsHaveKitchenObject() == false)
            {
                cuttingCount = 0;
                TransferKitchenObject(player, this);
            }
            else
            {

            }
        }
        else
        {
            if (IsHaveKitchenObject() == false)
            {

            }
            else
            {
                TransferKitchenObject(this, player);
                // progressBarUI.Hide();
            }
        }
    }
    public override void InteractOperate(Player player)
    {
        Debug.Log("Operate action reached CuttingCounter!");

        if (cuttingRecipeList == null)
        {
            Debug.LogError("ERROR: cuttingRecipeList is NULL! Check if it's assigned.");
            return;
        }

        if (!IsHaveKitchenObject())
        {
            Debug.LogWarning("WARNING: No kitchen object on counter!");
            return;
        }

        KitchenObject kitchenObject = GetKitchenObject();
        if (kitchenObject == null)
        {
            Debug.LogError("ERROR: GetKitchenObject() returned NULL! The object might not be properly instantiated.");
            return;
        }

        Debug.Log("Found KitchenObject: " + kitchenObject.name);

        KitchenObjectSO currentKitchenObjectSO = kitchenObject.GetKitchenObjectSO();

        if (currentKitchenObjectSO == null)
        {
            Debug.LogError("ERROR: GetKitchenObjectSO() is NULL! The kitchen object might not be assigned a reference.");
            Debug.Log("KitchenObject component on " + kitchenObject.name + " might be missing its KitchenObjectSO!");
            return;
        }

        Debug.Log("Checking recipe for: " + currentKitchenObjectSO.name);

        if (cuttingRecipeList.TryGetCuttingRecipe(currentKitchenObjectSO, out CuttingRecipe cuttingRecipe))
        {
            Debug.Log("Found recipe for: " + currentKitchenObjectSO.name);
            Cut();

            if (cuttingCount == cuttingRecipe.cuttingCountMax)
            {
                DestroyKitchenObject();
                CreateKitchenObject(cuttingRecipe.output.prefab);
                Debug.Log("Cutting complete! New object created: " + cuttingRecipe.output.name);
            }
        }
        else
        {
            Debug.LogWarning("No valid recipe found for: " + currentKitchenObjectSO.name);
        }
    }


    private void Cut()
    {
        OnCut?.Invoke(this, EventArgs.Empty);
        cuttingCount++;
        cuttingCounterVisual.PlayCut();
    }
    public new static void ClearStaticData()
    {
        OnCut = null;
    }


}
