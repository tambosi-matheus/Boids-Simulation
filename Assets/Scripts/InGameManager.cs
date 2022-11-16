using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    private UIManager uiManager;
    private Pooler pooler;
    [SerializeField] private BoxCollider2D box;

    public static int[] weights = new int[6]
    {
        1, // Alignment
        1, // Cohesion
        1, // Separation
        10, // Sight
        5, // Max Speed
        10 // Max Acceleration
    };

    public string[] groupList =  new string[3] {"Green", "Red", "Blue"};    
    public int activeGroup = 0;


    // Store each flock
    public Group[] groups = new Group[3];

    [System.Serializable]
    public class Group
    {        
        public string name;
        [HideInInspector] public GameObject parent;
        public Color color;
        public int quantity;
        public float
            alignment, cohesion,
            separation, sight,
            maxSpeed, maxAcceleration;
        public bool showVel, showAccel;
        private bool m_active;
        public bool active
        {
            get{return m_active;}
            set
            {
                m_active = value;
                parent.SetActive(value);
            }
        }
    }

    public Group GetGroup(string name) => Array.Find(groups, g => g.name == name);

    private void Awake()
    {
        Instance = this;        
        DontDestroyOnLoad(gameObject);  
        
        box.size = new Vector2(Camera.main.pixelWidth / 5.3f, Camera.main.pixelRect.height / 5.3f);
    }

    void Start()
    {
        pooler = Pooler.Instance;
        uiManager = UIManager.Instance;
        ApplySliderValue(groupList[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            uiManager.ChangeUIState();
    }

    public string GetNextGroup(int step)
    {
        activeGroup += step;
        if (activeGroup >= groupList.Length) activeGroup = 0;
        else if (activeGroup < 0) activeGroup = groupList.Length - 1;
        return groupList[activeGroup];
    }

    public void ApplySliderValue(string name)
    {
        var group = Array.Find(groups, g => g.name.Equals(name));

        if (uiManager == null) return;
        group.alignment = uiManager.alignmentSlider.value;
        group.cohesion = uiManager.cohesionSlider.value;
        group.separation = uiManager.separationSlider.value;
        group.sight = uiManager.sightSlider.value;
        group.maxSpeed = uiManager.maxSpeedSlider.value;
        group.maxAcceleration = uiManager.acceleractionSlider.value;
        group.showVel = uiManager.showVel.isOn;
        group.showAccel = uiManager.showAccel.isOn;

        ChangeUnityStats(name, group);
    }

    public void ChangeUnityStats(string name, Group groupInfo)
    {
        var group = pooler.pools[name];

        foreach(Object obj in group)
        {
            obj.alignmentStrength = groupInfo.alignment;
            obj.cohesionStrength = groupInfo.cohesion;
            obj.separationStrength = groupInfo.separation;
            obj.sightRange = groupInfo.sight;
            obj.maxSpeed = groupInfo.maxSpeed;
            obj.maxAceleration = groupInfo.maxAcceleration;
            obj.showVel = groupInfo.showVel;
            obj.showAccel = groupInfo.showAccel;
        }
    } 
}
