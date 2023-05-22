using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /**
     * @precondition The next scene in the build order is the main game scene.
     * Loads the next scene in the build order.
     */
    public void PlayGame()
    {
        // Requires that the game scene is the next scene in the build order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Switching scenes to: " + SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex+1).name);
    }

    /** Gracefully quit the game. */
    public void QuitGame()
    {
        Debug.Log("Quitting game!");
        Application.Quit();
    }
}
