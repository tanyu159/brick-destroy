using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    private void OnMouseDown()
    {
        if (GameManager.Instance.mainPlayer.jumpCount <= 2)
        {
           GameManager.Instance.mainPlayer.Jump();
        }
    }

}
