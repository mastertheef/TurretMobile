using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionBase : MonoBehaviour {
    public abstract bool Condition();
    public abstract string MissionDescription { get; }
}
