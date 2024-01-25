using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Drink> drinkList;

    public List<Drink> GetRandomDrinks()
    {
        int count = Random.Range(0,6);

        List<Drink> result = new List<Drink>();

        for (int i=0;i<count;i++)
        {
            int randomDrink = Random.Range(0,drinkList.Count);
            result.Add(Instantiate(drinkList[randomDrink]));
        }

        return result;

    }

}
