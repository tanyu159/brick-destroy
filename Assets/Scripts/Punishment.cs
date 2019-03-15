using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PunishmentType { BOMB,DECREASE_TIME }
public class Punishment : MonoBehaviour {
    //该脚本用于控制惩罚物体的移动，和接触到惩罚物体后的惩罚函数执行内容【根据类型】，
    public PunishmentType punishmentType;//当前所挂载惩罚类型
    private Rigidbody2D _rigidBody;
    public float moveHSpeed = 2;//惩罚水平移动的速度
    public int direction;//方向，0表示左，1表示右
    public float newHspeed;//左右移动碰撞后的新速度
    public bool isOnGround = false;//是否掉地，默认为false
    public float punishimentTime = 10;//惩罚时间

    public GameObject dieAnimatiomPrefab;//玩家死亡特效
    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }
    void Start () {
        if (Random.Range(0, 2) == 0)//左右方向各占50%的概率
        {
            moveHSpeed = -moveHSpeed;
            direction = 0;//当前向左            
        }
        else {
            direction = 1;//当前向右
        }
	}

    void Update()
    {
        if (Mathf.Abs(_rigidBody.velocity.x) < 0.05f&&isOnGround)//如果发现惩罚物品的速度小于0.05，视为停下来了，则更改方向【前提是接触砖块后】
        {
            
            if (direction == 1)//当前向右
            {
                newHspeed = -moveHSpeed;//改为向左
                direction = 0;//方向为左
                _rigidBody.velocity = new Vector2(newHspeed, _rigidBody.velocity.y);
                
            }
            else if (direction == 0)//当前向左
            {
                newHspeed = moveHSpeed;//改为向右
                direction = 1;//方向为右
                _rigidBody.velocity = new Vector2(newHspeed, _rigidBody.velocity.y);
                
            }
          
            
        }   
    }
    /// <summary>
    /// 玩家碰到惩罚物品后的操作
    /// </summary>
    public void GetPunishment()
    {
        AudioManager.Instance.PlaySound(SoundType.PUNISHMENT);
        if (punishmentType == PunishmentType.DECREASE_TIME)//减时间惩罚
        {
            Debug.Log("时间减少");
            //:游戏总时间减少，调用GameManager
            GameManager.Instance.timer -= punishimentTime;
        }
        else if (punishmentType == PunishmentType.BOMB)//遇到炸弹
        {
            if (GameObject.Find("Player").GetComponent<Player>().playerState == PlayerState.NORMAL)
            {
                Debug.Log("由于不是无敌，碰到了炸弹，游戏结束");
                Vector3 currentPlayerPos = GameObject.Find("Player").transform.position;
                Destroy(GameObject.Find("Player"));//销毁玩家
                Instantiate(dieAnimatiomPrefab, currentPlayerPos, Quaternion.identity);//死亡动画
                //TODO:正常状态下，调用GameManger，游戏结束的方法
                GameManager.Instance.GameEnd();
            }
            else if (GameObject.Find("Player").GetComponent<Player>().playerState == PlayerState.GOD)
            {
                GameObject.Find("Player").GetComponent<Player>().playerState = PlayerState.NORMAL;
                Debug.Log("你当前是无敌，保护帐就你一命，下次遇到就死了");
                //TODO： //TODO:给玩家提示已经变为一般模式【UI】

            }
        }
        //销毁这个惩罚物品
        Destroy(this.gameObject);
    }
    //碰撞检测，将会涉及到惩罚物体掉下来的移动，和出界自动销毁
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            _rigidBody.velocity = new Vector2(moveHSpeed, _rigidBody.velocity.y);
            isOnGround = true;
        }
        else if (collision.gameObject.tag == "DieLine")//惩罚物品滚下去的情况
        {
            Destroy(this.gameObject);
        }
    }

    
}
