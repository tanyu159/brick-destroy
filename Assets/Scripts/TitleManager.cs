using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum AudioSwicth{ OPEN,CLOSE }
public class TitleManager : MonoBehaviour {
    //控制Main场景的
    private static TitleManager _instance;

    public static TitleManager Instance
    {
        get
        {
            return _instance;
        }

     
    }
    //声音控制
    public AudioSwicth audioSwitch = AudioSwicth.OPEN;//默认打开
    public Button audioSwitchButton;
    public Sprite[] buttonSprites;//0是开启的，1是关闭的
    //面板控制
    public GameObject AboutUs;
    public GameObject firstTipsPanel;
    //动画控制
    public Animator firstTipsPanelAnimator;
    //分享功能
    public ShareToWeChat shareObject;
    private void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start () {
        //PlayerPrefs.DeleteKey("ShowTips");
        //PlayerPrefs.DeleteAll();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;
        LoadLastSetting();
        if (audioSwitch == AudioSwicth.CLOSE)
        {
            this.GetComponent<AudioSource>().Stop();
        }
        if (PlayerPrefs.GetInt("ShowTips", 1) == 1)
        {
            firstTipsPanel.SetActive(true);
        }
        else {
            firstTipsPanel.SetActive(false);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    //按钮点击事件
    public void StartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    public void AboutUSButtonClicked()
    {
        //打开Panel
        AboutUs.SetActive(true);
    }
    public void CloseAboutUsButtonClicked()
    {
        AboutUs.SetActive(false);
    }
    public void ExitButtonClicked()
    {
        Application.Quit();
    }
    public void ExitTipsPanelButtonClicked()
    {
        firstTipsPanelAnimator.SetTrigger("toUp");
        Destroy(firstTipsPanel, 3);
    }
    public void SwitchAudioButtonClicked()
    {
        if (audioSwitch == AudioSwicth.OPEN)
        {
            audioSwitch = AudioSwicth.CLOSE;
            //并更换图片
            audioSwitchButton.gameObject.GetComponent<Image>().sprite = buttonSprites[1];
            PlayerPrefs.SetInt("AudioSwitch", 0);//控制后面场景的音乐播放
                                                 //控制本场景的音乐播放
            this.GetComponent<AudioSource>().Stop();
        }
        else if (audioSwitch == AudioSwicth.CLOSE)
        {
            audioSwitch = AudioSwicth.OPEN;
            //并更换图片
            audioSwitchButton.gameObject.GetComponent<Image>().sprite = buttonSprites[0];
            PlayerPrefs.SetInt("AudioSwitch", 1);
            //控制本场景音乐的播放
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void ShareToWeChatButtonClicked()
    {
        shareObject.ShareToWeChatMethod();
    }
    //单选框事件
    public void NeverGiveTipsToggle(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("ShowTips", 0);
        }
    }
    //加载上次声音设定
    public void LoadLastSetting()
    {
        if (PlayerPrefs.GetInt("AudioSwitch", 1) == 1)//上次声音为打开的话
        {
            audioSwitch = AudioSwicth.OPEN;
            //并更换图片
            audioSwitchButton.gameObject.GetComponent<Image>().sprite = buttonSprites[0];
        }
        else {
            audioSwitch = AudioSwicth.CLOSE;
            //并更换图片
            audioSwitchButton.gameObject.GetComponent<Image>().sprite = buttonSprites[1];
        }
    }
}
