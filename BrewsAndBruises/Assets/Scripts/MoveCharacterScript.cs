using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterScript : MonoBehaviour
{
    public float speed = 0.00f;
    public float jumpHeight = 0.00f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
			gameObject.transform.position += Vector3.right * Mathf.Clamp(speed * Time.deltaTime,-10f, 10f);
		}
		if (Input.GetKey(KeyCode.LeftArrow)){
			gameObject.transform.position += Vector3.left* speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.UpArrow)){
			gameObject.transform.position += Vector3.forward * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.DownArrow)){
			gameObject.transform.position += Vector3.back* speed * Time.deltaTime;
		} 

		if (Input.GetKey(KeyCode.Space)){
			gameObject.transform.position += Vector3.up* jumpHeight * Time.deltaTime;
		} 

    }
}
