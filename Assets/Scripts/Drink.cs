using UnityEngine;
using DG.Tweening;

public class Drink : MonoBehaviour
{

    [SerializeField] private DrinkType type;

    public bool animating;
    
    public void SetType(DrinkType t)
    {
        this.type = t;
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
        Player.Instance.animationOnPlay = true;
        

        Transform slot = target.GetEmptySlot();

        transform.DOJump(slot.position,jumpPower,1,duration).SetDelay(delay).SetEase(ease).OnComplete(()=>{
            var t = transform;
            t.parent = slot;
            t.localPosition = Vector3.zero;
            t.localScale = new Vector3(4,1,4);
            source.UpdateDrinksList();
            target.UpdateDrinksList();
            animating = false;
            Player.Instance.animationOnPlay = false;


        });

    }


   

    

}
