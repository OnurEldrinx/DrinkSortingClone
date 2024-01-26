using UnityEngine;
using DG.Tweening;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    public void SetType(DrinkType type)
    {
        this.type = type;
    }

    public DrinkType GetDrinkType()
    {
        return type;
    }

    public void Animate(Container target,float jumpPower,float duration,Ease ease,float delay)
    {

        Transform slot = target.GetEmptySlot();

        transform.DOJump(slot.position,jumpPower,1,duration).SetDelay(delay).SetEase(ease).OnComplete(()=>{

            transform.parent = slot;
            transform.localPosition = Vector3.zero;
            target.UpdateDrinksList();

        });

    }

}
