using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public GameObject theModel;
    public void moveCharacter(){
        theModel.GetComponent<Animator>().Play("running");
    }
    // Start is called before the first frame update

}
