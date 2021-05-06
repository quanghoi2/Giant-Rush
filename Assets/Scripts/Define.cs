using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public const float TIME_FINISH_HIT = 1.02f;
    public const float TIME_FINISH_KICK = 0.2f;
    public const float TIME_AUTO_ATTACK = 1.5f;
}

public enum CHAR_COLOR
{
    GREEN,
    YELLOW,
    RED,
    COUNT,
}

public static class CHAR_ANIM
{
    public const string IDLE = "Idle";
    public const string RUNNING = "Running";
    public const string READY_HIT = "ReadyHit";
    public const string HITTED = "Hitted";
    public const string HIT = "Hit";
    public const string KICK = "Kick";
    public const string KNOCK_OUT = "KnockOut";
}

public static class TAG
{
    public const string COLOR = "Color";
    public const string STATION = "Station";
    public const string DIAMOND = "Diamond";
    public const string BOSS = "Boss";
    public const string END_RUN = "EndRun";
}

public enum STATE
{
    READY,
    START_GAME,
    PLAY_GAME,
    END_RUN,
    FIGHT,
}

public enum PLAYER_STATE
{
    READY,
    RUN,
    CONTROL,
    END_RUN,
    READY_HIT,
    HIT,
    FINISH_HIT,
    HITTED,
    FINISH_HITTED,
    KICK,
    FINISH_KICK,
    KNOCK_OUT,
    GAME_OVER,
}

public enum BOSS_STATE
{
    IDLE,
    READY_HIT,
    HIT,
    FINISH_HIT,
    HITTED,
    FINISH_HITTED,
    KICK,
    FINISH_KICK,
    KNOCK_OUT,
}
