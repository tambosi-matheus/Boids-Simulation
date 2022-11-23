using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public InGameManager manager;

    [SerializeField] private GameObject menu;
    public TextMeshProUGUI groupName;
    public Slider 
        alignmentSlider, cohesionSlider, separationSlider, 
        sightSlider, sightRangeSlider, maxSpeedSlider, acceleractionSlider;
    [SerializeField] private TextMeshProUGUI 
        alignmentText, cohesionText, separationText, 
        sightText, sightRangeText, maxSpeedText, accelerationText;
    public TextMeshProUGUI activeStatus;
    public Toggle showVel, showAccel, showSight;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetGroupData();
        UpdateUI();
        OnSetGroupActive();
    }

    public void ChangeUIState() => menu.SetActive(!menu.activeSelf);
    

    public void UpdateUI()
    {
        alignmentText.SetText(alignmentSlider.value.ToString("F2"));
        cohesionText.SetText(cohesionSlider.value.ToString("F2"));
        separationText.SetText(separationSlider.value.ToString("F2"));
        sightText.SetText(sightSlider.value.ToString("F2"));
        sightRangeText.SetText(sightRangeSlider.value.ToString("F2"));
        maxSpeedText.SetText(maxSpeedSlider.value.ToString("F2"));
        accelerationText.SetText(acceleractionSlider.value.ToString("F2"));
        
        manager.ApplySliderValue(groupName.text);        
    }

    public void OnSetGroupActive()
    {
        var group = manager.GetGroup(groupName.text);
        group.active = !group.active;
        if(group.active)
            activeStatus.SetText("Disable Group");
        else
            activeStatus.SetText("Enable Group");
    }

    // Very dirty way to get data, but every time a slider has its value changed
    // the engine calls the attached script (UpdateUI()) before all values are set
    // then the sliders change the base class values before it sets the slider
    // didn't find a better way to solve other then refactoring the whole code
    public void GetGroupData()
    {
        var values = manager.GetGroupSliderValues(groupName.text);
        var booleans = manager.GetGroupBooleands(groupName.text);
        alignmentSlider.value = values[0];
        cohesionSlider.value = values[1];
        separationSlider.value = values[2];
        sightSlider.value = values[3];
        sightRangeSlider.value = values[4];
        maxSpeedSlider.value = values[5];
        acceleractionSlider.value = values[6];
        showVel.isOn = booleans[0];
        showAccel.isOn = booleans[1];
        showSight.isOn = booleans[2];

        if (booleans[3])
            activeStatus.SetText("Disable Group");
        else
            activeStatus.SetText("Enable Group");
    }

    public void ChangeGroup(int index)
    {
        var group = manager.GetNextGroup(index);
        groupName.SetText(group);
        GetGroupData();
        UpdateUI();
    }
}
