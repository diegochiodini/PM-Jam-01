﻿using UnityEngine;
using System.Collections;

public class LogOutputHandler : MonoBehaviour
{
    public string token;
    public string logTag = "Unity3D";

    //Register the HandleLog function on scene start to fire on debug.log events 
    void Awake()
    {
        Application.RegisterLogCallback(HandleWithDebugStreamer);
        Application.RegisterLogCallback(HandleWithLoggly);
    } 

    //Create a string to store log level in
    string level = "";

    //Capture debug.log output, send logs to Loggly
    public void HandleWithLoggly(string logString, string stackTrace, LogType type) 
    {

        //Initialize WWWForm and store log level as a string
        level = type.ToString ();
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("LEVEL", level);
        loggingForm.AddField("Message", logString);
        loggingForm.AddField("Stack_Trace", stackTrace);

        //Add any User, Game, or Device MetaData that would be useful to finding issues later 
        loggingForm.AddField("Device_Model", SystemInfo.deviceModel);

        //Send WWW Form to Loggly, replace TOKEN with your unique ID from Loggly
        var sendLog = new WWW("http://logs-01.loggly.com/inputs/" + token + "/tag/" + logTag + "/", loggingForm);
    }

    public void HandleWithDebugStreamer(string logString, string stackTrace, LogType type)
    {
        DebugStreamer.messages.Add(logString);
    }
}