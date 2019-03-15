using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour {
    //该脚本控制胜利面板星星显示
    public Slider slider;
    public Text remainCountText;
    public GameObject[] stars;
    // Use this for initialization
    void Start() {

        slider.value = (float)GameManager.Instance.score / (float)GameManager.Instance.totalBarriersCount;
        remainCountText.text = GameManager.Instance.score + "/" + GameManager.Instance.totalBarriersCount;
        StartCoroutine(ShowStars());
    }

    // Update is called once per frame
    IEnumerator ShowStars()
    {
        switch (GameManager.Instance.GetCurrentLevelStarsCount())
        {
            case 1:
                yield return new WaitForSeconds(1);
                stars[0].SetActive(true);
                break;
            case 2:
                yield return new WaitForSeconds(1);
                stars[0].SetActive(true);
                yield return new WaitForSeconds(1);
                stars[1].SetActive(true);
                break;
            case 3:
                yield return new WaitForSeconds(1);
                stars[0].SetActive(true);
                yield return new WaitForSeconds(1);
                stars[1].SetActive(true);
                yield return new WaitForSeconds(1);
                stars[2].SetActive(true);
                break;
        }
    }
}
