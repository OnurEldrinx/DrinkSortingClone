using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    public void SetType(DrinkType type)
    {
        this.type = type;
    }

    public DrinkType GetDrinkType()
    {
        return type;
    }
}
