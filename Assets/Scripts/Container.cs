using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : MonoBehaviour
{

    [SerializeField] private GridInstance grid;
    [SerializeField] private List<Transform> drinkSlots;
    [SerializeField] private List<Drink> drinks;
    private BoxCollider boxCollider;
    private HashSet<DrinkType> existingTypes;
    private Dictionary<DrinkType, int> typeCountMap;

    private void Start()
    {
        if(grid is not null) drinkSlots.AddRange(grid.GetCells());

        boxCollider = GetComponent<BoxCollider>();

        existingTypes = new HashSet<DrinkType>();
        
        foreach (var t in drinkSlots.Where(t => t.childCount > 0))
        {
            Drink temp = t.GetChild(0).GetComponent<Drink>();
            drinks.Add(temp);
            existingTypes.Add(temp.GetDrinkType());
        }

    }

    public List<List<Drink>> CategorizeDrinks()
    {
        typeCountMap = new Dictionary<DrinkType, int>();

        List<List<Drink>> drinkGroups = new List<List<Drink>>();

        for (int i = 0; i < existingTypes.Count; i++)
        {
            List<Drink> temp = new List<Drink>();
            for (int j = 0; j < drinks.Count; j++)
            {
                if (drinks[j].GetDrinkType() == existingTypes.ElementAt(i))
                {
                    temp.Add(drinks[j]);
                }
            }
            drinkGroups.Add(temp);
        }

        //drinkGroups.Sort((x, y) => x.Count.CompareTo(y.Count));
        drinkGroups.Sort(SortByAscendingListCount);

        foreach (var group in drinkGroups)
        {
            typeCountMap.Add(group[0].GetDrinkType(),group.Count);
            print(group[0].GetDrinkType()+"-"+group.Count);
        }

        return drinkGroups;

    }
    
    
    private int SortByAscendingListCount(List<Drink> x, List<Drink> y)
    {
        return x.Count.CompareTo(y.Count);
    }
    

    public void Fill(List<Drink> drinksList)
    {
        drinkSlots.AddRange(grid.GetCells());


        List<Transform> availableSlots = new List<Transform>(drinkSlots);


        foreach(Drink d in drinksList)
        {
            int randomSlot = Random.Range(0, availableSlots.Count);
            d.transform.parent = availableSlots[randomSlot];
            d.transform.localPosition = Vector3.zero;
            availableSlots.RemoveAt(randomSlot);
            drinks.Add(d);
        }


    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    

}
