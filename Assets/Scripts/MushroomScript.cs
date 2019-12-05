using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float gravity = 20.0f;
    public bool faceLeft = false;

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }



    // Update is called once per frame
    void Update()
    {
        if(faceLeft){
           moveDirection.x = -speed;
        } else {
           moveDirection.x = speed;
        }

        if(characterController.isGrounded){
           moveDirection.y = 0.0f;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }



    public void PowerUpAction(){
        MarioManagerScript.S.Grow();
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other){
         if(other.gameObject.tag == "Turnaround"){
             faceLeft = !faceLeft;
         }
    }

    void OnControllerColliderHit(ControllerColliderHit collision) {
        if(collision.gameObject.tag == "Plane"){
            Destroy(this.gameObject);
        }
    }
}
