using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DirectionType { LEFT,RIGHT }
public class DirectionButton : MonoBehaviour {
    public DirectionType direction;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnMouseDown()
    {
        if (direction == DirectionType.LEFT)
        {
            Debug.Log("当前向左");
            GameManager.Instance.mainPlayer.hInput = -1;
        }
        else if (direction == DirectionType.RIGHT)
        {
            Debug.Log("当前向右");
            GameManager.Instance.mainPlayer.hInput = 1;
        }
    }
    private void OnMouseUp()
    {
        Debug.Log("方向键抬起");
        GameManager.Instance.mainPlayer.hInput = 0;
    }


}
