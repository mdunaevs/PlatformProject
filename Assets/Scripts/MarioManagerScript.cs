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

    public void InitRound(){
        if(player == null){
            player = GameObject.FindGameObjectWithTag("Player");

        }
        if(cam == null){
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }
        // if(livesLeftMessage == null){
        //     livesLeftMessage = GameObject.FindGameObjectWithTag("Display");
        // }
        ResetPlayerPosition();
        ResetCameraPosition();
        deathBeingRegistered = false;
        hitsUntilDeath = 1;
        player.GetComponent<HeroScript>().SetPlayerDead(false);

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
        MakeDeathSound();
        amtLives -= 1;

        if(amtLives == 0){
            Debug.Log("Lost the game!");
            //GoToLoseScreen();
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
          player.transform.GetChild(0).gameObject.active = true;
          yield return new WaitForSeconds(0.125f);
      }
      //player.transform.GetChild(0).gameObject.active = true;
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

    public void GoToWinScreen(){
        SceneManager.LoadScene("LoseScreen");
    }

    public void GoToLevelOne(){
        SceneManager.LoadScene("Level 2");
        isLoaded = true;
    }
}

/*

Coin animation - done
powerups - animations + abilities
revive enemies / blocks
lucky blocks - done
Pipe functionality -
Bowser player
black hole animation of enemy getting sucked in
*/
