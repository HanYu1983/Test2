using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;

[CanEditMultipleObjects()]
[CustomEditor(typeof(UIElement))]
public class UIElementEditor : Editor
{
    #region Settings Properties
    private SerializedProperty menuDependant;
    private SerializedProperty visible;
    private SerializedProperty controlledBy;
    private SerializedProperty prewarm;
    private SerializedProperty deactivateWhileInvisible;
    private SerializedProperty startAfter;
    private SerializedProperty hideAfter;
    private SerializedProperty duration;
    #endregion

    #region Movement Properties
    private SerializedProperty movementType;
    private SerializedProperty hidingPosition;
    private SerializedProperty customHidingPosition;
    private SerializedProperty edgeGap;
    private SerializedProperty movementEaseIntensity;
    private SerializedProperty movementBounces;
    private SerializedProperty movementBouncePower;
    private SerializedProperty movementDisableHidingBounce;
    private SerializedProperty movementStartAfter;
    private SerializedProperty movementHideAfter;
    private SerializedProperty movementDuration;
    private bool recordingPosition;
    private bool lastRecordingState;
    public Vector2 originalPosition;
    AnimBool showMovementProps;
    #endregion

    #region Rotation Properties
    private SerializedProperty rotationType;
    private SerializedProperty hidingRotation;
    private SerializedProperty rotationEaseIntensity;
    private SerializedProperty rotationBounces;
    private SerializedProperty rotationBouncePower;
    private SerializedProperty rotationDisableHidingBounce;
    private SerializedProperty rotationStartAfter;
    private SerializedProperty rotationHideAfter;
    private SerializedProperty rotationDuration;
    AnimBool showRotationProps;
    #endregion

    #region Scale Properties
    private SerializedProperty scaleType;
    private SerializedProperty hidingScale;
    private SerializedProperty scaleEaseIntensity;
    private SerializedProperty scaleBounces;
    private SerializedProperty scaleBouncePower;
    private SerializedProperty scaleDisableHidingBounce;
    private SerializedProperty scaleStartAfter;
    private SerializedProperty scaleHideAfter;
    private SerializedProperty scaleDuration;
    AnimBool showScaleProps;
    #endregion

    #region Opacity Properties
    private SerializedProperty opacityType;
    private SerializedProperty hidingOpacity;
    private SerializedProperty opacityEaseIntensity;
    private SerializedProperty opacityBounces;
    private SerializedProperty opacityBouncePower;
    private SerializedProperty opacityDisableHidingBounce;
    private SerializedProperty opacityStartAfter;
    private SerializedProperty opacityHideAfter;
    private SerializedProperty opacityDuration;
    AnimBool showOpacityProps;
    #endregion

    #region Activation Properties
    private SerializedProperty useSimpleActivation;
    #endregion

    #region Sounds Properties
    private SerializedProperty showingClip;
    private SerializedProperty hidingClip;
    #endregion

    #region Events Properties
    private SerializedProperty onShow;
    private SerializedProperty onHide;
    #endregion

    void OnEnable()
    {
        #region Settings Properties
        menuDependant = serializedObject.FindProperty("MenuDependent");
        visible = serializedObject.FindProperty("Visible");
        controlledBy = serializedObject.FindProperty("ControlledBy");
        prewarm = serializedObject.FindProperty("Prewarm");
        deactivateWhileInvisible = serializedObject.FindProperty("DeactivateWhileInvisible");
        startAfter = serializedObject.FindProperty("StartAfter");
        hideAfter = serializedObject.FindProperty("HideAfter");
        duration = serializedObject.FindProperty("Duration");
        #endregion

        #region Movement Properties
        movementType = serializedObject.FindProperty("MovementType");
        hidingPosition = serializedObject.FindProperty("HidingPosition");
        customHidingPosition = serializedObject.FindProperty("CustomHidingPosition");
        edgeGap = serializedObject.FindProperty("EdgeGap");
        movementEaseIntensity = serializedObject.FindProperty("MovementEaseIntensity");
        movementBounces = serializedObject.FindProperty("MovementBounces");
        movementBouncePower = serializedObject.FindProperty("MovementBouncePower");
        movementDisableHidingBounce = serializedObject.FindProperty("MovementDisableHidingBounce");
        movementStartAfter = serializedObject.FindProperty("MovementStartAfter");
        movementHideAfter = serializedObject.FindProperty("MovementHideAfter");
        movementDuration = serializedObject.FindProperty("MovementDuration");

        bool showMovementPropsBool = true;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            if (element.MovementType == UIElement.MotionType.None)
                showMovementPropsBool = false;
        }
        showMovementProps = new AnimBool(showMovementPropsBool);
        showMovementProps.valueChanged.AddListener(Repaint);
        #endregion

        #region Rotation Properties
        rotationType= serializedObject.FindProperty("RotationType");
        hidingRotation = serializedObject.FindProperty("HidingRotation");
        rotationEaseIntensity = serializedObject.FindProperty("RotationEaseIntensity");
        rotationBounces = serializedObject.FindProperty("RotationBounces");
        rotationBouncePower = serializedObject.FindProperty("RotationBouncePower");
        rotationDisableHidingBounce = serializedObject.FindProperty("RotationDisableHidingBounce");
        rotationStartAfter = serializedObject.FindProperty("RotationStartAfter");
        rotationHideAfter = serializedObject.FindProperty("RotationHideAfter");
        rotationDuration = serializedObject.FindProperty("RotationDuration");

        bool showRotationPropsBool = true;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            if (element.RotationType == UIElement.MotionType.None)
                showRotationPropsBool = false;
        }
        showRotationProps = new AnimBool(showRotationPropsBool);
        showRotationProps.valueChanged.AddListener(Repaint);
        #endregion

        #region Scale Properties
        scaleType = serializedObject.FindProperty("ScaleType");
        hidingScale = serializedObject.FindProperty("HidingScale");
        scaleEaseIntensity = serializedObject.FindProperty("ScaleEaseIntensity");
        scaleBounces = serializedObject.FindProperty("ScaleBounces");
        scaleBouncePower = serializedObject.FindProperty("ScaleBouncePower");
        scaleDisableHidingBounce = serializedObject.FindProperty("ScaleDisableHidingBounce");
        scaleStartAfter = serializedObject.FindProperty("ScaleStartAfter");
        scaleHideAfter = serializedObject.FindProperty("ScaleHideAfter");
        scaleDuration = serializedObject.FindProperty("ScaleDuration");

        bool showScalePropsBool = true;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            if (element.ScaleType == UIElement.MotionType.None)
                showScalePropsBool = false;
        }
        showScaleProps = new AnimBool(showScalePropsBool);
        showScaleProps.valueChanged.AddListener(Repaint);
        #endregion

        #region Opacity Properties
        opacityType = serializedObject.FindProperty("OpacityType");
        hidingOpacity = serializedObject.FindProperty("HidingOpacity");
        opacityEaseIntensity = serializedObject.FindProperty("OpacityEaseIntensity");
        opacityBounces = serializedObject.FindProperty("OpacityBounces");
        opacityBouncePower = serializedObject.FindProperty("OpacityBouncePower");
        opacityDisableHidingBounce = serializedObject.FindProperty("OpacityDisableHidingBounce");
        opacityStartAfter = serializedObject.FindProperty("OpacityStartAfter");
        opacityHideAfter = serializedObject.FindProperty("OpacityHideAfter");
        opacityDuration = serializedObject.FindProperty("OpacityDuration");

        bool showOpacityPropsBool = true;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            if (element.OpacityType == UIElement.MotionType.None)
                showOpacityPropsBool = false;
        }
        showOpacityProps = new AnimBool(showOpacityPropsBool);
        showOpacityProps.valueChanged.AddListener(Repaint);
        #endregion

        #region Activation Properties
        useSimpleActivation = serializedObject.FindProperty("UseSimpleActivation");
        #endregion

        #region Souds
        showingClip = serializedObject.FindProperty("ShowingClip");
        hidingClip = serializedObject.FindProperty("HidingClip");
        #endregion

        #region Events
        onShow = serializedObject.FindProperty("OnShow");
        onHide = serializedObject.FindProperty("OnHide");
        #endregion
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        bool usedActivationControl = false;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            if (element.UseSimpleActivation)
                usedActivationControl = true;
        }

        EditorGUILayout.Space();


        #region Settings
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        bool someoneControlled = false;
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement e = (UIElement)targets[i];
            if (e.ControlledBy != null)
                someoneControlled = true;
        }
        string controllerName = "NONE";

        //if we are only selecting one object and there's someone controlled in the selection array, then it is this object.
        if (targets.Length == 1 && someoneControlled)
            controllerName = controlledBy.objectReferenceValue.name;
        else if (targets.Length > 1 && someoneControlled)
            controllerName = "-";

        EditorGUILayout.LabelField("Controlled By: ", controllerName);
        if (someoneControlled && GUILayout.Button("Remove Control"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                UIElement e = (UIElement)targets[i];

                Undo.RecordObject(e, "Remove Control");
                Menu m = e.ControlledBy as Menu;
                if (m != null)
                {
                    Undo.RecordObject(m, "Remove Control");
                    m.AnimatedElements.Remove((UIElement)targets[i]);
                }
                else
                {
                    UIElementsGroup eg = e.ControlledBy as UIElementsGroup;

                    if (eg != null)
                    {
                        Undo.RecordObject(eg, "Remove Control");
                        eg.AnimatedElements.Remove((UIElement)targets[i]);
                    }
                    else
                    {
                        SideMenu sm = e.ControlledBy as SideMenu;

                        if (sm != null)
                        {
                            Undo.RecordObject(sm, "Remove Control");
                            sm.AnimatedElements.Remove((UIElement)targets[i]);
                        }
                        else
                        {
                            Popup p = e.ControlledBy as Popup;

                            if (p != null)
                            {
                                Undo.RecordObject(p, "Remove Control");
                                p.AnimatedElements.Remove((UIElement)targets[i]);
                            }
                        }
                    }
                }
                e.ControlledBy = null;
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Is Visible?", visible.boolValue.ToString());
        EditorGUILayout.PropertyField(menuDependant);
        if (!menuDependant.boolValue)
        {
            EditorGUILayout.PropertyField(visible);
            EditorGUILayout.PropertyField(prewarm);
        }
        EditorGUILayout.PropertyField(deactivateWhileInvisible);
        EditorGUILayout.PropertyField(startAfter);
        EditorGUILayout.PropertyField(hideAfter);
        EditorGUILayout.PropertyField(duration);
        #endregion

        if (!usedActivationControl)
        {
            EditorGUILayout.Space();

            #region Movement

            #region Group Checks
            bool showMovementPropsBool = true;
            bool allMovementEaseIn = true;
            bool allMovementBounce = true;
            bool allMovementStartAfterShow = true;
            bool allMovementHideAfterShow = true;
            bool allMovementDurationShow = true;
            bool allHidingPositionsCustom = true;
            for (int i = 0; i < targets.Length; i++)
            {
                UIElement element = targets[i] as UIElement;

                if (element.MovementStartAfter < 0)
                    allMovementStartAfterShow = false;
                if (element.MovementHideAfter < 0)
                    allMovementHideAfterShow = false;
                if (element.MovementDuration < 0)
                    allMovementDurationShow = false;

                if (element.MovementType == UIElement.MotionType.None)
                {
                    showMovementPropsBool = false;
                    allMovementEaseIn = false;
                    allMovementBounce = false;
                }
                else if (element.MovementType == UIElement.MotionType.EaseIn)
                {
                    allMovementBounce = false;
                }
                else if (element.MovementType == UIElement.MotionType.Linear)
                {
                    allMovementEaseIn = false;
                    allMovementBounce = false;
                }
                else if (element.MovementType == UIElement.MotionType.Bounce)
                {
                    allMovementEaseIn = false;
                }

                if (element.HidingPosition != UIElement.ScreenSides.Custom)
                    allHidingPositionsCustom = false;
            }
            #endregion

            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(movementType, new GUIContent("Type"));

            showMovementProps.target = showMovementPropsBool;

            if (EditorGUILayout.BeginFadeGroup(showMovementProps.faded))
            {
                EditorGUILayout.PropertyField(hidingPosition);
                if (allHidingPositionsCustom)
                {
                    #region Discard Button
                    if (recordingPosition)
                    {
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("X", GUILayout.Width(50), GUILayout.Height(15)))
                            DiscardRecording();
                        EditorGUILayout.EndHorizontal();
                    }
                    #endregion

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(customHidingPosition, new GUIContent("Hiding Position"));

                    if (recordingPosition)
                        GUI.color = Color.red;
                    recordingPosition = GUILayout.Toggle(recordingPosition, !recordingPosition ? "Record Position" : "Finish Recording", EditorStyles.miniButton);
                    GUI.color = Color.white;

                    if (lastRecordingState != recordingPosition)
                    {
                        //If recording start
                        if (recordingPosition)
                        {
                            for (int i = 0; i < targets.Length; i++)
                            {
                                UIElement element = targets[i] as UIElement;
                                RectTransform elementRT = element.GetComponent<RectTransform>();
                                RectTransform parentCanvas = element.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

                                element.startPosition = elementRT.position;

                                float canvasWidth = parentCanvas.lossyScale.x * parentCanvas.rect.width;
                                float canvasHeight = parentCanvas.lossyScale.y * parentCanvas.rect.height;

                                elementRT.position = new Vector3(
                                    parentCanvas.position.x + (element.CustomHidingPosition.x - 0.5f) * canvasWidth,
                                    parentCanvas.position.y + (element.CustomHidingPosition.y - 0.5f) * canvasHeight, elementRT.position.z);
                            }
                        }
                        //If recording end
                        if (!recordingPosition)
                        {
                            EndRecording();
                        }
                    }
                    lastRecordingState = recordingPosition;
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.PropertyField(edgeGap);
                }
                if (allMovementEaseIn)
                {
                    EditorGUILayout.PropertyField(movementEaseIntensity, new GUIContent("Ease Intensity"));
                }
                else if (allMovementBounce)
                {
                    EditorGUILayout.PropertyField(movementBounces, new GUIContent("Bounces Count"));
                    EditorGUILayout.PropertyField(movementBouncePower, new GUIContent("Bounce Power"));
                    EditorGUILayout.PropertyField(movementDisableHidingBounce, new GUIContent("Disable Hiding Bounce"));
                    EditorGUILayout.PropertyField(movementEaseIntensity, new GUIContent("Ease Intensity"));
                }

                #region Custom Properties

                #region Custom Properties Visible
                if (allMovementStartAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(movementStartAfter, new GUIContent("Start After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementStartAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allMovementHideAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(movementHideAfter, new GUIContent("Hide After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementHideAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allMovementDurationShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(movementDuration, new GUIContent("Duration"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementDuration = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion

                //Seperate in case there an option to add custom property
                if (!allMovementStartAfterShow || !allMovementHideAfterShow)
                    EditorGUILayout.Space();

                #region Custom Properties Adding
                if (!allMovementStartAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Start After\"" : "Add custom \"Start After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementStartAfter = element.StartAfter;
                        }
                    }
                }

                if (!allMovementHideAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Hide After\"" : "Add custom \"Hide After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementHideAfter = element.HideAfter;
                        }
                    }
                }

                if (!allMovementDurationShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Duration\"" : "Add custom \"Duration\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.MovementDuration = element.Duration;
                        }
                    }
                }
                #endregion

                #endregion
            }
            EditorGUILayout.EndFadeGroup();

            #endregion

            EditorGUILayout.Space();

            #region Rotation

            #region Group Checks
            bool showRotationPropsBool = true;
            bool allRotationEaseIn = true;
            bool allRotationBounce = true;
            bool allRotationStartAfterShow = true;
            bool allRotationHideAfterShow = true;
            bool allRotationDurationShow = true;
            for (int i = 0; i < targets.Length; i++)
            {
                UIElement element = targets[i] as UIElement;

                if (element.RotationStartAfter < 0)
                    allRotationStartAfterShow = false;
                if (element.RotationHideAfter < 0)
                    allRotationHideAfterShow = false;
                if (element.RotationDuration < 0)
                    allRotationDurationShow = false;

                if (element.RotationType == UIElement.MotionType.None)
                {
                    showRotationPropsBool = false;
                    allRotationEaseIn = false;
                    allRotationBounce = false;
                }
                else if (element.RotationType == UIElement.MotionType.EaseIn)
                {
                    allRotationBounce = false;
                }
                else if (element.RotationType == UIElement.MotionType.Linear)
                {
                    allRotationEaseIn = false;
                    allRotationBounce = false;
                }
                else if (element.RotationType == UIElement.MotionType.Bounce)
                {
                    allRotationEaseIn = false;
                }
            }
            #endregion

            EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(rotationType, new GUIContent("Type"));

            showRotationProps.target = showRotationPropsBool;

            if (EditorGUILayout.BeginFadeGroup(showRotationProps.faded))
            {
                EditorGUILayout.PropertyField(hidingRotation);
                if (allRotationEaseIn)
                {
                    EditorGUILayout.PropertyField(rotationEaseIntensity, new GUIContent("Ease Intensity"));
                }
                else if (allRotationBounce)
                {
                    EditorGUILayout.PropertyField(rotationBounces, new GUIContent("Bounces Count"));
                    EditorGUILayout.PropertyField(rotationBouncePower, new GUIContent("Bounce Power"));
                    EditorGUILayout.PropertyField(rotationDisableHidingBounce, new GUIContent("Disable Hiding Bounce"));
                    EditorGUILayout.PropertyField(rotationEaseIntensity, new GUIContent("Ease Intensity"));
                }

                #region Custom Properties

                #region Custom Properties Visible
                if (allRotationStartAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(rotationStartAfter, new GUIContent("Start After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationStartAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allRotationHideAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(rotationHideAfter, new GUIContent("Hide After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationHideAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allRotationDurationShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(rotationDuration, new GUIContent("Duration"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationDuration = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion

                //Seperate in case there an option to add custom property
                if (!allRotationStartAfterShow || !allRotationHideAfterShow)
                    EditorGUILayout.Space();

                #region Custom Properties Adding
                if (!allRotationStartAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Start After\"" : "Add custom \"Start After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationStartAfter = element.StartAfter;
                        }
                    }
                }

                if (!allRotationHideAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Hide After\"" : "Add custom \"Hide After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationHideAfter = element.HideAfter;
                        }
                    }
                }

                if (!allRotationDurationShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Duration\"" : "Add custom \"Duration\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.RotationDuration = element.Duration;
                        }
                    }
                }
                #endregion

                #endregion
            }
            EditorGUILayout.EndFadeGroup();

            #endregion

            EditorGUILayout.Space();

            #region Scale

            #region Group Checks
            bool showScalePropsBool = true;
            bool allScaleEaseIn = true;
            bool allScaleBounce = true;
            bool allScaleStartAfterShow = true;
            bool allScaleHideAfterShow = true;
            bool allScaleDurationShow = true;
            for (int i = 0; i < targets.Length; i++)
            {
                UIElement element = targets[i] as UIElement;

                if (element.ScaleStartAfter < 0)
                    allScaleStartAfterShow = false;
                if (element.ScaleHideAfter < 0)
                    allScaleHideAfterShow = false;
                if (element.ScaleDuration < 0)
                    allScaleDurationShow = false;

                if (element.ScaleType == UIElement.MotionType.None)
                {
                    showScalePropsBool = false;
                    allScaleEaseIn = false;
                    allScaleBounce = false;
                }
                else if (element.ScaleType == UIElement.MotionType.EaseIn)
                {
                    allScaleBounce = false;
                }
                else if (element.ScaleType == UIElement.MotionType.Linear)
                {
                    allScaleEaseIn = false;
                    allScaleBounce = false;
                }
                else if (element.ScaleType == UIElement.MotionType.Bounce)
                {
                    allScaleEaseIn = false;
                }
            }
            #endregion

            EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(scaleType, new GUIContent("Type"));

            showScaleProps.target = showScalePropsBool;

            if (EditorGUILayout.BeginFadeGroup(showScaleProps.faded))
            {
                EditorGUILayout.PropertyField(hidingScale);
                if (allScaleEaseIn)
                {
                    EditorGUILayout.PropertyField(scaleEaseIntensity, new GUIContent("Ease Intensity"));
                }
                else if (allScaleBounce)
                {
                    EditorGUILayout.PropertyField(scaleBounces, new GUIContent("Bounces Count"));
                    EditorGUILayout.PropertyField(scaleBouncePower, new GUIContent("Bounce Power"));
                    EditorGUILayout.PropertyField(scaleDisableHidingBounce, new GUIContent("Disable Hiding Bounce"));
                    EditorGUILayout.PropertyField(scaleEaseIntensity, new GUIContent("Ease Intensity"));
                }

                #region Custom Properties

                #region Custom Properties Visible
                if (allScaleStartAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(scaleStartAfter, new GUIContent("Start After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleStartAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allScaleHideAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(scaleHideAfter, new GUIContent("Hide After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleHideAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allScaleDurationShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(scaleDuration, new GUIContent("Duration"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleDuration = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion

                //Seperate in case there an option to add custom property
                if (!allScaleStartAfterShow || !allScaleHideAfterShow)
                    EditorGUILayout.Space();

                #region Custom Properties Adding
                if (!allScaleStartAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Start After\"" : "Add custom \"Start After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleStartAfter = element.StartAfter;
                        }
                    }
                }

                if (!allScaleHideAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Hide After\"" : "Add custom \"Hide After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleHideAfter = element.HideAfter;
                        }
                    }
                }

                if (!allScaleDurationShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Duration\"" : "Add custom \"Duration\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.ScaleDuration = element.Duration;
                        }
                    }
                }
                #endregion

                #endregion
            }
            EditorGUILayout.EndFadeGroup();

            #endregion

            EditorGUILayout.Space();

            #region Opacity

            #region Group Checks
            bool showOpacityPropsBool = true;
            bool allOpacityEaseIn = true;
            bool allOpacityBounce = true;
            bool allOpacityStartAfterShow = true;
            bool allOpacityHideAfterShow = true;
            bool allOpacityDurationShow = true;
            for (int i = 0; i < targets.Length; i++)
            {
                UIElement element = targets[i] as UIElement;

                if (element.OpacityStartAfter < 0)
                    allOpacityStartAfterShow = false;
                if (element.OpacityHideAfter < 0)
                    allOpacityHideAfterShow = false;
                if (element.OpacityDuration < 0)
                    allOpacityDurationShow = false;

                if (element.OpacityType == UIElement.MotionType.None)
                {
                    showOpacityPropsBool = false;
                    allOpacityEaseIn = false;
                    allOpacityBounce = false;
                }
                else if (element.OpacityType == UIElement.MotionType.EaseIn)
                {
                    allOpacityBounce = false;
                }
                else if (element.OpacityType == UIElement.MotionType.Linear)
                {
                    allOpacityEaseIn = false;
                    allOpacityBounce = false;
                }
                else if (element.OpacityType == UIElement.MotionType.Bounce)
                {
                    allOpacityEaseIn = false;
                }
            }
            #endregion

            EditorGUILayout.LabelField("Opacity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(opacityType, new GUIContent("Type"));

            showOpacityProps.target = showOpacityPropsBool;

            if (EditorGUILayout.BeginFadeGroup(showOpacityProps.faded))
            {
                EditorGUILayout.PropertyField(hidingOpacity);
                if (allOpacityEaseIn)
                {
                    EditorGUILayout.PropertyField(opacityEaseIntensity, new GUIContent("Ease Intensity"));
                }
                else if (allOpacityBounce)
                {
                    EditorGUILayout.PropertyField(opacityBounces, new GUIContent("Bounces Count"));
                    EditorGUILayout.PropertyField(opacityBouncePower, new GUIContent("Bounce Power"));
                    EditorGUILayout.PropertyField(opacityDisableHidingBounce, new GUIContent("Disable Hiding Bounce"));
                    EditorGUILayout.PropertyField(opacityEaseIntensity, new GUIContent("Ease Intensity"));
                }

                #region Custom Properties

                #region Custom Properties Visible
                if (allOpacityStartAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(opacityStartAfter, new GUIContent("Start After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityStartAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allOpacityHideAfterShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(opacityHideAfter, new GUIContent("Hide After"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityHideAfter = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

                if (allOpacityDurationShow)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(opacityDuration, new GUIContent("Duration"));
                    if (GUILayout.Button("Delete"))
                    {
                        Undo.RecordObjects(targets, "Delete Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityDuration = -1;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                #endregion

                //Seperate in case there an option to add custom property
                if (!allOpacityStartAfterShow || !allOpacityHideAfterShow)
                    EditorGUILayout.Space();

                #region Custom Properties Adding
                if (!allOpacityStartAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Start After\"" : "Add custom \"Start After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Start After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityStartAfter = element.StartAfter;
                        }
                    }
                }

                if (!allOpacityHideAfterShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Hide After\"" : "Add custom \"Hide After\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Hide After");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityHideAfter = element.HideAfter;
                        }
                    }
                }

                if (!allOpacityDurationShow)
                {
                    string txt = targets.Length == 1 ? "Add custom \"Duration\"" : "Add custom \"Duration\" to all";
                    if (GUILayout.Button(txt))
                    {
                        Undo.RecordObjects(targets, "Add Custom Duration");
                        for (int i = 0; i < targets.Length; i++)
                        {
                            UIElement element = targets[i] as UIElement;
                            element.OpacityDuration = element.Duration;
                        }
                    }
                }
                #endregion

                #endregion
            }
            EditorGUILayout.EndFadeGroup();

            #endregion
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("No animation controls available. This GameObject is being controlledby \"Simple Activate/Deactivate\" option.", EditorStyles.wordWrappedLabel);
        }
        EditorGUILayout.Space();

        #region Activation
        EditorGUILayout.LabelField("Simple Activate/Deactivate", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(useSimpleActivation, new GUIContent("Use Activation Control"));
        #endregion

        EditorGUILayout.Space();

        #region Sounds
        EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(showingClip, new GUIContent("Showing Clip"));
        EditorGUILayout.PropertyField(hidingClip, new GUIContent("Hiding Clip"));
        #endregion

        EditorGUILayout.Space();

        #region Events
        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(onShow, new GUIContent("On Show"));
        EditorGUILayout.PropertyField(onHide, new GUIContent("On Hide"));
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    void EndRecording()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            RectTransform elementRT = element.GetComponent<RectTransform>();
            RectTransform parentCanvas = element.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

            Undo.RecordObject(element, "Record Hiding Position");

            float canvasWidth = parentCanvas.lossyScale.x * parentCanvas.rect.width;
            float canvasHeight = parentCanvas.lossyScale.y * parentCanvas.rect.height;
            float canvasStartX = parentCanvas.position.x - canvasWidth / 2;
            float canvasStartY = parentCanvas.position.y - canvasHeight / 2;

            element.CustomHidingPosition = new Vector2((elementRT.position.x - canvasStartX) / canvasWidth,
                (elementRT.position.y - canvasStartY) / canvasHeight);

            elementRT.position = element.startPosition;
        }
    }
    void DiscardRecording()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            UIElement element = targets[i] as UIElement;
            RectTransform elementRT = element.GetComponent<RectTransform>();

            elementRT.position = element.startPosition;
        }
        lastRecordingState = recordingPosition = false;
    }

    void OnDisable()
    {
        if (recordingPosition)
        {
            if (EditorUtility.DisplayDialog("Recording", "You are recording a position, would you like to apply it?", "Apply", "No"))
                EndRecording();
            else
                DiscardRecording();
        }
    }
}
