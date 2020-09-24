using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
	public bool showController = false;//decides what to show in the hand presence array
	public InputDeviceCharacteristics controllerCharacteristics;
	public List<GameObject> controllerPrefabs;
	public GameObject handModelPrefab;

	private InputDevice targetDevice;
	private GameObject spawnedController;
	private GameObject spawnedHandModel;
	private Animator handAnimator;

	// Start is called before the first frame update
	void Start()
	{
		TryInitialize();//in case the controller isn't already stated we can update and initialize the controler later
	}

	void TryInitialize()
	{
		List<InputDevice> devices = new List<InputDevice>();
		InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);//gets devices

		foreach (var item in devices)//displays all devices
		{
			UnityEngine.Debug.Log(item.name + item.characteristics);
		}

		if (devices.Count > 0)
		{
			targetDevice = devices[0];
			GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);//finds the name of the prefab in the prefabs list from unity
			if (prefab)//if we found the prefab correctly
			{
				spawnedController = Instantiate(prefab, transform);
			}
			else
			{
				UnityEngine.Debug.LogError("Error");
				spawnedController = Instantiate(controllerPrefabs[0], transform);//makes it the first prefab on list
			}

			spawnedHandModel = Instantiate(handModelPrefab, transform);
			handAnimator = spawnedHandModel.GetComponent<Animator>();
		}
	}

	//this function has every button press and what action that button press should do
	void UpdateHandAnimation()
	{
		if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))//trigger is pressed
		{
			handAnimator.SetFloat("Trigger", triggerValue);
		}
		else//trigger not pressed
		{
			handAnimator.SetFloat("Trigger", 0);
		}
		if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))//grip is pressed
		{
			handAnimator.SetFloat("Grip", gripValue);
		}
		else//grip not pressed
		{
			handAnimator.SetFloat("Grip", 0);
		}
		//shows on debug console when primary button is pressed
		/* targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
		 if (primaryButtonValue)
			 UnityEngine.Debug.Log("Primary button pressed");*/
	}

	// Update is called once per frame
	void Update()
	{
		if (!targetDevice.isValid)
		{
			TryInitialize();
		}
		else
		{
			if (showController)
			{
				spawnedHandModel.SetActive(false);
				spawnedController.SetActive(true);
			}
			else
			{
				spawnedHandModel.SetActive(true);
				spawnedController.SetActive(false);
				UpdateHandAnimation();
			}
		}
	}
}
