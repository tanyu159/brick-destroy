using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState { NORMAL,GOD }//玩家的模式，与奖励物品有关
public enum PlayerDirection { LEFT,RIGHT }//跳跃动作方向问题，要按方向执行跳跃动作
public class Player : MonoBehaviour {
    private Rigidbody2D _rigidBody;
    public float horizontalSpeed = 5;
    public float force = 10;
    public int jumpCount = 0;//当前设计为 3段跳
    public PlayerState playerState = PlayerState.NORMAL;//玩家状态。默认为正常，吃到奖励物品之后，会变为无敌，直到受到炸弹的攻击
    public GameObject protecter;//保护罩

    public joystick stick;
    /// <summary>
    /// 横向输入 
    /// </summary>
    public float hInput;
    //动画状态控制
    private Animator _animator;
    public PlayerDirection direction = PlayerDirection.RIGHT;//玩家朝向与跳跃的动画方向有关
    public bool isJump = false;
    private int _layerMask=0;
    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();
        _layerMask = LayerMask.NameToLayer("brick");
        _layerMask = 1 << _layerMask;
    }
    void Start () {
        _animator.SetBool("IsGround", false);
        GameManager.Instance.mainPlayer = this;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerState == PlayerState.NORMAL&&protecter.gameObject.activeSelf==true)
        {
            protecter.SetActive(false);
        }
        else if (playerState == PlayerState.GOD&&protecter.gameObject.activeSelf==false)//&&性能优化
        {
            protecter.SetActive(true);
        }
        Controll();
        if (GameManager.Instance.gameState == GameState.RUNING)
        {
            if (_rigidBody.velocity.y < -5)
                isJump = true;
            if (isJump)
            {
                //Debug.Log(_rigidBody.velocity.y);
                if (Mathf.Abs(_rigidBody.velocity.y) == 0 && Physics2D.Linecast(this.transform.position, this.transform.position + Vector3.down * 0.3f, _layerMask).collider != null)
                {
                    _animator.SetBool("IsGround", true);
                    isJump = false;
                    
                    RaycastHit2D hit = Physics2D.Linecast(this.transform.position, this.transform.position + Vector3.down * 0.3f, _layerMask);
                    if (hit.collider != null)
                    {
                        hit.collider.GetComponent<Barrier>().Knock();
                        jumpCount = 0;
                    }
                }
            }

        }
    }
    public bool IsHitSelf()
    {
        RaycastHit2D hit = Physics2D.Linecast( this.transform.position, this.transform.position + Vector3.down, _layerMask);//（起点，终点）这句话是指做一条从起点【下一个位置】向终点【原来位置】做一条射线
        return (hit.collider == GetComponent<Collider2D>());//射线撞到
    }
    public void Controll()
    {
        if (GameManager.Instance.gameState == GameState.RUNING)
        {
            //竖直方向
            if (Input.GetKeyDown(KeyCode.W) && jumpCount <= 2)
            {
                _animator.speed = 1;
              
                Jump();

            }
            //左右，设置速度时必须保证原来的Y方向上速度，不可清0
            //hInput = Input.GetAxis("Horizontal");
            //hInput = stick.dir.x;
            if (hInput > 0)
            {
                _animator.speed = 1;
                //设置方向枚举
                direction = PlayerDirection.RIGHT;

                _rigidBody.velocity = new Vector2(horizontalSpeed, _rigidBody.velocity.y);
                //动画控制
                _animator.SetFloat("hSpeed", _rigidBody.velocity.x);
            }
            else if (hInput < 0)
            {
                
                //设置方向枚举
                direction = PlayerDirection.LEFT;



                //水平方向速度控制
                //动画控制
                _rigidBody.velocity = new Vector2(-horizontalSpeed, _rigidBody.velocity.y);
                _animator.SetFloat("hSpeed", _rigidBody.velocity.x);
            }
            else if (hInput == 0)
            {
                
                //水平输入为0的时候，水平方向速度归为0
                _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
                _animator.SetFloat("hSpeed", 0);
            }

        }
    }

    public void Jump()
    {
        AudioManager.Instance.PlaySound(SoundType.JUMP);
        CancelInvoke("SetJump");
        Invoke("SetJump", 0.05f);
        //播放跳跃动作
        if (direction == PlayerDirection.LEFT)
        {
            _animator.SetTrigger("toLeftJump");
            
        }
        else if (direction == PlayerDirection.RIGHT)
        {
            _animator.SetTrigger("toRightJump");
           
        }
        _rigidBody.AddForce(new Vector2(0, force));
        jumpCount++;

    }

    void SetJump()
    {
        isJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.gameObject.tag == "Barrier")
        {
            //jumpCount = 0;
            //Todo砖块受伤消失【Barrier脚本中实现】
            if (isJump)
            {
                collision.gameObject.SendMessage("Knock");//由于向下踩的受伤函数已经在该脚本中的Update中调用了。这里添加是保证向上顶的时候顺利触发
            }
            else {
                collision.gameObject.SendMessage("OnlyDecrease1HP");
            }

        }
        else if (collision.gameObject.tag == "Award")//吃到奖励，奖励有两种类型，加时间，加一个防弹衣，得到奖励的操作在Award中实现【根据类型】
        {
            collision.gameObject.SendMessage("GetAward");
        }
        else if (collision.gameObject.tag == "Punishment")//吃的惩罚，惩罚也有两种类型，减时间，和炸弹，得到奖励在Punishment中实现【根据类型】
        {
            collision.gameObject.SendMessage("GetPunishment");
        }
        else if (collision.gameObject.tag == "DieLine")
        {
            Debug.Log("触碰死线，游戏结束，胜负判定在GameManager进行");
            AudioManager.Instance.PlaySound(SoundType.FALL_DOWN);
            GameManager.Instance.GameEnd();
            Destroy(this.gameObject);
        }
    }
}
