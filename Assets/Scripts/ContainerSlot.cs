using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ContainerSlot : MonoBehaviour
{

    public Container currentContainer;
    public GridInstance grid;

    /*float jumpPower = 0.75f;
    float duration = 0.25f;
    Ease ease = Ease.Linear;
    float delayCounter;
    float delay = 0.2f;*/
    
    private float delayCounter;

    private void Start()
    {
        grid = transform.parent.parent.GetComponent<GridInstance>();
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

            Player.Instance.GetContainers().Add(container);
            Player.Instance.GetContainerSlotMap().TryAdd(this,container);
            
            SpawnManager.Instance.filledSlotCount--;
            if(SpawnManager.Instance.filledSlotCount == 0)
            {
                SpawnManager.Instance.SpawnContainers();
            }
            
            List<Transform> neighbors = grid.GetNeighborsOf(cell);
            
            Dictionary<Drink, Container> drinkContainerMap = new Dictionary<Drink, Container>();

            var drinkGroups = new Dictionary<DrinkType,List<Drink>>();
            
            foreach (var drink  in currentContainer.GetDrinks())
            {

                DrinkType type = drink.GetDrinkType();
                
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

                ContainerSlot neighborContainerSlot = neighbor.GetComponent<GridCell>().content.GetComponent<ContainerSlot>();

                Container n = neighborContainerSlot.currentContainer;

                if (n is null)
                {
                    continue;
                }

                neighborContainers.Add(n);

                foreach (var drink in n.GetDrinks())
                {
                    
                    DrinkType type = drink.GetDrinkType();
                    
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
            allGroups.Sort(Helper.SortByDescendingListCount);


            int containerCounter = 0;
            
            float d = 0.05f;
            float localDuration = 0.25f;
            float timer = 0;
            
            foreach (var group in allGroups)
            {

                delayCounter = 0;
                
                Container target = allContainers.ElementAt(containerCounter);
                containerCounter++;
                containerCounter %= allContainers.Count;
                
                foreach (var drink in group)
                {
                    Container source = drinkContainerMap[drink];

                    if (source.done) {
                        containerCounter--;
                        break;
                    }


                    var drinkTransform = drink.transform;
                    drinkTransform.parent = null;
                    
                    Transform targetSlot;

                    if (target.AreThereAnyEmptySlot())
                    {
                        targetSlot = target.GetEmptySlot();
                    }
                    else
                    {
                        targetSlot = allEmptySlots[^1];
                        target = slotContainerMap[targetSlot];
                    }
                    
                    
                    drinkContainerMap[drink] = target;

                    source.GetDrinks().Remove(drink);
                    target.GetDrinks().Add(drink);



                    drinkTransform.DOJump(targetSlot.position,1,1,localDuration).SetDelay(delayCounter).SetEase(Ease.InOutSine).OnComplete(() =>
                    {
                        drinkTransform.parent = targetSlot;
                        drinkTransform.localPosition = Vector3.zero;
                        drinkTransform.localScale = new Vector3(4,1,4);

                        drinkTransform.DOPunchRotation(drinkTransform.right * 10, 0.15f).SetEase(Ease.OutElastic);

                    });

                    delayCounter += d;
                    timer += d;
                    
                    allEmptySlots.Remove(targetSlot);


                }
                

            }

            Invoke(nameof(UpdateGrid), timer + localDuration);


        }
        else
        {
            containerTransform.DOLocalMove(Vector3.zero, 0.2f);
            //containerTransform.localPosition = Vector3.zero;
        }
        
    }

    private void UpdateGrid()
    {

        var containerSlots = Player.Instance.GetContainerSlotMap();

        delayCounter = 0;
        float d = 0.05f;

        var doneTypes = new HashSet<DrinkType>();

        foreach (var c in containerSlots.Keys)
        {


            if (c is null) { continue; }

            Container current = containerSlots[c];

            var neighbors = grid.GetNeighborsOf(c.transform.parent);

            List<Container> neighborContainers = new List<Container>();

            foreach (var neighbor in neighbors)
            {

                ContainerSlot neighborContainerSlot = neighbor.GetComponent<GridCell>().content.GetComponent<ContainerSlot>();

                Container n = neighborContainerSlot.currentContainer;

                if (n is null)
                {
                    continue;
                }

                neighborContainers.Add(n);

            }

            var currentDrinks = new List<Drink>(current.GetDrinks());

            foreach (var drink in currentDrinks)
            {

                DrinkType drinkType = drink.GetDrinkType();

                Transform drinkTransform = drink.transform;

                foreach (var container in neighborContainers)
                {

                    int typeCount = container.GetDrinks(drinkType).Count;

                    if (typeCount > 0 && typeCount != 6 && typeCount >= current.GetDrinks(drinkType).Count)
                    {

                        if (container.AreThereAnyEmptySlot())
                        {
                            Transform targetSlot = container.GetEmptySlot();

                            container.GetDrinks().Add(drink);
                            current.GetDrinks().Remove(drink);

                            drinkTransform.DOJump(targetSlot.position, 1, 1, 0.25f).SetDelay(delayCounter).SetEase(Ease.InOutSine).OnComplete(() =>
                            {
                                drinkTransform.parent = targetSlot;
                                drinkTransform.localPosition = Vector3.zero;
                                drinkTransform.localScale = new Vector3(4, 1, 4);
                                drinkTransform.DOPunchRotation(drinkTransform.right * 10, 0.15f).SetEase(Ease.OutElastic);

                            });
                            delayCounter += d;
                            doneTypes.Add(drinkType);
                            break;

                        }

                    }

                }

            }

        }

        Invoke(nameof(UpdateContainers),delayCounter + 0.25f);

    }

    private void UpdateContainers()
    {
        print("Update Containers");
        var all = new List<Container>(Player.Instance.GetContainers());

        foreach (var c in all)
        {
            c.CheckEmptyOrMatchInvoker();
        }
    }
    
    public void SetCurrentContainer(Container container)
    {
        currentContainer = null;
    }
    

}
