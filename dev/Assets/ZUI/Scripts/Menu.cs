using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("UI/ZUI/Menu", 1)]
public class Menu : ZUIElementBase {

    [Tooltip("The menu that will be opened after calling Back() on ZUIManager (optional).")]
    public Menu PreviousMenu;
    [Tooltip("The menu that will be opened after calling NextMenu() on ZUIManager (optional).")]
    public Menu NextMenu;
    [Tooltip("Should this menu deactivate while its not visible on the screen.")]
    public bool DeactivateWhileInvisible = true;

    [Tooltip("Click on \"Update Animated Elements\" to get all the  elements under this object.")]
    public List<UIElement> AnimatedElements = new List<UIElement>();
    [Tooltip("For elements that exists in more than one menu.")]
    public List<UIElement> MultiMenusAnimatedElements = new List<UIElement>();
    [Tooltip("The percentage of hiding time to pass before switching to the next menu after.")]
    [Range(0,1)]
    public float SwitchAfter = 0f;

    private float hidingTime;

    /// <summary>
    /// Change the visibilty of this menu by playing the desired animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    public override void ChangeVisibility(bool visible)
    {
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

        foreach (UIElement e in MultiMenusAnimatedElements)
        {
            if (e == null || !e.MenuDependent) continue;

            if (visible)
            {
                if (!UseSimpleActivation)
                {
                    if (!e.gameObject.activeSelf)
                        e.gameObject.SetActive(true);
                    e.ChangeVisibility(true);
                }
                else
                    e.gameObject.SetActive(true);
            }
            else
            {
                if (!ZUIManager.Instance.CurActiveMenu.MultiMenusAnimatedElements.Contains(e))
                {
                    if (!UseSimpleActivation)
                        e.ChangeVisibility(false);
                    else
                        e.gameObject.SetActive(false);
                }
            }
        }


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
    /// Change the visibilty of this menu instantly without playing animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    public override void ChangeVisibilityImmediate(bool visible)
    {
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

        foreach (UIElement e in MultiMenusAnimatedElements)
        {
            if (e == null || !e.MenuDependent) continue;

            if (visible)
            {
                if (!UseSimpleActivation)
                {
                    if (!e.gameObject.activeSelf)
                        e.gameObject.SetActive(true);
                    e.ChangeVisibilityImmediate(true);
                }
                else
                    e.gameObject.SetActive(true);
            }
            else
            {
                if (!ZUIManager.Instance.CurActiveMenu.MultiMenusAnimatedElements.Contains(e))
                {
                    if (!UseSimpleActivation)
                        e.ChangeVisibilityImmediate(false);
                    else
                        e.gameObject.SetActive(false);
                }
            }
        }


        if (Visible != visible)
        {
            if (SFXManager.Instance)
            SFXManager.Instance.PlayClip(visible ? ShowingClip : HidingClip);
        }

        Visible = visible;

        if (visible && OnShow != null)
            OnShow.Invoke();
        //else if (!visible && OnHide != null)
        //    OnHide.Invoke();

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
    /// Initialize UIElements, call this first if you plan to change visibility in the same frame this menu is activated at.
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
        for (int i = 0; i < MultiMenusAnimatedElements.Count; i++)
        {
            if (MultiMenusAnimatedElements[i] != null)
                MultiMenusAnimatedElements[i].Initialize();
        }
        hidingTime = GetAllHidingTime();
        Initialized = true;
    }

    void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}