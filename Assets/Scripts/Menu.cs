﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string sceneToLoad;
    public void GoToMain()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
