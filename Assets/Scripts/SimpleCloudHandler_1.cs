﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System;

public class SimpleCloudHandler_1 : MonoBehaviour, ICloudRecoEventHandler
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    //public GameObject Cleaner;
    //public ObjectTracker mImageTracker;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";
    public ImageTargetBehaviour ImageTargetTemplate;
    // Use this for initialization
    void Start()
    {
        // register this event handler at the cloud reco behaviour
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

        if (mCloudRecoBehaviour)
        {
            mCloudRecoBehaviour.RegisterEventHandler(this);
        }
    }

    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.TargetFinder.ClearTrackables(false);
        }
    }

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        // do something with the target metadata
        mTargetMetadata = targetSearchResult.MetaData;
        // stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;
        if (ImageTargetTemplate)
        {
            // enable the new result with the same ImageTargetBehaviour:
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            ImageTargetBehaviour imageTargetBehaviour =
                (ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(
                    targetSearchResult, ImageTargetTemplate.gameObject);
        }
    }

    // void OnGUI()
    // {
    //     // Display current 'scanning' status
    //     GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
    //     // Display metadata of latest detected cloud-target
    //     //GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
    //     // If not scanning, show button
    //     // so that user can restart cloud scanning
    //     if (!mIsScanning)
    //     {
    //         if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
    //         {
    //             // Restart TargetFinder
    //             mCloudRecoBehaviour.CloudRecoEnabled = true;
    //         }
    //     }
    // }
}

