using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 TeleportTo(float playerPosX){
        Vector3 pos = new Vector3(playerPosX, -20.0f, 0.0f);
        return pos;
    }

    public Vector3 TeleportOut(){
        Vector3 pos = new Vector3(x, y, z);
        return pos;
    }
}
