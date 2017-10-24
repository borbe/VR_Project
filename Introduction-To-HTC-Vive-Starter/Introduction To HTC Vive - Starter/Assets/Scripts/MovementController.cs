using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	// 1
	private SteamVR_TrackedObject trackedObj;
	// transform of [cameraRig]
	public Transform cameraRigTransform;
	// stores a reference to the teleport reticle prefab
	public GameObject teleportReticlePrefab;
	// a reference to an instance of the reticle
	private GameObject reticle;
	// stores a reference to the teleport reticle transform
	private Transform teleportReticleTransform;
	// reference to the players head
	public Transform headTransform;
	// reference to the players controller
	public Transform controllerTransform;
	// the reticle offset from the floor (no Z fighting with other surfaces)
	public Vector3 teleportReticleOffset;
	// layer mask to filter areas where teleporting is allowd
	public LayerMask teleportMask;
	// truw when a valid teleport location is found
	private bool shouldTeleport;
	// stores location of thumb on touchpad
	private Vector3 reticlePosition;
	// 2
	private SteamVR_Controller.Device Controller
	{
	    get { return SteamVR_Controller.Input((int)trackedObj.index); }
	}

	private void Teleport()
	{
	    // 1
	    shouldTeleport = false;
	    // 2
	    reticle.SetActive(false);
	    // 3
	    Vector3 difference = cameraRigTransform.position - headTransform.position;
	    // 4
	    difference.y = 0;
	    // 5
	    cameraRigTransform.position = new Vector3(reticlePosition.x, cameraRigTransform.position.y, reticlePosition.z) + difference;
	}

	void Awake() {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void Start() {
		// 1
		reticle = Instantiate(teleportReticlePrefab);
		// 2
		teleportReticleTransform = reticle.transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (Controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad)) {
			// 1
			reticle.SetActive(true);
			// // 3
			shouldTeleport = true;
			Debug.Log("Down"+gameObject.name + Controller.GetAxis());
		}
		if (Controller.GetAxis() != Vector2.zero && Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
		    Debug.Log(gameObject.name + Controller.GetAxis());
			// set reticle position to current head position
			Vector3 trackpadPos = new Vector3(Controller.GetAxis().x, 0, Controller.GetAxis().y);
			trackpadPos = trackpadPos * 2;
			Vector3 trackPadPosition = controllerTransform.rotation * trackpadPos;
			reticlePosition = headTransform.position + trackPadPosition;
			reticlePosition.y = teleportReticleOffset.y;
			teleportReticleTransform.position = reticlePosition;
		}
		if (Controller.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad)) {
			Teleport();
			Debug.Log("Up"+gameObject.name);

		}
	}
}
