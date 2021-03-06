using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;
    public Transform transPlayer;
    public Transform boardMultiScore;
    public List<Material> listMatMultiSocre = new List<Material>();
    public ScoreBlock prefabScoreBlock;
    public ScoreBlockLock prefabScoreBlockLock;
    public Boss mBoss;

    [HideInInspector]
    public List<ScoreBlock> listMultiScore = new List<ScoreBlock>();

    const float MULTI_SCORE = 0.2f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < Define.MULTI_SCORE_LOCK_ID; i++)
        {
            int ID = i + 1;
            ScoreBlock sb = Instantiate(prefabScoreBlock, boardMultiScore);
            sb.ID = ID;
            float multiScore = 1 + MULTI_SCORE * ID;
            string multiScoreText = "X" + multiScore;
            sb.SetTextMultiScore(multiScoreText);
            sb.matActive = listMatMultiSocre[i];
            Vector3 pos = sb.transform.position;
            pos.z += i;
            sb.transform.position = pos;
            listMultiScore.Add(sb);

            if(ID == Define.MULTI_SCORE_LOCK_ID)
            {
                ScoreBlockLock sbl = Instantiate(prefabScoreBlockLock, boardMultiScore);
                sbl.ID = ID;
                sbl.textMultiScore.text = multiScoreText;
                sbl.matActive = listMatMultiSocre[i];
                sbl.UpdateActive();
                Vector3 posSBL = sbl.transform.position;
                posSBL.z += i;
                sbl.transform.position = posSBL;
            }
            
        }
        boardMultiScore.gameObject.SetActive(false);
    }

    public void ActiveBoardMultiScore()
    {
        boardMultiScore.gameObject.SetActive(true);
    }
}
