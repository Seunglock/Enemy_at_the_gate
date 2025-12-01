using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;

    
    public Transform[][] paths;

    void Awake()
    {
        instance = this;

     
        int count = transform.childCount;
        paths = new Transform[count][];

        for (int i = 0; i < count; i++)
        {
            Transform pathGroup = transform.GetChild(i);

           
            int wpCount = pathGroup.childCount;
            paths[i] = new Transform[wpCount];

            for (int j = 0; j < wpCount; j++)
            {
                paths[i][j] = pathGroup.GetChild(j);
            }
        }
    }


    public Transform[] GetPath(int pathIndex)
    {
        return paths[pathIndex];
    }
}
