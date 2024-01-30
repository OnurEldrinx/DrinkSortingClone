
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ContainerSlot : MonoBehaviour
{

    private Container currentContainer;
    public GridInstance grid;

    private void Start()
    {
        grid = transform.parent.parent.GetComponent<GridInstance>();
    }

    public bool IsEmpty()
    {
        return currentContainer is null;
    }

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
           

            float jumpPower = 1;
            float duration = 0.25f;
            Ease ease = Ease.Linear;
            float delayCounter;
            float delay = 0.2f;



            Dictionary<Drink, Container> drinkContainerMap = new Dictionary<Drink, Container>();

            foreach (var drink  in currentContainer.GetDrinks())
            {
                drinkContainerMap.Add(drink,currentContainer);
            }


            foreach (var neighbor in neighbors)
            {

                Container n = neighbor.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                if (n is null) continue;

                foreach (var drink in n.GetDrinks())
                {
                    if (!drinkContainerMap.ContainsKey(drink))
                    {
                        drinkContainerMap.Add(drink, n);
                    }
                }


            }


            List<Container> allContainers = new List<Container>(drinkContainerMap.Values);
            List<Drink> allDrinks = new List<Drink>(drinkContainerMap.Keys);



            for (int i=0;i<5;i++)
            {
                // Most Stable
                delayCounter = 0;
                allDrinks = new List<Drink>(drinkContainerMap.Keys);
                allContainers = new List<Container>(drinkContainerMap.Values);

                // each drink looks for the most
                foreach (var drink in allDrinks)
                {

                    Container source = drinkContainerMap[drink];

                    Container target = allContainers[0];

                    DrinkType type = drink.GetDrinkType();

                    foreach (Container c in allContainers)
                    {

                        if (c.GetDrinks(type).Count > target.GetDrinks(type).Count)
                        {

                            target = c;

                        }
                        else if (c.GetDrinks(type).Count == target.GetDrinks(type).Count)
                        {

                            if (c != target)
                            {
                                target = c;
                            }

                        }

                    }

                    List<Drink> targetDrinks = target.GetDrinks(type);
                    List<Drink> sourceDrinks = source.GetDrinks(type);

                    if (target != source && target.AreThereAnyEmptySlot() && !drink.animating)
                    {
                        drink.Animate(source, target, jumpPower, duration, ease, delayCounter);
                        delayCounter += delay;
                        drinkContainerMap[drink] = target;
                    }
                    else if (target != source && !target.AreThereAnyEmptySlot() && source.AreThereAnyEmptySlot() && drink.GetDrinkType() == type && (target.GetDrinks(type).Count + source.GetDrinks(type).Count < 6))
                    {

                        targetDrinks = target.GetDrinks(type);
                        print(targetDrinks.Count + " - " + type);
                        foreach (var d in targetDrinks)
                        {

                            if (source.AreThereAnyEmptySlot() && !d.animating)
                            {
                                d.Animate(target, source, jumpPower, duration, ease, delayCounter);
                                delayCounter += delay;
                                drinkContainerMap[d] = source;

                            }
                            else if (currentContainer.AreThereAnyEmptySlot() && !d.animating)
                            {
                                d.Animate(target, currentContainer, jumpPower, duration, ease, delayCounter);
                                delayCounter += delay;
                                drinkContainerMap[d] = currentContainer;

                            }

                        }

                    }


                }
            }

                



            


            


        }
        else
        {
            containerTransform.localPosition = Vector3.zero;
        }
        
    }

    private void SendMinToMax()
    {

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
