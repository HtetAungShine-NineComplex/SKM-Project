using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : UiBase
{
    [SerializeField] private Button _lobbyBtn; //sample

    [Header("Avator Selection")]
    [SerializeField] private GameObject _UIAvatorSelection;
    [SerializeField] private Button _avatorSelectionBtn; //sample //srat
    [SerializeField] private Sprite _avatorMale;//srat
    [SerializeField] private Sprite _avatorFemale;//srat
    [SerializeField] private bool _choosedMale;//srat
    [SerializeField] private Button MaleBtn;//srat
    [SerializeField] private Button FemaleBtn;//srat

    [Header("Settings")]
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _CloseSettingBtn;
    [SerializeField] private GameObject _UIMainMenuSetting;
    public override void OnShow(UIBaseData data)
    {
        base.OnShow(data);

        _lobbyBtn.onClick.AddListener(ToLobby);
        _avatorSelectionBtn.onClick.AddListener(OpenAvatorSelection);//srat

        MaleBtn.onClick.AddListener(ChooseMale);
        FemaleBtn.onClick.AddListener(ChooseFemale);


        _settingBtn.onClick.AddListener(OpenMainMenuSetting);
        _CloseSettingBtn.onClick.AddListener(CloseMainMenuSetting);
        UpdateAvator();
    }

    private void UpdateAvator() //srat
    {
        if (_choosedMale)
        {
            //change male avator
            _avatorSelectionBtn.image.sprite =_avatorMale;
        }
        else
        {
            //change female avator
            _avatorSelectionBtn.image.sprite =_avatorFemale;
        }
    }

    private void ChooseMale()//srat
    {
        if (_UIAvatorSelection != null)
        {
            bool isActive = _UIAvatorSelection.activeSelf;
            //change avator
            ChooseAvator(true);
            UpdateAvator();
            _UIAvatorSelection.SetActive(!isActive);
            Debug.Log("closed avator selection panel!");
        }
    }

    private void ChooseFemale()//srat
    {
        if (_UIAvatorSelection != null)
        {
            bool isActive = _UIAvatorSelection.activeSelf;
            //change avator
            ChooseAvator(false);
            UpdateAvator() ;
            _UIAvatorSelection.SetActive(!isActive);
            Debug.Log("closed avator selection panel!");
        }
    }

    public void ChooseAvator(bool ismale)//srat
    {
        _choosedMale = ismale;
    }
    private void OpenAvatorSelection() //srat
    {
        if(_UIAvatorSelection!=null)
        {
            _UIAvatorSelection.SetActive(true);
            Debug.Log("opened avator selection panel!");
        }
    }
/////////////////////////////////////////////////////////////////////////////

    public void OpenMainMenuSetting()
    {
        if (_UIMainMenuSetting != null)
        {
            _UIMainMenuSetting.SetActive(true);
            Debug.Log("opened mainmenu setting panel!");
        }
    }

    public void CloseMainMenuSetting()
    {
        if (_UIMainMenuSetting != null)
        {
            _UIMainMenuSetting.SetActive(false);
            Debug.Log("closed mainmenu setting panel!");
        }
    }
    /// ///////////////////////////////////////////////////////////////////////////


    public override void OnClose()
    {
        base.OnClose();
        _lobbyBtn.onClick.RemoveAllListeners();
        _avatorSelectionBtn.onClick.RemoveAllListeners();//srat
    }

    private void ToLobby()
    {
        Managers.UIManager.ShowUI(UIs.UIRoom);
    }

}
