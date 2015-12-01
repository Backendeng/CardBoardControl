﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using CardboardControlDelegates;

/**
* Bring all the control scripts together to provide a convenient API
*/
public class CardboardControl : MonoBehaviour {
  [HideInInspector]
  public CardboardControlTrigger trigger;
  [HideInInspector]
  public CardboardControlGaze gaze;
  [HideInInspector]
  public CardboardControlBox box;

  private const float TIME_TO_CALIBRATE = 1f;
  private Dictionary<string,float> cooldownCounter = new Dictionary<string,float>() {
    {"OnUp", TIME_TO_CALIBRATE},
    {"OnDown", TIME_TO_CALIBRATE},
    {"OnClick", TIME_TO_CALIBRATE},
    {"OnChange", TIME_TO_CALIBRATE},
    {"OnTilt", TIME_TO_CALIBRATE}
  };
  public float eventCooldown = 0.2f;

  public void Awake() {
    trigger = gameObject.GetComponent<CardboardControlTrigger>();
    gaze = gameObject.GetComponent<CardboardControlGaze>();
    box = gameObject.GetComponent<CardboardControlBox>();
  }

  public void Update() {
    List<string> keys = new List<string>(cooldownCounter.Keys);
    foreach(string key in keys) {
      if (cooldownCounter[key] > 0f) cooldownCounter[key] -= Time.deltaTime;
    }
  }

  public bool EventReady(string name) {
    if (cooldownCounter[name] <= 0f) {
      cooldownCounter[name] = eventCooldown;
      return true;
    }
    return false;
  }
}
