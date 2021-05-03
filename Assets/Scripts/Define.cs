using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
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
}

public static class TAG
{
    public const string COLOR = "Color";
    public const string STATION = "Station";
    public const string DIAMOND = "Diamond";
    public const string BOSS = "Boss";
}

public enum STATE
{
    READY,
    START_GAME,
    PLAY_GAME,
}
