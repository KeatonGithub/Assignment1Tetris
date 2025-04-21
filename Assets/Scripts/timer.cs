using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class timer : MonoBehaviour //defining all the public variables
{

    public float TimeCounter = 300; //how much time
    public TextMeshProUGUI Timer; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (TimeCounter > 0) //counts down the timer and loads scene loss when it reaches 0
        {
            TimeCounter -= Time.deltaTime;
        }
        else if (TimeCounter < 0)
        {
            SceneManager.LoadScene("Loss"); //this is what loads the loss screen when the timer reaches 0
        }

        Timer.text = "" + TimeCounter.ToString("F0");//these strings are whats outputted to the UI elements

    }
}
