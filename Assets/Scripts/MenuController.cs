using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartTutorial(int number)
    {
        SceneManager.LoadScene("Tutorial scene " + number.ToString());
    }


    public void StartCampaign(int number)
    {
        SceneManager.LoadScene("Camaign scene " + number.ToString());
    }


    public void StartEndless(int number)
    {
        SceneManager.LoadScene("Endless scene " + number.ToString());
    }
    

    public void Exit()
    {
        Application.Quit();
    }
}
