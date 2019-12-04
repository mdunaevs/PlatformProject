using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour
{

    public int hitsUntilBreaks;
    public string blockType;

    public Sprite solidBlock;

    public GameObject lifePrefab;
    public GameObject mushroomPrefab;
    public GameObject glitchPrefab;
    public GameObject blackHolePrefab;

    public GameObject coinPrefab;

    public string powerUpType;

    public bool spawnCoin = true;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnLife(){
        GameObject LifeUp;
        LifeUp = Instantiate(lifePrefab,
                             transform.position + new Vector3(0.0f, 2.0f, 0.0f),
                             transform.rotation,
                             transform);
        LifeUp.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        LifeUp.transform.parent = null;
    }

    void SpawnMushroom(){
        GameObject mushroom;
        mushroom = Instantiate(mushroomPrefab,
                             transform.position + new Vector3(0.0f, 2.0f, 0.0f),
                             transform.rotation,
                             transform);
        mushroom.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        mushroom.transform.parent = null;
    }

    void SpawnGlitch(){
        GameObject glitch;
        glitch = Instantiate(glitchPrefab,
                             transform.position + new Vector3(0.0f, 2.0f, 0.0f),
                             transform.rotation,
                             transform);
        glitch.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        glitch.transform.parent = null;
    }

    void SpawnBlackHole(){
        GameObject blackHole;
        blackHole = Instantiate(blackHolePrefab,
                             transform.position + new Vector3(0.0f, 2.0f, 0.0f),
                             transform.rotation,
                             transform);
        blackHole.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        blackHole.transform.parent = null;
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
                          transform.position + new Vector3(0.0f, 0.0f, 2.0f),
                          transform.rotation,
                          transform);
        coin.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        coin.transform.parent = null;

        coin.GetComponent<Animator>().SetBool("hitBlock", true);

        RemoveCoin(coin);
        //Destroy(coin.gameObject);
        //coin.gameObject.active = false;
    }


    private void BlockDestroyedAction(){
        if(blockType == "Destroy"){
            Destroy(this.gameObject);
        } else if(blockType == "Solid"){
            // Change look of the block
            GameObject sprite = this.transform.GetChild(0).gameObject;
            SpriteRenderer s = sprite.GetComponent<SpriteRenderer>();
            s.sprite = solidBlock;
            s.enabled = true;
        }
    }

    public void BrickHit(){
        Debug.Log("Player hit the brick");
        hitsUntilBreaks -= 1;
        if(spawnCoin && hitsUntilBreaks >= 0){
            MarioManagerScript.S.IncrementCoins();
            CoinAnimation();
        }
        if(hitsUntilBreaks == 0){
            BlockDestroyedAction();
            spawnCoin = false;
            if(powerUpType == "1up"){
                SpawnLife();
            } else if(powerUpType == "Mushroom"){
                SpawnMushroom();
            } else if(powerUpType == "Glitch"){
                SpawnGlitch();
            } else if(powerUpType == "BlackHole"){
                SpawnBlackHole();
            }
            // Add other powerups
        }
    }
}
