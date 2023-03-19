using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGame : MonoBehaviour
{
    bool isBossGen;
    float bossSpeed;
    public GameObject boss;
    
    void Start()
    {
        isBossGen = false;
        bossSpeed = 1f;
    }

    void Update()
    {
        if (GameOver.gameOver==2 && !isBossGen)
        {
            boss = (GameObject)Instantiate(boss, new Vector3(0f, 0f, 0f), transform.rotation);

            isBossGen = true;
        }
        else if (GameOver.gameOver == 2 && isBossGen)
        {
            if (boss.transform.position.x <= 7f)
            {
                boss.transform.Translate(Time.deltaTime * bossSpeed, 0f, 0f);
            }
        }
    }
}
