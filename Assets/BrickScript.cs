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

    public string powerUpType;



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

    private void CoinAnimation(){
        return ;
    }



    private void BlockDestroyedAction(){
        if(blockType == "Destroy"){
            Destroy(this.gameObject);
        } else if(blockType == "Solid"){
            // Change look of the block
            GameObject sprite = this.transform.GetChild(0).gameObject;
            SpriteRenderer s = sprite.GetComponent<SpriteRenderer>();
            s.sprite = solidBlock;
        }
    }

    public void BrickHit(){
        Debug.Log("Player hit the brick");
        hitsUntilBreaks -= 1;
        MarioManagerScript.S.IncrementCoins();
        CoinAnimation();
        if(hitsUntilBreaks == 0){
            BlockDestroyedAction();
            if(powerUpType == "1up"){
                SpawnLife();
            } else if(powerUpType == "Mushroom"){
                SpawnMushroom();
            } else if(powerUpType == "Glitch"){
                SpawnGlitch();
            }
            // Add other powerups
        }
    }
}
