using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 计分控制器
public class RoundController : MonoBehaviour
{
    public IActionManager actionManager;
    public DiskFactory diskFactory;
    public ScoreRecorder scoreRecorder;
    public UserGUI userGUI;

    int round;
    int diskNum;
    int trial;
    float time;

    bool start;

    void Start() {
        // 通过单例模式获取对象
        actionManager = Singleton<CCActionManager>.Instance;
        diskFactory = Singleton<DiskFactory>.Instance;
        userGUI = Singleton<UserGUI>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        // 初始设置
        time = 0.0F;
        round = 1;
        trial = 0;
        diskNum = 5;
        start = false;
        SetMode(true);
        scoreRecorder.Reset();
    }

    public void SetMode(bool isPhysis) {
        // 物理运动
        if(isPhysis) {
            actionManager = Singleton<PhysisActionManager>.Instance as IActionManager;
        }
        // 非物理运动
        else {
            actionManager = Singleton<CCActionManager>.Instance as IActionManager;
        }
    }

    // 设置飞碟的速度等参数
    public void GenerateDisk(int level) {
        GameObject disk = diskFactory.GetDisk(level);
        disk.transform.position = new Vector3(0, 0, 0);
        disk.SetActive(true);
        actionManager.Fly(disk);
    }

    public void UpdateScore(DiskData disk){
        scoreRecorder.UpdateScore(disk);
    }

    public int GetRound(){
        return round;
    }

    public void GameStart(){
        start = true;
    }

    public void Reset(){
        scoreRecorder.Reset();
        time = 0.0F;
        round = 1;
        trial = 0;
        diskNum = 5;
        start = false;
    }

    void Update() {
        if(start) {
            time += Time.deltaTime;

            if(round > 5){
                start = false;
                return;
            }

            if(trial == 10){
                // NEXT ROUND
                trial = 0;
                round++;
                diskNum += round;
            }

            if(time >= 2.0F - round * 0.2F){
                // NEXT TRIAL
                time = 0.0F;
                for(int i = 0; i < diskNum; i++){
                    GenerateDisk(Random.Range(1,6));
                }            
                trial++;
            }
        }
    }
}
