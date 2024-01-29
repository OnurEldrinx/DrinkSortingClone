using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    private int movementCounter = 0;

    public bool moved;
    
    public void SetType(DrinkType type)
    {
        this.type = type;
    }

    public DrinkType GetDrinkType()
    {
        return type;
    }

    public void Animate(Container source,Container target,float jumpPower,float duration,Ease ease,float delay)
    {

        if (!target.gameObject.activeSelf && !source.gameObject.activeSelf) {
            DOTween.Clear();
            return;
        }

        moved = true;

        movementCounter++;
        Transform slot = target.GetEmptySlot();

        transform.DOJump(slot.position,jumpPower,1,duration).SetDelay(delay).SetEase(ease).OnComplete(()=>{

            transform.parent = slot;
            transform.localPosition = Vector3.zero;
            source.UpdateDrinksList();
            target.UpdateDrinksList();
            movementCounter = 0;
            moved = false;
        });

    }


    private void ResetMovementCounter()
    {
        movementCounter = 0;
    }

    public int GetMovementCounter()
    {
        return movementCounter;
    }

    public Container FindTargetContainerFor(Container source,List<Container> containers)
    {

        Container result = source;

        DrinkType t = GetDrinkType();

        foreach (Container c in containers)
        {

            if((c.GetTypeCount(t) >= result.GetTypeCount(t)) && result.GetTypeCount(t) != 0)
            {

               result = c;
               
            }

          

        }



        return result;
    }

}
