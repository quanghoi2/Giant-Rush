using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewIngame : MonoBehaviour
{
    public GameObject mObjIndicator;
    public GameObject mObjIndicatorTap;
    public GameObject mObjPlayerHP;
    public GameObject mObjBossHP;
    public TextMeshProUGUI mTextLevel;

    float maxMouseX;

    const string PREFIX = "Level ";

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.Instance.State)
        {
            case STATE.READY:
                if(!mObjIndicator.activeSelf)
                {
                    UpdateTextLevel();
                    mObjIndicator.SetActive(true);
                }
                break;
            case STATE.START_GAME:
                if (mObjIndicator.activeSelf)
                {
                    mObjIndicator.SetActive(false);
                }
                break;

            case STATE.FIGHT:
                if(!mObjIndicatorTap.activeSelf)
                {
                    mObjIndicatorTap.SetActive(true);
                    mObjPlayerHP.SetActive(true);
                    mObjBossHP.SetActive(true);
                }
                else
                {
                    UpdatePlayerHP();
                    UpdateBossHP();
                }
                break;

            default:
                if (mObjIndicator.activeSelf)
                {
                    mObjIndicator.SetActive(false);
                }
                if (mObjIndicatorTap.activeSelf)
                {
                    mObjIndicatorTap.SetActive(false);
                    mObjPlayerHP.SetActive(false);
                    mObjBossHP.SetActive(false);
                }
                break;
        }
    }

    private void UpdatePlayerHP()
    {
        mObjPlayerHP.GetComponent<HpControl>().UpdateHP(GameManager.Instance.PlayerHP);
    }

    private void UpdateBossHP()
    {
        mObjBossHP.GetComponent<HpControl>().UpdateHP(GameManager.Instance.BossHP);
    }

    public void UpdateTextLevel()
    {
        mTextLevel.gameObject.SetActive(true);
        mTextLevel.text = PREFIX + ProfileMgr.Instance.Level;
    }
}
