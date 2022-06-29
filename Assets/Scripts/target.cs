using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class target : MonoBehaviour
{
    //Handles the UI elements for the target text and player count 
    public Text targetText;
    public Text playerText;
    public int playerCount;
   
    void Start()
    {
        targetText.gameObject.SetActive(false);
        playerCount = 0;
    }

    //Keeping count of the agents that hit the target
    private void Update()
    {
        playerText.text = playerCount.ToString();
    }

    //Turns the UI target text on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Target Hit");
            targetText.gameObject.SetActive(true);
            playerCount += 1;
            
        }
    }

    //Turns off the UI target text
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            targetText.gameObject.SetActive(false);
            playerCount = 0;
        }
    }


}
