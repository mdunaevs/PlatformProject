using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleScript : MonoBehaviour
{

    CharacterController characterController;
    private bool calledBH = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerUpAction(){
        Destroy(this.gameObject);
        if(!calledBH){
            calledBH = true;
            MarioManagerScript.S.BlackHole();
        }


    }
}
