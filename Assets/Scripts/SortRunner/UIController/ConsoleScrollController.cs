using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScrollController : ScrollController , ILogger
{
    private void Start()
    {
        MainSortRunner.Instance.AddLogger(this);
    }

    public void Log(string message)
    {
       var element = AddElement();
        element.GetComponent<Text>().text = message;
        element.transform.SetSiblingIndex(0);
    }
}
