using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
   
    public void clickStart(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void clickMenuSelect(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}