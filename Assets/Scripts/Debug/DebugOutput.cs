using UnityEngine;
using System.Collections;

public class DebugOutput : MonoBehaviour
{
    public static DebugOutput debugOutput;

    public AudioSource beep;
    public UILabel consoleLabel;

    void Awake()
    {
        debugOutput = this;
    }
    
    public static void PlayBeep()
    {
        debugOutput.beep.Play();
    }

    public static void Log(string msg, Color color = default(Color))
    {
       // Debug.Log(msg);
        if(color == default(Color)) color = Color.white;

        if (debugOutput.consoleLabel)
        {
            debugOutput.consoleLabel.text += "[" + ConversionUtils.ColorToHex(color) + "]" + msg + "[" + ConversionUtils.ColorToHex(Color.white) + "]" + "\n";
        }
    }

    public static void LogError(string msg)
    {
        //Debug.LogError(msg);
        if (debugOutput.consoleLabel)
        {
            debugOutput.consoleLabel.text += "[" + ConversionUtils.ColorToHex(Color.red) + "]" + msg + "[" + ConversionUtils.ColorToHex(Color.white) + "]" + "\n";
        }
    }

}
