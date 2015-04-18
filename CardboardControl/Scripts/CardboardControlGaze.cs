using UnityEngine;
using System.Collections;
using CardboardInputDelegates;

/**
* Creating a vision raycast and handling the data from it
* Relies on Google Cardboard SDK API's
*/
public class CardboardControlGaze : MonoBehaviour {
  public float maxDistance = Mathf.Infinity;
  public LayerMask layerMask = Physics.DefaultRaycastLayers;
  public bool vibrateOnChange = false;
  
  private GameObject recentObject = null;
  private float gazeStartTime = 0f;
  private CardboardHead head;
  private RaycastHit hit;
  private bool isHeld;
  
  public CardboardInputDelegate OnChange = delegate {};

  public void Start() {
    StereoController stereoController = Camera.main.GetComponent<StereoController>();
    head = stereoController.Head;
  }
  
  public void Update() {
    isHeld = Physics.Raycast(head.Gaze, out hit, maxDistance, layerMask);
    CheckGaze();
  }

  private void CheckGaze() {
    if (recentObject != Object()) ReportGazeChange();
    recentObject = Object();
  }

  private void ReportGazeChange() {
    OnChange(this, new CardboardInputEvent());
    //if (debugNotificationsEnabled) Debug.Log(" *** Gaze Changed *** ");
    if (vibrateOnChange) Handheld.Vibrate();
    gazeStartTime = Time.time;
  }

  public bool IsHeld() {
    return isHeld;
  }

  public float SecondsHeld() {
    if (gazeStartTime == 0f) return 0f;
    return Time.time - gazeStartTime;
  }

  public RaycastHit Hit() {
    return hit;
  }

  public GameObject Object() {
    if (IsHeld()) {
      return hit.transform.gameObject;
    } else {
      return null;
    }
  }
}
