
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

            //Dictionary<Container, List<List<Drink>>> containerDrinkGroupMap = new Dictionary<Container, List<List<Drink>>>();

            /*foreach (var n in neighbors)
            {
               //n.gameObject.SetActive(false);

               
               if (tempContainer is not null)
               {
                   List<List<Drink>> tempGroupList = tempContainer.CategorizeDrinks();
                   containerDrinkGroupMap.Add(tempContainer,tempGroupList);

               }
               
            }*/
            
            /*List<List<Drink>> thisGroupList = currentContainer.CategorizeDrinks();
            
            containerDrinkGroupMap.Add(currentContainer,thisGroupList);


            foreach(Container c in containerDrinkGroupMap.Keys)
            {

                List<List<Drink>> tempGroup = containerDrinkGroupMap[c];


                tempGroup.Sort(SortByAscendingListCount);

                foreach(List<Drink> g in tempGroup)
                {

                    

                }


            }*/



            float jumpPower = 1;
            float duration = 0.25f;
            Ease ease = Ease.Linear;
            float delayCounter = 0;
            float delay = 0.2f;





            // compare this with neighbors
            foreach (Drink d in currentContainer.GetDrinks())
            {
                foreach (var n in neighbors)
                {

                    Container tempContainer = n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                    if (tempContainer is null) continue;


                    if((currentContainer.GetTypeCount(d.GetDrinkType()) < tempContainer.GetTypeCount(d.GetDrinkType())))
                    {
                        print("case1");

                        //Debug.DrawLine(d.transform.position,tempContainer.transform.position,Color.red,100);
                        if (tempContainer.AreThereAnyEmptySlot() && tempContainer.gameObject.activeInHierarchy) {
                        
                            d.Animate(currentContainer,tempContainer,jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;
                            //currentContainer.UpdateDrinksList();
                        }
                        else if(currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                        {
                            d.Animate(tempContainer, currentContainer, jumpPower, duration, ease, delayCounter);
                            delayCounter += delay;
                        }

                    }

                    if ((currentContainer.GetTypeCount(d.GetDrinkType()) > tempContainer.GetTypeCount(d.GetDrinkType())))
                    {
                        print("case2");

                        if (tempContainer.GetTypeCount(d.GetDrinkType()) < 0) continue;

                        foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                        {
                            if (currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                            {
                                
                                neighborDrink.Animate(tempContainer,currentContainer, jumpPower, duration, ease,delayCounter);
                                delayCounter += delay;
                                //currentContainer.UpdateDrinksList();

                            }
                            else if(tempContainer.AreThereAnyEmptySlot() && tempContainer.gameObject.activeInHierarchy)
                            {
                                neighborDrink.Animate(currentContainer, tempContainer, jumpPower, duration, ease, delayCounter);
                                delayCounter += delay;
                                //continue;
                            }
                        }


                    }

                    if((currentContainer.GetTypeCount(d.GetDrinkType()) == tempContainer.GetTypeCount(d.GetDrinkType())))
                    {
                        print("case3");

                        if (currentContainer.AreThereAnyEmptySlot())
                        {
                            foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                            {
                                if (currentContainer.AreThereAnyEmptySlot() && currentContainer.gameObject.activeInHierarchy)
                                {
                                    
                                    neighborDrink.Animate(tempContainer,currentContainer, jumpPower, duration, ease,delayCounter);
                                    delayCounter += delay;
                                    //currentContainer.UpdateDrinksList();


                                }
                                else { continue; }
                            }
                        }
                        else if (tempContainer.AreThereAnyEmptySlot() && tempContainer.gameObject.activeInHierarchy)
                        {

                            
                            d.Animate(currentContainer,tempContainer, jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;
                            //currentContainer.UpdateDrinksList();

                        }
                    }

                 
                                //currentContainer.UpdateDrinksList();


                }


            }


            delayCounter = 0;

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
