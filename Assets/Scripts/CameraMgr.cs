using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MonoBehaviour

{
    public static CameraMgr Instance;
    public List<GameObject> listCamera;

    [HideInInspector]
    public Timer timerControl = new Timer();

    private int cameraIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < listCamera.Count; i++)
        {
            listCamera[i].SetActive(i == 0);

        }
    }

    void Update()
    {
        switch (GameManager.Instance.State)
        {
            case STATE.START_GAME:
                timerControl.Update(Time.deltaTime);
                if(timerControl.JustFinished())
                {
                    GameManager.Instance.State = STATE.PLAY_GAME;
                }
                break;
            case STATE.PLAY_GAME:
                break;
            case STATE.END_RUN:
                break;
        }
    }

    public void UpdateCamera()
    {
        listCamera[cameraIndex].SetActive(false);
        cameraIndex += 1;
        listCamera[cameraIndex].SetActive(true);
    }
}
