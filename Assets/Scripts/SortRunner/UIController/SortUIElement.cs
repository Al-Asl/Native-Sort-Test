using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class SortUIElement : MonoBehaviour 
{
    
    private static SortSettings clipBoard;
    private const string cacheKey = "clipBoardData";

    [SerializeField]
    private Text Label;
    [SerializeField]
    private Image LabelBackground;
    [SerializeField]
    private Dropdown Type;
    [SerializeField]
    private Dropdown Count;
    [SerializeField]
    private Dropdown Min;
    [SerializeField]
    private Dropdown Max;
    [SerializeField]
    private Dropdown TestCount;
    [SerializeField]
    private Dropdown Sorted;
    [SerializeField]
    private Button Copy;
    [SerializeField]
    private Button Paste;
    [SerializeField]
    private Button Run;

    private static void CacheClipBoard ()
    {
        IOUtil.Write(cacheKey, clipBoard);
    }

    public static void GetClipBoardFromCache()
    {
        clipBoard = IOUtil.Read<SortSettings>(cacheKey);
    }

    int toNumber (string num)
    {
        int sign = num.ToCharArray()[0] == '-' ? -1 : 1;

        if(sign == -1)
        {
            num = num.TrimStart('-');
        }

        if(num == "0")
        {
            return 0;
        }
        else if (num == "1")
        {
            return sign * 1;
        }
        else if (num == "10")
        {
            return sign * 10;
        }
        else if (num == "100")
        {
            return sign * 100;
        }
        else if (num == "1K")
        {
            return sign * 1000;
        }
        else if (num == "10K")
        {
            return sign * 10000;
        }
        else if (num == "100K")
        {
            return sign * 100000;
        }
        else if (num == "1M")
        {
            return sign * 1000000;
        }
        else if (num == "10M")
        {
            return sign * 10000000;
        }
        else if (num == "100M")
        {
            return sign * 100000000;
        }
        else if (num == "1B")
        {
            return sign * 1000000000;
        }
        return 0;
    }

    string fromNumber(int num)
    {
        int absNum = Mathf.Abs(num);
        string sign = (num < 0 ? "-" : "");

        if (absNum == 10)
        {
            return sign + "10";
        }
        else if (absNum == 100)
        {
            return sign + "100";
        }
        else if (absNum == 1000)
        {
            return sign + "1K";
        }
        else if (absNum == 10000)
        {
            return sign + "10K";
        }
        else if (absNum == 100000)
        {
            return sign + "100K";
        }
        else if (absNum == 1000000)
        {
            return sign + "1M";
        }
        else if (absNum == 10000000)
        {
            return sign + "10M";
        }
        else if (absNum == 100000000)
        {
            return sign + "100M";
        }
        else if (absNum == 1000000000)
        {
            return sign + "1B";
        }
        else
            return "invalid number";
    }

    public void SetAction (UnityEngine.Events.UnityAction action)
    {
        Run.onClick.AddListener(action);
    }

    private void OnEnable()
    {
        Copy.onClick.AddListener(CopyToClipBoard);
        Paste.onClick.AddListener(PasteFromClipBoard);
    }

    public void RunSort()
    {
        Run.onClick.Invoke();
    }

    void CopyToClipBoard()
    {
        clipBoard = ToSettings();
        CacheClipBoard();
    }

    public void PasteFromClipBoard()
    {
        if (clipBoard == null) return;

        Type.value = Type.options.FindIndex((option) => { return option.text == clipBoard.elementType; });
        Count.value = Count.options.FindIndex((option) => { return toNumber(option.text) == clipBoard.Count; });
        Max.value = Max.options.FindIndex((option) => { return toNumber(option.text) == clipBoard.Max; });
        Min.value = Min.options.FindIndex((option) => { return toNumber(option.text) == clipBoard.Min; });
        TestCount.value = TestCount.options.FindIndex((option) => { return toNumber(option.text) == clipBoard.TestCount; });
        Sorted.value = Sorted.options.FindIndex((option) => { return toNumber(option.text) / 100f == clipBoard.Sorted; });
    }

    public void SetTypes(List<string> types)
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(types.Count);
        for (int i = 0; i < types.Count; i++)
        {
            options.Add(new Dropdown.OptionData(types[i]));
        }
        Type.options = options;
    }

    public void SetLabel (string label)
    {
        Label.text = label;
    }

    public void SetLablelColor (Color color)
    {
        LabelBackground.color = color;
    }

    public SortSettings ToSettings ()
    {
        string[] label = Label.text.Split(' ');

        return new SortSettings()
        {
            sortType = label.Length > 1 ? SortType.Native : SortType.Managed,
            name = label[0],
            elementType = GetDropDownValue(Type),
            Count = toNumber(GetDropDownValue(Count)),
            Max = toNumber(GetDropDownValue(Max)),
            Min = toNumber(GetDropDownValue(Min)),
            TestCount = toNumber(GetDropDownValue(TestCount)),
            Sorted = toNumber(GetDropDownValue(Sorted)) /100f,
            MemoryDebug = false
        };
    }

    private string GetDropDownValue (Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }
}
