using UnityEngine;
using UnityEngine.SceneManagement;

/*
 Loads a new scene when triggered
 */

public class sceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
