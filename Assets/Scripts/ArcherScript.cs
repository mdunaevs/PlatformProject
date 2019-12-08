using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherScript : MonoBehaviour
{

    CharacterController characterController;

    public GameObject enemySprite;
    private Animator animator;
    private SpriteRenderer sprite;


    public GameObject arrowPrefab;
    public float power;
    private AudioSource audio;
    public AudioClip arrowSound;
    private int kpcounter = 0;

    public float speed = 1.0f;
    public float gravity = 20.0f;
    private bool faceLeft = true;

    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = enemySprite.GetComponent<Animator>();
        sprite = enemySprite.GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(kpcounter == 0){
            StartCoroutine(KillPlayer());
        }

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

    // Killed by enemy
    private void KillEnemy(){
       characterController.enabled = false;
       gameObject.GetComponent<Collider>().enabled = false;
       animator.SetBool("alive", false);
       Destroy(this.gameObject, 1.0f);
    }

    public void ShootArrow(){
        GameObject arrow;
        arrow = Instantiate(arrowPrefab, transform);

        arrow.transform.parent = null;

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        if(faceLeft){
            //arrow.GetComponent<SpriteRenderer>().flipX = true;
            arrow.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
            rb.AddForce(Vector3.left * power);
        } else {
            //arrow.GetComponent<SpriteRenderer>().flipX = false;
            arrow.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = true;
            rb.AddForce(Vector3.right * power);
        }

        StartCoroutine(RemoveArrow(arrow));
    }

    private void ShootSound(){
        audio.Stop();

        audio.clip = arrowSound;

        audio.Play();
    }


    public IEnumerator RemoveArrow(GameObject arrow){
        yield return new WaitForSeconds(10.0f);
        Destroy(arrow.gameObject);
    }

    public IEnumerator KillPlayer(){

        bool flag = false;
        if (MarioManagerScript.S.gameState == GameState.playing){
            flag = true;
            kpcounter += 1;
        }
        while(flag){
            if (MarioManagerScript.S.gameState != GameState.playing){
                Debug.Log("broke from kill player");
                break;
            }

            animator.SetBool("shoot", true);
            yield return new WaitForSeconds(1.0f);
            ShootArrow();
            // Start the shoot animation
            yield return new WaitForSeconds(1.0f);
            animator.SetBool("shoot", false);
            yield return new WaitForSeconds(1.0f);
            //print("finished yielding");
        }
    }


    void OnControllerColliderHit(ControllerColliderHit collision) {
         //Debug.Log("hit someyhing");
         if(!MarioManagerScript.S.playerIsHittable) return;
         //Debug.Log("Two characters collided");
         if(collision.gameObject.tag == "BackWall"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "BlackHole"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "BowserPlayer"){
               //Debug.Log("Collisiotn Normal: " + collision.normal);
               Debug.Log("Collided with player");
               if(collision.normal.y < -0.6f){
                   //Debug.Log("Kills the enemy");
                   collision.gameObject.GetComponent<HeroScript>().BouncePlayer();
                   KillEnemy();

               } else {
                   //Debug.Log("Kills the player");
                   MarioManagerScript.S.hitsUntilDeath -= 1;
                   kpcounter = 0;
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


// Arrow: https://www.google.com/search?q=arrow+sprite&safe=active&sxsrf=ACYBGNRsEmeCq2ExhKyXYUyBMhEYXYZKXw:1575439606835&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjIpoXxqZvmAhUiTd8KHU53BAoQ_AUoAXoECAsQAw&biw=1440&bih=742#imgrc=B2tcgNBafg70vM:
// Archer: https://www.google.com/search?q=sprite+archer&safe=strict&sxsrf=ACYBGNQi6e80qdQl8JqiE5bUtjJ2xU22SQ:1575436904758&source=lnms&tbm=isch&sa=X&ved=2ahUKEwi0ysvon5vmAhXSs1kKHfhhAwoQ_AUoAXoECA8QAw&biw=1440&bih=742#imgrc=Q5FRhIctEwqHMM:
