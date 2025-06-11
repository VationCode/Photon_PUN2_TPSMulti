using System;
using UnityEngine;

public interface IInputService
{
    event Action IsRun;
    event Action IsCrouch;
    event Action IsJump;
    event Action IsDodge;
    event Action IsSlide;
}
