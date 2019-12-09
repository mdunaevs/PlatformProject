using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState {setup, getReady, playing, gameOver, hit};

public class MarioManagerScript : MonoBehaviour
{

    public GameState gameState = GameState.setup;

    public GameObject player;
    public GameObject cam;

    public AudioClip playerDeathSound;
    private AudioSource audio;

    public int amtLives = 3;
    public int amtCoinsCollected = 0;

    public TextMeshProUGUI livesLeftMessage;

    public bool deathBeingRegistered = false;
    public bool canGlitch = false;

    public int hitsUntilDeath = 1;

    public bool playerIsHittable = true;

    public GameObject blackHolePrefab;
    private GameObject bh;

    public bool insideSideRoom = false;

    public int level = 1;

    private bool isLoaded = false;

    public GameObject platforms;
    public GameObject platformsPrefab1;
    public GameObject platformsPrefab2;
    public GameObject platformsPrefab3;
    public GameObject platformsPrefab4;
    public GameObject platformsPrefab5;

    public GameObject enemies;
    public GameObject enemiesPrefab1;
    public GameObject enemiesPrefab2;
    public GameObject enemiesPrefab3;
    public GameObject enemiesPrefab4;
    public GameObject enemiesPrefab5;

    public bool useBowser = true;

    public static MarioManagerScript S; // Singleton


    void Awake(){
        if(MarioManagerScript.S != null){
            Destroy(this.gameObject);
        }
        S = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        audio = GetComponent<AudioSource>();


        InitRound();
        PlaySong();
    }

    public void PlaySong(){
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(level == 2){
            player = player;
            if(isLoaded){
              Start();
            }
            isLoaded = false;
        } else if(level == 3){
            player = player;
            if(isLoaded){
              Start();
            }
            isLoaded = false;
        }  else if(level == 4){
            player = player;
            if(isLoaded){
              Start();
            }
            isLoaded = false;
        }  else if(level == 5){
            player = player;
            if(isLoaded){
              Start();
            }
            isLoaded = false;
        }
    }

    public void ResetPlayerPosition(){
        Vector3 resetPos = new Vector3(-15.0f, 6.0f, 0.0f);
        player.transform.position = resetPos;
    }

    public void ResetCameraPosition(){
        Vector3 resetPos = new Vector3(0.0f, 3.0f, -150.0f);
        cam.transform.position = resetPos;
    }

    public IEnumerator ReadyBlink(){
    for (int i = 0; i < 3; i++){
        player.active = false;
        yield return new WaitForSeconds(0.5f);
        player.active = true;
        yield return new WaitForSeconds(0.5f);
    }
    gameState = GameState.playing;
  }


    public void ReinstantiatePlatforms(){
        if(platforms != null){
            Debug.Log("deleting platforms");
            Destroy(platforms.gameObject);
        }
        if(level == 1){
            platforms = Instantiate(platformsPrefab1,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 2){
            platforms = Instantiate(platformsPrefab2,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 3){
            platforms = Instantiate(platformsPrefab3,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 4){
            platforms = Instantiate(platformsPrefab4,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 5){
            platforms = Instantiate(platformsPrefab5,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        }
    }

    public void ReinstantiateEnemies(){
        if(enemies != null){
            Debug.Log("deleting enemies");
            Destroy(enemies.gameObject);
        }
        if(level == 1){
            enemies = Instantiate(enemiesPrefab1,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 2){
            enemies = Instantiate(enemiesPrefab2,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 3){
            enemies = Instantiate(enemiesPrefab3,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 4){
            enemies = Instantiate(enemiesPrefab4,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        } else if (level == 5){
            enemies = Instantiate(enemiesPrefab5,
                                    transform.position + new Vector3(0.0f, 2.75f, 0.0f),
                                    transform.rotation,
                                    transform);
        }
    }

    public void InitRound(){
        if(player == null){
            if(useBowser){
                player = GameObject.FindGameObjectWithTag("BowserPlayer");
                GameObject.FindGameObjectWithTag("Player").active = false;
            } else {
                player = GameObject.FindGameObjectWithTag("Player");
                GameObject.FindGameObjectWithTag("BowserPlayer").active = false;
            }

        }
        if(cam == null){
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }
        // if(livesLeftMessage == null){
        //     livesLeftMessage = GameObject.FindGameObjectWithTag("Display").;
        // }
        ResetPlayerPosition();
        ResetCameraPosition();
        ReinstantiatePlatforms();
        ReinstantiateEnemies();
        deathBeingRegistered = false;
        hitsUntilDeath = 1;
        //player.GetComponent<HeroScript>().SetPlayerDead(false);

        gameState = GameState.getReady;
        livesLeftMessage.text = "" + amtLives;

        StartCoroutine(ReadyBlink());
    }

    public IEnumerator BowserFace(float time){
        for(int i = 0; i < 1; i++){
            yield return new WaitForSeconds(time);
        }

        gameState = GameState.setup;
        InitRound();
    }

    public void DeathDelay(){
        StartCoroutine(BowserFace(1.0f));
    }

    private void MakeDeathSound(){
        audio.Stop();
        audio.clip = playerDeathSound;
        audio.Play();
    }

    public void WarpPipeAnimation(){
        Debug.Log("Should be doing some animation noe");
    }

    public void WarpPipeAction(Vector3 pos){
        if(level == 2){
          WarpPipeAnimation();
          player.transform.position = pos;
          return;
        }
        insideSideRoom = !insideSideRoom;
        WarpPipeAnimation();
        player.transform.position = pos;
    }

    public void RegisterDeath(){
        gameState = GameState.hit;

        if(deathBeingRegistered) return;
        if(bh != null) Destroy(bh.gameObject);
        canGlitch = false;
        //MakeDeathSound();
        amtLives -= 1;

        if(amtLives == 0){
            //Debug.Log("Lost the game!");
            GoToLoseScreen();
        } else {
            DeathDelay();
        }
    }

    public void GainLife(){
        amtLives += 1;
        livesLeftMessage.text = "" + amtLives;
    }

    public IEnumerator HitDelay(){
      Debug.Log("player giot hit");
      playerIsHittable = false;
      for(int i = 0; i < 6; i++){
          player.transform.GetChild(0).gameObject.active = false;
          yield return new WaitForSeconds(0.125f);
          Debug.Log("Turning sprite off");
          player.transform.GetChild(0).gameObject.active = true;
          yield return new WaitForSeconds(0.125f);
      }
      //player.transform.GetChild(0).gameObject.active = true;
      Debug.Log("exiting hit delay");
      playerIsHittable = true;
    }

    // Growing from small mario to large mario
    public void Grow(){
        Debug.Log("Mario is meant to grow now");
        player.gameObject.transform.localScale = new Vector3(1.0f, 3.0f, 1.0f);
        player.GetComponent<CharacterController>().center = new Vector3(0.0f, 0.1f, 0.0f);
    }

    // Shrinking from large Mario to small mario
    public void Shrink(){
        Debug.Log("Mario is meant to shrink now");
        player.gameObject.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Shrinking from glitch mario to normal mario
    public void ShrinkGlitch(){
        Debug.Log("Mario is meant to shrink from glich to normal now");
        canGlitch = false;
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Growing by getting glitch skin
    public void GlicthableSkin(){
        // TO-DO: Sprite skin of mario is glitch skin
        player.gameObject.transform.localScale = new Vector3(1.0f, 3.0f, 1.0f);
        player.GetComponent<CharacterController>().center = new Vector3(0.0f, 0.1f, 0.0f);
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    public void Glitchable(){
        Debug.Log("Mario is meant to glitch now");
        GlicthableSkin();
        canGlitch = true;
    }

    public void BlackHoleSkin(){
        player.transform.Find("Mario_animation").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
    }

    public void ShrinkBlackHole(){
        player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }


    public void InitializeBlackHole(){
        bh = Instantiate(blackHolePrefab, player.gameObject.transform);
        Destroy(bh.gameObject, 10.0f);
    }

    public void BlackHole(){
        Debug.Log("Mario should have a black hole now");
        //BlackHoleSkin();
        InitializeBlackHole();
        //ShrinkBlackHole();
    }

    public void IncrementCoins(){
        amtCoinsCollected += 1;
        if(amtCoinsCollected == 100){
            GainLife();
            amtCoinsCollected = 0;
        }
        Debug.Log(amtCoinsCollected);
    }

    public void GoToLoseScreen(){
        SceneManager.LoadScene("Lose");
        level = 1;
    }

    public void GoToLevelOne(){
        SceneManager.LoadScene("Level 1");
        isLoaded = true;
    }

    public void GoToLevelTwo(){
        SceneManager.LoadScene("Level 2");
        isLoaded = true;
    }

    public void GoToLevelThree(){
        SceneManager.LoadScene("Level 3");
        isLoaded = true;
    }
}

/*
P1
dark one boss

P2

black hole animation

P3
lava at bottom
*/
