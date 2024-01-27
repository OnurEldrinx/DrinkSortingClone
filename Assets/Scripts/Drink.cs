using UnityEngine;
using DG.Tweening;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    private int movementCounter = 0;
    
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
        movementCounter++;
        Transform slot = target.GetEmptySlot();

        transform.DOJump(slot.position,jumpPower,1,duration).SetDelay(delay).SetEase(ease).OnComplete(()=>{

            transform.parent = slot;
            transform.localPosition = Vector3.zero;
            target.UpdateDrinksList();
            source.UpdateDrinksList();
            movementCounter = 0;
        });

    }

    public int GetMovementCounter()
    {
        return movementCounter;
    }

}
