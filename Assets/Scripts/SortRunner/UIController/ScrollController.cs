using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ScrollController : MonoBehaviour
{
    [SerializeField]
    protected RectTransform elementTemplate;
    [SerializeField]
    protected RectTransform content;

    Stack<RectTransform> elements = new Stack<RectTransform>();

    public void ClearAll()
    {
        ChangeContentHeight(-elementTemplate.rect.height * elements.Count);
        while (elements.Count > 0)
        {
            Destroy(elements.Pop().gameObject);
        }
    }

    protected RectTransform AddElement()
    {
        var element = Instantiate(elementTemplate);
        element.gameObject.SetActive(true);
        element.transform.SetParent(content.transform);
        ChangeContentHeight(elementTemplate.rect.height);
        elements.Push(element);
        return element;
    }

    protected void ChangeContentHeight(float delta)
    {
        Vector2 offset = content.offsetMin;
        offset.y -= delta;
        content.offsetMin = offset;
    }
}
