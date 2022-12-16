using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class Console : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Console &t")]
    public static void ShowExample()
    {
        Console wnd = GetWindow<Console>();

        wnd.maxSize = new Vector2(800, 480);
        wnd.minSize = new Vector2(800, 480);

        wnd.titleContent = new GUIContent("Unity Terminal");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Project/Editor/Console.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        //var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Project/Editor/Console.uss");
        //VisualElement labelWithStyle = new Label("Hello World! With Style");
        //labelWithStyle.styleSheets.Add(styleSheet);
        //root.Add(labelWithStyle);
    }
}