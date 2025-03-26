using UnityEngine;

/*
 Exits the game
 */

public class quit : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }
}
