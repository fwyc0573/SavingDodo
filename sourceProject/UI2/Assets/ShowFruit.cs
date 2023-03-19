using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFruit : MonoBehaviour
{
    private float tempTime;
    void Start()
    {
        tempTime=0;
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(
                gameObject.GetComponent<SpriteRenderer>().material.color.r,
                gameObject.GetComponent<SpriteRenderer>().material.color.g,
                gameObject.GetComponent<SpriteRenderer>().material.color.b,
                //需要改的就是这个属性：Alpha值  
                0f);
    }

    private void OnEnable()
    {
        tempTime = 0;
        gameObject.GetComponent<SpriteRenderer>().material.color = new Color(
                gameObject.GetComponent<SpriteRenderer>().material.color.r,
                gameObject.GetComponent<SpriteRenderer>().material.color.g,
                gameObject.GetComponent<SpriteRenderer>().material.color.b,
                //需要改的就是这个属性：Alpha值  
                0f);
        this.gameObject.transform.position = new Vector3(Random.Range(15f, 21f), Random.Range(-3f, 3f), transform.position.z);
    }


    void Update()
    {
        if (RamdonMapObject.can_Generate1)
        {
            ShowUp();
            transform.Translate(Vector3.left * 1.2f * Time.deltaTime);
        }

    }

    void ShowUp()
    {
        if (tempTime < 1)
        {
            tempTime = tempTime + Time.deltaTime;
        }
        if (gameObject.GetComponent<SpriteRenderer>().material.color.a <= 1)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = new Color
            (
                gameObject.GetComponent<SpriteRenderer>().material.color.r,
                gameObject.GetComponent<SpriteRenderer>().material.color.g,
                gameObject.GetComponent<SpriteRenderer>().material.color.b,
                gameObject.GetComponent<SpriteRenderer>().material.color.a + tempTime / 2.3f * Time.deltaTime
            );
        }
    }
}
