using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIAnimation))]
public class UIAnimationEditor : Editor {

    SerializedProperty loop;
    SerializedProperty restartOnVisible;

    SerializedProperty animationFrames;

    static int chosenFrame = -1;
    int lastAddedFrame = -1;

    private bool recordingPosition;
    private bool lastRecordingState;
    private int recordingFrame;
    public Vector2 originalPosition;

    void OnEnable()
    {
        loop = serializedObject.FindProperty("Loop");
        restartOnVisible = serializedObject.FindProperty("RestartOnVisible");
        animationFrames = serializedObject.FindProperty("AnimationFrames");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(loop);
        EditorGUILayout.PropertyField(restartOnVisible);

        EditorGUILayout.LabelField("Frames", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.color = Color.green;
        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            animationFrames.InsertArrayElementAtIndex(0);
            lastAddedFrame = 0;
            InitializeFrame(animationFrames.GetArrayElementAtIndex(0));
        }
        GUI.color = Color.white;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        for (int i = 0; i < animationFrames.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Frame " + (i + 1) + (lastAddedFrame == i? " (Newest)" : ""), GUILayout.Height(30)))
            {
                chosenFrame = chosenFrame != i ? i : -1;
            }

            #region Delete Button
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
            {
                animationFrames.DeleteArrayElementAtIndex(i);
                if (i == lastAddedFrame)
                    lastAddedFrame = -1;
                i--;
                continue;
            }
            GUI.color = Color.white;
            #endregion
            EditorGUILayout.EndHorizontal();

            #region Frame Details
            //If this frame is selected
            if (chosenFrame == i)
            {
                SerializedProperty curFrame = animationFrames.GetArrayElementAtIndex(i);
                #region Properties
                SerializedProperty startAfter = curFrame.FindPropertyRelative("StartAfter");
                SerializedProperty duration = curFrame.FindPropertyRelative("Duration");

                SerializedProperty movementType = curFrame.FindPropertyRelative("MovementType");
                SerializedProperty startPosition = curFrame.FindPropertyRelative("StartPosition");
                SerializedProperty movementHidingPosition = curFrame.FindPropertyRelative("MovementHidingPosition");
                SerializedProperty customHidingPosition = curFrame.FindPropertyRelative("CustomHidingPosition");
                SerializedProperty edgeGap = curFrame.FindPropertyRelative("EdgeGap");
                SerializedProperty movementDuration = curFrame.FindPropertyRelative("MovementDuration");
                SerializedProperty movementBounces = curFrame.FindPropertyRelative("MovementBounces");
                SerializedProperty movementBouncePower = curFrame.FindPropertyRelative("MovementBouncePower");

                SerializedProperty rotationType = curFrame.FindPropertyRelative("RotationType");
                SerializedProperty startRotation = curFrame.FindPropertyRelative("StartRotation");
                SerializedProperty euler = curFrame.FindPropertyRelative("Euler");
                SerializedProperty rotationDuration = curFrame.FindPropertyRelative("RotationDuration");
                SerializedProperty rotationBounces = curFrame.FindPropertyRelative("RotationBounces");
                SerializedProperty rotationBouncePower = curFrame.FindPropertyRelative("RotationBouncePower");

                SerializedProperty scaleType = curFrame.FindPropertyRelative("ScaleType");
                SerializedProperty startScale = curFrame.FindPropertyRelative("StartScale");
                SerializedProperty scaleVector = curFrame.FindPropertyRelative("ScaleVector");
                SerializedProperty scaleDuration = curFrame.FindPropertyRelative("ScaleDuration");
                SerializedProperty scaleBounces = curFrame.FindPropertyRelative("ScaleBounces");
                SerializedProperty scaleBouncePower = curFrame.FindPropertyRelative("ScaleBouncePower");

                SerializedProperty opacityType = curFrame.FindPropertyRelative("OpacityType");
                SerializedProperty startOpacity = curFrame.FindPropertyRelative("StartOpacity");
                SerializedProperty opacityWanted = curFrame.FindPropertyRelative("OpacityWanted");
                SerializedProperty opacityDuration = curFrame.FindPropertyRelative("OpacityDuration");
                SerializedProperty opacityBounces = curFrame.FindPropertyRelative("OpacityBounces");
                SerializedProperty opacityBouncePower = curFrame.FindPropertyRelative("OpacityBouncePower");
                #endregion

                EditorGUILayout.PropertyField(startAfter);
                EditorGUILayout.PropertyField(duration);

                EditorGUILayout.Space();

                #region Movement
                EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(movementType, new GUIContent("Type"));
                if (movementType.enumValueIndex != 0)
                {
                    EditorGUILayout.PropertyField(startPosition, new GUIContent("Move to start position"));
                    if (!startPosition.boolValue)
                    {
                        EditorGUILayout.PropertyField(movementHidingPosition, new GUIContent("Hiding Position"));
                        if (movementHidingPosition.enumValueIndex == 8)   //Custom position
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

                            if (recordingPosition) GUI.color = Color.red;
                            recordingPosition = GUILayout.Toggle(recordingPosition, !recordingPosition ? "Record Position" : "Finish Recording", EditorStyles.miniButton);
                            GUI.color = Color.white;

                            if (lastRecordingState != recordingPosition)
                            {
                                //If recording start
                                if (recordingPosition)
                                {
                                    recordingFrame = i;

                                    UIAnimation uiAnimation = target as UIAnimation;
                                    UIElement element = uiAnimation.GetComponent<UIElement>();
                                    RectTransform elementRT = element.GetComponent<RectTransform>();
                                    RectTransform parentCanvas = element.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

                                    float canvasWidth = parentCanvas.lossyScale.x * parentCanvas.rect.width;
                                    float canvasHeight = parentCanvas.lossyScale.y * parentCanvas.rect.height;

                                    element.startPosition = elementRT.position;

                                    Vector2 chp = curFrame.FindPropertyRelative("CustomHidingPosition").vector2Value;

                                    elementRT.position = new Vector3(
                                            parentCanvas.position.x + (chp.x - 0.5f) * canvasWidth,
                                            parentCanvas.position.y + (chp.y - 0.5f) * canvasHeight, elementRT.position.z);
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
                            EditorGUILayout.PropertyField(edgeGap, new GUIContent("Edge Gap"));
                    }
                    if (movementType.enumValueIndex == 3)
                    {
                        EditorGUILayout.PropertyField(movementBounces, new GUIContent("Bounces Count"));
                        EditorGUILayout.PropertyField(movementBouncePower, new GUIContent("Bounce Power"));
                    }
                    #region Custom Propeties
                    if (movementDuration.floatValue > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(movementDuration, new GUIContent("Duration"));
                        if (GUILayout.Button("Delete"))
                        {
                            movementDuration.floatValue = -1;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button("Add custom \"Duration\""))
                        {
                            movementDuration.floatValue = duration.floatValue;
                        }
                    }
                    #endregion
                    
                }
                #endregion

                EditorGUILayout.Space();

                #region Rotation
                EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(rotationType, new GUIContent("Type"));
                if (rotationType.enumValueIndex != 0)
                {
                    EditorGUILayout.PropertyField(startRotation, new GUIContent("Rotate to start rotation"));
                    if (!startRotation.boolValue)
                    {
                        EditorGUILayout.PropertyField(euler, new GUIContent("Hiding Euler"));
                    }
                    if (rotationType.enumValueIndex == 3)
                    {
                        EditorGUILayout.PropertyField(rotationBounces, new GUIContent("Bounces Count"));
                        EditorGUILayout.PropertyField(rotationBouncePower, new GUIContent("Bounce Power"));
                    }
                    #region Custom Propeties
                    if (rotationDuration.floatValue > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(rotationDuration, new GUIContent("Duration"));
                        if (GUILayout.Button("Delete"))
                        {
                            rotationDuration.floatValue = -1;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button("Add custom \"Duration\""))
                        {
                            rotationDuration.floatValue = duration.floatValue;
                        }
                    }
                    #endregion
                    
                }
                #endregion

                EditorGUILayout.Space();

                #region Scale
                EditorGUILayout.LabelField("Scale", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(scaleType, new GUIContent("Type"));
                if (scaleType.enumValueIndex != 0)
                {
                    EditorGUILayout.PropertyField(startScale, new GUIContent("Change to start scale"));
                    if (!startScale.boolValue)
                    {
                        EditorGUILayout.PropertyField(scaleVector, new GUIContent("Hiding Scale"));
                    }
                    if (scaleType.enumValueIndex == 3)
                    {
                        EditorGUILayout.PropertyField(scaleBounces, new GUIContent("Bounces Count"));
                        EditorGUILayout.PropertyField(scaleBouncePower, new GUIContent("Bounce Power"));
                    }
                    #region Custom Propeties
                    if (scaleDuration.floatValue > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(scaleDuration, new GUIContent("Duration"));
                        if (GUILayout.Button("Delete"))
                        {
                            scaleDuration.floatValue = -1;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button("Add custom \"Duration\""))
                        {
                            scaleDuration.floatValue = duration.floatValue;
                        }
                    }
                    #endregion
                }
                
                #endregion

                EditorGUILayout.Space();

                #region Opacity
                EditorGUILayout.LabelField("Opacity", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(opacityType, new GUIContent("Type"));
                if (opacityType.enumValueIndex != 0)
                {
                    EditorGUILayout.PropertyField(startOpacity, new GUIContent("Change to start opacity"));
                    if (!startOpacity.boolValue)
                    {
                        EditorGUILayout.PropertyField(opacityWanted, new GUIContent("Hiding Opacity"));
                    }
                    if (opacityType.enumValueIndex == 3)
                    {
                        EditorGUILayout.PropertyField(opacityBounces, new GUIContent("Bounces Count"));
                        EditorGUILayout.PropertyField(opacityBouncePower, new GUIContent("Bounce Power"));
                    }
                    #region Custom Propeties
                    if (opacityDuration.floatValue > 0)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(opacityDuration, new GUIContent("Duration"));
                        if (GUILayout.Button("Delete"))
                        {
                            opacityDuration.floatValue = -1;
                        }
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        if (GUILayout.Button("Add custom \"Duration\""))
                        {
                            opacityDuration.floatValue = duration.floatValue;
                        }
                    }
                    #endregion
                }
                #endregion

                EditorGUILayout.Space();

            }
            #endregion

            #region Add Button
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUI.color = Color.green;
            if (GUILayout.Button("Add", GUILayout.Width(60)))
            {
                animationFrames.InsertArrayElementAtIndex(i + 1);
                lastAddedFrame = i + 1;
                InitializeFrame(animationFrames.GetArrayElementAtIndex(i + 1));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            #endregion

            GUI.color = Color.white;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void InitializeFrame(SerializedProperty frame)
    {
        frame.FindPropertyRelative("Duration").floatValue = 0.3f;

        frame.FindPropertyRelative("MovementDuration").floatValue = -1;
        frame.FindPropertyRelative("MovementBounces").intValue = 1;
        frame.FindPropertyRelative("MovementBouncePower").intValue = 3;

        frame.FindPropertyRelative("RotationDuration").floatValue = -1;
        frame.FindPropertyRelative("RotationBounces").intValue = 1;
        frame.FindPropertyRelative("RotationBouncePower").intValue = 3;

        frame.FindPropertyRelative("ScaleDuration").floatValue = -1;
        frame.FindPropertyRelative("ScaleBounces").intValue = 1;
        frame.FindPropertyRelative("ScaleBouncePower").intValue = 3;

        frame.FindPropertyRelative("OpacityDuration").floatValue = -1;
        frame.FindPropertyRelative("OpacityBounces").intValue = 1;
        frame.FindPropertyRelative("OpacityBouncePower").intValue = 3;

    }

    void EndRecording()
    {
        UIAnimation uiAnimation = target as UIAnimation;
        UIElement element = uiAnimation.GetComponent<UIElement>();
        RectTransform elementRT = element.GetComponent<RectTransform>();
        RectTransform parentCanvas = element.GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Undo.RecordObject(element, "Record Hiding Position");

        float canvasWidth = parentCanvas.lossyScale.x * parentCanvas.rect.width;
        float canvasHeight = parentCanvas.lossyScale.y * parentCanvas.rect.height;
        float canvasStartX = parentCanvas.position.x - canvasWidth / 2;
        float canvasStartY = parentCanvas.position.y - canvasHeight / 2;

        uiAnimation.AnimationFrames[recordingFrame].CustomHidingPosition = new Vector2((elementRT.position.x - canvasStartX) / canvasWidth,
                (elementRT.position.y - canvasStartY) / canvasHeight);

        elementRT.position = element.startPosition;
    }
    void DiscardRecording()
    {
        UIAnimation uiAnimation = target as UIAnimation;
        UIElement element = uiAnimation.GetComponent<UIElement>();
        RectTransform elementRT = element.GetComponent<RectTransform>();

        elementRT.position = element.startPosition;
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
