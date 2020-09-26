using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField]
    public GameObject genericPrefab;

    public GroupPool[] groupsPools;

    public Dictionary<string, List<GameObject>> pools = new Dictionary<string, List<GameObject>>();

    [System.Serializable]
    public class GroupPool
    {
        public string name;
        public Color color;
        public int quantity;

        [HideInInspector]
        public Transform parent;

        public float sightRange = 60;
        public float maxSpeed = 25;
        public float maxAceleration;
        public float separationStrength = 1;
        public float alignmentStrength = .1f;
        public float cohesionStrength = 1;
        public float destinationSpeed = 1;
    }

    #region Singleton

    public static Pooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    private void Start()
    {
        CreatePools();
    }


    private void CreatePools()
    {
        foreach (GroupPool p in groupsPools)
        {
            List<GameObject> list = new List<GameObject>();
            GameObject parent = new GameObject();
            parent.transform.SetParent(GameObject.FindGameObjectWithTag("Respawn").transform);
            parent.name = p.name;
            p.parent = parent.transform;
            for (int i = 0; i < p.quantity; i++)
            {
                GameObject obj = Instantiate(genericPrefab);
                obj.transform.SetParent(p.parent);
                obj.GetComponent<Object>().group = p.name;
                obj.GetComponent<Renderer>().material.color = p.color;
                Object objScript = obj.GetComponent<Object>();
                objScript.maxSpeed = p.maxSpeed;
                objScript.sightRange = p.sightRange;
                objScript.alignmentStrength = p.alignmentStrength;
                objScript.cohesionStrength = p.cohesionStrength;
                objScript.separationStrength = p.separationStrength;
                objScript.maxAceleration = p.maxAceleration;
                objScript.destinationSpeed = p.destinationSpeed;


                obj.SetActive(false);
                list.Add(obj);
            }
            pools.Add(p.name, list);
        }
    }

    
}
