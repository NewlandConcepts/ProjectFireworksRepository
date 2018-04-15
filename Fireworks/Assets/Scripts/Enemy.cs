using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveForce = 365f;
    public Rigidbody2D rbody;

	// Use this for initialization
	void Awake () {
        rbody = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        rbody.velocity = new Vector2(1f * -1 * moveForce, rbody.velocity.y); 
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(col.gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
