using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/ZUI/UI Elements Group", 4)]
public class UIElementsGroup : ZUIElementBase
{
    [Tooltip("Should this menu prewarm the animation at start?")]
    public bool Prewarm = true;

    [Tooltip("Click on \"Update Animated Elements\" to get all the  elements under this object.")]
    public List<UIElement> AnimatedElements = new List<UIElement>();
    [Tooltip("Should this group deactivate while its not visible on the screen.")]
    public bool DeactivateWhileInvisible = true;

    private float hidingTime;
    private bool forceVisibilityCall;          //Used to indicate if there was a ChangeVisibility call before changing it by default at Start() so it doesn't do it.

    void Start()
    {
        if (!forceVisibilityCall)
        {
            if (!Prewarm)
                ChangeVisibility(Visible);
            else
                ChangeVisibilityImmediate(Visible);
        }
    }

    /// <summary>
    /// Change the visibilty of the menu by playing the desired animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    public override void ChangeVisibility(bool visible)
    {
        forceVisibilityCall = true;

        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
        {
            ChangeVisibilityImmediate(Visible);
            gameObject.SetActive(true);
        }

        if (!UseSimpleActivation)
        {
            foreach (UIElement e in AnimatedElements)
            {
                if (e == null || !e.MenuDependent) continue;

                e.ChangeVisibility(visible);
            }
        }
        else
            gameObject.SetActive(visible);

        if (Visible != visible)
        {
            if (SFXManager.Instance)
                SFXManager.Instance.PlayClip(visible ? ShowingClip : HidingClip);
        }

        Visible = visible;

        if (visible && OnShow != null)
            OnShow.Invoke();
        else if (!visible && OnHide != null)
            OnHide.Invoke();

        if (DeactivateWhileInvisible)
        {
            if (!visible)
                Invoke("DeactivateMe", hidingTime);
            else
                CancelInvoke("DeactivateMe");
        }
    }
    /// <summary>
    /// Change the visibilty of the menu instantly without playing animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    public override void ChangeVisibilityImmediate(bool visible)
    {
        forceVisibilityCall = true;

        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (!UseSimpleActivation)
        {
            foreach (UIElement e in AnimatedElements)
            {
                if (e == null || !e.MenuDependent) continue;

                e.ChangeVisibilityImmediate(visible);
            }
        }
        else
            gameObject.SetActive(visible);

        if (Visible != visible)
        {
            if (SFXManager.Instance)
                SFXManager.Instance.PlayClip(visible ? ShowingClip : HidingClip);
        }

        Visible = visible;

        if (visible && OnShow != null)
            OnShow.Invoke();
        else if (!visible && OnHide != null)
            OnHide.Invoke();

        if (DeactivateWhileInvisible && !visible)
            DeactivateMe();

    }

    /// <summary>
    /// The duration it takes to finish the hiding animation.
    /// </summary>
    /// <returns></returns>
    public float GetAllHidingTime()
    {
        if (hidingTime != 0)
            return hidingTime;

        for (int i = 0; i < AnimatedElements.Count; i++)
        {
            UIElement uiA = AnimatedElements[i];
            if (uiA.HideAfter + uiA.Duration > hidingTime)
                hidingTime = uiA.HideAfter + uiA.Duration;

            float movementDuration = (uiA.MovementDuration > 0 ? uiA.MovementDuration : uiA.Duration);
            if (uiA.MovementHideAfter + movementDuration > hidingTime)
                hidingTime = uiA.MovementHideAfter + movementDuration;

            float rotationDuration = (uiA.RotationDuration > 0 ? uiA.RotationDuration : uiA.Duration);
            if (uiA.RotationHideAfter + rotationDuration > hidingTime)
                hidingTime = uiA.RotationHideAfter + rotationDuration;

            float scaleDuration = (uiA.ScaleDuration > 0 ? uiA.ScaleDuration : uiA.Duration);
            if (uiA.ScaleHideAfter + scaleDuration > hidingTime)
                hidingTime = uiA.ScaleHideAfter + scaleDuration;

            float opacityDuration = (uiA.OpacityDuration > 0 ? uiA.OpacityDuration : uiA.Duration);
            if (uiA.OpacityHideAfter + opacityDuration > hidingTime)
                hidingTime = uiA.OpacityHideAfter + opacityDuration;
        }
        return hidingTime;
    }
    /// <summary>
    /// Initialize UIElements, call this first if you plan to change visibility in the same frame this menu is activate at.
    /// </summary>
    public void InitializeElements()
    {
        if (Initialized) return;


        for (int i = 0; i < AnimatedElements.Count; i++)
        {
            if (AnimatedElements[i] != null)
                AnimatedElements[i].Initialize();
            else
            {
                AnimatedElements.RemoveAt(i);
                i--;
            }
        }
        hidingTime = GetAllHidingTime();
        Initialized = true;
    }

    void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}