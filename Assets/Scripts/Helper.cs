using System.Collections.Generic;
using UnityEngine;

public class Helper:MonoBehaviour
{
        
    public static int SortByAscendingListCount(List<Drink> x, List<Drink> y)
    {
        return x.Count.CompareTo(y.Count);
    }
    
    public static int SortByDescendingListCount(List<Drink> x, List<Drink> y)
    {
        return y.Count.CompareTo(x.Count);
    }
    
}
