using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Container : MonoBehaviour
{

    [SerializeField] private GridInstance grid;
    [SerializeField] private List<Transform> drinkSlots;
    [SerializeField] private List<Drink> drinks;
    private BoxCollider boxCollider;
    private HashSet<DrinkType> existingTypes;
    private Dictionary<DrinkType, int> typeCountMap;
    [SerializeField] private List<Transform> emptySlots;

    public LayerMask containerSlotMask;

    public bool animationPlaying;

    public bool empty;
    private bool allSame;

    private void Start()
    {
        if(grid is not null && drinkSlots.Count == 0) drinkSlots.AddRange(grid.GetCells());
        emptySlots = new List<Transform>();

        boxCollider = GetComponent<BoxCollider>();


     

        if(drinks.Count == 0)
        {
            existingTypes = new HashSet<DrinkType>();

            foreach (var t in drinkSlots.Where(t => t.childCount > 0))
            {
                Drink temp = t.GetChild(0).GetComponent<Drink>();
                drinks.Add(temp);
                existingTypes.Add(temp.GetDrinkType());
            }

            
        }
        
        

        foreach (var t in drinkSlots.Where(t => t.childCount == 0))
        {
            emptySlots.Add(t);
        }

        CategorizeDrinks();
        

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
            if(temp.Count == 0) { continue; }
            drinkGroups.Add(temp);
        }

        drinkGroups.Sort(SortByAscendingListCount);

        foreach (var group in drinkGroups)
        {
            if(group.Count == 0) { continue; }
            typeCountMap.Add(group[0].GetDrinkType(),group.Count);
        }

        return drinkGroups;

    }

    public void UpdateDrinksList()
    {
        drinks = new List<Drink>();
        existingTypes.Clear();
        typeCountMap.Clear();
        emptySlots.Clear();

        foreach (var t in drinkSlots.Where(t => t.childCount == 0))
        {
            emptySlots.Add(t);
        }

        foreach (var t in drinkSlots.Where(t => t.childCount > 0))
        {
            Drink temp = t.GetChild(0).GetComponent<Drink>();
            drinks.Add(temp);
            existingTypes.Add(temp.GetDrinkType());
        }
        

        float invokeTime;

        if (Player.Instance.animationOnPlay)
        {
            invokeTime = 0.5f;
        }
        else
        {
            invokeTime = 0.25f;
        }

        Invoke(nameof(CheckEmptyOrMatchInvoker),invokeTime);
        
    }

    public void CheckEmptyOrMatchInvoker()
    {
        //Invoke(nameof(CheckIsContainerEmpty), 0.25f);

        CheckIsContainerEmpty();

        if (empty) return;

        //Invoke(nameof(CheckIsAllSame), 0.25f);

        CheckIsAllSame();

    }

    private void CheckIsContainerEmpty()
    {
        if (drinks.Count == 0)
        {

            empty = true;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, float.MaxValue, containerSlotMask))
            {
                hit.transform.GetComponent<ContainerSlot>().SetCurrentContainer(null);
            }


            transform.parent = null;
            //Invoke(nameof(DisableGameObject), 0.25f);
            DisableGameObject();

        }
    }

    private void CheckIsAllSame()
    {

        if (empty) return;

        allSame = true;
        DrinkType sample = drinks[0].GetDrinkType();
        foreach (var d in drinks)
        {
            if (d.GetDrinkType() != sample)
            {
                allSame = false;
                break;
            }
        }

        if (allSame && (drinks.Count == 6) && (emptySlots.Count == 0))
        {
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, float.MaxValue, containerSlotMask))
            {
                hit.transform.GetComponent<ContainerSlot>().SetCurrentContainer(null);
            }


            transform.parent = null;
            //Invoke(nameof(DisableGameObject), 0.25f);
            DisableGameObject();

        }
    }

    private void DisableGameObject()
    {
        if (animationPlaying) return;
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        });
        
    }

    
    
    private int SortByAscendingListCount(List<Drink> x, List<Drink> y)
    {
        return x.Count.CompareTo(y.Count);
    }

    public int GetTypeCount(DrinkType type)
    {
        return GetDrinks(type).Count;
    }

    public DrinkType MostExistingType()
    {
        DrinkType result = drinks[0].GetDrinkType();


        var n = CategorizeDrinks();

        n.Sort(SortByAscendingListCount);


        result = n.Last()[0].GetDrinkType();

        return result;
    }

    public List<Drink> GetDrinks()
    {
        return drinks;
    }

    public List<Drink> GetDrinks(DrinkType type)
    {

        List<Drink> result = new List<Drink>();

        foreach (Drink d in drinks)
        {
            if(d.GetDrinkType() == type)
            {
                result.Add(d);
            }
        }

        return result;

    }

    public void InsertDrink(Drink drink)
    {
        drinks.Add(drink);
        existingTypes.Add(drink.GetDrinkType());
        
        CategorizeDrinks();
    }

    public void RemoveDrink(Drink drink)
    {
        drinks.Remove(drink);

        existingTypes.Clear();
        foreach (Drink d in drinks)
        {
            existingTypes.Add(d.GetDrinkType());
        }

        CategorizeDrinks();
    }

    public void Fill(List<Drink> drinksList)
    {
        if(drinkSlots.Count == 0)
        {
            drinkSlots.AddRange(grid.GetCells());
        }


        List<Transform> availableSlots = new List<Transform>(drinkSlots);
        existingTypes = new HashSet<DrinkType>();

        foreach(Drink d in drinksList)
        {
            int randomSlot = Random.Range(0, availableSlots.Count);
            d.transform.parent = availableSlots[randomSlot];
            d.transform.localPosition = Vector3.zero;
            d.transform.localRotation = Quaternion.Euler(Vector3.zero);
            availableSlots.RemoveAt(randomSlot);
            drinks.Add(d);
            existingTypes.Add(d.GetDrinkType());
        }


    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

    public Transform GetEmptySlot()
    {
        Transform s = emptySlots[0];
        emptySlots.Remove(s);
        return s;
    }

    public bool AreThereAnyEmptySlot()
    {
        return emptySlots.Count > 0;
    }

    public int EmptySlotCount()
    {
        return emptySlots.Count;
    }

    public void ClearAllSlots()
    {
        emptySlots.Clear();
        emptySlots = new List<Transform>(drinkSlots);
    }

    public List<Transform> GetEmptySlots()
    {
        return emptySlots;
    }

}
