using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTransform : MonoBehaviour
{
    public Transform player;
    public GameObject ob;
    float y, a;
    float startSize, endSize = 0.25f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ob.activeInHierarchy)
            this.gameObject.SetActive(true);
        else
            this.gameObject.SetActive(false);

        shadowSizeAndPosition();
        this.transform.position = new Vector3(player.position.x, y, this.transform.position.z);

        ShadowSize();
        this.transform.localScale = new Vector3(endSize, endSize, 1f);
    }


    void shadowSizeAndPosition()
    {//根据主角的大小赋给影子初始位置和大小
        if (player.localScale.x >= 0.14f && 0.22f > player.localScale.x)
        {
            y = -3.49f;
            startSize = 0.03f;
        }
        else if (player.localScale.x >= 0.22f && player.localScale.x < 0.3f)
        {
            y = -3.63f;
            startSize = 0.05f;
        }
        else if (player.localScale.x == 0.3f)
        {
            y = -3.75f;
            startSize = 0.07f;
        }
        else if (player.localScale.x > 0.3f && player.localScale.x <= 0.38f)
        {
            y = -3.88f;
            startSize = 0.09f;
        }
        else if (player.localScale.x > 0.38f && player.localScale.x <= 0.46f)
        {
            y = -4.02f;
            startSize = 0.11f;
        }
    }
    void ShadowSize()
    {//根据主角y位置变化大小
        if (player.localScale.x >= 0.14f && 0.22f > player.localScale.x)
        {
            a = -2.808432f;
        }
        else if (player.localScale.x >= 0.22f && player.localScale.x < 0.3f)
        {
            a = -2.560945f;
        }
        else if (player.localScale.x == 0.3f)
        {
            a = -2.313458f;
        }
        else if (player.localScale.x > 0.3f && player.localScale.x <= 0.38f)
        {
            a = -2.065971f;
        }
        else if (player.localScale.x > 0.38f && player.localScale.x <= 0.46f)
        {
            a = -1.818484f;
        }


        if (player.position.y >= a && player.position.y <= 3.19f)
            endSize = startSize - (player.position.y - a) / (3.19f - a) * 0.02f;
        else if (player.position.y < a)
            endSize = startSize;
        else if (player.position.y > 3.19f)
            endSize = startSize - 0.02f;
    }
}