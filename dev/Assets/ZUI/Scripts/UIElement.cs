using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[AddComponentMenu("UI/ZUI/UI Element", 0)]
public class UIElement : ZUIElementBase {

    #region Variables

    #region Settings
    [Tooltip("Should this element be controlled by a menu?")]
    public bool MenuDependent = true;
    [Tooltip("Should this element prewarm the animation at start?")]
    public bool Prewarm = true;
    [Tooltip("Should this element deactivate while its not visible on the screen.")]
    public bool DeactivateWhileInvisible;
    public enum ScreenSides { Top, Bottom, Left, Right, TopLeftCorner, TopRightCorner, BotLeftCorner, BotRightCorner, Custom }
    public enum MotionType { None, EaseIn, Linear, Bounce }
    [Tooltip("The duration in seconds it takes before the object starts animating in to the screen.")]
    public float StartAfter = 0.0f;
    [Tooltip("The duration in seconds it takes before the object starts animating out of the screen.")]
    public float HideAfter = 0.0f;
    [Tooltip("Duration of the animation in seconds (controls the speed).")]
    public float Duration = 0.3f;
    public ZUIElementBase ControlledBy;
    #endregion

    #region Movement
    [Tooltip("Type of movement animation.")]
    public MotionType MovementType = MotionType.EaseIn;
    [Tooltip("The position the element will move to when its hiding.")]
    public ScreenSides HidingPosition;
    [Tooltip("Custom hiding position as percentage of the screen.")]
    public Vector2 CustomHidingPosition;
    [Tooltip("The gap between the element and the edge of the screen while it's hiding (in percentage of element's width or height).")]
    public float EdgeGap = 0.25f;
    [Tooltip("Intensity of the ease.")]
    public float MovementEaseIntensity = 2;
    [Tooltip("Number of bounces the element should make.")]
    public int MovementBounces = 1;
    [Tooltip("Bouncing power.")]
    [Range(0,10)]
    public int MovementBouncePower = 3;
    [Tooltip("Should the hiding bounce be disabled? (when the object is hiding).")]
    public bool MovementDisableHidingBounce = false;
    [Tooltip("Custom time to start after (seconds).")]
    public float MovementStartAfter = -1;
    [Tooltip("Custom time to hide after (seconds).")]
    public float MovementHideAfter = -1;
    [Tooltip("Custom duration for the animation (seconds).")]
    public float MovementDuration = -1;
    [HideInInspector]
    public Vector3 startPosition;
    private Vector3 outOfScreenPos;
    private IEnumerator movementEnum;
    #endregion

    #region Rotation
    [Tooltip("Type of rotation animation.")]
    public MotionType RotationType = MotionType.None;
    [Tooltip("The rotation the element will rotate to when its hiding.")]
    public Vector3 HidingRotation = new Vector3(0,0,90);
    [Tooltip("Intensity of the ease.")]
    public float RotationEaseIntensity = 2;
    [Tooltip("Number of bounces the element should make.")]
    public int RotationBounces = 1;
    [Tooltip("Bouncing power.")]
    [Range(0, 10)]
    public int RotationBouncePower = 3;
    [Tooltip("Should the hiding bounce be disabled? (when the object is hiding).")]
    public bool RotationDisableHidingBounce = false;
    [Tooltip("Custom time to start after (seconds).")]
    public float RotationStartAfter = -1;
    [Tooltip("Custom time to hide after (seconds).")]
    public float RotationHideAfter = -1;
    [Tooltip("Custom duration for the animation (seconds).")]
    public float RotationDuration = -1;
    private Vector3 startEuler;
    private IEnumerator rotationEnum;
    #endregion

    #region Scale
    [Tooltip("Type of scale animation.")]
    public MotionType ScaleType = MotionType.None;
    [Tooltip("The scale the element will change to when its hiding.")]
    public Vector3 HidingScale = Vector3.zero;
    [Tooltip("Intensity of the ease.")]
    public float ScaleEaseIntensity = 2;
    [Tooltip("Number of bounces the element should make.")]
    public int ScaleBounces = 1;
    [Tooltip("Bouncing power.")]
    [Range(0,10)]
    public int ScaleBouncePower = 3;
    [Tooltip("Should the hiding bounce be disabled? (when the object is hiding).")]
    public bool ScaleDisableHidingBounce = false;
    [Tooltip("Custom time to start after (seconds).")]
    public float ScaleStartAfter = -1;
    [Tooltip("Custom time to hide after (seconds).")]
    public float ScaleHideAfter = -1;
    [Tooltip("Custom duration for the animation (seconds).")]
    public float ScaleDuration = -1;
    private Vector3 startScale;
    private IEnumerator scaleEnum;
    #endregion

    #region Opacity
    [Tooltip("Type of opacity animation.")]
    public MotionType OpacityType = MotionType.None;
    [Range(0,1)]
    [Tooltip("The opacity the element will change to when its hiding.")]
    public float HidingOpacity = 0;
    [Tooltip("Intensity of the ease.")]
    public float OpacityEaseIntensity = 2;
    [Tooltip("Number of bounces the element should make.")]
    public int OpacityBounces = 1;
    [Tooltip("Bouncing power.")]
    [Range(0, 10)]
    public int OpacityBouncePower = 3;
    [Tooltip("Should the hiding bounce be disabled? (when the object is hiding).")]
    public bool OpacityDisableHidingBounce = false;
    [Tooltip("Custom time to start after (seconds).")]
    public float OpacityStartAfter = -1;
    [Tooltip("Custom time to hide after (seconds).")]
    public float OpacityHideAfter = -1;
    [Tooltip("Custom duration for the animation (seconds).")]
    public float OpacityDuration = -1;

    [HideInInspector]
    public Image myImage;
    [HideInInspector]
    public Text myText;
    [HideInInspector]
    public CanvasGroup myCanvasGroup;

    private IEnumerator opacityEnum;
    private float startOpacity;
    #endregion

    #region Actiate/Deactivate
    private IEnumerator activationEnum;
    #endregion


    #region Private Variables
    private RectTransform myRT;
    private CanvasScaler parentCanvasScaler;
    private RectTransform parentCanvasRT;
    private float canvasHalfWidth;
    private float canvasHalfHeight;
    public float myRTWidth;
    public float myRTHeight;
    private bool forceVisibilityCall;          //Used to indicate if there was a ChangeVisibility call before changing it by default at Start() so it doesn't do it.
    private float hidingTime;
    #endregion

    #endregion

    void Start()
    {
        if (!Initialized)
            Initialize();

        if (MenuDependent)
        {
            if (!ControlledBy)
                Debug.Log("Element \"" + gameObject.name + "\" is Menu Dependent but there's no holder controlling it, are you sure it was meant to be Menu Dependent?", gameObject);
            return;
        }

        if (!forceVisibilityCall)
        {
            if (Prewarm)
                ChangeVisibilityImmediate(Visible);
            else
                ChangeVisibility(Visible);
        }
    }

    public void Initialize()
    {
        if (Initialized) return;

        hidingTime = GetAllHidingTime();

        myRT = GetComponent<RectTransform>();
        parentCanvasScaler = GetComponentInParent<CanvasScaler>();
        parentCanvasRT = parentCanvasScaler.GetComponent<RectTransform>();

        Vector2 canvasLossyScale = parentCanvasRT.lossyScale;

        canvasHalfWidth = canvasLossyScale.x * parentCanvasRT.rect.width / 2;
        canvasHalfHeight = canvasLossyScale.y * parentCanvasRT.rect.height / 2;

        myRTWidth = canvasLossyScale.x * myRT.rect.width;
        myRTHeight = canvasLossyScale.y * myRT.rect.height;

        startPosition = myRT.position;
        startEuler = myRT.eulerAngles;
        startScale = myRT.localScale;
        outOfScreenPos = GetHidingPosition(HidingPosition, EdgeGap, CustomHidingPosition);

        myImage = GetComponent<Image>();
        if (myImage)
        {
            startOpacity = myImage.color.a;
        }
        else
        {
            myText = GetComponent<Text>();
            if (myText)
                startOpacity = myText.color.a;
            else
            {
                myCanvasGroup = GetComponent<CanvasGroup>();
                if (myCanvasGroup)
                    startOpacity = myCanvasGroup.alpha;
                else
                {
                    if (OpacityType != MotionType.None)
                        Debug.LogError("Theres no Image, Text nor CanvasGroup component on " + gameObject.name + " must have one of them to use the Opacity trasition.", gameObject);
                }
            }
        }
        Initialized = true;
    }
    /// <summary>
    /// The duration it takes to finish the hiding animation.
    /// </summary>
    /// <returns></returns>
    public float GetAllHidingTime()
    {
        if (hidingTime != 0)
            return hidingTime;

        if (HideAfter + Duration > hidingTime)
            hidingTime = HideAfter + Duration;

        float movementDuration = (MovementDuration > 0 ? MovementDuration : Duration);
        if (MovementHideAfter + movementDuration > hidingTime)
            hidingTime = MovementHideAfter + movementDuration;

        float rotationDuration = (RotationDuration > 0 ? RotationDuration : Duration);
        if (RotationHideAfter + rotationDuration > hidingTime)
            hidingTime = RotationHideAfter + rotationDuration;

        float scaleDuration = (ScaleDuration > 0 ? ScaleDuration : Duration);
        if (ScaleHideAfter + scaleDuration > hidingTime)
            hidingTime = ScaleHideAfter + scaleDuration;

        float opacityDuration = (OpacityDuration > 0 ? OpacityDuration : Duration);
        if (OpacityHideAfter + opacityDuration > hidingTime)
            hidingTime = OpacityHideAfter + opacityDuration;
        return hidingTime;
    }


    /// <summary>
    /// Change the visibility of this by playing the desired animation.
    /// </summary>
    /// <param name="visible">Should this element be visible or not?</param>
    public override void ChangeVisibility(bool visible)
    {
        forceVisibilityCall = true;

        if (!Initialized)
            Initialize();

        if (!gameObject.activeSelf)
        {
            ChangeVisibilityImmediate(Visible);
            gameObject.SetActive(true);
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

        if (UseSimpleActivation)
        {
            ControlActivation(visible);
            if (visible && OnShow != null)
                OnShow.Invoke();
            else if (!visible && OnHide != null)
                OnHide.Invoke();
            return;
        }

        ControlMovement(visible, MovementType, HidingPosition, MovementDuration < 0 ? Duration : MovementDuration, EdgeGap, MovementBounces, MovementBouncePower, CustomHidingPosition);
        ControlRotation(visible, RotationType, HidingRotation, RotationDuration < 0 ? Duration : RotationDuration, RotationBounces, RotationBouncePower);
        ControlScale(visible, ScaleType, HidingScale, ScaleDuration < 0 ? Duration : ScaleDuration, ScaleBounces, ScaleBouncePower);
        ControlOpacity(visible, OpacityType, HidingOpacity, OpacityDuration < 0 ? Duration : OpacityDuration, OpacityBounces, OpacityBouncePower);

        if (DeactivateWhileInvisible)
        {
            if (!visible)
                Invoke("DeactivateMe", hidingTime);
            else
                CancelInvoke("DeactivateMe");
        }
    }
    /// <summary>
    /// Change the visibility of this element instantly without playing animation.
    /// </summary>
    /// <param name="visible">Should this element be visible or not?</param>
    public override void ChangeVisibilityImmediate(bool visible)
    {
        forceVisibilityCall = true;

        if (!Initialized)
            Initialize();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);


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


        if (UseSimpleActivation)
        {
            gameObject.SetActive(visible);
            return;
        }
        if (MovementType != MotionType.None)
        {
            Vector3 ePos = visible ? startPosition : outOfScreenPos;
            myRT.position= ePos;
        }
        if (RotationType != MotionType .None)
        {
            Vector3 eEuler = visible ? startEuler : HidingRotation;
            myRT.eulerAngles = eEuler;
        }
        if (ScaleType != MotionType.None)
        {
            Vector3 eScale = visible ? startScale : HidingScale;
            myRT.localScale = eScale;
        }
        if (OpacityType != MotionType.None)
        {
            float eOpacity = visible ? startOpacity : HidingOpacity;
            if (myImage)
            {
                Color col = myImage.color;
                col.a = eOpacity;
                myImage.color = col;
            }
            else if (myText)
            {
                Color col = myText.color;
                col.a = eOpacity;
                myText.color = col;
            }
            else if (myCanvasGroup)
                myCanvasGroup.alpha = eOpacity;
        }
        if (DeactivateWhileInvisible && !visible)
            DeactivateMe();
    }

    #region Movement Control
    /// <summary>
    /// Play movement animation.
    /// </summary>
    /// <param name="visible"></param>
    /// <param name="motionType"></param>
    /// <param name="side"></param>
    /// <param name="duration"></param>
    /// <param name="bouncesCount"></param>
    /// <param name="bouncePower"></param>
    public void ControlMovement(bool visible, MotionType motionType, ScreenSides side, float duration, float edgeGap = 0.25f, int bouncesCount = 0, int bouncePower = 0, Vector3 customPosition = new Vector3())
    {
        if (motionType == MotionType.None)
            return;

        Vector3 outPos = outOfScreenPos;

        if (side != HidingPosition || edgeGap != EdgeGap)
        {
            outPos = GetHidingPosition(side, edgeGap, customPosition);
        }

        Vector3 ePos = visible? startPosition : outPos;
        Vector3 sPos = myRT.position;

        //If the GameObject isn't active then we can't play co-routines so change it instantly.
        if (!gameObject.activeInHierarchy)
        {
            myRT.position = ePos;
            return;
        }

        if (movementEnum != null)
            StopCoroutine(movementEnum);

        switch (motionType)
        {
            case MotionType.Linear:
                movementEnum = LinearMovement(sPos, ePos, duration);
                break;
            case MotionType.EaseIn:
                movementEnum = EaseInMovement(sPos, ePos, duration);
                break;
            case MotionType.Bounce:
                movementEnum = BounceMovement(sPos, ePos, duration, bouncesCount, bouncePower);
                break;
        }
        StartCoroutine(movementEnum);
    }

    Vector3 GetHidingPosition(ScreenSides hidingPos, float edgeGap, Vector2 customPosition)
    {
        Vector3 pos = new Vector3();
        float y = 0;
        float x = 0;

        Vector2 distanceToEdge = new Vector2(myRT.pivot.x, myRT.pivot.y);

        Vector3 originalPosition = startPosition;

        switch (hidingPos)
        {
            case ScreenSides.Top:
                y = parentCanvasRT.position.y + canvasHalfHeight + myRTHeight * (distanceToEdge.y + edgeGap);
                pos = new Vector3(originalPosition.x, y, originalPosition.z);
                break;
            case ScreenSides.Bottom:
                y = parentCanvasRT.position.y - canvasHalfHeight - myRTHeight * (1 - distanceToEdge.y + edgeGap);
                pos = new Vector3(originalPosition.x, y, originalPosition.z);
                break;
            case ScreenSides.Left:
                x = parentCanvasRT.position.x - canvasHalfWidth - myRTWidth * (1 - distanceToEdge.x + edgeGap);
                pos = new Vector3(x, originalPosition.y, originalPosition.z);
                break;
            case ScreenSides.Right:
                x = parentCanvasRT.position.x + canvasHalfWidth + myRTWidth * (distanceToEdge.x + edgeGap);
                pos = new Vector3(x, originalPosition.y, originalPosition.z);
                break;
            case ScreenSides.TopLeftCorner:
                y = parentCanvasRT.position.y + canvasHalfHeight + myRTHeight * (distanceToEdge.y + edgeGap);
                x = parentCanvasRT.position.x - canvasHalfWidth - myRTWidth * (1 - distanceToEdge.x + edgeGap);
                pos = new Vector3(x, y, originalPosition.z);
                break;
            case ScreenSides.TopRightCorner:
                y = parentCanvasRT.position.y + canvasHalfHeight + myRTHeight * (distanceToEdge.y + edgeGap);
                x = parentCanvasRT.position.x + canvasHalfWidth + myRTWidth * (distanceToEdge.x + edgeGap);
                pos = new Vector3(x, y, originalPosition.z);
                break;
            case ScreenSides.BotLeftCorner:
                y = parentCanvasRT.position.y - canvasHalfHeight - myRTHeight * (1 - distanceToEdge.y + edgeGap);
                x = parentCanvasRT.position.x - canvasHalfWidth - myRTWidth * (1 - distanceToEdge.x + edgeGap);
                pos = new Vector3(x, y, originalPosition.z);
                break;
            case ScreenSides.BotRightCorner:
                y = parentCanvasRT.position.y - canvasHalfHeight - myRTHeight * (1 - distanceToEdge.y + edgeGap);
                x = parentCanvasRT.position.x + canvasHalfWidth + myRTWidth * (distanceToEdge.x + edgeGap);
                pos = new Vector3(x, y, originalPosition.z);
                break;
            case ScreenSides.Custom:
                pos = new Vector3(
                    parentCanvasRT.position.x + (customPosition.x - 0.5f) * canvasHalfWidth * 2,
                    parentCanvasRT.position.y + (customPosition.y - 0.5f) * canvasHalfHeight * 2, originalPosition.z);
                break;
        }
        return pos;
    }

    IEnumerator LinearMovement(Vector3 start, Vector3 end, float duration)
    {
        float startAfter = Visible? (MovementStartAfter < 0 ? StartAfter : MovementStartAfter) :
            (MovementHideAfter < 0? HideAfter : MovementHideAfter);
        yield return new WaitForSeconds(startAfter);

        float startTime = Time.time;


        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            myRT.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        myRT.position = end;
    }
    IEnumerator EaseInMovement(Vector3 start, Vector3 end, float duration)
    {
        float startAfter = Visible ? (MovementStartAfter < 0 ? StartAfter : MovementStartAfter) :
            (MovementHideAfter < 0 ? HideAfter : MovementHideAfter);
        yield return new WaitForSeconds(startAfter);

        float t = 0;
        while (t <= 1.0)
        {
            t += Time.deltaTime / duration;
            float ease = t;
            for (int i = 0; i < MovementEaseIntensity; i++)
                ease = Mathf.SmoothStep(0, 1, ease);

            myRT.position = Vector3.Lerp(start, end, ease);
            yield return null;
        }
        myRT.position = end;
    }
    IEnumerator BounceMovement(Vector3 start, Vector3 end, float duration, int bouncesCount, int bouncePower)
    {
        float startAfter = Visible ? (MovementStartAfter < 0 ? StartAfter : MovementStartAfter) :
            (MovementHideAfter < 0 ? HideAfter : MovementHideAfter);
        yield return new WaitForSeconds(startAfter);

        List<Vector3> positions = new List<Vector3>();
        List<float> durations = new List<float>();

        int bounceIterations = bouncesCount * 2 + 1;
        Vector3 posDiff = end - start;
        float upInt = 1;
        float lastDuration = duration * (1 / (float)bounceIterations);
        for (int i = 0; i < bounceIterations; i++)
        {
            if (i != bounceIterations - 1)
            {
                durations.Add((duration - lastDuration) / (bounceIterations - 1));

                Vector3 bounceVec = (posDiff * (bouncePower / (i + 1))) / 10;
                Vector3 wantedPos = end + upInt * bounceVec;

                positions.Add(wantedPos);
            }
            else
            {
                durations.Add(lastDuration);
                positions.Add(end);
            }
            upInt *= -1;
        }
        if (Visible || !MovementDisableHidingBounce)
        {
            for (int i = 0; i < bounceIterations; i++)
            {
                float startTime = Time.time;

                Vector3 sPos = myRT.position;

                float t = 0;
                while (t <= 1.0)
                {
                    t += Time.deltaTime / durations[i];
                    float ease = t;
                    for (int j = 0; j < MovementEaseIntensity; j++)
                        ease = Mathf.SmoothStep(0, 1, ease);

                    myRT.position = Vector3.Lerp(sPos, positions[i], ease);
                    yield return null;
                }
            }
        }
        else
        {
            float startTime = Time.time;

            Vector3 sPos = myRT.position;

            float t = 0;
            while (t <= 1.0)
            {
                t += Time.deltaTime / durations[0];
                float ease = t;
                for (int j = 0; j < MovementEaseIntensity; j++)
                    ease = Mathf.SmoothStep(0, 1, ease);

                myRT.position = Vector3.Lerp(sPos, positions[positions.Count -1], ease);
                yield return null;
            }
        }
        myRT.position = end;
    }
    #endregion

    #region Rotation Control
    /// <summary>
    /// Play rotation animation.
    /// </summary>
    /// <param name="visible"></param>
    /// <param name="motionType"></param>
    /// <param name="euler"></param>
    /// <param name="duration"></param>
    public void ControlRotation(bool visible, MotionType motionType, Vector3 euler, float duration, int bouncesCount, int bouncePower)
    {
        if (motionType == MotionType.None)
            return;

        Vector3 eEuler = visible ? startEuler : euler;
        Vector3 sEuler = myRT.eulerAngles;
      
        //If the GameObject isn't active then we can't play co-routines so change it instantly.
        if (!gameObject.activeInHierarchy)
        {
            myRT.eulerAngles = eEuler;
            return;
        }

        if (rotationEnum != null)
            StopCoroutine(rotationEnum);

        switch (motionType)
        {
            case MotionType.Linear:
                rotationEnum = LinearRotation(sEuler, eEuler, duration);
                break;
            case MotionType.EaseIn:
                rotationEnum = EaseInRotation(sEuler, eEuler, duration);
                break;
            case MotionType.Bounce:
                rotationEnum = BounceRotation(sEuler, eEuler, duration, bouncesCount, bouncePower);
                break;
        }
        StartCoroutine(rotationEnum);

    }

    IEnumerator LinearRotation(Vector3 start, Vector3 end, float duration)
    {
        float startAfter = Visible ? (RotationStartAfter < 0 ? StartAfter : RotationStartAfter) :
            (RotationHideAfter < 0 ? HideAfter : RotationHideAfter);
        yield return new WaitForSeconds(startAfter);

        float startTime = Time.time;


        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            myRT.eulerAngles = Vector3.Lerp(start, end, t);
            yield return null;
        }
        myRT.eulerAngles = end;
    }
    IEnumerator EaseInRotation(Vector3 start, Vector3 end, float duration)
    {
        float startAfter = Visible ? (RotationStartAfter < 0 ? StartAfter : RotationStartAfter) :
            (RotationHideAfter < 0 ? HideAfter : RotationHideAfter);
        yield return new WaitForSeconds(startAfter);

        float t = 0;
        while (t <= 1.0)
        {
            t += Time.deltaTime / duration;
            float ease = t;
            for (int i = 0; i < RotationEaseIntensity; i++)
                ease = Mathf.SmoothStep(0, 1, ease);

            myRT.eulerAngles = Vector3.Lerp(start, end, ease);
            yield return null;
        }
        myRT.eulerAngles = end;
    }
    IEnumerator BounceRotation(Vector3 start, Vector3 end, float duration, int bouncesCount, int bouncePower)
    {
        float startAfter = Visible ? (RotationStartAfter < 0 ? StartAfter : RotationStartAfter) :
            (RotationHideAfter < 0 ? HideAfter : RotationHideAfter);
        yield return new WaitForSeconds(startAfter);

        List<Vector3> eulers = new List<Vector3>();
        List<float> durations = new List<float>();

        int bounceIterations = bouncesCount * 2 + 1;
        Vector3 eulerDiff = end - start;
        int upInt = 1;
        float lastDuration = duration * (1 / (float)bounceIterations);
        for (int i = 0; i < bounceIterations; i++)
        {
            if (i != bounceIterations - 1)
            {
                durations.Add((duration - lastDuration) / (bounceIterations - 1));

                Vector3 bounceVec = (eulerDiff * (bouncePower / (i + 1))) / 7;
                Vector3 wantedEuler = end + upInt * bounceVec;
                wantedEuler = new Vector3(wantedEuler.x < 0 ? 0 : wantedEuler.x, wantedEuler.y < 0 ? 0 : wantedEuler.y, wantedEuler.z < 0 ? 0 : wantedEuler.z);

                eulers.Add(wantedEuler);
            }
            else
            {
                durations.Add(lastDuration);
                eulers.Add(end);
            }
            upInt *= -1;
        }
        if (Visible || !RotationDisableHidingBounce)
        {
            for (int i = 0; i < bounceIterations; i++)
            {
                float startTime = Time.time;
                Vector3 sEuler = myRT.eulerAngles;

                float t = 0;
                while (t <= 1.0)
                {
                    t += Time.deltaTime / durations[i];
                    float ease = t;
                    for (int j = 0; j < RotationEaseIntensity; j++)
                        ease = Mathf.SmoothStep(0, 1, ease);

                    myRT.eulerAngles = Vector3.Lerp(sEuler, eulers[i], ease);
                    yield return null;
                }
            }
        }
        else
        {
            float startTime = Time.time;

            Vector3 sEuler = myRT.eulerAngles;

            float t = 0;
            while (t <= 1.0)
            {
                t += Time.deltaTime / durations[0];
                float ease = t;
                for (int j = 0; j < RotationEaseIntensity; j++)
                    ease = Mathf.SmoothStep(0, 1, ease);

                myRT.eulerAngles = Vector3.Lerp(sEuler, eulers[eulers.Count - 1], ease);
                yield return null;
            }
        }
        myRT.eulerAngles = end;
    }
    #endregion

    #region Scale Control
    /// <summary>
    /// Play scale animation
    /// </summary>
    /// <param name="visible"></param>
    /// <param name="motionType"></param>
    /// <param name="scale"></param>
    /// <param name="duration"></param>
    public void ControlScale(bool visible, MotionType motionType, Vector3 scale, float duration, int bouncesCount, int bouncePower)
    {
        if (motionType == MotionType.None)
            return;

        Vector3 eScale = visible ? startScale : scale;
        Vector3 sScale = myRT.localScale;

        //If the GameObject isn't active then we can't play co-routines so change it instantly.
        if (!gameObject.activeInHierarchy)
        {
            myRT.localScale = eScale;
            return;
        }

        if (scaleEnum != null)
            StopCoroutine(scaleEnum);

        switch (motionType)
        {
            case MotionType.Linear:
                scaleEnum = LinearScale(sScale, eScale, duration);
                break;
            case MotionType.EaseIn:
                scaleEnum = EaseInScale(sScale, eScale, duration);
                break;
            case MotionType.Bounce:
                scaleEnum = BounceScale(sScale, eScale, duration, bouncesCount, bouncePower);
                break;
        }
        StartCoroutine(scaleEnum);

    }

    IEnumerator LinearScale(Vector3 start, Vector3 end, float duration)
    {
        float startAfter = Visible ? (ScaleStartAfter < 0 ? StartAfter : ScaleStartAfter) :
            (ScaleHideAfter < 0 ? HideAfter : ScaleHideAfter);
        yield return new WaitForSeconds(startAfter);

        float startTime = Time.time;


        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            myRT.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }
        myRT.localScale = end;
    }
    IEnumerator EaseInScale(Vector3 start, Vector3 end, float duration)
    {        
        float startAfter = Visible ? (ScaleStartAfter < 0 ? StartAfter : ScaleStartAfter) :
            (ScaleHideAfter < 0 ? HideAfter : ScaleHideAfter);

        yield return new WaitForSeconds(startAfter);

        float t = 0;
        while (t <= 1.0)
        {
            t += Time.deltaTime / duration;
            float ease = t;
            for (int i = 0; i < ScaleEaseIntensity; i++)
                ease = Mathf.SmoothStep(0, 1, ease);

            myRT.localScale = Vector3.Lerp(start, end, ease);
            yield return null;
        }

        myRT.localScale = end;
    }
    IEnumerator BounceScale(Vector3 start, Vector3 end, float duration, int bouncesCount, int bouncePower)
    {
        float startAfter = Visible ? (ScaleStartAfter < 0 ? StartAfter : ScaleStartAfter) :
            (ScaleHideAfter < 0 ? HideAfter : ScaleHideAfter);
        yield return new WaitForSeconds(startAfter);

        List<Vector3> scales = new List<Vector3>();
        List<float> durations = new List<float>();

        int bounceIterations = bouncesCount * 2 + 1;
        Vector3 scaleDiff = end - start;
        int upInt = 1;
        float lastDuration = duration * (1 / (float)bounceIterations);
        for (int i = 0; i < bounceIterations; i++)
        {
            if (i != bounceIterations - 1)
            {
                durations.Add((duration - lastDuration) / (bounceIterations - 1));

                Vector3 bounceVec = (scaleDiff * (bouncePower / (i + 1))) / 10;
                Vector3 wantedScale = end + upInt * bounceVec;
                wantedScale = new Vector3(wantedScale.x < 0 ? 0 : wantedScale.x, wantedScale.y < 0 ? 0 : wantedScale.y, wantedScale.z < 0 ? 0 : wantedScale.z);

                scales.Add(wantedScale);
            }
            else
            {
                durations.Add(lastDuration);
                scales.Add(end);
            }
            upInt *= -1;
        }
        if (Visible || !ScaleDisableHidingBounce)
        {
            for (int i = 0; i < bounceIterations; i++)
            {
                float startTime = Time.time;

                Vector3 sScale = myRT.localScale;

                float t = 0;
                while (t <= 1.0)
                {
                    t += Time.deltaTime / durations[i];
                    float ease = t;
                    for (int j = 0; j < ScaleEaseIntensity; j++)
                        ease = Mathf.SmoothStep(0, 1, ease);

                    myRT.localScale = Vector3.Lerp(sScale, scales[i], ease);
                    yield return null;
                }
            }
        }
        else
        {
            float startTime = Time.time;

            Vector3 sScale = myRT.localScale;

            float t = 0;
            while (t <= 1.0)
            {
                t += Time.deltaTime / durations[0];
                float ease = t;
                for (int j = 0; j < ScaleEaseIntensity; j++)
                    ease = Mathf.SmoothStep(0, 1, ease);

                myRT.localScale = Vector3.Lerp(sScale, scales[scales.Count - 1], ease);
                yield return null;
            }
        }
        myRT.localScale = end;
    }
    #endregion

    #region Opacity Control
    /// <summary>
    /// Play opacity animation
    /// </summary>
    /// <param name="visible"></param>
    /// <param name="motionType"></param>
    /// <param name="opac"></param>
    /// <param name="duration"></param>
    public void ControlOpacity(bool visible, MotionType motionType, float opac, float duration, int bouncesCount, int bouncePower)
    {
        if (motionType == MotionType.None || (!myImage && !myText && !myCanvasGroup))
            return;

        float eOpacity = visible ? startOpacity : opac;
        float sOpacity = 0;

        //If the GameObject isn't active then we can't play co-routines so change it instantly.
        if (!gameObject.activeInHierarchy)
        {
            if (myImage)
            {
                Color col = myImage.color;
                col.a = eOpacity;
                myImage.color = col;
            }
            else if (myText)
            {
                Color col = myText.color;
                col.a = eOpacity;
                myText.color = col;
            }
            else if (myCanvasGroup)
                myCanvasGroup.alpha = eOpacity;
            return;
        }

        if (myImage)
            sOpacity = myImage.color.a;
        else if (myText)
            sOpacity = myText.color.a;
        else if (myCanvasGroup)
            sOpacity = myCanvasGroup.alpha;

        if (opacityEnum != null)
            StopCoroutine(opacityEnum);
        
        switch (motionType)
        {
            case MotionType.Linear:
                opacityEnum = LinearOpacity(sOpacity, eOpacity, duration);
                break;
            case MotionType.EaseIn:
                opacityEnum = EaseInOpacity(sOpacity, eOpacity, duration);
                break;
            case MotionType.Bounce:
                opacityEnum = BounceOpacity(sOpacity, eOpacity, duration, bouncesCount, bouncePower);
                break;
        }
        StartCoroutine(opacityEnum);

    }

    IEnumerator LinearOpacity(float start, float end, float duration)
    {
        float startAfter = Visible ? (OpacityStartAfter < 0 ? StartAfter : OpacityStartAfter) :
            (OpacityHideAfter < 0 ? HideAfter : OpacityHideAfter);
        yield return new WaitForSeconds(startAfter);

        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            float op = Mathf.Lerp(start, end, t);
            if (myImage)
            {
                Color col = myImage.color;
                col.a = op;
                myImage.color = col;
            }
            else if (myText)
            {
                Color col = myText.color;
                col.a = op;
                myText.color = col;
            }
            else if (myCanvasGroup)
                myCanvasGroup.alpha = op;
            yield return null;
        }
        if (myImage)
        {
            Color col = myImage.color;
            col.a = end;
            myImage.color = col;
        }
        else if (myText)
        {
            Color col = myText.color;
            col.a = end;
            myText.color = col;
        }
        else if (myCanvasGroup)
            myCanvasGroup.alpha = end;
    }
    IEnumerator EaseInOpacity(float start, float end, float duration)
    {
        float startAfter = Visible ? (OpacityStartAfter < 0 ? StartAfter : OpacityStartAfter) :
            (OpacityHideAfter < 0 ? HideAfter : OpacityHideAfter);
        yield return new WaitForSeconds(startAfter);

        float t = 0;
        while (t <= 1.0)
        {
            t += Time.deltaTime / duration;
            float ease = t;
            for (int i = 0; i < OpacityEaseIntensity; i++)
                ease = Mathf.SmoothStep(0, 1, ease);

            float op = Mathf.Lerp(start, end, ease);

            if (myImage)
            {
                Color col = myImage.color;
                col.a = op;
                myImage.color = col;
            }
            else if (myText)
            {
                Color col = myText.color;
                col.a = op;
                myText.color = col;
            }
            else if (myCanvasGroup)
                myCanvasGroup.alpha = op;
            yield return null;
        }
        if (myImage)
        {
            Color col = myImage.color;
            col.a = end;
            myImage.color = col;
        }
        else if (myText)
        {
            Color col = myText.color;
            col.a = end;
            myText.color = col;
        }
        else if (myCanvasGroup)
            myCanvasGroup.alpha = end;
    }
    IEnumerator BounceOpacity(float start, float end, float duration, int bouncesCount, int bouncePower)
    {
        float startAfter = Visible ? (OpacityStartAfter < 0 ? StartAfter : OpacityStartAfter) :
            (OpacityHideAfter < 0 ? HideAfter : OpacityHideAfter);
        yield return new WaitForSeconds(startAfter);

        List<float> opacities = new List<float>();
        List<float> durations = new List<float>();

        int bounceIterations = bouncesCount * 2 + 1;
        float opacityDiff = end - start;
        int upInt = 1;
        float lastDuration = duration * (1 / (float)bounceIterations);
        for (int i = 0; i < bounceIterations; i++)
        {
            if (i != bounceIterations - 1)
            {
                durations.Add((duration - lastDuration) / (bounceIterations - 1));

                float bounceFloat = (opacityDiff * (bouncePower / (i + 1))) / 5;
                float wantedOpacity = end + upInt * bounceFloat;
                wantedOpacity = Mathf.Clamp01(wantedOpacity);

                opacities.Add(wantedOpacity);
            }
            else
            {
                durations.Add(lastDuration);
                opacities.Add(end);
            }
            upInt *= -1;
        }

        if (Visible || !OpacityDisableHidingBounce)
        {
            for (int i = 0; i < bounceIterations; i++)
            {
                float startTime = Time.time;

                float sOpacity = 0;
                if (myImage)
                    sOpacity = myImage.color.a;
                else if (myText)
                    sOpacity = myText.color.a;
                else if (myCanvasGroup)
                    sOpacity = myCanvasGroup.alpha;
                float t = 0;
                while (t <= 1.0)
                {
                    t += Time.deltaTime / durations[i];
                    float ease = t;
                    for (int j = 0; j < OpacityEaseIntensity; j++)
                        ease = Mathf.SmoothStep(0, 1, ease);

                    float op = Mathf.Lerp(sOpacity, opacities[i], ease);

                    if (myImage)
                    {
                        Color col = myImage.color;
                        col.a = op;
                        myImage.color = col;
                    }
                    else if (myText)
                    {
                        Color col = myText.color;
                        col.a = op;
                        myText.color = col;
                    }
                    else if (myCanvasGroup)
                        myCanvasGroup.alpha = op;
                    yield return null;
                }
            }
        }
        else
        {
            float startTime = Time.time;

            float sOpacity = 0;
            if (myImage)
                sOpacity = myImage.color.a;
            else if (myText)
                sOpacity = myText.color.a;
            else if (myCanvasGroup)
                sOpacity = myCanvasGroup.alpha;
            float t = 0;
            while (t <= 1.0)
            {
                t += Time.deltaTime / durations[0];
                float ease = t;
                for (int j = 0; j < OpacityEaseIntensity; j++)
                    ease = Mathf.SmoothStep(0, 1, ease);

                float op = Mathf.Lerp(sOpacity, opacities[opacities.Count - 1], ease);

                if (myImage)
                {
                    Color col = myImage.color;
                    col.a = op;
                    myImage.color = col;
                }
                else if (myText)
                {
                    Color col = myText.color;
                    col.a = op;
                    myText.color = col;
                }
                else if (myCanvasGroup)
                    myCanvasGroup.alpha = op;
                yield return null;
            }
        }
        if (myImage)
        {
            Color col = myImage.color;
            col.a = end;
            myImage.color = col;
        }
        else if (myText)
        {
            Color col = myText.color;
            col.a = end;
            myText.color = col;
        }
        else if (myCanvasGroup)
            myCanvasGroup.alpha = end;
    }
    #endregion

    #region Activation Control
    /// <summary>
    /// Activate/deactivate element.
    /// </summary>
    /// <param name="visible"></param>
    public void ControlActivation(bool visible)
    {
        gameObject.SetActive(visible);
    }
    #endregion

    void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}
