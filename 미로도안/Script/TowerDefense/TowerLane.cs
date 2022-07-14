using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLane : MonoBehaviour
{
    // 타워 레인의 상태
    public enum TowerLaneState
    {
        BuildReady, BuildOn
    }
    public TowerLaneState towerLaneState = TowerLaneState.BuildReady;

    // 타워 건설된 레인
    public void TowerBuildOn()
    {
        towerLaneState = TowerLaneState.BuildOn;
    }
}
