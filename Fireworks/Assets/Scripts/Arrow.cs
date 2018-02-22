using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDestroyCountdown()
    {
        // Destroy the Arrow after 2 seconds if it doesn't get destroyed before then.
        Destroy(gameObject, 2);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }

        if (col.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
