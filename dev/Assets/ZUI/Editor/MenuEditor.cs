using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Menu))]
public class MenuEditor : Editor {

    #region Inherited Properties
    SerializedProperty visible;
    SerializedProperty useSimpleActivation;

    SerializedProperty showingClip;
    SerializedProperty hidingClip;

    SerializedProperty onShow;
    SerializedProperty onHide;
    #endregion

    SerializedProperty previousMenu;
    SerializedProperty nextMenu;
    SerializedProperty deactivateWhileInvisible;

    SerializedProperty animatedElements;
    SerializedProperty multiMenusAnimatedElements;
    SerializedProperty switchAfter;

    void OnEnable()
    {
        #region Inherited Properties
        visible = serializedObject.FindProperty("Visible");
        useSimpleActivation = serializedObject.FindProperty("UseSimpleActivation");

        showingClip = serializedObject.FindProperty("ShowingClip");
        hidingClip = serializedObject.FindProperty("HidingClip");

        onShow = serializedObject.FindProperty("OnShow");
        onHide = serializedObject.FindProperty("OnHide");
        #endregion

        previousMenu = serializedObject.FindProperty("PreviousMenu");
        nextMenu = serializedObject.FindProperty("NextMenu");
        deactivateWhileInvisible = serializedObject.FindProperty("DeactivateWhileInvisible");

        animatedElements = serializedObject.FindProperty("AnimatedElements");
        multiMenusAnimatedElements = serializedObject.FindProperty("MultiMenusAnimatedElements");
        switchAfter = serializedObject.FindProperty("SwitchAfter");
    }

    public override void OnInspectorGUI()
    {
        Menu myMenu = target as Menu;
        ZUIManager zuiManager = FindObjectOfType<ZUIManager>();

        #region Activate Button
        if (GUILayout.Button("Activate", GUILayout.Height(30)))
        {
            foreach (Menu m in zuiManager.AllMenus)
            {
                if (m == null) continue;

                Undo.RecordObject(m.gameObject, "Activate Menu");
                if (m == myMenu)
                {
                    m.gameObject.SetActive(true);
                }
                else
                {
                    m.gameObject.SetActive(false);
                    foreach (UIElement ue in m.MultiMenusAnimatedElements)
                    {
                        if (ue != null)
                            ue.gameObject.SetActive(false);
                    }
                }
            }
            foreach (UIElement ue in myMenu.MultiMenusAnimatedElements)
            {
                if (ue != null)
                    ue.gameObject.SetActive(true);
            }

        }
        #endregion

        #region User Interface
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Is Visible?", visible.boolValue.ToString());
        EditorGUILayout.PropertyField(previousMenu);
        EditorGUILayout.PropertyField(nextMenu);
        EditorGUILayout.PropertyField(deactivateWhileInvisible);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Menu Elements", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(animatedElements, true);
        EditorGUILayout.PropertyField(multiMenusAnimatedElements, true);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Switching", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(useSimpleActivation);
        EditorGUILayout.PropertyField(switchAfter);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Sounds", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(showingClip);
        EditorGUILayout.PropertyField(hidingClip);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(onShow);
        EditorGUILayout.PropertyField(onHide);

        EditorGUILayout.Space();
        #endregion

        #region Tools
        EditorGUILayout.LabelField("Tools",EditorStyles.boldLabel);

        #region Update Animated Elements Button
        if (GUILayout.Button("Update Animated Elements", GUILayout.Height(30)))
        {
            Undo.RecordObject(myMenu, "Update Animated Items");
            myMenu.AnimatedElements = GetAnimatedElements(myMenu.transform);
            UIElement menuUE = myMenu.GetComponent<UIElement>();
            if (menuUE)
                myMenu.AnimatedElements.Insert(0, menuUE);
        }
        #endregion

        if (!zuiManager)
        {
            Debug.LogError("There's no ZUIManager script in the scene, you can have it by using the menu bar ZUI>Creation Window>Setup. Or by creating an empty GameObject and add ZUIManager script to it.");
            return;
        }

        #region Set As Default Menu Button
        if (zuiManager.CurActiveMenu != myMenu)
        {
            if (GUILayout.Button("Set As Default Menu", GUILayout.Height(30)))
            {
                Undo.RecordObject(zuiManager, "Set Default Menu");

                zuiManager.CurActiveMenu = myMenu;
            }
        }
        else
        {
            GUILayout.Space(7.5f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Default Menu", GUILayout.Height(15));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(7.5f);
        }
        #endregion

        #region Check Menu Independant Elements
        if (myMenu.AnimatedElements != null)
        {
            for (int i = 0; i < myMenu.AnimatedElements.Count; i++)
            {
                if (myMenu.AnimatedElements[i] == null) continue;

                if (!myMenu.AnimatedElements[i].MenuDependent)
                {
                    if (EditorUtility.DisplayDialog("Error", myMenu.AnimatedElements[i].gameObject.name + " is menu independant but is inside this menu's elements list.", "Remove it from the list", "Switch it to menu dependant"))
                    {
                        Undo.RecordObject(myMenu, "Removing from list");
                        myMenu.AnimatedElements.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        Undo.RecordObject(myMenu, "Switch to menu dependant");
                        myMenu.AnimatedElements[i].MenuDependent = true;
                    }
                }
                if (myMenu.AnimatedElements[i].ControlledBy != myMenu)
                    myMenu.AnimatedElements[i].ControlledBy = myMenu;
            }
        }
        if (myMenu.MultiMenusAnimatedElements != null)
        {
            for (int i = 0; i < myMenu.MultiMenusAnimatedElements.Count; i++)
            {
                if (myMenu.MultiMenusAnimatedElements[i] == null) continue;

                if (!myMenu.MultiMenusAnimatedElements[i].MenuDependent)
                {
                    if (EditorUtility.DisplayDialog("Error", myMenu.MultiMenusAnimatedElements[i].gameObject.name + " is menu independant but is inside this Menu's elements list.", "Remove it from the list", "Switch it to menu dependant"))
                    {
                        Undo.RecordObject(myMenu, "Removing from list");
                        myMenu.MultiMenusAnimatedElements.RemoveAt(i);
                        i--;
                        continue;
                    }
                    else
                    {
                        Undo.RecordObject(myMenu, "Switch to menu dependant");
                        myMenu.MultiMenusAnimatedElements[i].MenuDependent = true;
                    }
                }
            }
        }
        #endregion
        #endregion

        serializedObject.ApplyModifiedProperties();

    }

    List<UIElement> GetAnimatedElements(Transform holder)
    {
        List<UIElement> ue = new List<UIElement>();

        foreach (Transform c in holder)
        {
            UIElement cUE = c.GetComponent<UIElement>();
            if (cUE && cUE.MenuDependent && (cUE.ControlledBy == null || cUE.ControlledBy == (Menu)target))
                ue.Add(cUE);

            ue.AddRange(GetAnimatedElements(c));
        }
        return ue;
    }

}
