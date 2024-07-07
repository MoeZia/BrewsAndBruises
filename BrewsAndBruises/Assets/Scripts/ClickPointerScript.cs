using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Richtiges Bild /GameObject hinzufügen
// TODO: Die Richtige Maus position finden.

public class ClickPointerScript : MonoBehaviour
{
    public GameObject image;


    void OnMouseOver(){
        if(Input.GetMouseButtonDown(1)) {
            GameObject[] pointers =  GameObject.FindGameObjectsWithTag("Pointer");
            foreach(GameObject pointer in pointers){
                Destroy(pointer);
            }
            Debug.Log("Bla baz Foo");
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Debug.Log(image.name);
            GameObject.Instantiate(image, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity);
        }  
    }
}
