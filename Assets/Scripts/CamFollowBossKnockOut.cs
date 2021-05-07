using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowBossKnockOut : MonoBehaviour
{
    private Vector3 moveVector;
    private Vector3 moveOffset = new Vector3(0, 0, -10);
    private Timer timerControl = new Timer();

    // Start is called before the first frame update
    void Start()
    {
        timerControl.SetDuration(Define.TIME_FOLLOW_BOSS_KNOCK_OUT);
    }

    // Update is called once per frame
    void Update()
    {
        timerControl.Update(Time.deltaTime);
        if(timerControl.JustFinished())
        {
            if(GameManager.Instance.State != STATE.BOSS_KNOCK_OUT)
            {
                GameManager.Instance.State = STATE.BOSS_KNOCK_OUT;
            }
        }

        switch(GameManager.Instance.State)
        {
            case STATE.BOSS_KNOCK_OUT:
                moveVector = LevelManager.Instance.mBoss.transform.position + moveOffset;
                moveVector.x = transform.position.x;
                moveVector.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, moveVector, Time.deltaTime);
                break;
        }
    }
}
