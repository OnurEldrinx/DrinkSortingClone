using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private List<Drink> drinkList;
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private Container containerPrefab;
    public int filledSlotCount;

    private void Awake()
    {
        SpawnContainers();
    }

    public List<Drink> GetRandomDrinks()
    {
        int count = Random.Range(2,5);

        List<Drink> result = new List<Drink>();

        for (int i=0;i<count;i++)
        {
            int randomDrink = Random.Range(0,drinkList.Count);
            result.Add(Instantiate(drinkList[randomDrink]));
        }

        return result;

    }


    public void SpawnContainers()
    {
        for (int i=0;i<spawnPositions.Count;i++)
        {

            Container c = Instantiate(containerPrefab);
            c.Fill(GetRandomDrinks());
            c.transform.localPosition = spawnPositions[i].position;
            c.transform.localRotation = spawnPositions[i].rotation;
            c.transform.localScale = Vector3.zero;
            var index = i;
            c.transform.DOScale(Vector3.one, 0.45f).SetDelay(0.05f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                c.transform.parent = spawnPositions[index];
            });
            filledSlotCount++;

        }
    }


}
