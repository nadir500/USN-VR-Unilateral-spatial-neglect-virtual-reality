using UnityEngine;

using UnityEngine.VR;

namespace Leap.Unity {

  /// <summary>
  /// Wraps various (but not all) "XR" calls with Unity 5.6-supporting "VR" calls
  /// via #ifdefs.
  /// </summary>
  public static class XRSupportUtil {

    public static bool IsXREnabled() {
      return VRSettings.enabled;
  
    }

    public static bool IsXRDevicePresent() {
      return VRDevice.isPresent;
     
    }

    public static Vector3 GetXRNodeCenterEyeLocalPosition() {
      return InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye);
    
    }

    public static Quaternion GetXRNodeCenterEyeLocalRotation() {
      return InputTracking.GetLocalRotation(VRNode.CenterEye);
    
    }

    public static Vector3 GetXRNodeHeadLocalPosition() {
      return InputTracking.GetLocalPosition(VRNode.Head);
    
    }

    public static Quaternion GetXRNodeHeadLocalRotation() {
      return InputTracking.GetLocalRotation(VRNode.Head);
     
    }

    public static void Recenter() {
      InputTracking.Recenter();
    }

    public static string GetLoadedDeviceName() {
      return VRSettings.loadedDeviceName;
     
    }

  }

}