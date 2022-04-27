using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AN_HeroInteractive : MonoBehaviour
{
    [Tooltip("Are you have any key?")]
    public bool RedKey = false, BlueKey = false;
    [Tooltip("Child empty object for plug following")]
    public Transform GoalPosition;
}
