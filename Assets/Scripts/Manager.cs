using System;
using System.Collections.Generic;
using UnityEngine;
using static Pooler;

public class Manager : MonoBehaviour
{
    Pooler pooler;

    public List<bool> groupStates;

    public static Manager Instance;

    public UIManager ui;

    private void Awake()
    {        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        pooler = Pooler.Instance;

        groupStates = new List<bool>();

        for (int i = 0; i < pooler.pools.Count; i++)
        {
            groupStates.Add(false);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for(int i = 0; i < pooler.pools.Count; i++)
                ChangeGroupState(i);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeGroupState(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeGroupState(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeGroupState(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeGroupState(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeGroupState(4);
    }

    public void ChangeGroupState(int index)
    {
        groupStates[index] = !groupStates[index];

        string groupName = pooler.groupsPools[index].name;
        bool state = groupStates[index];

        if (!pooler.pools.ContainsKey(groupName))
            return;
        List<GameObject> pool = pooler.pools[groupName];
        foreach (GameObject obj in pool)
        {
            obj.SetActive(state);
        }
    }

    public void ApplySliderValue(string name)
    {
        GroupPool g = Array.Find(pooler.groupsPools, g1 => g1.name.Equals(name));
        UIManager.Group groupUI = Array.Find(ui.groups, g2 => g2.name.text.Equals(g.name));
            

        g.alignmentStrength = groupUI.alignmentSlider.value;
        g.cohesionStrength = groupUI.cohesionSlider.value;
        g.separationStrength = groupUI.separationSlider.value;
        g.sightRange = groupUI.sightSlider.value;
        g.maxSpeed = groupUI.maxSpeedSlider.value;
        g.maxAceleration = groupUI.acceleractionSlider.value;

        ChangeGroupStats(name, g);
    }

    public void ChangeGroupStats(string name, GroupPool groupInfo)
    {
        List<GameObject> group = pooler.pools[name];

        foreach(GameObject obj in group)
        {
            Object script = obj.GetComponent<Object>();
            script.alignmentStrength = groupInfo.alignmentStrength;
            script.cohesionStrength = groupInfo.cohesionStrength;
            script.separationStrength = groupInfo.separationStrength;
            script.sightRange = groupInfo.sightRange;
            script.maxSpeed = groupInfo.maxSpeed;
            script.maxAceleration = groupInfo.maxAceleration;
        }
    }
}
