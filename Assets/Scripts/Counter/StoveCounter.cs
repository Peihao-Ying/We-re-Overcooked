using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StoveCounter : BaseCounter
{

    [SerializeField] private FryingRecipeListSO fryingRecipeList;
    [SerializeField] private FryingRecipeListSO burningRecipeList;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;
    // [SerializeField] private ProgressBarUI progressBarUI;
    [SerializeField] private AudioSource sound;

    public enum StoveState
    {
        Idle,
        Frying,
        Burning
    }

    private FryingRecipe fryingRecipe;
    private float fryingTimer = 0;
    private StoveState state = StoveState.Idle;
    // private WarningControl warningControl;

    private void Start()
    {
        // warningControl = GetComponent<WarningControl>();
    }



    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {
            if (IsHaveKitchenObject() == false )
            {

                if(fryingRecipeList.TryGetFryingRecipe(
                player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe fryingRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartFrying(fryingRecipe);
                }else if(burningRecipeList.TryGetFryingRecipe(
                player.GetKitchenObject().GetKitchenObjectSO(), out FryingRecipe burningRecipe))
                {
                    TransferKitchenObject(player, this);
                    StartBurning(burningRecipe);
                }
                else
                {

                }
                
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
                TurnToIdle();
                TransferKitchenObject(this, player);
            }
        }

    }

    private void Update()
    {
        switch (state)
        {
            case StoveState.Idle:
                break;
            case StoveState.Frying:
                fryingTimer += Time.deltaTime;
                // progressBarUI.UpdateProgress(fryingTimer/ fryingRecipe.fryingTime);
                if (fryingTimer >= fryingRecipe.fryingTime)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);
                    
                    burningRecipeList.TryGetFryingRecipe(GetKitchenObject().GetKitchenObjectSO(),
                        out FryingRecipe newFryingRecipe);
                    StartBurning(newFryingRecipe);
                }
                break;
            case StoveState.Burning:
                fryingTimer += Time.deltaTime;
                // progressBarUI.UpdateProgress(fryingTimer / fryingRecipe.fryingTime);

                float warningTimeNormalize = .5f;
                if (fryingTimer / fryingRecipe.fryingTime > warningTimeNormalize)
                {
                    // warningControl.ShowWarning();
                }

                if (fryingTimer >= fryingRecipe.fryingTime)
                {
                    DestroyKitchenObject();
                    CreateKitchenObject(fryingRecipe.output.prefab);
                    TurnToIdle();
                }
                
                break;
            default:
                break;
        }
    }

    private void StartFrying(FryingRecipe fryingRecipe)
    {
        fryingTimer = 0;
        this.fryingRecipe = fryingRecipe;
        state = StoveState.Frying;
        stoveCounterVisual.ShowStoveEffect();
        sound.Play();
    }
    private void StartBurning(FryingRecipe fryingRecipe)
    {
        if (fryingRecipe == null)
        {
            Debug.LogWarning("No frying Recipe");
            TurnToIdle();
            return;
        }
        stoveCounterVisual.ShowStoveEffect();
        fryingTimer = 0;
        this.fryingRecipe = fryingRecipe;
        state = StoveState.Burning;
        sound.Play();
    }
    private void TurnToIdle()
    {
        // progressBarUI.Hide();
        state = StoveState.Idle;
        stoveCounterVisual.HideStoveEffect();
        sound.Pause();
        // warningControl.StopWarning();
    }

}
