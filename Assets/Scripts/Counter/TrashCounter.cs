using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnObjectTrashed;

    public override void Interact(Player player)
    {
        if (player.IsHaveKitchenObject())
        {
            player.DestroyKitchenObject();
            OnObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

    public new static void ClearStaticData()
    {
        OnObjectTrashed = null;
    }

}
