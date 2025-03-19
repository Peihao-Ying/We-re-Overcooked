using System.Collections.Generic;
using UnityEngine;

public class CookingStation : KitchenObject
{
    private List<KitchenObject> ingredientList = new List<KitchenObject>();

    [SerializeField] private KitchenObjectSO burgerObjectSO;
    
    [SerializeField] private Transform holdPoint;
    private KitchenObject kitchenObject;

    public void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {
            if (this.kitchenObject != null)
            {
                print("Stations is already full");
                return;
            }
            KitchenObject kitchenObject = player.GetKitchenObject();
            player.ClearKitchenObject();

            AddKitchenObject(kitchenObject);
        }
        else
        {
            print("interacting, player do not  have kitchen, current station object is " + this.kitchenObject.name);
            if (this.kitchenObject != null)
            {
                player.AddKitchenObject(kitchenObject);
                this.kitchenObject = null;
            }
        }
    }
    
    public void AddKitchenObject(KitchenObject kitchenObject)
    {
        Debug.Log("添加食材: " + kitchenObject.name + "；当前数量: " + ingredientList.Count);

        kitchenObject.transform.SetParent(holdPoint);
        ingredientList.Add(kitchenObject);
        kitchenObject.transform.localPosition = Vector3.zero;

        foreach (KitchenObject o in ingredientList)
        {
            print("Current ingredients: "+o.name);
        }
    }
    
    public Transform GetHoldPoint()
    {
        return holdPoint;
    }
    
    public List<KitchenObject> GetIngredientList()
    {
        return ingredientList;
    }
    
    private void ClearAllIngredients()
    {
        print("Clear ing all ingredients");
        foreach (KitchenObject o in ingredientList)
        {
            Destroy(o.gameObject);
        }
        ingredientList.Clear();
    }
    
    public void CookAllIngredients()
    {
        if (ingredientList.Count == 0)
        {
            return;
        }
        
        if (CanMakeBurger())
        {
           if (Instantiate(burgerObjectSO.prefab, GetHoldPoint()).TryGetComponent<KitchenObject>(out KitchenObject burger))
           {
               this.kitchenObject = burger; 
               burger.transform.localPosition = Vector3.zero;
               burger.transform.localScale = Vector3.one * 0.4f;
           }
           else
           {
               print("Error instantiating burger");
           }
        }
        else
        {
            print("nothing can be made");
        }
        
        ClearAllIngredients();
    }
    
    private bool CanMakeBurger()
    {
        // Count how many times each ingredient appears by name
        int cheeseCount = 0;
        int meatCount   = 0;
        int breadCount  = 0;
        int tomatoCount = 0;
        int cabbageCount = 0;
    
        foreach (KitchenObject item in ingredientList)
        {
            print(item.name);
            switch (item.name)
            {
                case "CheeseBlockSlices(Clone)":
                    cheeseCount++;
                    break;
                case "MeetPattyCooked(Clone)":
                    meatCount++;
                    break;
                case "Bread(Clone)":
                    breadCount++;
                    break;
                case "TomatoSlices(Clone)":
                    tomatoCount++;
                    break;
                case "CabbageSlices(Clone)":
                    cabbageCount++;
                    break;
            }
        }
        
        // We want exactly 1 of each
        return cheeseCount == 1 &&
               meatCount   == 1 &&
               breadCount  == 1 &&
               tomatoCount == 1 &&
               cabbageCount == 1;
    }
}