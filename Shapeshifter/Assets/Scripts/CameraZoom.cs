using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class CameraPivotZoom
{
    public int passingDistance;
    public int zoomSize;

    public bool zoomOut = true;
}

public class CameraZoom : MonoBehaviour
{
    public GameObject player;
    public CinemachineVirtualCamera vcam;

    public List<CameraPivotZoom> pivots;

    void FixedUpdate()
    {
        for(int i = pivots.Count - 1; i >= 0; i--)
        {
            if (player.transform.position.x > pivots[i].passingDistance)
            {
                if (pivots[i].zoomOut && vcam.m_Lens.OrthographicSize < pivots[i].zoomSize)
                {
                    vcam.m_Lens.OrthographicSize += 0.1f;
                }
                if (!pivots[i].zoomOut && vcam.m_Lens.OrthographicSize > pivots[i].zoomSize)
                {
                    vcam.m_Lens.OrthographicSize -= 0.1f;
                }
                return;
            }
        }
    }
}
