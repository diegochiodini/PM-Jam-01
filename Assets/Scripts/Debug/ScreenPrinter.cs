using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GUIText))]

public class ScreenPrinter : MonoBehaviour
{
    public TextAnchor anchorAt = TextAnchor.LowerLeft;
    public int numberOfLines = 5;
    public int pixelOffset = 5;

    static ScreenPrinter defaultPrinter = null;
    static bool quitting = false;

    List<string> newMessages = new List<string>();
    TextAnchor _anchorAt;
    float _pixelOffset;
    List<string> messageHistory = new List<string>();

    // static Print method: finds a ScreenPrinter in the project,
    // or creates one if necessary, and prints to that.
    public static void Print(object message)
    {
        if (quitting) return;       // don't try to print while quitting
        if (!defaultPrinter)
        {
            GameObject gob = GameObject.Find("Screen Printer");
            if (!gob) gob = new GameObject("Screen Printer");
            defaultPrinter = gob.GetComponent<ScreenPrinter>();
            if (!defaultPrinter) defaultPrinter = gob.AddComponent<ScreenPrinter>();
        }
        defaultPrinter.LocalPrint(message);
    }

    // member LocalPrint method: prints to this particular screen printer.
    // Called LocalPrint because C# won't let us use the same name for both
    // static and instance method.  Grr.  Argh.  >:(
    public void LocalPrint(object message)
    {
        if (quitting) return;       // don't try to print while quiting
        newMessages.Add(message.ToString());
    }

    void Awake()
    {
        if (!GetComponent<GUIText>())
        {
            gameObject.AddComponent<GUIText>();
            transform.position = Vector3.zero;
            transform.localScale = new Vector3(0, 0, 1);
        }
        _anchorAt = anchorAt;
        UpdatePosition();
    }

    void OnApplicationQuitting()
    {
        quitting = true;
    }

    void Update()
    {
        // if anchorAt or pixelOffset has changed while running, update the text position
        if (_anchorAt != anchorAt || _pixelOffset != pixelOffset)
        {
            _anchorAt = anchorAt;
            _pixelOffset = pixelOffset;
            UpdatePosition();
        }

        //  if the message has changed, update the display
        if (newMessages.Count > 0)
        {
            for (int messageIndex = 0; messageIndex < newMessages.Count; messageIndex++)
            {
                messageHistory.Add(newMessages[messageIndex]);
            }
            if (messageHistory.Count > numberOfLines)
            {
                messageHistory.RemoveRange(0, messageHistory.Count - numberOfLines);
            }

            //  create the multi-line text to display
            GetComponent<GUIText>().text = string.Join("\n", messageHistory.ToArray());
            newMessages.Clear();
        }
    }

    void UpdatePosition()
    {
        switch (anchorAt)
        {
            case TextAnchor.UpperLeft:
                transform.position = new Vector3(0.0f, 1.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Left;
                GetComponent<GUIText>().pixelOffset = new Vector2(pixelOffset, -pixelOffset);
                break;
            case TextAnchor.UpperCenter:
                transform.position = new Vector3(0.5f, 1.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Center;
                GetComponent<GUIText>().pixelOffset = new Vector2(0, -pixelOffset);
                break;
            case TextAnchor.UpperRight:
                transform.position = new Vector3(1.0f, 1.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Right;
                GetComponent<GUIText>().pixelOffset = new Vector2(-pixelOffset, -pixelOffset);
                break;
            case TextAnchor.MiddleLeft:
                transform.position = new Vector3(0.0f, 0.5f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Left;
                GetComponent<GUIText>().pixelOffset = new Vector2(pixelOffset, 0.0f);
                break;
            case TextAnchor.MiddleCenter:
                transform.position = new Vector3(0.5f, 0.5f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Center;
                GetComponent<GUIText>().pixelOffset = new Vector2(0, 0);
                break;
            case TextAnchor.MiddleRight:
                transform.position = new Vector3(1.0f, 0.5f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Right;
                GetComponent<GUIText>().pixelOffset = new Vector2(-pixelOffset, 0.0f);
                break;
            case TextAnchor.LowerLeft:
                transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Left;
                GetComponent<GUIText>().pixelOffset = new Vector2(pixelOffset, pixelOffset);
                break;
            case TextAnchor.LowerCenter:
                transform.position = new Vector3(0.5f, 0.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Center;
                GetComponent<GUIText>().pixelOffset = new Vector2(0, pixelOffset);
                break;
            case TextAnchor.LowerRight:
                transform.position = new Vector3(1.0f, 0.0f, 0.0f);
                GetComponent<GUIText>().anchor = anchorAt;
                GetComponent<GUIText>().alignment = TextAlignment.Right;
                GetComponent<GUIText>().pixelOffset = new Vector2(-pixelOffset, pixelOffset);
                break;
        }
    }
}