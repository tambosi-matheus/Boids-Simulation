using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public InGameManager manager;


    [SerializeField] private Canvas canvas;
    public TextMeshProUGUI groupName;
    public Slider 
        alignmentSlider, cohesionSlider, separationSlider, 
        sightSlider, maxSpeedSlider, acceleractionSlider;
    [SerializeField] private TextMeshProUGUI 
        alignmentText, cohesionText, separationText, 
        sightText, maxSpeedText, accelerationText;
    public TextMeshProUGUI activeStatus;
    public Toggle showVel, showAccel;

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

    public void ChangeUIState() => canvas.enabled = !canvas.enabled;

    public void UpdateUI()
    {        
        alignmentText.SetText(alignmentSlider.value.ToString("F2"));
        cohesionText.SetText(cohesionSlider.value.ToString("F2"));
        separationText.SetText(separationSlider.value.ToString("F2"));
        sightText.SetText(sightSlider.value.ToString("F2"));
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

    public void GetGroupData()
    {
        var group = manager.GetGroup(groupName.text);
        alignmentSlider.value = group.alignment;
        cohesionSlider.value = group.cohesion;
        separationSlider.value = group.separation;
        sightSlider.value = group.sight;
        maxSpeedSlider.value = group.maxSpeed;
        acceleractionSlider.value = group.maxAcceleration;
        showVel.isOn = group.showVel;
        showAccel.isOn = group.showAccel;

        if (group.active)
            activeStatus.SetText("Disable Group");
        else
            activeStatus.SetText("Enable Group");
    }

    public void ChangeGroup(int index)
    {
        var group = manager.GetNextGroup(index);
        groupName.SetText(group);
        GetGroupData();
        //UpdateUI();
    }
}
