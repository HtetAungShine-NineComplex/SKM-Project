using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _uiRoot;

    private Dictionary<string, UiBase> m_uis = new Dictionary<string, UiBase>();
    private UiBase m_activeUi;
    [SerializeField]
    private GameObject m_showExit;
    public void Initialize()
    {
        UiBase[] uis = _uiRoot.GetComponentsInChildren<UiBase>();

        Debug.Log(uis.Length);
        for (int i = 0; i < uis.Length; i++)
        {
            UiBase ui = uis[i];
            ui.OnInitialize();
            m_uis.Add(ui.UIName, ui);
        }
        CloseAllUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_showExit.SetActive(!m_showExit.activeSelf);
        }
        if (m_activeUi == null) return;
        m_activeUi.UpdateUI();
    }

    public Transform GetUIRoot()
    {
        return _uiRoot;
    }

    public void ShowUI(UIs ui, bool showOverlay = false, UIBaseData data = null)
    {
        string uiName = ui.ToString();

        UiBase nextUI = GetUI(uiName);
        if (nextUI == null) return;

        if (!showOverlay)
        {
            CloseAllUI();
            m_activeUi = nextUI;
            m_activeUi.ShowUI(data);
            return;
        }
        nextUI.ShowUI(data);

    }

    public void CloseAllUI()
    {
        for (int i = 0; i < m_uis.Count; i++)
        {
            m_uis.ElementAt(i).Value.CloseUI();
        }
    }

    private UiBase GetUI(string uiName)
    {
        UiBase ui = m_uis.FirstOrDefault(u => u.Key == uiName).Value;
        if (ui == null)
        {
            Debug.LogError(string.Format("Cannot find ui {0}", uiName));
            return null;
        }
        return ui;
    }

    public void ShowExit()
    {
        m_showExit.SetActive(!m_showExit.activeSelf);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
