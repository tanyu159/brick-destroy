using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//该脚本用于生成奖励物品和惩罚物品，分枚举类型分别挂在两个生成器上【屏幕上方生成】
public enum ItemType { AWARD,PUNISHMENT }
public class ItemSpawn : MonoBehaviour {
    public ItemType itemType;//当前所挂载的生成器类型
    public GameObject[] punishmentsPrefabs;//惩罚物品的预制体数组
    public GameObject[] awardPrefabs;//奖励物品的预制体数组
    public float startTime;//多久开始生成
    public float creatRate;//生成速率，单位秒

	
	void Start () {
        InvokeRepeating("CreatItems", startTime, creatRate);
	}
	
	// Update is called once per frame
	
    public void CreatItems()
    {//惩罚的生成在屏幕上方，整个可能生成的范围就是整个屏幕宽度，高度同样：和玩家一样，在unity中测量确定
        //Y坐标则是直接在unity中测得的,+5比较合适
        int idx = Random.Range(0, 2);//取值只有可能是0或1，代表两种惩罚类型
        float xMin = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
        float xMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
        float xPosition = Random.Range(xMin, xMax);
        Vector2 creatPosition = new Vector2(xPosition, 4);
        //Vector2 creatPosition = GameManager.Instance.mainPlayer.transform.position+Vector3.up*2f;
        //根据类型生成
        if (itemType == ItemType.PUNISHMENT)
        {
            Instantiate(punishmentsPrefabs[idx], creatPosition, Quaternion.identity);
        }
        else if (itemType == ItemType.AWARD)
        {
            Instantiate(awardPrefabs[idx], creatPosition, Quaternion.identity);
        }   
    }

    public void CancelSpawnItem()
    {
        CancelInvoke("CreatItems");
    }
    /// <summary>
    /// 复活后调用
    /// </summary>
    public void ReStartSpawn()
    {
        InvokeRepeating("CreatItems", startTime, creatRate);
    }
}
