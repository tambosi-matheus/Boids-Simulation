using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(this);    

    public string sceneToLoad;

    public void GoToScene() => SceneManager.LoadScene(sceneToLoad);

    public void GoToScene(string sccene) => SceneManager.LoadScene(sccene);
}
