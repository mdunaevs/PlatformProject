using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other){
      if(other.gameObject.tag == "Player"){
          other.transform.SetParent(null);
      }
    }
}


/*
Create and animate sprite sheet for character. Make animations

Make it pretty
*/
