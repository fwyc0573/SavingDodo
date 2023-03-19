using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlyaer : MonoBehaviour
{
    public int light;
    public GameObject Player;
    

    void Start()
    {
        light = LightAndScore.Light;
        Player = GameObject.Find("Player");
        this.transform.position = new Vector3(1f, -2.8f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        light = LightAndScore.Light;
        if (this.transform.position.x < 1f)
            this.transform.position=new Vector3(1f, this.transform.position.y, this.transform.position.z);

        if (light < 50 && (this.transform.position.x < (Player.transform.position.x-3f)))
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(1f,0f);
        if (light > 70 && (this.transform.position.x > 1f))
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 0f);
    }
}
