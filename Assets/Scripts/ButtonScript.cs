using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void PlaySim()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
