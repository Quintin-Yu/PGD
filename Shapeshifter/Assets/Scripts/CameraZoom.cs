using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/**
 * A camera zoom controller to give the player more sight when needed
 * 
 * @Author Nicolaas Schuddeboom
 */ 

public class CameraZoom : MonoBehaviour
{
    // Declare variables
    public GameObject player;
    public CinemachineVirtualCamera vcam;

    // The list of positions where we zoom.
    public List<CameraPivotZoom> pivots;

    void FixedUpdate()
    {
        // Go though the list backwards. If the player is at the last pivot, why check the others?
        for(int i = pivots.Count - 1; i >= 0; i--)
        {
            // If the player is past the pivot...
            if (player.transform.position.x > pivots[i].passingDistance)
            {
                // Check if we zoom in and out. Zoom the camera to the right value
                if (pivots[i].zoomOut && vcam.m_Lens.OrthographicSize < pivots[i].zoomSize)
                {
                    vcam.m_Lens.OrthographicSize += 0.1f;
                }
                if (!pivots[i].zoomOut && vcam.m_Lens.OrthographicSize > pivots[i].zoomSize)
                {
                    vcam.m_Lens.OrthographicSize -= 0.1f;
                }

                // Stop checking the list
                return;
            }
        }
    }
}
