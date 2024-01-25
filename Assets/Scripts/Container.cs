using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{

    [SerializeField] private GridInstance grid;
    [SerializeField] private List<Transform> drinkSlots;
    [SerializeField] private List<Drink> drinks;
    private BoxCollider boxCollider;
    private void Start()
    {
        if(grid is not null) drinkSlots.AddRange(grid.GetCells());

        boxCollider = GetComponent<BoxCollider>();

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
