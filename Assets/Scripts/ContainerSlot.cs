using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ContainerSlot : MonoBehaviour
{

    private Container currentContainer;
    public GridInstance grid;

    float jumpPower = 0.75f;
    float duration = 0.25f;
    Ease ease = Ease.Linear;
    float delayCounter;
    float delay = 0.2f;
    
    
    private void Start()
    {
        grid = transform.parent.parent.GetComponent<GridInstance>();
    }

    public bool IsEmpty()
    {
        return currentContainer is null;
    }

    private HashSet<Container> allContainers;
    
    public void AcceptOrReject(Container container)
    {
        Transform t = transform;
        Transform containerTransform = container.transform;
        if (currentContainer is null)
        {
            
            Transform cell = transform.parent;

            containerTransform.parent = null;
            containerTransform.position = t.position;
            containerTransform.rotation = t.rotation;
            container.DisableCollider();
            currentContainer = container;

            SpawnManager.Instance.filledSlotCount--;
            if(SpawnManager.Instance.filledSlotCount == 0)
            {
                SpawnManager.Instance.SpawnContainers();
            }
            
            List<Transform> neighbors = grid.GetNeighborsOf(cell);
            
            
            
            
            Dictionary<Drink, Container> drinkContainerMap = new Dictionary<Drink, Container>();

            var drinkGroups = new Dictionary<DrinkType,List<Drink>>();

            var allTypes = new HashSet<DrinkType>();
            
            foreach (var drink  in currentContainer.GetDrinks())
            {

                DrinkType type = drink.GetDrinkType();

                allTypes.Add(type);
                
                drinkContainerMap.Add(drink,currentContainer);

                if (drinkGroups.TryGetValue(type, out var group))
                {
                    group.Add(drink);
                }
                else
                {
                    drinkGroups.Add(type,new List<Drink>(){drink});
                }
                
            }


            List<Container> neighborContainers = new List<Container>();

            foreach (var neighbor in neighbors)
            {

                Container n = neighbor.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                if (n is null) continue;

                neighborContainers.Add(n);

                foreach (var drink in n.GetDrinks())
                {
                    
                    DrinkType type = drink.GetDrinkType();

                    allTypes.Add(type);
                    
                    drinkContainerMap.TryAdd(drink, n);
                    
                    if (drinkGroups.TryGetValue(type, out var group))
                    {
                        group.Add(drink);
                    }
                    else
                    {
                        drinkGroups.Add(type,new List<Drink>(){drink});
                    }
                    
                }


            }


            if (neighborContainers.Count == 0)
            {
                return;
            }
            
            

            allContainers = new HashSet<Container>(drinkContainerMap.Values);
            var allEmptySlots = new List<Transform>();

            var slotContainerMap = new Dictionary<Transform, Container>();
            
            foreach (var c in allContainers)
            {
                c.ClearAllSlots();
                foreach (var slot in c.GetEmptySlots())
                {
                    slotContainerMap.Add(slot,c);
                    allEmptySlots.Add(slot);
                }
                
            }

            var allGroups = new List<List<Drink>>();
            allGroups.AddRange(drinkGroups.Values);
            allGroups.Sort(SortByDescendingListCount);


            int containerCounter = 0;
            
            delayCounter = 0;
            float d = 0.025f;
            float localDuration = 0.25f;
            
            foreach (var group in allGroups)
            {

                Container target = allContainers.ElementAt(containerCounter);
                containerCounter++;
                containerCounter %= allContainers.Count;
                
                foreach (var drink in group)
                {
                    var drinkTransform = drink.transform;
                    drinkTransform.parent = null;



                    Transform targetSlot;

                    if (target.AreThereAnyEmptySlot())
                    {
                        targetSlot = target.GetEmptySlot();
                    }
                    else
                    {
                        targetSlot = allEmptySlots.Find(s=> s.childCount == 0);
                        target = slotContainerMap[targetSlot];
                    }
                    
                    Container source = drinkContainerMap[drink];
                    drinkContainerMap[drink] = target;

                    source.GetDrinks().Remove(drink);
                    target.GetDrinks().Add(drink);

                    if (target == source)
                    {
                        drinkTransform.parent = targetSlot;
                        drinkTransform.localPosition = Vector3.zero;
                        drinkTransform.localScale = new Vector3(4,1,4);
                    }
                    else
                    {
                        drinkTransform.DOJump(targetSlot.position,1,1,localDuration).SetDelay(delayCounter).SetEase(Ease.InOutSine).OnComplete(() =>
                        {
                            drinkTransform.parent = targetSlot;
                            drinkTransform.localPosition = Vector3.zero;
                            drinkTransform.localScale = new Vector3(4,1,4);
                        
                        });

                        delayCounter += d;

                    }
                    
                    allEmptySlots.Remove(targetSlot);

                }
                

            }

            Invoke(nameof(UpdateContainers), delayCounter + localDuration);






        }
        else
        {
            containerTransform.localPosition = Vector3.zero;
        }
        
    }

    private void UpdateContainers()
    {
        foreach (var c in allContainers)
        {
            c.CheckEmptyOrMatchInvoker();
        }
    }
    
    public int SortByAscendingListCount(List<Drink> x, List<Drink> y)
    {
        return x.Count.CompareTo(y.Count);
    }

    public int SortByDescendingListCount(List<Drink> x, List<Drink> y)
    {
        return y.Count.CompareTo(x.Count);
    }
    public Container GetCurrentContainer()
    {
        return currentContainer;
    }

    public void SetCurrentContainer(Container container)
    {
        currentContainer = null;
    }
    
    

    public GridInstance GetGrid()
    {
        return grid;
    }

}
