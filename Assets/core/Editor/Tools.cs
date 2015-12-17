using UnityEngine;
using UnityEditor;
using System.Collections;
using GameUtils;
using System;


public class Tools 
{
    public const string LOCAL_SCREENSHOT_FILES = "./Screenshots/screenshot_";

    [MenuItem("Chiodini/Tools/Clear Console %#k")]
    public static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }

    [MenuItem("Chiodini/Tools/Menu forward %.")]
    public static void MenuForward()
    {
        //Debug.Log("Menu forward");
        SwitchMenu menu = GameObject.FindObjectOfType<SwitchMenu>();
        Assert.NotNull(menu, "SwitchMenu not found.");
        menu.EditorMenuForward();
        
    }

    [MenuItem("Chiodini/Tools/Menu Backward %,")]
    public static void MenuBackward()
    {
        //Debug.Log("Menu backward");
        SwitchMenu menu = GameObject.FindObjectOfType<SwitchMenu>();
        Assert.NotNull(menu, "SwitchMenu not found.");
        menu.EditorMenuBackward();
    }

    [MenuItem("Chiodini/Tools/Take screenshot %F2")]
    public static void TakeScreenshot()
    {
        Texture2D shot = GraphicsUtils.TakeScreenshot(350, 622, null);
        string filePath = LOCAL_SCREENSHOT_FILES + Format.SafeTimestampNow() + ".jpg";
        IO.SaveTextureToFile(filePath, shot);
        Debug.Log("Screenshot taken: " + filePath);
    }
}
