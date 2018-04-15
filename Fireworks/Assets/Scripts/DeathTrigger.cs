using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(col.gameObject);

        if (col.tag == "Player")
        { 
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
