using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
// A custome editor for the EnemyVisibiity class. Visualizes and allows for editing the visibility range.
[CustomEditor(typeof(EnemyVisibility))]

public class Enemy2VisibilityEditor : Editor
{
    // Called when Unity needs to draw the Scene view.
    private void OnSceneGUI()
    {
        //get a reference to the EnemyVisibility script we're looking at
        var visibility = target as EnemyVisibility;
        //start drawing at 10% opacity
        Handles.color = new Color(1, 1, 1, 0.1f);
        //Drawing an arc sweeps from the point you give it. We want to draw the arc such that the middle of the arc is 
        //in front of the object, we we'll take the forward direction and rotate it by half the angle.
        var forwardPointMinusHalfAngle =
            //rotate around the y-axis by half the angle
            Quaternion.Euler(0, -visibility.stateManager.visibilityAngle / 2, 0)
                        //rotate the forward direction by this
                        * visibility.transform.forward;
        //Draw the arc to visualize the visibiity arc
        Vector3 arcStart = forwardPointMinusHalfAngle * visibility.stateManager.maxDetectDistance;
        Handles.DrawSolidArc(
            visibility.transform.position,        //center of the arc
            Vector3.up,                           //up direction of the arc
            arcStart,                              //point where it begins
            visibility.stateManager.visibilityAngle,                     //angle of the arc
            visibility.stateManager.maxDetectDistance               //radius of the arc
            );
        //Draw a scale handle at the edge of the arc; if the user drags it, update the arc size.
        //Reset handle color to full opacity.
        Handles.color = Color.white;
        //Compute the position of the handle, based on the object's position, the direction it's facing, and the distance
        Vector3 handlePosition = visibility.transform.position + visibility.transform.forward * visibility.stateManager.maxDetectDistance;
        //Draw the handle and store the result
        visibility.stateManager.maxDetectDistance = Handles.ScaleValueHandle(
            visibility.stateManager.maxDetectDistance,            //current value
            handlePosition,                  //handle position
            visibility.transform.rotation,   //orientation
            1,                                  //size
            Handles.ConeHandleCap,         //cap to draw
            0.25f);                             //snap to multiples of this if snapping key is held down
    }
}
#endif
