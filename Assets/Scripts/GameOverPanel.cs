using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour {
    public Slider slider;
    public Text remainCountText;
    // Use this for initialization
    void Start () {
        slider.value = (float)GameManager.Instance.score / (float)GameManager.Instance.totalBarriersCount;
        remainCountText.text = GameManager.Instance.score + "/" + GameManager.Instance.totalBarriersCount;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
