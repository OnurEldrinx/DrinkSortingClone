
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

               Container tempContainer = n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

               if (tempContainer is not null)
               {
                   List<List<Drink>> tempGroupList = tempContainer.CategorizeDrinks();
                   //containerDrinkGroupMap.Add(tempContainer,tempGroupList);

               }
               
            }*/
            
            //List<List<Drink>> thisGroupList = currentContainer.CategorizeDrinks();
            
            //containerDrinkGroupMap.Add(currentContainer,thisGroupList);

            float jumpPower = 1;
            float duration = 0.25f;
            Ease ease = Ease.Linear;
            float delayCounter = 0;
            float delay = 0.2f;

            // compare this with neighbors
            foreach(Drink d in currentContainer.GetDrinks())
            {
                foreach (var n in neighbors)
                {

                    Container tempContainer = n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                    if (tempContainer is null) continue;

                    if(currentContainer.GetTypeCount(d.GetDrinkType()) < tempContainer.GetTypeCount(d.GetDrinkType()))
                    {

                        //Debug.DrawLine(d.transform.position,tempContainer.transform.position,Color.red,100);
                        if (tempContainer.AreThereAnyEmptySlot()) {
                        
                            d.Animate(tempContainer,jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;
                        }

                    }
                    else if (currentContainer.GetTypeCount(d.GetDrinkType()) > tempContainer.GetTypeCount(d.GetDrinkType()))
                    {

                        foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                        {
                            if (currentContainer.AreThereAnyEmptySlot())
                            {
                                
                                neighborDrink.Animate(currentContainer, jumpPower, duration, ease,delayCounter);
                                delayCounter += delay;
                            }
                        }


                    }
                    else if(currentContainer.GetTypeCount(d.GetDrinkType()) == tempContainer.GetTypeCount(d.GetDrinkType()))
                    {
                        if (currentContainer.AreThereAnyEmptySlot())
                        {
                            foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                            {
                                if (currentContainer.AreThereAnyEmptySlot())
                                {
                                    
                                    neighborDrink.Animate(currentContainer, jumpPower, duration, ease,delayCounter);
                                    delayCounter += delay;


                                }
                            }
                        }
                        else if (tempContainer.AreThereAnyEmptySlot())
                        {

                            
                            d.Animate(tempContainer, jumpPower, duration, ease,delayCounter);
                            delayCounter += delay;

                        }
                    }

                 
                    

                }


            }

            currentContainer.UpdateDrinksList();





            /*delayCounter = 0;

            //compare neighbours to each other
            foreach (Transform n in neighbors)
            {
                List<Transform> others = new List<Transform>(neighbors);
                others.Add(cell);
                others.Remove(n);

                if (n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer is null) continue;

                foreach(Drink d in n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer.GetDrinks())
                {

                    foreach (Transform o in others)
                    {
                        Container tempContainer = o.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

                        if (tempContainer is null) continue;

                        if (currentContainer.GetTypeCount(d.GetDrinkType()) < tempContainer.GetTypeCount(d.GetDrinkType()))
                        {

                            //Debug.DrawLine(d.transform.position,tempContainer.transform.position,Color.red,100);
                            if (tempContainer.AreThereAnyEmptySlot())
                            {
                                
                                d.Animate(tempContainer, jumpPower, duration, ease,delayCounter);
                                delayCounter += delay;
                                currentContainer.RemoveDrink(d);


                            }

                        }
                        else if (currentContainer.GetTypeCount(d.GetDrinkType()) > tempContainer.GetTypeCount(d.GetDrinkType()))
                        {

                            foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                            {
                                if (currentContainer.AreThereAnyEmptySlot())
                                {
                                    
                                    neighborDrink.Animate(currentContainer, jumpPower, duration, ease,delayCounter);
                                    delayCounter += delay;
                                    tempContainer.RemoveDrink(neighborDrink);


                                }
                            }


                        }
                        else if (currentContainer.GetTypeCount(d.GetDrinkType()) == tempContainer.GetTypeCount(d.GetDrinkType()))
                        {
                            if (currentContainer.AreThereAnyEmptySlot())
                            {
                                foreach (Drink neighborDrink in tempContainer.GetDrinks(d.GetDrinkType()))
                                {
                                    if (currentContainer.AreThereAnyEmptySlot())
                                    {
                                        
                                        neighborDrink.Animate(currentContainer, jumpPower, duration, ease,delayCounter);
                                        delayCounter += delay;
                                        tempContainer.RemoveDrink(neighborDrink);

                                    }
                                }
                            }
                            else if (tempContainer.AreThereAnyEmptySlot())
                            {

                                
                                d.Animate(tempContainer, jumpPower, duration, ease,delayCounter);
                                delayCounter += delay;
                                currentContainer.RemoveDrink(d);

                            }
                        }




                    }

                }


                

            }*/



        }
        else
        {
            containerTransform.localPosition = Vector3.zero;
        }
        
    }

    public Container GetCurrentContainer()
    {
        return currentContainer;
    }
    
    
    

    public GridInstance GetGrid()
    {
        return grid;
    }

}
