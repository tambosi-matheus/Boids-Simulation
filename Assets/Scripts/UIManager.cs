using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public Manager m;

    [System.Serializable]
    public class Group
    {
        public TextMeshProUGUI name;
        public Slider alignmentSlider, cohesionSlider, separationSlider, sightSlider, maxSpeedSlider, acceleractionSlider;
        public TextMeshProUGUI alignmentText, cohesionText, separationText, sightText, maxSpeedText, accelerationText;
        public TextMeshProUGUI activeStatus;
    }

    public Group[] groups;

    private void Start()
    {
        OnSliderChange();
        OnSetGroupActive();
    }

    public void OnSliderChange()
    {
        foreach (Group g in groups)
        {
            g.alignmentText.SetText(g.alignmentSlider.value.ToString("F2"));
            g.cohesionText.SetText(g.cohesionSlider.value.ToString("F2"));
            g.separationText.SetText(g.separationSlider.value.ToString("F2"));
            g.sightText.SetText(g.sightSlider.value.ToString("F2"));
            g.maxSpeedText.SetText(g.maxSpeedSlider.value.ToString("F2"));
            g.accelerationText.SetText(g.acceleractionSlider.value.ToString("F2"));
            m.ApplySliderValue(g.name.text.ToString());
        }
    }

    public void OnSetGroupActive()
    {
        for(int i = 0; i < groups.Length; i++)
        {
            bool active = m.groupStates[i];
            if(active)
                groups[i].activeStatus.SetText("Disable");
            else
                groups[i].activeStatus.SetText("Enable");
        }
    }
}
