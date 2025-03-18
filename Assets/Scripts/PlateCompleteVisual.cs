using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public class KitchenObjectSO_Model {
        public KitchenObjectSO kitchenObjecSO;
        public GameObject model;
    }

    [SerializeField] private List<KitchenObjectSO_Model> modelMap;
    

    public void ShowKitchenObject(KitchenObjectSO kitchenObjectSO)
    {
        foreach(KitchenObjectSO_Model item in modelMap)
        {
            if (item.kitchenObjecSO == kitchenObjectSO)
            {
                item.model.SetActive(true);return;
            }
        }
    }

}
