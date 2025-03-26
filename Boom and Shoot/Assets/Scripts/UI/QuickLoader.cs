using UnityEngine;
using UnityEngine.SceneManagement;

/*
 Script to quickly load between scenes for testing purposes
 */

public class QuickLoader : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("L1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("L2");
        }
    }
}
