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
    [HideInInspector] public bool isRest = false;

    [Header("Enemies")] [SerializeField] private float defaultHp;
    [SerializeField] private float defaultDamage;
    [SerializeField] [Range(0, 99)] private float diffMultiplier;

    [Header("System Statistics")] 
    public UnityEvent<InvokeType, float> onStatistics;
    [SerializeField] private float totalDamage = 0;
    [SerializeField] private float totalKills = 0;
    [SerializeField] private float totalFallDamage = 0;

    private UnityEvent onGameEnded = new UnityEvent();
    public List<Enemy> enemies = new List<Enemy>();

    public MapGenerator mapGenerator;
    public GameObject Player;
    
    public static GameSystem instance;
    
    private void Awake()
    {
        //InitGameSystem();
        mapGenerator = FindFirstObjectByType<MapGenerator>(); 
        
        if(mapGenerator == null)
            Debug.LogError("MapGenerator not found");
        
        if (instance == null) instance = this;
        else Destroy(gameObject);
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
        if(!isRest) mapGenerator.Generate();
        

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
        
        MoveTo(Player, mapGenerator.startPoint, isRest);
        
    }

    public void MoveTo(GameObject obj, Vector3 pos, bool _isRest)
    {
        if(!_isRest)
        {
            mapGenerator.MapTotalReset();
            obj.transform.position = new Vector3(70, -30, 0);
        }
        //Add Extra vfx when move whatev you want
        else
        {
            obj.transform.position = pos;
            mapGenerator.Generate();
        }
        
        isRest = !_isRest;
    }
}
