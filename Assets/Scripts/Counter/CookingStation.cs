using System.Collections.Generic;
using UnityEngine;

public class CookingStation : KitchenObject
{
    private List<KitchenObject> ingredientList = new List<KitchenObject>();

    [SerializeField] private KitchenObjectSO resultKitchenObjectSO;
    
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
        foreach (KitchenObject o in ingredientList)
        {
            Destroy(o.gameObject);
        }
        ingredientList.Clear();
    }
    
    public void CookAllIngredients()
    {
        print("Current Ingredients: " + ingredientList.ToString());
        if (ingredientList.Count == 0)
        {
            Debug.Log("还没有任何食材，无法合成！");
            return;
        }

        Debug.Log("合成中... 已有 " + ingredientList.Count + " 个食材");

        ClearAllIngredients();

        if (resultKitchenObjectSO != null)
        {
            KitchenObject resultObject = GameObject.Instantiate(resultKitchenObjectSO.prefab, GetHoldPoint()).GetComponent<KitchenObject>();
            this.kitchenObject =  resultObject;
            kitchenObject.transform.localPosition = Vector3.zero;
            Debug.Log("合成完成！生成: " + resultKitchenObjectSO.name);
        }
        else
        {
            Debug.Log("未指定合成结果 resultKitchenObjectSO，无法生成成品！");
        }
    }
}
