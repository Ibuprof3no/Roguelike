using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    [Header("Comida")]
    [Tooltip("Saciedad que llena la comida")]
    [Range(0, 101)]
    public int FoodValue;
    //override quiere dceir que va a hacer el virtual a su manera/version
    public override void PlayerEntered()
    {
        Destroy(gameObject);

        GameManager.Instance.ChangeFood(FoodValue);
    }
}