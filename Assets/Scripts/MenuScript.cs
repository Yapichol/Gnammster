using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{

    // Goes to the "GameScene" when "PlayButton" is pressed   
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        SceneManager.UnloadSceneAsync("MenuScene");
    }


    // Exits the menu and ends the game when the "QuitButton" is pressed 
    public void QuitGame()
    {
        Application.Quit();
    }
}
