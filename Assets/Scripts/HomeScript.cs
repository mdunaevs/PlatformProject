using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class HomeScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool bowser = false;

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToBowserCastleMario(){
        SceneManager.LoadScene("Bowser_Castle");
        bowser = false;
    }

    public void GoToBowserCastleBowser(){
        SceneManager.LoadScene("Bowser_Castle");
        bowser = true;
    }
}
