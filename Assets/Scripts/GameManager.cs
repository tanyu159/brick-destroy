using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { RUNING,PAUSE }
public class GameManager : MonoBehaviour {
    //该脚本为游戏总控，并暴露数据为UIManager使用
    private static GameManager _instance;
    public int minBricksCount;//通过该关卡【游戏判定成功的最少条件,1也是1星条件】的至少踩过的砖块数量
    public int twoStarsNeedCount;//通过该关卡并获得两心的最少数量,总星星个数的80%
    public List<Barrier> barriersList;//障碍列表
    public int score = 0;//当前得分，也是接触过的砖块的个数
    public int totalBarriersCount;//所有障碍的个数
    public float timer = 60;//总计时器
	public string currentLevelName;//当前关卡名字
    public bool isGameEndRuned = false;//保证这个游戏结算函数只执行一次，因为是放在Update中实时监听的
    public GameState gameState = GameState.RUNING;
    public Player mainPlayer = null;
    public GameObject playerPrefab;//玩家预制体，复活用
    public ItemSpawn punishmentSpawn;//惩罚生成器
    public ItemSpawn awardsSpawn;//奖励生成器
    //微信分享功能
    public ShareToWeChat shareToWeChat; 
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }

    }
    private void Awake()
    {
        _instance = this;
    }
    void Start () {
        Time.timeScale = 1;
        int currentLevelNumber = int.Parse(System.Text.RegularExpressions.Regex.Replace(PlayerPrefs.GetString("nowLevel"), @"[^0-9]+", ""));
        Debug.Log("当前关卡号"+currentLevelNumber);
        currentLevelName ="LEVEL\n\n"+currentLevelNumber;
        //初始化障碍物列表
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (var temp in barriers)
        {
            barriersList.Add(temp.GetComponent<Barrier>());
        }
        //初始化所有障碍的个数
        totalBarriersCount = barriersList.Count;
        Debug.Log("障碍物总数为" + totalBarriersCount);
        //初始化一星条件【也是最小通关条件】
        minBricksCount = totalBarriersCount / 2;
        //初始化两星条件
        twoStarsNeedCount =(int) (totalBarriersCount * 0.8);
        //当前关卡字符串初始化
        //currentLevelName ="LEVEL:"+ PlayerPrefs.GetInt("nowLevelNuml", 1);
	}
	
	// Update is called once per frame
	void Update () {
        //计时器开始计时【仅在游戏运行时才计时】
        if (gameState == GameState.RUNING)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameEnd();
                timer = 0;//防止掉下去
            }
            if (score >= totalBarriersCount)
            {
                GameEnd();
            }
        }
	}

    public void GameEnd()//游戏结束函数，做结算【两中情况调用它：1时间结束，2玩家掉下去】
    {
        if (isGameEndRuned == false)//保证该函数只执行一次
        {
            gameState = GameState.PAUSE;//游戏停止，不能再被控制，不再计时
            //无论是成功还是失败，都应该要隐藏玩家和停止敌人与奖励物品的生成
            mainPlayer.GetComponent<SpriteRenderer>().enabled = false;
            punishmentSpawn.CancelSpawnItem();
            awardsSpawn.CancelSpawnItem();

            //结算逻辑判断----开始-----
            if (score >= minBricksCount)//如果得分大于要求最低
            {
                Debug.Log("游戏成功");
                gameState = GameState.PAUSE;
                //TODO:打开胜利结算面板
                //UIManager.instance.xxxxx//根据我获取星星个数的函数来【GetCurrentLevelStarsCount】
                //对应开启星星的显示【显示效果一颗一颗，利用协程，UIManager中完成】
                UIManager.Instance.OpenWinPanel(); 
                //TODO：我拿到你根据分数而得来的星星个数，通过PlayerPrefas，存入到对应关卡的，星星数据键值对，保证关卡控制器上星星的正确显示，以及下一关的开启
                //前提是，如果有记录，必须破纪录再更新

				//js add func
				UpdataMaxStarNum();
				LvManager.Instance.UnlockNextLv (PlayerPrefs.GetInt ("nowLevelNum",1));
				//js add func
            }
            else {
                Debug.Log("游戏失败");
                gameState = GameState.PAUSE;
                //未达成最低要求，显示失败结算面板
                //TODO：打开失败结算面板
                //UIManager.instance.xxxxx
                UIManager.Instance.OpenGameOverPanel();
            }
            
            //结算逻辑判断----结束-----
            isGameEndRuned = true;//保证该函数只执行一次
        }
    }

	//js add func
	//更新当前获得的最大星星数
	void UpdataMaxStarNum(){
		int starNum = GetCurrentLevelStarsCount ();
		if (starNum > PlayerPrefs.GetInt (PlayerPrefs.GetString ("nowLevel") + "CurrentLevelStarsCount")) {
			PlayerPrefs.SetInt (PlayerPrefs.GetString ("nowLevel") + "CurrentLevelStarsCount", starNum);
		}
	}
	//js add func


    //以下为UIManager，调用的我GameManager的函数
    public int GetCurrentLevelStarsCount()//【策划所定】
    {
        if (score >= minBricksCount && score < twoStarsNeedCount)
        {
            return 1;
        }
        else if (score >= twoStarsNeedCount && score < totalBarriersCount)
        {
            return 2;
        }
        else {
            return 3;
        }
    }
    //以下为UIManager调用我的按钮点击事件函数
    public void PauseGame()
    {
        AudioManager.Instance.ControllBGM(false);
        if (gameState == GameState.RUNING)
        {
            gameState = GameState.PAUSE;
        }
    }
    public void ContinueGame()
    {
        AudioManager.Instance.ControllBGM(true);
        if (gameState == GameState.PAUSE)
        {
            gameState = GameState.RUNING;
        }
    }
    public void ReStartGame()
    {
        SceneManager.LoadScene("02-game");
    }
    public void ReturnToMain()
    {
        SceneManager.LoadScene("01-level");
    }
    public void EnterNextLevel()
    {
        //获取当前关卡号，加1后，隐藏该关卡，动态加载下一个关卡，延时销毁该关卡
		int nextLvNum = PlayerPrefs.GetInt ("nowLevelNum")+1;
		PlayerPrefs.SetString ("nowLevel", "level" + nextLvNum);
		PlayerPrefs.SetInt ("nowLevelNum", nextLvNum);

		this.gameObject.transform.parent.gameObject.SetActive(false);
		Instantiate(Resources.Load(PlayerPrefs.GetString ("nowLevel")));
		//销毁当前关卡
		Destroy(this.gameObject.transform.parent.gameObject,2);
        //销毁当前还存在奖励物品和惩罚物品
        GameObject[] awards = GameObject.FindGameObjectsWithTag("Award");
        GameObject[] punishments = GameObject.FindGameObjectsWithTag("Punishment");
        foreach (var temp in awards)
        {
            Destroy(temp.gameObject);
        }
        foreach (var temp in punishments)
        {
            Destroy(temp.gameObject);
        }
    }
    //跳跃按钮
    public void JumpButtonClicked()
    {
        if (mainPlayer.jumpCount <= 2)
        {
            mainPlayer.Jump();
        }
    }

    //追加功能-分享微信后复活，重新恢复游戏
    public void Resurrection()
    {
        if(mainPlayer==null)
        {
            //执行微信分享功能
            shareToWeChat.ShareToWeChatMethod();
            //只有当玩家被销毁的情况下才会调用该函数，重新生成玩家时应当遍历现在存在的砖块，在其上方0.8的位置生成,上方0.8不行，有时上下两个方块连续时，可能生成卡在中间
            //所以只要保证生成后脚底下有砖即可，y就固定3.8
            Vector3 radomLiveBrickPos = barriersList[Random.Range(0, barriersList.Count)].gameObject.transform.position;
            Vector3 spawnPlayerPosition = new Vector3(radomLiveBrickPos.x,3.8f, radomLiveBrickPos.z);
            GameObject player = Instantiate(playerPrefab, spawnPlayerPosition, Quaternion.identity);
            player.name = "Player";//必须把名字改为Player，因为有些地方用到了GameObject.Find("Player")，虽然所有的这个都可以用GameManager中的mainPlayer进行替换，但懒得改了，mainplayer是后期加进来用的
            //恢复游戏状态
            gameState = GameState.RUNING;//【改为Runing后继续计时】
            Time.timeScale = 1;//【没用可删除，安全性确保】
            UIManager.Instance.HideGameOverPanel();//关闭gameover面板
            awardsSpawn.ReStartSpawn();//重启生产奖励和惩罚
            punishmentSpawn.ReStartSpawn();
            //为保证以后的游戏结束判定正常，GameEnd函数的表示位要重新还原
            isGameEndRuned = false;
        }
    }
}
