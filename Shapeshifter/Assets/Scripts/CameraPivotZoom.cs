using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is meant to be used with the class CameraZoom to control when to zoom the camera, how and how much we zoom.
 * 
 * @Author Nicolaas Schuddeboom
 */ 

[System.Serializable]
public class CameraPivotZoom
{
    // The x position the player needs to be in for the camera to zoom
    public int passingDistance;
    
    // How much the camera needs to zoom
    public int zoomSize;

    // If we zoom in or out
    public bool zoomOut = true;
}
