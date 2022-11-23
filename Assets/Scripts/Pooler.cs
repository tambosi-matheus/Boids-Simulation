using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler Instance;

    private InGameManager ingameManager;
    [SerializeField] private GameObject prefab;
    public Dictionary<string, List<Object>> pools = new Dictionary<string, List<Object>>();

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        ingameManager = InGameManager.Instance;
        CreatePools();
        ingameManager.ChangeUnityStats("Green", ingameManager.groups[0]);
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
                obj.GetComponentInChildren<SpriteRenderer>().color = group.color;
                Object cell = obj.GetComponent<Object>();
                cell.group = group.name;
                cell.maxSpeed = group.maxSpeed;
                cell.sightRange = group.sightRange;
                cell.alignmentStrength = group.alignment;
                cell.cohesionStrength = group.cohesion;
                cell.separationStrength = group.separation;
                cell.maxAceleration = group.maxAcceleration;
                list.Add(cell);
            }
            pools.Add(group.name, list);
        }
    }

    
}
