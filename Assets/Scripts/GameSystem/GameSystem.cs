using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InvokeType
{
    Damaged,
    Fall,
    Kill,
}

public class GameSystem : MonoBehaviour
{
    [SerializeField] public int currlevel = 0;
    [SerializeField] public int maxLevel = 7;

    [Header("Enemies")] [SerializeField] private float defaultHp;
    [SerializeField] private float defaultDamage;
    [SerializeField] [Range(0, 99)] private float diffMultiplier;

    [Header("System Statistics")] public UnityEvent<InvokeType, float> onStatistics;
    [SerializeField] private float totalDamage = 0;
    [SerializeField] private float totalKills = 0;
    [SerializeField] private float totalFallDamage = 0;

    private UnityEvent onGameEnded;
    public List<Enemy> enemies = new List<Enemy>();

    MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = FindFirstObjectByType<MapGenerator>();
    }

    private void Statics(InvokeType invokeType, float damage)
    {
        switch (invokeType)
        {
            case InvokeType.Damaged:
                totalDamage += damage;
                break;
            case InvokeType.Fall:
                totalFallDamage += damage;
                break;
            case InvokeType.Kill:
                totalKills += damage;
                break;
            
        }
    }

private void OnEnable()
    {
        mapGenerator.onMapGenerated.AddListener(InitGameSystem);
        onStatistics.AddListener(Statics);
        
    }
    private void OnDisable()
    {
        mapGenerator.onMapGenerated.RemoveListener(InitGameSystem);
        onStatistics.RemoveListener(Statics);
    }


    //Add this to player when he interacts with door or smth that makes him to fo to the map with mapgen.
    public void InitGameSystem()
    {
        currlevel += 1;
        if(currlevel >= maxLevel)
        {
            onGameEnded.Invoke();
            return;
        }
        
        foreach (Enemy e in enemies)
        {
            float setHp = defaultHp;
            float setDamage = defaultHp;
            for (int i = 0; i < currlevel; i++)
            {
                setHp *= (100 + diffMultiplier); 
                
            }
            e.SetEnemyStats(setHp, setDamage);
        }
        
        //MoveTo(Player, mapGenerator.startPoint, false);
    }

    void MoveTo(GameObject obj, Vector3 pos, bool isRest)
    {
        if(isRest)
        {
            mapGenerator.MapTotalReset();
            obj.transform.position = new Vector3(70, -30, 0);
        }
        //Add Extra vfx when move whatev you want
        else obj.transform.position = pos;
    }
}
