﻿using UnityEngine;
using System.Collections;

public class cscript_gun : MonoBehaviour {
	
	bool active = true;
	
	public int clipSize = 7;
	
	public float fireRate = 0.54f;
	public bool automatic = false;
	
	private float fireTimer = 0;

	public GameObject bloodPrefab;
	
	public Vector3 recoil = new Vector3(0, 2, 0);
	
	private bool canFire = true;
	
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canFire == false)
		{
			fireTimer -= Time.deltaTime;
			
			if (fireTimer <= 0)
				canFire = true;
		}
		else
		{
			if (automatic == true)
			{
				if (Input.GetMouseButton (0))
					Fire();
			}
			else
			{
				if (Input.GetMouseButtonDown(0))
					Fire();
			}
			
			if (Input.GetKeyDown (KeyCode.R))
			{
				if (GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().GetCurrentAmmo () <= clipSize)
					return;
				
				GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().SpendAmmo (clipSize);
				GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().SetClip (clipSize);
			}
		}
	}
	
	private void Fire()
	{
		Screen.lockCursor = true;
		
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().GetCurrentGunClip () > 0)
			GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().SpendClip (1);
		else
		{
			if (GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().GetCurrentAmmo() < clipSize)
				return;
			
			GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().SpendAmmo (clipSize);
			GameObject.FindGameObjectWithTag("Player").GetComponent<cscript_player>().SetClip (clipSize);
		}
		
		RaycastHit hit;
		gameObject.GetComponent<Animation>().Play ();
		
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 1));
		
        if (Physics.Raycast(ray, out hit))
		{
			switch (hit.collider.gameObject.name)
			{
				case "face":
					hit.collider.transform.parent.parent.GetComponent<c_script_zombie_brain>().Damage ("Head", 100);
					Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
					break;
				case "head":
					hit.collider.transform.parent.GetComponent<c_script_zombie_brain>().Damage ("Head", 100);
					Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
					break;
				case "arm":
					hit.collider.transform.parent.parent.GetComponent<c_script_zombie_brain>().Damage ("Arm", 10);
					Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
					break;
				case "leg":
					hit.collider.transform.parent.parent.GetComponent<c_script_zombie_brain>().Damage ("Leg", 20);
					Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
					break;
				case "body":
					hit.collider.transform.parent.GetComponent<c_script_zombie_brain>().Damage ("Body", 20);
					Instantiate(bloodPrefab, hit.transform.position, Quaternion.identity);
					break;
			}
		}
		
		Camera.main.GetComponent<MouseLook>().AddRecoil (recoil);
		//Camera.main.transform.parent.transform.GetComponent<MouseLook>().AddRecoil (recoil);
		
//		//Old
//		GameObject projectile = Instantiate (bullet, bulletSpawn.transform.position, Quaternion.identity) as GameObject;
//		projectile.rigidbody.velocity = transform.TransformDirection (Vector3.forward * 1000);
//		
//		canFire = false;
//		fireTimer = fireRate;
	}
	
	void OnGUI()
	{
		GUI.Label (new Rect(Screen.width / 2, Screen.height / 2, 12, 12), "O");
	}
}
