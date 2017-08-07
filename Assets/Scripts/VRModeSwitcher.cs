using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRModeSwitcher : MonoBehaviour
{    
    [SerializeField]
    List<Camera> cameras;

    public void MagicWindowButtonPressed()
    {
        UnityEngine.VR.VRSettings.enabled = !UnityEngine.VR.VRSettings.enabled;// .VRModeEnabled = !cardboard.VRModeEnabled;
    }

    private void Update()
    {
        if (!UnityEngine.VR.VRSettings.enabled)
        {
            foreach(var cam in cameras)
            {
                cam.transform.rotation = UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
            }
        }
    }
}
