using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

     void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Player"){
            Destroy(this.gameObject);
            MarioManagerScript.S.hitsUntilDeath -= 1;
            if(MarioManagerScript.S.hitsUntilDeath == 2){
                MarioManagerScript.S.ShrinkGlitch();
                StartCoroutine(MarioManagerScript.S.HitDelay());
            } else if(MarioManagerScript.S.hitsUntilDeath == 1) {
                MarioManagerScript.S.Shrink();
                StartCoroutine(MarioManagerScript.S.HitDelay());
            } else if(MarioManagerScript.S.hitsUntilDeath == 0){
                collision.gameObject.GetComponent<HeroScript>().SetPlayerDead(true);
                MarioManagerScript.S.RegisterDeath();
                MarioManagerScript.S.deathBeingRegistered = true;
            }
        } else {
            Destroy(this.gameObject);
        }

     }
}
