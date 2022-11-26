using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemData : MonoBehaviour{
    public int gemID;           // 宝石编号
    public int gemRoomID;       // 宝石所在房间号
    public bool isValid;        // 宝石是否还存在于地图中
    public bool isCatch;        // 玩家是否碰到宝石
}
