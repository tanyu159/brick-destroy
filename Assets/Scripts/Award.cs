using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AwardType { ADD_TIME, GOD}//加时间，变为上帝模式
public class Award : MonoBehaviour {
    //该脚本用于控制奖励的移动，以及吃到奖励之后的操作【要分种类】，奖励掉下去后的自动销毁
    public AwardType awardType;//该奖励的类型
    public float awardTime = 20;//奖励增加的时间

    /// <summary>
    /// 得到奖励后的操作，根据
    /// </summary>
    public void GetAward()
    {
        AudioManager.Instance.PlaySound(SoundType.AWARD);
        if (awardType == AwardType.ADD_TIME)
        {
            //TODO:调用GameManegr中计时器变量
            Debug.Log("加时间了");
            GameManager.Instance.timer += 20;
        }
        else if (awardType == AwardType.GOD)
        {
            //变为无敌模式
            GameObject.Find("Player").GetComponent<Player>().playerState = PlayerState.GOD;
            Debug.Log("变为了无敌模式");
            //TODO:给玩家提示已经变为无敌模式【UI】

        }
        //销毁掉这个奖励物品
        //TODO:销毁特效
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 碰撞检测仅用于，奖励物品，掉下去，出屏幕的销毁
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "DieLine")
        {
            Destroy(this.gameObject);
        }
    }
}
