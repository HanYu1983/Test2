using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIElement))]
[AddComponentMenu("UI/ZUI/UI Animation", 5)]
public class UIAnimation : MonoBehaviour {

    //The UIElement that will be animated.
    public UIElement myUIElement;

    
    [System.Serializable]
    public class AnimationFrame
    {
        [Tooltip("The duration in seconds it would take to start this animation after the last one has finished.")]
        public float StartAfter = 0.0f;
        [Tooltip("The duration of the animation.")]
        public float Duration = 0.3f;


        [Tooltip("The type of the movement animation.")]
        public UIElement.MotionType MovementType;
        [Tooltip("Should the element move to its start position?")]
        public bool StartPosition;
        [Tooltip("The position the object will move to.")]
        public UIElement.ScreenSides MovementHidingPosition;
        [Tooltip("Custom hiding position as percentage of the screen.")]
        public Vector2 CustomHidingPosition;
        [Tooltip("The gap between the element and the edge of the screen while it's hiding (in percentage of element's width or height).")]
        public float EdgeGap = 0.25f;
        [Tooltip("Custom duration for movement, negative values means no custom duration.")]
        public float MovementDuration = -1;
        [Tooltip("Number of bounces the element should make.")]
        public int MovementBounces = 1;
        [Tooltip("Bouncing power.")]
        [Range(1, 10)]
        public int MovementBouncePower = 3;


        [Tooltip("The type of the rotation animation.")]
        public UIElement.MotionType RotationType;
        [Tooltip("Should the element move to its start rotation?")]
        public bool StartRotation;
        [Tooltip("The euler the object will rotate to.")]
        public Vector3 Euler;
        [Tooltip("Custom duration for rotation, negative values means no custom duration.")]
        public float RotationDuration = -1;
        [Tooltip("Number of bounces the element should make.")]
        public int RotationBounces = 1;
        [Tooltip("Bouncing power.")]
        [Range(1, 10)]
        public int RotationBouncePower = 3;


        [Tooltip("The type of the scale animation.")]
        public UIElement.MotionType ScaleType;
        [Tooltip("Should the element change to its start scale?")]
        public bool StartScale;
        [Tooltip("The scale the object will change to.")]
        public Vector3 ScaleVector;
        [Tooltip("Custom duration for scale, negative values means no custom duration.")]
        public float ScaleDuration = -1;
        [Tooltip("Number of bounces the element should make.")]
        public int ScaleBounces = 1;
        [Tooltip("Bouncing power.")]
        [Range(1, 10)]
        public int ScaleBouncePower = 9;

        [Tooltip("The type of the opacity animation.")]
        public UIElement.MotionType OpacityType;
        [Tooltip("Should the element change to its start opacity?")]
        public bool StartOpacity;
        [Tooltip("The opacity the object will change to.")]
        public float OpacityWanted;
        [Tooltip("Custom duration for opacity, negative values means no custom duration.")]
        public float OpacityDuration = -1;
        [Tooltip("Number of bounces the element should make.")]
        public int OpacityBounces = 1;
        [Tooltip("Bouncing power.")]
        [Range(1, 10)]
        public int OpacityBouncePower = 3;

        public AnimationFrame(float sAfter, float duration, bool visible, float movementDuration, float rotDuration, float scalDuration, float opacDuration)
        {
            StartAfter = sAfter;
            Duration = duration;
            
            MovementDuration = movementDuration;

            RotationDuration = rotDuration;

            ScaleDuration = scalDuration;

            OpacityDuration = opacDuration;
        }
    }
    public List<AnimationFrame> AnimationFrames = new List<AnimationFrame>(1) { new AnimationFrame(0.3f, 0.3f, false, -1, -1, -1, -1) };

    [Tooltip("Should the animation loop?")]
    public bool Loop = false;
    [Tooltip("Should the animation restart every time the element is visible?")]
    public bool RestartOnVisible = true;

    private RectTransform elementRT;
    private List<float> startingTimes = new List<float>();
    private IEnumerator cycleEnum;
    private bool positionControlled;
    private Vector3 startPosition;
    private bool eulerControlled;
    private Vector3 startEuler;
    private bool scaleControlled;
    private Vector3 startScale;
    private bool opacityControlled;
    private float startOpacity;

    void Awake()
    {
        myUIElement = GetComponent<UIElement>();

        myUIElement.OnShow.AddListener(() => { Play(); });
        myUIElement.OnHide.AddListener(() => { Stop(); });

        elementRT = myUIElement.GetComponent<RectTransform>();

        for (int i = 0; i < AnimationFrames.Count; i++)
        {
            AnimationFrame frame = AnimationFrames[i];

            if (frame.MovementType != UIElement.MotionType.None)
                positionControlled = true;
            if (frame.RotationType != UIElement.MotionType.None)
                eulerControlled = true;
            if (frame.ScaleType != UIElement.MotionType.None)
                scaleControlled = true;
            if (frame.OpacityType != UIElement.MotionType.None)
                opacityControlled = true;
        }

        if (positionControlled)
            startPosition = elementRT.position;
        if (eulerControlled)
            startEuler = elementRT.eulerAngles;
        if (scaleControlled)
            startScale = elementRT.localScale;
        if (opacityControlled)
        {
            if (myUIElement.myImage)
                startOpacity = myUIElement.myImage.color.a;
            else if (myUIElement.myText)
                startOpacity = myUIElement.myText.color.a;
            else if (myUIElement.myCanvasGroup)
                startOpacity = myUIElement.myCanvasGroup.alpha;
        }
    }

    /// <summary>
    /// Start playing the animation.
    /// </summary>
    public void Play()
    {
        if (RestartOnVisible)
            ResetElement();

        cycleEnum = AnimationCycle();
        StartCoroutine(cycleEnum);

        CalculateTimeLine();
    }
    /// <summary>
    /// Stop playing the animation.
    /// </summary>
    public void Stop()
    {
        if (cycleEnum != null)
            StopCoroutine(cycleEnum);
    }

    void CalculateTimeLine()
    {
        float t = 0;
        for (int i = 0; i < AnimationFrames.Count; i++)
        {
            t += AnimationFrames[i].StartAfter;
            startingTimes.Add(t);
            t += AnimationFrames[i].Duration;
        }
    }

    void ResetElement()
    {
        if (positionControlled)
            elementRT.position = startPosition;
        if (eulerControlled)
            elementRT.eulerAngles = startEuler;
        if (scaleControlled)
            elementRT.localScale = startScale;
        if (opacityControlled)
        {
            if (myUIElement.myImage)
                myUIElement.myImage.color = new Color(myUIElement.myImage.color.r, myUIElement.myImage.color.g, myUIElement.myImage.color.b, startOpacity);
            else if (myUIElement.myText)
                myUIElement.myText.color = new Color(myUIElement.myText.color.r, myUIElement.myText.color.g, myUIElement.myText.color.b, startOpacity);
            else if (myUIElement.myCanvasGroup)
                myUIElement.myCanvasGroup.alpha = startOpacity;
        }
    }

    IEnumerator AnimationCycle()
    {
        for (int i = 0; i < AnimationFrames.Count; i++)
        {
            AnimationFrame frame = AnimationFrames[i];
            yield return new WaitForSeconds(frame.StartAfter);

            if (frame.MovementType != UIElement.MotionType.None)
                myUIElement.ControlMovement(frame.StartPosition, frame.MovementType, frame.MovementHidingPosition, frame.MovementDuration > 0 ? frame.MovementDuration : frame.Duration, frame.EdgeGap, frame.MovementBounces, frame.MovementBouncePower, frame.CustomHidingPosition);
            if (frame.RotationType != UIElement.MotionType.None)
                myUIElement.ControlRotation(frame.StartRotation, frame.RotationType, frame.Euler, frame.RotationDuration > 0 ? frame.RotationDuration : frame.Duration, frame.RotationBounces, frame.RotationBouncePower);
            if (frame.ScaleType != UIElement.MotionType.None)
                myUIElement.ControlScale(frame.StartScale, frame.ScaleType, frame.ScaleVector, frame.ScaleDuration > 0 ? frame.ScaleDuration : frame.Duration, frame.ScaleBounces, frame.ScaleBouncePower);
            if (frame.OpacityType != UIElement.MotionType.None)
                myUIElement.ControlOpacity(frame.StartOpacity, frame.OpacityType, frame.OpacityWanted, frame.OpacityDuration > 0 ? frame.OpacityDuration : frame.Duration, frame.OpacityBounces, frame.OpacityBouncePower);

            if (i == AnimationFrames.Count - 1 && Loop)
                i = -1;

            yield return new WaitForSeconds(frame.Duration);
        }

        yield break;
    }

}
