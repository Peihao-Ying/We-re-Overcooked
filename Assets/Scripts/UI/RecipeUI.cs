using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RecipeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform kitchenObjectParent;
    [SerializeField] private Image iconUITemplate;
    private void Start()
    {
        iconUITemplate.gameObject.SetActive(false);

    }
    public void UpdateUI( RecipeSO recipeSO )
    {
        recipeNameText.text = recipeSO.recipleName;
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Image newIcon = GameObject. Instantiate(iconUITemplate);
            newIcon.transform.SetParent(kitchenObjectParent);
            newIcon.sprite = kitchenObjectSO.sprite;
            newIcon.gameObject.SetActive(true);
        }
    }
}