
using System.Collections.Generic;
using System.Linq;
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
            
            print(currentContainerGroups.Count);

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

            foreach (var group in currentContainerGroups)
            {
                
                foreach (var neighborGroup in allGroupsInNeighbors)
                {

                    Container neighborContainer = drinkGroupsContainerMap[neighborGroup];
                    
                    if (group[0].GetDrinkType() == neighborGroup[0].GetDrinkType())
                    {

                        print(group[0].GetDrinkType());

                        int maxMoveCount = neighborContainer.EmptySlotCount();
                        int counter=0;
                        
                        foreach (var drink in group)
                        {
                            if (drink.GetMovementCounter() == 0 && neighborContainer.AreThereAnyEmptySlot())
                            {
                                drink.Animate(currentContainer,neighborContainer,jumpPower,duration,ease,delayCounter);
                                delayCounter += delay;
                                counter++;
                                
                                if (counter == maxMoveCount)
                                {
                                    break;
                                }
                                
                            }
                            
                            
                        }
                        
                    }
                    
                    
                    
                }
                
                
            }


            // compare this with neighbors
            /*foreach (Drink d in currentContainer.GetDrinks())
            {
                foreach (var n in neighbors)
                {

                    Container tempContainer = n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                    if (tempContainer is null) continue;


                    if(currentContainer.GetTypeCount(d.GetDrinkType()) < tempContainer.GetTypeCount(d.GetDrinkType()))
                    {
                        print("case1");

                        //Debug.DrawLine(d.transform.position,tempContainer.transform.position,Color.red,100);
                        if (tempContainer.AreThereAnyEmptySlot() && tempContainer.gameObject.activeInHierarchy) {
                            d.Animate(currentContainer,tempContainer,jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;
                        }
                        else if(currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                        {

                            foreach (var tempDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                            {
                                tempDrink.Animate(tempContainer, currentContainer, jumpPower, duration, ease, delayCounter);
                                delayCounter += delay;
                            }
                            
                            
                        }

                    }
                    else if ((currentContainer.GetTypeCount(d.GetDrinkType()) > tempContainer.GetTypeCount(d.GetDrinkType())))
                    {
                        print("case2");

                        if (tempContainer.GetTypeCount(d.GetDrinkType()) <= 0) continue;

                        foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                        {
                            if (currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                            {
                                
                                neighborDrink.Animate(tempContainer,currentContainer, jumpPower, duration, ease,delayCounter);
                                delayCounter += delay;
                                //currentContainer.UpdateDrinksList();

                            }
                            
                        }
                        
                    }
                    else if((currentContainer.GetTypeCount(d.GetDrinkType()) == tempContainer.GetTypeCount(d.GetDrinkType())))
                    {
                        print("case3");

                        if (currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                        {
                            foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                            {
                                if (currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                                {
                                    
                                    neighborDrink.Animate(tempContainer,currentContainer, jumpPower, duration, ease,delayCounter);
                                    delayCounter += delay;


                                }
                            }
                        }
                        else if (tempContainer.AreThereAnyEmptySlot() && tempContainer.gameObject.activeInHierarchy)
                        {

                            
                            d.Animate(currentContainer,tempContainer, jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;
                            //currentContainer.UpdateDrinksList();

                        }
                    }
                    

                }


            }*/



            //compare neighbours to each other








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
