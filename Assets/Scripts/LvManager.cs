using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvManager : MonoBehaviour {

	public int playCount = 0;

	public GameObject LvParents;

	private static LvManager _instance;
	public static LvManager Instance
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

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
		PlayerPrefs.SetInt ("Level" + 1 + "isPlayable", 1);
		if (PlayerPrefs.GetInt ("PlayCount") == 0) {
			PlayerPrefs.SetInt ("PlayCount", playCount+1);
			int lvCount = LvParents.transform.childCount ;
		
			for (int j = 0; j < lvCount; j++) {
				PlayerPrefs.SetInt ("Level" + (j + 1) + "isPlayable",0);
			}
		} else {
			playCount = PlayerPrefs.GetInt ("PlayCount");
			PlayerPrefs.SetInt ("PlayCount", playCount+1);
		}

		Debug.Log ("In LvManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isPlayable(int lvNum){
		/*
		int lvCount = LvParents.transform.childCount ;
		for (int j = 0; j < lvCount; j++) {
			isLvPlayable [j] = PlayerPrefs.GetInt ("Level" + (j + 1) + "isPlayable");
			//Debug.Log ("In LvManager " + "Level" + (j + 1) + "=" + isLvPlayable [j]);
		}
		//Debug.Log (lvNum);
		*/
		//Debug.Log("In LvManager Level"+lvNum+":"+isLvPlayable[lvNum-1]);
		if (PlayerPrefs.GetInt("Level" + lvNum + "isPlayable")==1) {
			return true;
		} else {
			return false;
		}
	}

	public void UnlockNextLv(int nowLv){
		PlayerPrefs.SetInt ("Level" + (nowLv + 1) + "isPlayable",1);
	}

    public void ReturnButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
