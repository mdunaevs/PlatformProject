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

    public int level = 1;

    public int amtLives = 3;
    public int amtCoinsCollected = 0;

    public TextMeshProUGUI livesLeftMessage;

    public bool deathBeingRegistered = false;
    public bool canGlitch = false;

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
        ResetPlayerPosition();
        ResetCameraPosition();
        deathBeingRegistered = false;
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

    public void RegisterDeath(){
        gameState = GameState.hit;
        if(deathBeingRegistered) return;
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

    public void Grow(){
        Debug.Log("Mario is meant to grow now");
    }


    public void GlicthableSkin(){
        // TO-DO: Sprite skin of mario is glitch skin
        return;
    }

    public void Glitchable(){
        Debug.Log("Mario is meant to glitch now");
        GlicthableSkin();
        canGlitch = true;
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
}

/*

Coin animation
powerups - animations + abilities
revive enemies / blocks
lucky blocks
Pipe functionality

*/
