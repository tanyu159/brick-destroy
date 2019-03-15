using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_DestroySelf : MonoBehaviour {

    // Use this for initialization
    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    
	}
}
