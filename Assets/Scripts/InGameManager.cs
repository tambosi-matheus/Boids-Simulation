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
        100, // Sight
        50, // Max Speed
        100 // Max Acceleration
    };

    public string[] groupList =  new string[3] {"Green", "Red", "Blue"};    
    public int activeGroup = 0;


    // Store each flock
    public Group[] groups = new Group[3];

    [System.Serializable]
    public class Group
    {        
        public string name;
        public GameObject parent;
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

    private void Awake()
    {
        Instance = this;
        box.size = new Vector2(Camera.main.pixelWidth / 5.3f, Camera.main.pixelRect.height / 5.3f);
    }

    void Start()
    {
        pooler = Pooler.Instance;
        uiManager = UIManager.Instance;        

        //ApplySliderValue("Green");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            uiManager.ChangeUIState();
    }

    #region Group Management

    public Group GetGroup(string name) => Array.Find(groups, g => g.name == name);

    public float[] GetGroupSliderValues(string name)
    {
        var values = new float[6];
        var group = Array.Find(groups, g => g.name == name);
        values[0] = group.alignment;
        values[1] = group.cohesion;
        values[2] = group.separation;
        values[3] = group.sight;
        values[4] = group.maxSpeed;
        values[5] = group.maxAcceleration;
        return values;
    }

    public bool[] GetGroupBooleands(string name)
    {
        var values = new bool[3];
        var group = Array.Find(groups, g => g.name == name);
        values[0] = group.showVel;
        values[1] = group.showAccel;
        values[2] = group.active;
        return values;
    }

    public string GetNextGroup(int step)
    {
        activeGroup += step;
        if (activeGroup >= groupList.Length) activeGroup = 0;
        else if (activeGroup < 0) activeGroup = groupList.Length - 1;
        return groupList[activeGroup];
    }

    #endregion
    
    public void ApplySliderValue(string name)
    {
        var group = Array.Find(groups, g => g.name.Equals(name));

        //if (uiManager == null) return;
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
            obj.alignmentStrength = groupInfo.alignment * weights[0];
            obj.cohesionStrength = groupInfo.cohesion * weights[1];
            obj.separationStrength = groupInfo.separation * weights[2];
            obj.sightRange = groupInfo.sight * weights[3];
            obj.maxSpeed = groupInfo.maxSpeed * weights[4];
            obj.maxAceleration = groupInfo.maxAcceleration * weights[5];
            obj.showVel = groupInfo.showVel;
            obj.showAccel = groupInfo.showAccel;
        }
    } 
}
