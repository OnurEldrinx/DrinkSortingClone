using UnityEngine;
using DG.Tweening;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    public bool animating;
    
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
        if (animating) return;

        if (!target.gameObject.activeInHierarchy || !source.gameObject.activeInHierarchy) {
            return;
        }

        animating = true;

        source.animationPlaying = true;
        target.animationPlaying = true;

        Transform slot = target.GetEmptySlot();

        transform.DOJump(slot.position,jumpPower,1,duration).SetDelay(delay).SetEase(ease).OnComplete(()=>{

            transform.parent = slot;
            transform.localPosition = Vector3.zero;
            transform.localScale = new Vector3(4,1,4);
            source.UpdateDrinksList();
            target.UpdateDrinksList();
            source.animationPlaying = false;
            target.animationPlaying = false;
            animating = false;

        });

    }


   

    

}
