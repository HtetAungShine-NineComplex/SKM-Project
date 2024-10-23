using System;
using UnityEngine;
using UnityEngine.UI;
using Sfs2X.Entities;
using TMPro;

/**
 * Script attached to Game List Item prefab.
 */
public class GameListItem : MonoBehaviour
{
	public Button joinBtn;
	public TMP_Text nameText;
	public TMP_Text totalPlayerTxt;

	public int roomId;

	/**
	 * Initialize the prefab instance.
	 */
	public void Init(Room room)
	{
		//nameText.text = room.Name;
		roomId = room.Id;

		SetState(room);
	}

	/**
	 * Update prefab instance based on the corresponding Room state.
	 */
	public void SetState(Room room)
	{
		int playerSlots = room.MaxUsers - room.UserCount;

		// Set player count and spectator count in game list item
		totalPlayerTxt.text = $"{room.UserCount} / 8";

		// Enable/disable game play button
		joinBtn.interactable = playerSlots > 0;
	}
}
