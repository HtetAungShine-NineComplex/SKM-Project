using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBase : MonoBehaviour
{
    public string UIName { get { return this.GetType().Name; } }

    public virtual void OnInitialize() { }

    public virtual void OnShow(UIBaseData data)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);
    }
    public virtual void OnEvent(byte eventCode, params object[] parameters)
    {

    }
    public virtual void OnUpdate() { }

    public void UpdateUI()
    {
        OnUpdate();
    }
    public void UIEvent(byte eventCode, params object[] parameters)
    {
        OnEvent(eventCode, parameters);
    }
    public void ShowUI(UIBaseData data)
    {
        OnShow(data);
    }
    public void CloseUI()
    {
        OnClose();
    }
}

public abstract class UIBaseData
{

}
