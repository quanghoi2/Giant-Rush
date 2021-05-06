using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;

    private Vector3 targetPos;
    private Vector3 StartPos = new Vector3(2, 4, -8.5f);
    private Vector3 StartRot = new Vector3(13, -8, 0);

    private Vector3 PlayPos = new Vector3(1.5f, 7, -14);
    private Vector3 PlayRot = new Vector3(18, -5.5f, 0);
    private Vector3 moveVector;
    private Vector3 moveOffset = new Vector3(0, 0, 5);
    private float mSpeed = 10f;

    private Vector3 FightPos = new Vector3(4.5f, 4, 145.7f);
    private Quaternion FightRot = Quaternion.Euler(26, -90, 0);
    private float mSpeedFight = 3f;
    private float transition = 0;
    private float timeAnim = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(GameManager.Instance.State)
        {
            //case STATE.START_GAME:
            //    transform.position = Vector3.Lerp(transform.position, PlayPos, Time.deltaTime);
            //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(PlayRot), transition);
            //    transition += Time.deltaTime * mSpeed / timeAnim;
            //    if(transition > 1)
            //    {
            //        GameManager.Instance.State = STATE.PLAY_GAME;
            //    }
            //    break;
            case STATE.PLAY_GAME:
                moveVector = lookAt.position + PlayPos + moveOffset;
                moveVector.x = PlayPos.x;
                moveVector.y = PlayPos.y;
                transform.position = Vector3.Lerp(transform.position, moveVector, Time.deltaTime);
                break;
            //case STATE.END_RUN:
            //    transform.position = Vector3.Lerp(transform.position, FightPos, mSpeedFight * Time.deltaTime);
            //    transform.rotation = Quaternion.Slerp(transform.rotation, FightRot, mSpeedFight * Time.deltaTime);
            //    break;
        }
    }
}
