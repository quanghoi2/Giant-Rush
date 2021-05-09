using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    //private Vector3 targetPos;
    //private Vector3 StartPos = new Vector3(2, 4, -8.5f);
    //private Vector3 StartRot = new Vector3(13, -8, 0);

    private Vector3 PlayPos = new Vector3(1.5f, 7, -14);
    private Vector3 moveVector;
    private Vector3 moveOffset = new Vector3(0, 0, 5);

    void Update()
    {
        switch(GameManager.Instance.State)
        {
            case STATE.PLAY_GAME:
                moveVector = GameManager.Instance.mPlayer.transform.position + PlayPos + moveOffset;
                moveVector.x = PlayPos.x;
                //moveVector.y = PlayPos.y;
                transform.position = Vector3.Lerp(transform.position, moveVector, Time.deltaTime);
                break;
        }
    }
}
