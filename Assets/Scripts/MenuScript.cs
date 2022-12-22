using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{

    // Goes to the "GameScene" when "PlayButton" is pressed   
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        //Debug.Log("PlayButton pressed");
        SceneManager.UnloadSceneAsync("MenuScene");
        //Debug.Log("MenuScene unloaded");
    }


    // Exits the menu and ends the game when the "QuitButton" is pressed 
    public void QuitGame()
    {
        //Debug.Log("QuitButton pressed");
        Application.Quit();
    }
}
