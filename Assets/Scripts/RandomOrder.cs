using System.Collections.Generic;
using UnityEngine;

public class RandomOrder
{   
    public static List<T> RandomOrderGenerator<T>(List<T> listToRandom)
    {
        List<T> temp = listToRandom;
        List<T> list = new List<T>();
        while (temp.Count > 0)
        {
            int tempInt = Random.Range(0, temp.Count);
            list.Add(temp[tempInt]);
            temp.RemoveAt(tempInt);
        }
        return list;
    }
}
