using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SortScrollController : ScrollController
{
    [SerializeField]
    private Button PasteAll;
    [SerializeField]
    private Button RunAll;

    private List<SortUIElement> elements = new List<SortUIElement>();

    void Start()
    {
        SortUIElement.GetClipBoardFromCache();
        var supportedTypes = SortSource.Instance.GetSupportedTypes();
        elementTemplate.GetComponent<SortUIElement>().SetTypes(supportedTypes);

        var managedMethods = SortSource.Instance.GetMethods(SortType.Managed);
        for (int i = 0; i < managedMethods.Count; i++)
        {
            var element = AddElement().GetComponent<SortUIElement>();
            element.SetAction(()=> { MainSortRunner.Instance.RunSort(element.ToSettings()); });
            element.SetLabel(managedMethods[i]);
            element.SetLablelColor(new Color(114/255f, 178 / 255f, 221 / 255f));
            element.PasteFromClipBoard();
            elements.Add(element);
        }

        var nativeMethods = SortSource.Instance.GetMethods(SortType.Native);
        for (int i = 0; i < nativeMethods.Count; i++)
        {
            var element = AddElement().GetComponent<SortUIElement>();
            element.SetAction(() => { MainSortRunner.Instance.RunSort(element.ToSettings()); });
            element.SetLabel(nativeMethods[i]+" (Native)");
            element.SetLablelColor(new Color(121 / 255f, 114 / 255f, 221 / 255f));
            element.PasteFromClipBoard();
            elements.Add(element);
        }

        var customMethods = CustomSortSource.Instance.GetMethods();
        for (int i = 0; i < customMethods.Count; i++)
        {
            var element = AddElement().GetComponent<SortUIElement>();
            element.SetAction(() => { MainSortRunner.Instance.RunCustomSort(element.ToSettings()); });
            element.SetLabel(customMethods[i] + " (Custom)");
            element.SetLablelColor(new Color(221 / 255f, 141 / 255f, 114 / 255f));
            element.PasteFromClipBoard();
            elements.Add(element);
        }

        PasteAll.onClick.AddListener(PasteToAll);
        RunAll.onClick.AddListener(RunToAll);
    }

    void PasteToAll()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].PasteFromClipBoard();
        }
    }

    void RunToAll()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].RunSort();
        }
    }
}
