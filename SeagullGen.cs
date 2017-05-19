using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeagullGen : MonoBehaviour {
    public float speed;

    public Gull[] gulls;
    public GameObject gull;
    public AudioClip source;
    // Use this for initialization
    void Start () {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        Random.seed = Mathf.RoundToInt(transform.position.x * transform.position.y * transform.position.z);
        int count = Mathf.RoundToInt(Random.Range(3, 7));
        gulls = new Gull[count];
        
        for(int i = 0;i<count; i++)
        {
            Vector3 pos = new Vector3(transform.position.x+ Random.Range(-5, 5), transform.position.y + Random.Range(15, 30), transform.position.z + Random.Range(-5, 5));
            GameObject newG = Instantiate(gull, pos, transform.rotation, transform) as GameObject;
            gulls[i] = new Gull(newG);
        }
        for(int i = 0; i < gulls.Length; i++)
        {
            gulls[i].gull.transform.Rotate(Vector3.up, Random.Range(0, 360));
            gulls[i].gull.GetComponent<Animator>().speed = Random.Range(1, 3);
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < gulls.Length; i++)
        {
            gulls[i].gull.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            gulls[i].gull.transform.Rotate(Vector3.up, gulls[i].turn);
            if (gulls[i].delay <= 0)
            {
                gulls[i].gull.GetComponent<AudioSource>().pitch = (Random.Range(0.9f, 1f));
                gulls[i].gull.GetComponent<AudioSource>().Play();
                gulls[i].delay = Random.Range(30, 80);
                gulls[i].turn = Random.Range(-4, 4);
            }
            gulls[i].delay--;
        }
        
	}

    
}

public struct Gull
{
    public GameObject gull;
    public float delay;
    public float turn;

    public Gull(GameObject gully)
    {
        this.gull = gully;
        this.delay = 0;
        this.turn = 0;
    }
}
