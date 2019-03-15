using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGameTipsPanel : MonoBehaviour {
    //该脚本用于开启游戏提示的panel
    private static OpenGameTipsPanel _instance;

    public static OpenGameTipsPanel Instance
    {
        get
        {
            return _instance;
        }

        
    }
    public GameObject tipsPanel;
    //当前提示Panel要素过于简单，需要给一些加载动画
    private void Awake()
    {
        _instance = this;
    }

    public void ShowGameTipsPanel()
    {
        tipsPanel.SetActive(true);
    }
}

