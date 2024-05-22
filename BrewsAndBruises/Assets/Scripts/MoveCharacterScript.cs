using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacterScript : MonoBehaviour
{
    public float speed = 0.00f;
    public float jumpHeight = 0.00f;

	public AnimationController animationController;

    void Start()
    {
        
    }

    // Update is called once per frame
	// 
	// By Moe: Kurzes Beispeil um zu zeigen wie Animation controller aufgerufen wird.
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
			gameObject.transform.position += Vector3.right * Mathf.Clamp(speed * Time.deltaTime,-10f, 10f);
			
		}
		else if (Input.GetKey(KeyCode.LeftArrow)){
			gameObject.transform.position += Vector3.left* speed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.UpArrow)){
			gameObject.transform.position += Vector3.forward * speed * Time.deltaTime;
			// Hier wird der AnimationController erst abgefragt ob er in einem bestimmten Zustand ist.
			// Wenn er in einem bestimmten Zustand ist, wird die Animation nicht ausgeführt.
		 if (!animationController.IsInAnimationState("RunForward"))
			{
				// Hier wird die Animation gestartet.
				animationController.TriggerAnimation("RunForward");
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow)){
			gameObject.transform.position += Vector3.back* speed * Time.deltaTime;
		} 

		else if (Input.GetKey(KeyCode.Space)){
			gameObject.transform.position += Vector3.up* jumpHeight * Time.deltaTime;
		}else{
			if (!animationController.IsInAnimationState("Idle"))
			{
				animationController.TriggerAnimation("Idle");
			}
			
		}

    }
}
