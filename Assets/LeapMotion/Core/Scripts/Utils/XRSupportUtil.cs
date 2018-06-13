using UnityEngine;

using UnityEngine.VR;

namespace Leap.Unity {

  /// <summary>
  /// Wraps various (but not all) "XR" calls with Unity 5.6-supporting "VR" calls
  /// via #ifdefs.
  /// </summary>
  public static class XRSupportUtil {

    public static bool IsXREnabled() {
      return UnityEngine.XR.XRSettings.enabled;
  
    }

    public static bool IsXRDevicePresent() {
      return UnityEngine.XR.XRDevice.isPresent;
     
    }

    public static Vector3 GetXRNodeCenterEyeLocalPosition() {
      return UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
    
    }

    public static Quaternion GetXRNodeCenterEyeLocalRotation() {
      return UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye);
    
    }

    public static Vector3 GetXRNodeHeadLocalPosition() {
      return UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.Head);
    
    }

    public static Quaternion GetXRNodeHeadLocalRotation() {
      return UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);
     
    }

    public static void Recenter() {
      UnityEngine.XR.InputTracking.Recenter();
    }

    public static string GetLoadedDeviceName() {
      return UnityEngine.XR.XRSettings.loadedDeviceName;
     
    }

  }

}