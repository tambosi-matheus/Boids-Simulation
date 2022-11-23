using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    private UIManager uiManager;
    private Pooler pooler;
    [SerializeField] private BoxCollider2D box;

    // Screen
    public static Vector2 boidBounds, gameSize;
    public static float distanceToScreenBound;

    // Stars Background
    [SerializeField] private ParticleSystem stars;

    public static int[] weights = new int[7]
    {
        1, // Alignment
        1, // Cohesion
        1, // Separation
        100, // Sight Range
        1, // Sight Angle
        100, // Max Speed
        400 // Max Acceleration
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
            separation, sightRange, sightAngle,
            maxSpeed, maxAcceleration;
        public bool showVel, showAccel, showSight;
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
        gameSize = new Vector2(Camera.main.aspect * 200, 200);
        boidBounds = gameSize * 0.9f;

        var starsShape = stars.shape;
        starsShape.scale = (Vector3)gameSize * 3;
        stars.emission.SetBurst(0, new ParticleSystem.Burst(0f, gameSize.y * 20));
        stars.Play();
    }

    void Start()
    {
        pooler = Pooler.Instance;
        uiManager = UIManager.Instance;
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
        var values = new float[7];
        var group = Array.Find(groups, g => g.name == name);
        values[0] = group.alignment;
        values[1] = group.cohesion;
        values[2] = group.separation;
        values[3] = group.sightRange;
        values[4] = group.sightAngle;
        values[5] = group.maxSpeed;
        values[6] = group.maxAcceleration;
        return values;
    }

    public bool[] GetGroupBooleands(string name)
    {
        var values = new bool[4];
        var group = Array.Find(groups, g => g.name == name);
        values[0] = group.showVel;
        values[1] = group.showAccel;
        values[2] = group.showSight;
        values[3] = group.active;
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
        group.sightRange = uiManager.sightSlider.value;
        group.sightAngle = uiManager.sightRangeSlider.value;
        group.maxSpeed = uiManager.maxSpeedSlider.value;
        group.maxAcceleration = uiManager.acceleractionSlider.value;
        group.showVel = uiManager.showVel.isOn;
        group.showAccel = uiManager.showAccel.isOn;
        group.showSight = uiManager.showSight.isOn;

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
            obj.sightImageSize.sizeDelta = new Vector2(groupInfo.sightRange * 2 * weights[3], groupInfo.sightRange * 2 * weights[3]);
            obj.sightRange = groupInfo.sightRange * weights[3];
            obj.sightAngle = groupInfo.sightAngle * weights[4];
            obj.maxSpeed = groupInfo.maxSpeed * weights[5];
            obj.maxAceleration = groupInfo.maxAcceleration * weights[6];
            obj.showVel = groupInfo.showVel;
            obj.showAccel = groupInfo.showAccel;
            obj.showSight = groupInfo.showSight;
        }
    } 
}
