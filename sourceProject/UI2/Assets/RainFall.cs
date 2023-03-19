using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFall : MonoBehaviour
{
    public AudioSource getAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.name=="前景1")
            Invoke("disappear", 0.15f);
        if (collision.transform.name == "Player")
        {
            getAudio.Play();
            LightAndScore.Light += 20;
            LightAndScore.Score += 20;
            Destroy(gameObject);
        }
    }

    void disappear()
    {
        Destroy(gameObject);
    }
}
