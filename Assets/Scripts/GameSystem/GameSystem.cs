using System;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
    }

    void MoveTo(GameObject obj, Vector3 pos, bool toRest)
    {
        if (toRest) mapGenerator.MapTotalReset();
        else mapGenerator.Generate();
        //Add Extra vfx when move whatev you want
        obj.transform.position = pos;
    }
}
