using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
    //该脚本用于控制障碍物的被撞击
    public int HP = 4;
    public Sprite[] damagedPics;//受损后的图片，0小破，1中破，2大破
    private SpriteRenderer _spriteRenderer;
    public bool isKnocked = false;//接触过该砖块吗？默认未接触，计分只是接触，而非击碎得分。保证这个砖块只会在第一次接触时加分
    //TODO
    //销毁特效

    private void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// 用于玩家撞击在砖墙上的砖墙的反应
    /// </summary>
    /// <param name="playerVelocity">玩家当前的速度Vector2</param>
    public void Knock()
    {
        AudioManager.Instance.PlaySound(SoundType.BRICK);
        //加分
        if (isKnocked == false)
        {
            GameManager.Instance.score++;
            isKnocked = true;//保证这个砖块只会在第一次接触时加分
        }
        HP--;
        if (HP <= 0)
        {
            //TODO,爆炸特效实例化，并销毁
            AudioManager.Instance.PlaySound(SoundType.BRICK_DESTROY);
            Destroy(this.gameObject);
            return;
        }
        /*【存在问题的逻辑】
        if (Mathf.Abs(playerVelocity.y) >0)//只要竖直方向上有速度就会减血
        {
            HP--;
            if (HP <= 0)
            {
                //TODO,爆炸特效实例化，并销毁
                Destroy(this.gameObject);
                return;
            }
        }
        else if (Mathf.Abs(playerVelocity.y) < 0.05f && Mathf.Abs(playerVelocity.x) > 0)//接触但Y轴方向上无速度
        {//不执行减血操作?【血量归为2】,只进行贴图切换，目前变成亮黄色
         //  HP = 3;
         //   Debug.Log("竖直方向无速度，仅在水平方向移动");
        }*/
        //根据血量进行，图片的更换
        //TODO：暂无美术素材，用颜色变换来表示，归一化RGBA【已完成】
        switch (HP)
        {
            case 3://被点亮
                //高亮黄
                //_spriteRenderer.color = new Color(230f / 255f, 1, 0, 1);
                _spriteRenderer.sprite = damagedPics[0];
                break;
            case 2://小破
                //高亮蓝
                //_spriteRenderer.color = new Color(30f/255f,0,1,1);
                _spriteRenderer.sprite = damagedPics[1];
                break;
            case 1://大破
                   // _spriteRenderer.color = new Color(1, 0, 0, 1);
                _spriteRenderer.sprite = damagedPics[2];
                break;
        }
    }
    public void OnlyDecrease1HP()
    {
        if (isKnocked == false)
        {
            AudioManager.Instance.PlaySound(SoundType.BRICK);
            HP -= 1;//!!!
            GameManager.Instance.score++;
            _spriteRenderer.sprite = damagedPics[0];
            isKnocked = true;
        }
    }
    public void OnDestroy()
    {
        GameManager.Instance.barriersList.Remove(this);
    }
}
