using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class OpenGitTerminal 
{
    [MenuItem("Tools/Git")]
    public static void Open()
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.WindowStyle = ProcessWindowStyle.Normal;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/c \"C:\\Program Files\\Git\\bin\\sh.exe\"";
        process.StartInfo = startInfo;
        process.Start();
    }
}
