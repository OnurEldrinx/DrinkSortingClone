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
    [SerializeField] private List<Transform> emptySlots;

    public LayerMask containerSlotMask;
    
    public bool empty;
    public bool allSame;

    public bool done;
    
    private void Start()
    {
        if(grid is not null && drinkSlots.Count == 0) drinkSlots.AddRange(grid.GetCells());
        emptySlots = new List<Transform>();

        boxCollider = GetComponent<BoxCollider>();
        

        if(drinks.Count == 0)
        {

            foreach (var t in drinkSlots.Where(t => t.childCount > 0))
            {
                Drink temp = t.GetChild(0).GetComponent<Drink>();
                drinks.Add(temp);
            }

            
        }
        
        

        foreach (var t in drinkSlots.Where(t => t.childCount == 0))
        {
            emptySlots.Add(t);
        }

    }

    public void UpdateDrinksList()
    {
        drinks = new List<Drink>();
        emptySlots.Clear();

        foreach (var t in drinkSlots.Where(t => t.childCount == 0))
        {
            emptySlots.Add(t);
        }

        foreach (var t in drinkSlots.Where(t => t.childCount > 0))
        {
            Drink temp = t.GetChild(0).GetComponent<Drink>();
            drinks.Add(temp);
        }


        var invokeTime = Player.Instance.animationOnPlay ? 0.5f : 0.25f;

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
            
            drinks.Clear();

            ContainerSlot slot=null;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, float.MaxValue, containerSlotMask))
            {
                slot = hit.transform.GetComponent<ContainerSlot>();
                slot.SetCurrentContainer(null);
            }


            transform.parent = null;
            //Invoke(nameof(DisableGameObject), 0.25f);
            Player.Instance.GetContainers().Remove(this);
            if (slot is not null)
            {
                Player.Instance.GetContainerSlotMap().Remove(slot);
            }
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
            drinks.Clear();

            ContainerSlot containerSlot = null;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, float.MaxValue, containerSlotMask))
            {
                containerSlot = hit.transform.GetComponent<ContainerSlot>();
                containerSlot.SetCurrentContainer(null);
            }

            done = true;
            float delayCounter = 0;
            foreach (var slot in drinkSlots)
            {
                slot.GetChild(0).DOPunchPosition(Vector3.up * 0.5f, 0.25f).SetDelay(delayCounter);
                delayCounter += 0.05f;
            }


            transform.parent = null;
            Player.Instance.GetContainers().Remove(this);
            if (containerSlot is not null)
            {
                Player.Instance.GetContainerSlotMap().Remove(containerSlot);
            }

            Invoke(nameof(DisableGameObject), delayCounter + 0.25f);
            //DisableGameObject();

        }
        
    }

    private void DisableGameObject()
    {
        done = true;
        //if (animationPlaying) return;
        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        });
        
    }
    
    public int GetTypeCount(DrinkType type)
    {
        return GetDrinks(type).Count;
    }

    public List<Drink> GetDrinks()
    {
        return drinks;
    }

    public List<Drink> GetDrinks(DrinkType type)
    {
        var result = new List<Drink>(drinks.FindAll(s=>s.GetDrinkType() == type));
        return result;
    }

    public void InsertDrink(Drink drink)
    {
        drinks.Add(drink);
    }

    public void RemoveDrink(Drink drink)
    {
        drinks.Remove(drink);
    }

    public void Fill(List<Drink> drinksList)
    {
        if(drinkSlots.Count == 0)
        {
            drinkSlots.AddRange(grid.GetCells());
        }


        List<Transform> availableSlots = new List<Transform>(drinkSlots);

        foreach(Drink d in drinksList)
        {
            int randomSlot = Random.Range(0, availableSlots.Count);
            var t = d.transform;
            t.parent = availableSlots[randomSlot];
            t.localPosition = Vector3.zero;
            d.transform.localRotation = Quaternion.Euler(Vector3.zero);
            availableSlots.RemoveAt(randomSlot);
            drinks.Add(d);
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
