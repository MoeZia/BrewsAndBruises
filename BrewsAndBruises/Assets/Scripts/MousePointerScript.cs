using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerScript : MonoBehaviour
{
   [SerializeField]
    public Texture2D texture;
    // Start is called before the first frame update
    void Start()
    {
        Material material = new Material(Shader.Find("Transparent/Diffuse"));
        material.mainTexture = texture;
        gameObject.GetComponent<Renderer>().material = material;
    }
}
