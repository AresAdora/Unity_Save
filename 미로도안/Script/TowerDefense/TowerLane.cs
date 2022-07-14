using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLane : MonoBehaviour
{
    // Ÿ�� ������ ����
    public enum TowerLaneState
    {
        BuildReady, BuildOn
    }
    public TowerLaneState towerLaneState = TowerLaneState.BuildReady;

    // Ÿ�� �Ǽ��� ����
    public void TowerBuildOn()
    {
        towerLaneState = TowerLaneState.BuildOn;
    }
}
