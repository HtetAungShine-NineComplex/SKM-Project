﻿using TMPro;
using UnityEngine.UI;

/**
 * Script attached to Warning Panel prefab.
 */
public class WarningPanel : BasePanel
{
	public TMP_Text warningText;

	/**
	 * Show panel instance with a warning message.
	 */
	public void Show(string warningMsg)
	{
		warningText.text = warningMsg;
		this.gameObject.SetActive(true);
	}
}
