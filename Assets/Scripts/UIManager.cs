using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private static UIManager _instance;
    public Text currrentLevelText;
    public Text timerText;
    public Slider processSlider;
    public Text processText;
    //对应的面板
    public GameObject pausePanel;//常显示
    public GameObject winPanel;
    public GameObject gameOverPanel;
    //对应的按钮
    public Button pauseButton;
    public static UIManager Instance
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
		
	}
	
	// Update is called once per frame
	void Update () {
        UIShow();

    }
    public void UIShow()
    {
        //计时器显示
        timerText.text = GameManager.Instance.timer.ToString("0");
        //当前关卡
        currrentLevelText.text = GameManager.Instance.currentLevelName;
        //进度显示
        processText.text = (((float)GameManager.Instance.score /(float)GameManager.Instance.totalBarriersCount)*100).ToString("0")+"%";
        //进度条显示
        processSlider.value= (float)GameManager.Instance.score/ (float)GameManager.Instance.totalBarriersCount;
    }
    //按钮点击事件
    public void PauseButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.BUTTON);
        pausePanel.GetComponent<Animator>().SetTrigger("toDown");
        GameManager.Instance.PauseGame();
        Invoke("TimeScale0", 1);
        pauseButton.interactable = false;
    }
    public void ContinueButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.BUTTON);
        Time.timeScale = 1;
        pausePanel.GetComponent<Animator>().SetTrigger("toUp");
        GameManager.Instance.ContinueGame();
        
        pauseButton.interactable = true;
    }
    public void ReStartButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.BUTTON);
        GameManager.Instance.ReStartGame();
        Time.timeScale = 1;
    }
    public void ReturnButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.BUTTON);
        GameManager.Instance.ReturnToMain();
    }
    public void NextButtonClicked()
    {
        AudioManager.Instance.PlaySound(SoundType.BUTTON); 
        GameManager.Instance.EnterNextLevel();
    }
    //复活按钮点击
    public void ResurrectionButtonClicked()
    {
        GameManager.Instance.Resurrection();
    }
    //延时调用-保证暂停面板滑下来再暂停
    public void TimeScale0()
    {
        Time.timeScale = 0;
    }
    public void ShowGameOverPanel()
    {
        AudioManager.Instance.ControllBGM(false);
        gameOverPanel.SetActive(true);
    }
    /// <summary>
    /// 隐藏失败面板-复活后调用
    /// </summary>
    public void HideGameOverPanel()
    {
        gameOverPanel.SetActive(false);
    }
    //开启面板
    public void OpenWinPanel()
    {
        AudioManager.Instance.ControllBGM(false);
        if (GameManager.Instance.GetCurrentLevelStarsCount() <= 2)
        {
            AudioManager.Instance.PlaySound(SoundType.WIN);
        }
        else {
            AudioManager.Instance.PlaySound(SoundType.PREFECT);
        }
        //根据星星个数，显示对应的图片，这个由WinPanel自己去看，专门写一个WinPanel来管理
        winPanel.SetActive(true);
    }
    public void OpenGameOverPanel()
    {
        AudioManager.Instance.PlaySound(SoundType.FAILED);
        Invoke("ShowGameOverPanel",1f);
    }
}
