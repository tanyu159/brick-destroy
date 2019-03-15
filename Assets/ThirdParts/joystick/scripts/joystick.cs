using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystick : MonoBehaviour {
    public RectTransform stick;
    public Canvas canvas;
    public int max_r = 100;

    private Vector2 stick_touch;
    public Vector2 dir {
        get {
            return this.stick_touch;
        }
    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void on_drag() {
        Vector2 pos = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, Input.mousePosition, this.canvas.worldCamera, out pos);

        
        float len = pos.magnitude;
        if (len <= 0){
            this.stick_touch = Vector2.zero;
            return;
        }

        this.stick_touch.x = pos.x / len;
        this.stick_touch.y = pos.y / len;

        if (len >= this.max_r) {
            pos.x = pos.x * this.max_r / len;
            pos.y = pos.y * this.max_r / len;
        }

        this.stick.localPosition = pos;
    }

    public void on_end_drag() {
        this.stick_touch = Vector2.zero;
        this.stick.localPosition = Vector2.zero;
    }
}
