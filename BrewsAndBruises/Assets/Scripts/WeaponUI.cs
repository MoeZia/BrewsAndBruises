using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Texture[] textures = new Texture[2];

    public RawImage rawImage;

    public CombatModel.WeaponType weaponType;


    void Start() {
        rawImage.texture = textures[0];
    }

    public void setTexture(int idx) {
        rawImage.texture = textures[idx];
    }
}
