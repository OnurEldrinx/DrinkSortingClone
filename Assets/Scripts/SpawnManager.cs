using System.Collections.Generic;
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
            c.transform.parent = spawnPositions[i];
            filledSlotCount++;

        }
    }


}
