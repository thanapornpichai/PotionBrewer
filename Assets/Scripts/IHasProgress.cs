using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    public event EventHandler<OnPregressChangedEventArgs> OnPregressChange;
    public class OnPregressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
