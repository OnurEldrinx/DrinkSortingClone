using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            
            List<Transform> neighbors = grid.GetNeighborsOf(cell);

            Dictionary<Container, List<List<Drink>>> containerDrinkGroupMap = new Dictionary<Container, List<List<Drink>>>();

            foreach (var n in neighbors)
            {
               n.gameObject.SetActive(false);

               Container tempContainer = n.GetComponent<GridCell>().content.GetComponent<ContainerSlot>().currentContainer;

               if (tempContainer is not null)
               {
                   List<List<Drink>> tempGroupList = tempContainer.CategorizeDrinks();
                   containerDrinkGroupMap.Add(tempContainer,tempGroupList);

               }
               
            }
            
            List<List<Drink>> thisGroupList = currentContainer.CategorizeDrinks();
            
            containerDrinkGroupMap.Add(currentContainer,thisGroupList);

            
            
            /*for (int i = 0; i < containerDrinkGroupMap.Keys.Count; i++)
            {
                Container c = containerDrinkGroupMap.Keys.ElementAt(i);
                List<Drink> l = containerDrinkGroupMap[c];

                var containers = containerDrinkGroupMap.Where(query => query.Value.Count > l.Count && query.Value[0].GetDrinkType() == l[0].GetDrinkType());

                foreach (var containerCurrent in containers)
                {
                    print(containerCurrent.Key.transform.parent.parent.name);
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
