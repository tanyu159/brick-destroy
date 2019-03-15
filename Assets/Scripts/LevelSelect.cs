using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public bool isSelect = false;
	public Sprite levelBG;
	private Image image;
    


    public GameObject[] stars;

	// Use this for initialization
	void Awake(){
		
		image = GetComponent<Image>();
	}

	void Start () {
		//Debug.Log (gameObject.name + LvManager.Instance.isPlayable (int.Parse (gameObject.name)));
		if(LvManager.Instance.isPlayable(int.Parse(gameObject.name))||int.Parse(gameObject.name)==1){
			image.overrideSprite = levelBG;
			ShowStarInLvSelect ();
            this.GetComponent<Image>().raycastTarget = true;//【谭宇添加-解决未解锁也可以点击进入的BUG，并且在Unity中将所有按钮的默认设置为射线检测设置为不可检测】
		}

	}
	
	// Update is called once per frame
	void Update () {
       
    }

	public void Selected(){
		if(LvManager.Instance.isPlayable(int.Parse(gameObject.name))||int.Parse(gameObject.name)==1){
		PlayerPrefs.SetString ("nowLevel", "level" + gameObject.name);
		PlayerPrefs.SetInt ("nowLevelNum", int.Parse (gameObject.name));
            //Debug.Log ("level" + gameObject.name);
            //游戏提示面板打开，使用异步加载
            OpenGameTipsPanel.Instance.ShowGameTipsPanel();
            StartCoroutine(SimulateLoadSceneDelay());
           
           

        }
	}

	//获取现在关卡对应的名字，从而获得星星数。
	public void ShowStarInLvSelect(){
		int starNum = PlayerPrefs.GetInt ("level" + gameObject.name+ "CurrentLevelStarsCount");

		if (starNum > 0) {
			for (int i = 0; i < starNum; i++) {
				stars [i].SetActive (true);
			}
		}
	}

    IEnumerator SimulateLoadSceneDelay()
    {
        yield return new WaitForSeconds(Random.Range(2.9f,4.5f));
        SceneManager.LoadSceneAsync("02-game");
    }
   
}
