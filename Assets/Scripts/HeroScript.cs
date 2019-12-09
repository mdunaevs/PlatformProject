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

    public GameObject coinPrefab;


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

             if(MarioManagerScript.S.canGlitch && characterController.isGrounded && Input.GetKeyDown(KeyCode.Tab)){
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
              MarioManagerScript.S.Shrink();
          } else if(collision.gameObject.tag == "BowserEnemy"){
              MarioManagerScript.S.RegisterDeath();
              MarioManagerScript.S.Shrink();

          } else if(collision.gameObject.tag == "DropButton"){
              collision.gameObject.GetComponent<Collider>().enabled = false;
              MarioManagerScript.S.DefeatBowser();
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
              MarioManagerScript.S.hitsUntilDeath = 2;
              collision.gameObject.GetComponent<MushroomScript>().PowerUpAction();
          } else if (collision.gameObject.tag == "Glitch") {
              MarioManagerScript.S.hitsUntilDeath = 3;
              collision.gameObject.GetComponent<GlitchScript>().PowerUpAction();
          } else if (collision.gameObject.tag == "BlackHole") {
              collision.gameObject.GetComponent<BlackHoleScript>().PowerUpAction();
          } else if(collision.gameObject.tag == "EnterPipe" &&
                    characterController.isGrounded && Input.GetKeyDown(KeyCode.DownArrow)){
              Debug.Log("Should enter pipe");
              Vector3 newPos = collision.gameObject.GetComponent<TeleportScript>().TeleportTo(this.transform.position.x);
              MarioManagerScript.S.WarpPipeAction(newPos);
          } else if(collision.gameObject.tag == "ExitPipe" &&
                    characterController.isGrounded && Input.GetKeyDown(KeyCode.RightArrow)){
              Debug.Log("Should exit into pipe");
              Vector3 newPos = collision.gameObject.GetComponent<TeleportScript>().TeleportOut();
              MarioManagerScript.S.WarpPipeAction(newPos);
          } else if(collision.gameObject.tag == "Coin"){
              Destroy(collision.gameObject);
              MarioManagerScript.S.IncrementCoins();
              CoinAnimation();
          } else if(collision.gameObject.tag == "levelEnd"){
              MarioManagerScript.S.level += 1;
              if(MarioManagerScript.S.level == 2){
                  MarioManagerScript.S.GoToLevelOne();
              } else if(MarioManagerScript.S.level == 3){
                  MarioManagerScript.S.GoToLevelTwo();
              } else if(MarioManagerScript.S.level == 4){
                  MarioManagerScript.S.GoToLevelThree();
              } else if(MarioManagerScript.S.level == 5){
                  MarioManagerScript.S.GoToLoseScreen();
              }

          }

      }

      public void RemoveCoin(GameObject coin){
          Debug.Log("Entering coroutine");

          //yield WaitForSeconds(1.0f);
          //coin.GetComponent<Animator>().SetBool("hitBlock", false);
          //Destroy(this.gameObject)
          Debug.Log("Exiting coroutine");
          Destroy(coin.gameObject, 0.38f);

      }

      private void CoinAnimation(){
          Debug.Log("Entered coin animation");
          GameObject coin;
          coin = Instantiate(coinPrefab,
                            transform.position + new Vector3(2.0f, 1.0f, 2.0f),
                            transform.rotation,
                            transform);
          coin.transform.localScale = new Vector3(1.7f, 1.0f, 1.0f);
          coin.transform.parent = null;
          coin.GetComponent<Collider>().enabled = false;

          coin.GetComponent<Animator>().SetBool("hitBlock", true);

          RemoveCoin(coin);
          //Destroy(coin.gameObject);
          //coin.gameObject.active = false;
      }

      public void BouncePlayer(){
          moveDirection.y = 10.0f;
      }

      public void SetPlayerDead(bool isDead){
          animator.SetBool("dead", isDead);
      }

}
