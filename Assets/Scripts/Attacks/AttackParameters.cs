using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackParameters{
    public Attack attack;
    public float speed = 1;
    public bool randomRotation = true;
    public bool simultaneoausWithPrevious = false;
    public float delay;
    [HideInInspector]
    public float realTime;
    [HideInInspector]
    public Quaternion originalRotation;
}
