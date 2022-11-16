using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Persistent Audio Button
    private bool musicPlaying = true;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Image musicImage;
    [SerializeField] private Color musicOn, musicOff;
    [SerializeField] private Toggle musicToggle;

    private void Awake()
    {
        if (Instance != null)       
            Destroy(gameObject);
        else
        {

        Instance = this;
        DontDestroyOnLoad(this);        
        }
    }

    private void Update()
    {
        // Still need to understand better how to handle persistent data through scenes
        // This is a very bad way to find the same button in different scenes,
        // try other solution first
        if (musicToggle == null)
        {
            var music = GameObject.FindGameObjectWithTag("Music");
            musicToggle = music.GetComponent<Toggle>();
            musicToggle.isOn = musicPlaying;
            musicImage = music.GetComponent<Image>();
            ChangeMusicStatus();
            musicToggle.onValueChanged.RemoveAllListeners();
            musicToggle.onValueChanged.AddListener(delegate { ChangeMusicStatus(); });
        }
    }
    public void ChangeMusicStatus()
    {
        musicPlaying = musicToggle.isOn;
        if (musicPlaying)
        {
            musicImage.color = musicOn;
            musicSource.UnPause();
        }
        else
        {
            musicImage.color = musicOff;
            musicSource.Pause();
        }
    }    
}
