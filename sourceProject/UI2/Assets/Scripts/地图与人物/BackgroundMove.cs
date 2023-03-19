
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [Header("图层参数")]
    public GameObject bg1;
    public GameObject bg2;

    [Header("前景移动参数")]
    
    public static float t=1;

    float moveDistance;

    float speed;

    float pictureLength = 54f;
    public static float bcSpeed;

    void Start()
    {
        speed = -2f;
        bcSpeed = speed;
    }

    void FixedUpdate()
    {
        if(Time.frameCount%30 ==0)
        {
            moveDistance = bg1.transform.position.x - bg2.transform.position.x;
            if (moveDistance > 0)
                bg1.transform.position = new Vector3(bg2.transform.position.x + pictureLength, transform.position.y, transform.position.z);
            else if (moveDistance < 0)
                bg2.transform.position = new Vector3(bg1.transform.position.x + pictureLength, transform.position.y, transform.position.z);
        }
        //Debug.Log("小moveDistance:" + moveDistance);

        Time.timeScale = t;
        if(Music.musicTime !=0)
            speed = -5.0f / (Music.musicTime * Music.musicTime) * (Music.musicCurTime * Music.musicCurTime) - 2;

        bcSpeed = speed;

        bg1.transform.Translate(Time.deltaTime * speed, 0f, 0f);
        bg2.transform.Translate(Time.deltaTime * speed, 0f, 0f);

        if (bg1.transform.position.x <= -pictureLength)
        { 
            bg1.transform.position = new Vector3(pictureLength, transform.position.y, transform.position.z); 
        }
        if (bg2.transform.position.x <= -pictureLength)
        {
            bg2.transform.position = new Vector3(pictureLength, transform.position.y, transform.position.z);
        }

        
        //Debug.Log("1-2:"+(bg1.transform.position.x-bg2.transform.position.x)+ "         2-1:" + (bg2.transform.position.x - bg1.transform.position.x));
        
    }
}
