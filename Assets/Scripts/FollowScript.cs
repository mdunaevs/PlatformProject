using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{

    public GameObject playerObject;

    private float xVel = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
          Vector3 playerPosition = playerObject.transform.position;
          Vector3 cameraPosition = transform.position;

          if(!MarioManagerScript.S.insideSideRoom && playerPosition.x > cameraPosition.x){
              cameraPosition.x = Mathf.SmoothDamp(cameraPosition.x, playerPosition.x, ref xVel, 0.5f);
              transform.position = cameraPosition;
          }
          if((playerPosition.y < cameraPosition.y - 20.0f)){
              cameraPosition.y = playerPosition.y - 20.0f;
              cameraPosition.x = playerPosition.x + 20.0f;
              transform.position = cameraPosition;
          }
          if((playerPosition.y > cameraPosition.y + 20.0f)){
              cameraPosition.y = 3.0f;
              cameraPosition.x = playerPosition.x + 15.0f;
              transform.position = cameraPosition;
          }
    }
}
