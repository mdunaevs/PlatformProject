using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroScript : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private bool onSlope = false;

    private Animator animator;
    private SpriteRenderer sprite;
    private bool faceRight = true;


    private Vector3 moveDirection = Vector3.zero;

    public AudioClip playerJumpSound;
    private AudioSource audio;

    public GameObject heroSprites;


    //public bool isPlayerDead = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        animator = heroSprites.GetComponent<Animator>();
        sprite = heroSprites.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // For transition
        animator.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        if(Input.GetAxis("Horizontal") < 0 && faceRight){
            faceRight = false;
            sprite.flipX = true;
        } else if(Input.GetAxis("Horizontal") > 0 && !faceRight){
            faceRight = true;
            sprite.flipX = false;
        }

        if(MarioManagerScript.S.gameState == GameState.playing){
            if (characterController.isGrounded)
            {
                // We are grounded, so recalculate
                // move direction directly from axes

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
                moveDirection *= speed;

                if(onSlope){ moveDirection.y -= 100.0f; }

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    MakeJumpSound();
                    animator.SetBool("jumping", true);
                }
             } else {
                moveDirection.x += Input.GetAxis("Horizontal") * 0.8f;
                moveDirection.x = Mathf.Clamp(moveDirection.x, -speed, speed);
                animator.SetBool("jumping", false);
             }

             // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
             // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
             // as an acceleration (ms^-2)
             moveDirection.y -= gravity * Time.deltaTime;

             // Move the controller
             characterController.Move(moveDirection * Time.deltaTime);

             if(MarioManagerScript.S.canGlitch && Input.GetKeyDown(KeyCode.Tab)){
                GlitchMovement();
             }
          }
      }

      private void GlitchMovement(){
          Vector3 currPos = this.transform.position;
          float glitchAmt = -6.0f;
          if(faceRight){
              glitchAmt = 6.0f;
          }
          currPos += new Vector3(glitchAmt, 0.0f, 0.0f);
          this.transform.position = currPos;
      }

      private void MakeJumpSound(){
          audio.Stop();
          audio.clip = playerJumpSound;
          audio.Play();
      }

      void OnTriggerEnter(Collider other){
          if(other.gameObject.tag == "SlopeTrigger"){
              onSlope = true;
          }
      }

      void OnTriggerExit(Collider other){
          if(other.gameObject.tag == "SlopeTrigger"){
              onSlope = false;
          }
      }


      void OnControllerColliderHit(ControllerColliderHit collision) {
          //Debug.Log("Two characters collided");

          if(collision.gameObject.tag == "Plane"){
              MarioManagerScript.S.RegisterDeath();
          } else if(collision.moveDirection == Vector3.up){

              if(collision.gameObject.tag == "Platform" && (moveDirection.y > 0.0f)){
                  moveDirection.y = 0.0f; // reset our motion
              }
              if(collision.gameObject.tag == "Brick" && (moveDirection.y > 0.0f)){
                  moveDirection.y = 0.0f; // reset our motion
                  collision.gameObject.GetComponent<BrickScript>().BrickHit();
              }

              if(collision.gameObject.tag == "LuckyBlock" && (moveDirection.y > 0.0f)){
                  moveDirection.y = 0.0f; // reset our motion
                  collision.gameObject.GetComponent<LuckyBlockScript>().LuckyBlockHit();
              }
          } else if (collision.gameObject.tag == "1up") {
              collision.gameObject.GetComponent<LifeScript>().PowerUpAction();
          } else if (collision.gameObject.tag == "Mushroom") {
              collision.gameObject.GetComponent<MushroomScript>().PowerUpAction();
          } else if (collision.gameObject.tag == "Glitch") {
              collision.gameObject.GetComponent<GlitchScript>().PowerUpAction();
          }

      }

      public void BouncePlayer(){
          moveDirection.y = 10.0f;
      }

      public void SetPlayerDead(bool isDead){
          animator.SetBool("dead", isDead);
      }

}
