using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowserBossScript : MonoBehaviour
{
    CharacterController characterController;

    public GameObject enemySprite;
    private Animator animator;
    private SpriteRenderer sprite;


    public GameObject fireballPrefab;
    public float power;
    private AudioSource audio;
    public AudioClip fireballSound;
    private int kpcounter = 0;

    public float speed = 0.0f;
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


    public void ShootFireball(){
        GameObject fireball;
        fireball = Instantiate(fireballPrefab,
                               transform.position + new Vector3(0.0f, Random.Range(-1.0f, 2.0f), 0.0f),
                               transform.rotation,
                               transform);

        fireball.transform.parent = null;

        Rigidbody rb = fireball.GetComponent<Rigidbody>();

        Debug.Log("Shoot");
        //fireball.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().flipX = false;
        rb.AddForce(Vector3.left * power);

        StartCoroutine(RemoveFireball(fireball));
    }

    private void ShootSound(){
        audio.Stop();

        audio.clip = fireballSound;

        audio.Play();
    }


    public IEnumerator RemoveFireball(GameObject fireball){
        yield return new WaitForSeconds(10.0f);
        Destroy(fireball.gameObject);
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
            yield return new WaitForSeconds(Random.Range(0.75f, 0.95f));
            ShootFireball();
            // Start the shoot animation
            yield return new WaitForSeconds(Random.Range(0.75f, 0.95f));
            animator.SetBool("shoot", false);
            yield return new WaitForSeconds(Random.Range(0.75f, 0.95f));
            //print("finished yielding");
        }
    }


    void OnControllerColliderHit(ControllerColliderHit collision) {
         //Debug.Log("hit someyhing");
         if(!MarioManagerScript.S.playerIsHittable) return;
         //Debug.Log("Two characters collided");
         if(collision.gameObject.tag == "BackWall"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "Plane"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "BlackHole"){
               Destroy(this.gameObject);
         } else if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "BowserPlayer"){
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
