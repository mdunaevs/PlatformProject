using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchScript : MonoBehaviour
{
    CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void PowerUpAction(){
        MarioManagerScript.S.Glitchable();
        Destroy(this.gameObject);
    }


}
