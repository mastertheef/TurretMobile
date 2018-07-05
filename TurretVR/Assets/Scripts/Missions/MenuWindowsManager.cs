using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindowsManager : MonoBehaviour {

	#region Fields

	[Header("Buttons")]
	public Button levelSelectToInventoryButton;
	public Button inventoryToShopButton;
	public Button shopToInventorySelectButton;
	public Button inventoryToLevelSelectButton;

	[Header("Windows")]
	public GameObject levelSelectWindow;
	public GameObject inventorySelectWindow;
	public GameObject shopSelectWindow;

	#endregion

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void clickOnLevelSelectToInventoryButton()
	{
		levelSelectWindow.SetActive(false);
		inventorySelectWindow.SetActive(true);
	}

	public void clickOnInventoryToShopButton()
	{
		inventorySelectWindow.SetActive(false);
		shopSelectWindow.SetActive(true);
	}

	public void clickOnShopToInventorySelectButton()
	{
		shopSelectWindow.SetActive(false);
		inventorySelectWindow.SetActive(true);
	}

	public void clickOnInventoryToLevelSelectButton()
	{
		inventorySelectWindow.SetActive(false);
		levelSelectWindow.SetActive(true);
	}
}
