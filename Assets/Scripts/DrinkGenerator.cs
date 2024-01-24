using UnityEngine;

public class DrinkGenerator : MonoBehaviour
{
    [SerializeField] private DrinkType type;
    [SerializeField] private GameObject model;
    [SerializeField] private float modelPlacementOffsetY;

    public void Create()
    {
        GameObject parent = new GameObject(type.ToString());
        parent.AddComponent<Drink>().SetType(type);
        GameObject m = Instantiate(model);
        m.transform.parent = parent.transform;
        m.transform.localPosition = Vector3.zero + new Vector3(0, modelPlacementOffsetY, 0);
    }

}

public enum DrinkType
{
    cola,
    fanta,
    energy,
    soda
}