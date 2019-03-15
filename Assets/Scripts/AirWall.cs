using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWall : MonoBehaviour {
    //该脚本用于动态生成空气墙
    private GameObject _topWall;
    private GameObject _bottomWall;
    private GameObject _leftWall;
    private GameObject _rightWall;

    private void Awake()
    {
        _topWall = this.transform.Find("top").gameObject;
        _bottomWall = this.transform.Find("bottom").gameObject;
        _leftWall = this.transform.Find("left").gameObject;
        _rightWall = this.transform.Find("right").gameObject;
    }
    void Start () {
        SetPositionAndSize();

    }
    /// <summary>
    /// 设置空气墙的位置和大小
    /// </summary>
    public void SetPositionAndSize()
    {
        //位置初始化
        _topWall.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2,Screen.height));
        _bottomWall.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, 0));
        _leftWall.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height/2));
        _rightWall.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height/2));
        //大小设置
        _topWall.GetComponent<BoxCollider2D>().size = new Vector2(Camera.main.ScreenToWorldPoint(new  Vector2( Screen.width,0)).x*2 , 0.1f);
        _bottomWall.GetComponent<BoxCollider2D>().size = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x * 2, 0.1f);
        _leftWall.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y * 2);
        _rightWall.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y * 2);
    }

}
