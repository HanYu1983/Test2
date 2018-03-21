using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public  class ZUIElementBase : MonoBehaviour {

    public bool Visible;
    [Tooltip("If enabled this GameObject will be activated and deactivated instead of animating its elements.")]
    public bool UseSimpleActivation = false;

    [Tooltip("The clip that will be played once this menu is opened.")]
    public AudioClip ShowingClip;
    [Tooltip("The clip that will be played once this menu is closed.")]
    public AudioClip HidingClip;

    public UnityEvent OnShow;
    public UnityEvent OnHide;

    public bool Initialized;

    /// <summary>
    /// Change the visibilty of the object by playing the desired animation.
    /// </summary>
    /// <param name="visible"></param>
    public virtual void ChangeVisibility(bool visible)
    {
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Change the visibilty of the object instantly without playing animation.
    /// </summary>
    /// <param name="visible"></param>
    public virtual void ChangeVisibilityImmediate(bool visible)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Switch the visibility of the object by playing the desired animation.
    /// </summary>
    public virtual void SwitchVisibility()
    {
        ChangeVisibility(!Visible);
    }
    /// <summary>
    /// Switch the visibility of the object instantly without playing animation.
    /// </summary>
    public virtual void SwitchVisibilityImmediate()
    {
        ChangeVisibilityImmediate(!Visible);
    }
}
