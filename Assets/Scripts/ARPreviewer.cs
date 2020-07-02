using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
//using UnityEngine.Experimental.XR;
using UnityEngine.UI;

public class ARPreviewer : MonoBehaviour
{
    //Vars
    private ARSessionOrigin sessionOrigin;
    private ARRaycastManager rcManager;
    private Pose previewPose;

    private int rotationOffset;

    public GameObject placeholder;
    public GameObject buddyPrefab;

    public Text debugText;

    //Slider
    public GameObject rotationSlider;
    public GameObject rotationIndicator;

    // Start is called before the first frame update
    void Start()
    {
        sessionOrigin = FindObjectOfType<ARSessionOrigin>();
        rcManager = sessionOrigin.GetComponent<ARRaycastManager>();
        placeholder.SetActive(false);

        //Set some shit
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
    }

    private void UpdatePlacementPose()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rcManager.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            previewPose = hits[0].pose;

            //Set placeholder
            placeholder.SetActive(true);
            Quaternion placeholderRotation = Quaternion.Euler(0, -rotationOffset, 0) * previewPose.rotation;
            placeholder.transform.SetPositionAndRotation(previewPose.position, placeholderRotation);

            //Set slider
            rotationSlider.SetActive(true);
            rotationSlider.transform.SetPositionAndRotation(previewPose.position, previewPose.rotation);
            Quaternion indicatorRotation = Quaternion.Euler(0, 0, rotationOffset);
            rotationIndicator.transform.localRotation = indicatorRotation;
        }
        else
        {
            placeholder.SetActive(false);
            rotationSlider.SetActive(false);
        }
    }

    public void PlaceBuddy()
    {
        GameObject obj = Instantiate(buddyPrefab, placeholder.transform.position, placeholder.transform.rotation);
    }

    public void AdjustRotation(int change) 
    {
        rotationOffset = Mathf.Clamp(rotationOffset + change, -180, 180);
    }

    public void AdjustRotationSlider()
    {
        Touch touch = Input.GetTouch(0);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rcManager.Raycast(touch.position, hits, TrackableType.Planes);
        Vector3 touchPoint = Camera.main.transform.InverseTransformPoint(hits[0].pose.position) * 100;
        Vector3 dir = (Camera.main.transform.InverseTransformPoint(rotationSlider.transform.position) * 100) - touchPoint;
        float angle = (Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg) - 90;
        angle = (angle <= 0) ? (angle + 360) : angle;
        if(angle > 90 && angle < 270)
        {
            angle = angle < 180 ? 90 : 270;
        }
        rotationOffset = Mathf.RoundToInt(angle);
        debugText.text = rotationOffset.ToString();
    }
}
