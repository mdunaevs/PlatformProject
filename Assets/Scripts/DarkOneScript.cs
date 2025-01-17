﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkOneScript : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    //public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private bool faceLeft = true;


    private bool isDead = false;
    private float deathTimer = 0.0f;
    private float timeToDestroy = 1.0f;

    private Vector3 moveDirection = Vector3.zero;

    public GameObject enemySprite;

    private Animator animator;
    private SpriteRenderer sprite;

    private int lives = 3;
    private bool hittable = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = enemySprite.GetComponent<Animator>();
        sprite = enemySprite.GetComponent<SpriteRenderer>();
    }

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



    void OnTriggerEnter(Collider other){
         if(other.gameObject.tag == "Turnaround"){
             faceLeft = !faceLeft;
             sprite.flipX = !sprite.flipX;
         }
    }

    private void KillEnemy(){
       characterController.enabled = false;
       gameObject.GetComponent<Collider>().enabled = false;
       animator.SetBool("dead", true);
       Destroy(this.gameObject, 1.0f);

       MarioManagerScript.S.DefeatDarkOne();
    }

    public IEnumerator Invinsible(){
        hittable = false;
        for(int i = 0; i < 8; i++){
            this.transform.GetChild(0).gameObject.active = false;
            yield return new WaitForSeconds(0.125f);
            Debug.Log("Turning sprite off");
            this.transform.GetChild(0).gameObject.active = true;
            yield return new WaitForSeconds(0.125f);
        }
        hittable = true;
    }

    void OnControllerColliderHit(ControllerColliderHit collision) {
         if(!MarioManagerScript.S.playerIsHittable) return;
         //Debug.Log("Two characters collided");
         if(collision.gameObject.tag == "Enemy"){
               faceLeft = !faceLeft;
         } else if(collision.gameObject.tag == "BackWall"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "BlackHole"){
               Destroy(this.gameObject);
         } else if(hittable && collision.gameObject.tag == "Player" || collision.gameObject.tag == "BowserPlayer"){
               //Debug.Log("Collisiotn Normal: " + collision.normal);

               if(collision.normal.y < -0.6f){
                   //Debug.Log("Kills the enemy");
                   collision.gameObject.GetComponent<HeroScript>().BouncePlayer();
                   lives -= 1;
                   animator.SetBool("hit", true);
                   if(lives == 0){
                      KillEnemy();
                   } else {
                      speed += 4.0f;
                      StartCoroutine(Invinsible());
                      animator.SetBool("hit", false);
                   }

               } else {
                   //Debug.Log("Kills the player");
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
               }

         }
    }
}

// Dark one: https://www.google.com/search?q=dark+one+sprite&safe=strict&sxsrf=ACYBGNSaKO49w-h7dnbd-aBbefCu1uAJFQ:1575869181558&source=lnms&tbm=isch&sa=X&ved=2ahUKEwi9yZ2W6qfmAhWk1FkKHTZEBjEQ_AUoAXoECAwQAw&biw=1440&bih=742#imgrc=JqYoPGJ2nPzxEM:
