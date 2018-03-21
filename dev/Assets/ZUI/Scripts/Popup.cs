using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/ZUI/Pop-up", 2)]
public class Popup : ZUIElementBase {

    [Tooltip("The text to type the title in, if there's any (optional).")]
    public Text TitleHolder;
    [Tooltip("The text to type the body in, if there's any (optional).")]
    public Text BodyHolder;

    [Tooltip("All the elements to animate inside this popup")]
    public List<UIElement> AnimatedElements = new List<UIElement>();
    [Tooltip("Should this pop-up deactivate while its not visible on the screen.")]
    public bool DeactivateWhileInvisible = true;

    private bool forceVisible;          //Used to make sure we do not intend to keep this popup visible before hiding it at Start()
    private float hidingTime;


    void Start()
    {
        if (!forceVisible)
            ChangeVisibilityImmediate(false);
    }

    /// <summary>
    /// Change the visibilty of the menu by playing the desired animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    public override void ChangeVisibility(bool visible)
    {
        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
        {
            ChangeVisibilityImmediate(Visible);
            gameObject.SetActive(true);
        }

        forceVisible = true;

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
        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        forceVisible = true;

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
    /// Update both, the title and the body of the pop-up.
    /// </summary>
    public void UpdateInformation(string info, string title)
    {
        if (BodyHolder)
            BodyHolder.text = info;
        else
            Debug.LogError("You're trying to update the body of the pop-up while there's no text component to change.", gameObject);

        if (TitleHolder)
            TitleHolder.text = title;
        else
            Debug.LogError("You're trying to update the title of the pop-up while there's no text component to change.", gameObject);
    }
    /// <summary>
    /// Update the body of the pop-up.
    /// </summary>
    public void UpdateBody(string info)
    {
        if (BodyHolder)
            BodyHolder.text = info;
        else
            Debug.LogError("You're trying to update the body of the pop-up while there's no text component to change.", gameObject);
    }
    /// <summary>
    /// Update the title of the pop-up.
    /// </summary>
    public void UpdateTitle(string title)
    {
        if (TitleHolder)
            TitleHolder.text = title;
        else
            Debug.LogError("You're trying to update the title of the pop-up while there's no text component to change.", gameObject);
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
        hidingTime = GetAllHidingTime();
        Initialized = true;
    }

    void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}
