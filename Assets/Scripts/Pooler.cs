using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler Instance;

    private InGameManager ingameManager;
    [SerializeField] private GameObject prefab;
    private Vector3 spawnPosition;
    public Dictionary<string, List<Object>> pools = new Dictionary<string, List<Object>>();

    private void Awake()
    {
        Instance = this;
        
        spawnPosition = 
            Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        spawnPosition.z = 0;        
    }



    private void Start()
    {
        ingameManager = InGameManager.Instance;
        CreatePools();
    }


    private void CreatePools()
    {
        foreach (InGameManager.Group group in ingameManager.groups)
        {
            var list = new List<Object>();
            var parent = new GameObject();
            parent.name = group.name;
            group.parent = parent;
            parent.SetActive(false);
            for (int i = 0; i < group.quantity; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.SetParent(group.parent.transform);
                obj.GetComponent<SpriteRenderer>().color = group.color;
                Object cell = obj.GetComponent<Object>();
                cell.group = group.name;
                cell.maxSpeed = group.maxSpeed;
                cell.sightRange = group.sight;
                cell.alignmentStrength = group.alignment;
                cell.cohesionStrength = group.cohesion;
                cell.separationStrength = group.separation;
                cell.maxAceleration = group.maxAcceleration;


                //obj.SetActive(false);
                list.Add(cell);
            }
            pools.Add(group.name, list);
        }
    }

    
}
