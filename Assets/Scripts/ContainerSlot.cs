
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
            float delayCounter = 0;
            float delay = 0.2f;


            List<List<Drink>> currentContainerGroups = currentContainer.CategorizeDrinks();
            

            Dictionary<List<Drink>, Container> drinkGroupsContainerMap = new Dictionary<List<Drink>, Container>();

            foreach (var neighbor in neighbors)
            {
                
                Container n = neighbor.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;
                
                if (n is null) continue;

                
                List<List<Drink>> groups = n.CategorizeDrinks();
                
                foreach (var group in groups)
                {
                    drinkGroupsContainerMap.Add(group,n);
                }
                
            }
            

            List<List<Drink>> allGroupsInNeighbors = new List<List<Drink>>(drinkGroupsContainerMap.Keys);

            currentContainerGroups.Sort(SortByAscendingListCount);
            allGroupsInNeighbors.Sort(SortByDescendingListCount);




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


                // Most Stable
                delayCounter = 0;
                // each drink looks for the most
                foreach (var drink in drinkContainerMap.Keys)
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

                    if (target != source && target.AreThereAnyEmptySlot() && drink.GetMovementCounter() == 0)
                    {
                        drink.Animate(source, target, jumpPower, duration, ease, delayCounter);
                        delayCounter += delay;
                        continue;
                    }
                    /*else if (target != source && !target.AreThereAnyEmptySlot() && source.AreThereAnyEmptySlot() && ((source.GetDrinks(type).Count + target.GetDrinks(type).Count) >= 6))
                    {

                        List<Drink> targetDrinks = target.GetDrinks(type);
                        foreach (var d in targetDrinks)
                        {

                            if (source.AreThereAnyEmptySlot() && !d.moved)
                            {
                                d.Animate(target, source, jumpPower, duration, ease, delayCounter);
                                delayCounter += delay;
                            }

                        }

                    }*/

                }
            

           

        }
        else
        {
            containerTransform.localPosition = Vector3.zero;
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
