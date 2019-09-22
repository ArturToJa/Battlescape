using UnityEngine;
using System.Collections;

public class GUIExampleFantasy : MonoBehaviour 
{

    public GUISkin mSkin;
    public Rect mWindowRect = new Rect(200, 200, 350, 566);
    public GUIContent mWindowContent;
    public GUIContent mGUIContent;
    public float mSliderValue = 3.0f;
    public bool mToggleValue;
    public bool mForceSize;
    Vector2 mScrollPosition;


    public void OnGUI()
    {
        if (mSkin != null)
        {
            GUI.skin = mSkin;
        }
        mWindowRect = GUI.Window(0, mWindowRect, DrawWindow, mWindowContent);
    }


    void DrawWindow(int windowID)
    {
        mScrollPosition = GUILayout.BeginScrollView(mScrollPosition);
		//GUILayout.Label("Label");
        GUILayout.Label(mGUIContent);
        GUILayout.Button("Button 1");
	GUILayout.Button("Button 2");
	GUILayout.Box("This is a Box\nIt has text in it\nLots of it");
        mSliderValue = GUILayout.HorizontalSlider(mSliderValue, 0, 8);
        mGUIContent.text = GUILayout.PasswordField(mGUIContent.text, '*');
        GUILayout.TextField("Text Field");
        GUILayout.TextArea("This is a Text Area\nIt too has several\nlines of text");
        mToggleValue = GUILayout.Toggle(mToggleValue, "Toggle 1");
	mToggleValue = !GUILayout.Toggle(!mToggleValue, "Toggle 2");
        mSliderValue = GUILayout.VerticalSlider(mSliderValue, 0, 8);
	GUILayout.Box("This is another Box\nIt is just here to\nfill up space");
        GUILayout.EndScrollView();
        GUI.DragWindow();

    }


}
