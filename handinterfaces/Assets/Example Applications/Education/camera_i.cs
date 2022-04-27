using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class camera_i : MonoBehaviour
{
    Camera snapcam;
    AudioSource shutterSound;

    private int width;
    private int height;
    private bool isFar_last = true;
    private bool isFar_current = true;
    [SerializeField]
    private GameObject handToTrackMovement;
    private GameObject trackpoint;
    private GameObject trackpoint2;
    private GameObject trackpoint3;
    [SerializeField]
    private GestureRetrieval_Education GD;
    private GameObject snapshotThumbnail;
    [SerializeField]
    private float distanceThreshold = 0.02f; 

#region Oculus Types

    private OVRHand ovrHand;

    private OVRSkeleton ovrSkeleton;
    private OVRSkeleton theOtherovrSkeleton;

    private OVRBone boneToTrack;
    private OVRBone boneToTrack2;
    private OVRBone boneToTrack3;
#endregion
    // Start is called before the first frame update
    void Awake()
    {
        snapcam = GetComponent<Camera>();
        if (snapcam.targetTexture != null){
            width = snapcam.targetTexture.width;
            height = snapcam.targetTexture.height;
        }
        ovrHand = handToTrackMovement.GetComponent<OVRHand>();
        ovrSkeleton = handToTrackMovement.GetComponent<OVRSkeleton>();
        // theOtherovrSkeleton = theOtherhandToTrackMovement.GetComponent<OVRSkeleton>();
        // get initial bone to track
        boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                .SingleOrDefault();
        
        boneToTrack2 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();

        boneToTrack3 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
                .SingleOrDefault();

    }
    void Start()
    {
        snapshotThumbnail = GameObject.Find("snapshotThumbnail");
        shutterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boneToTrack == null)
        {
            boneToTrack = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                .SingleOrDefault();
            if (boneToTrack != null)
                trackpoint = boneToTrack.Transform.gameObject;
        }
        if (boneToTrack2 == null)
        {
            boneToTrack2 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Thumb3)
                .SingleOrDefault();
            if (boneToTrack2 != null)
                trackpoint2 = boneToTrack2.Transform.gameObject;
        }
        if (boneToTrack3 == null)
        {
            boneToTrack3 = ovrSkeleton.Bones
                .Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
                .SingleOrDefault();
            if (boneToTrack3 != null)
                trackpoint3 = boneToTrack3.Transform.gameObject;
        }

        bool shutterPressed = CameraInteractionDetection();
        // Debug.Log("test: " + test);

        if (GD.currentGesture_stable.name == "cameraHD" 
        && shutterPressed){
            shutterSound.Play();
            TakeSnapshot();
        }
    }

    private float DistanceCalculator(GameObject thumbtip,GameObject index1)
    {
        float distance = 0;
        
        distance = Vector3.Distance(thumbtip.transform.position,index1.transform.position);
        //Debug.Log("distance is " + distance);
        
        return distance;
    }

    private bool CameraInteractionDetection()
    {
        bool shutterOn = false;
        float h2h_distance = DistanceCalculator(trackpoint2, trackpoint3);
        isFar_current = (h2h_distance > distanceThreshold); //false if in the proximity
        if (isFar_current && !isFar_last){
            shutterOn = true;
        }                
        isFar_last = isFar_current;
        // Debug.Log("shutterOn: "+shutterOn+"; distance: "+h2h_distance);
        return shutterOn;
    }
    void TakeSnapshot(){
        Texture2D snapshot = new Texture2D(width, height, TextureFormat.RGB24, true);
        snapcam.Render();
        RenderTexture.active = snapcam.targetTexture;
        snapshot.ReadPixels(new Rect(0,0,width,height),0,0);
        // Sprite sprite = null;
        snapshot.Apply(true);
        snapshotThumbnail.GetComponent<RawImage>().texture = snapshot;
        // sprite = Sprite.Create(snapshot, new Rect(0, 0, snapshot.width, snapshot.height), new Vector2(0.5f, 0.5f));
        // if (sprite != null)
        // {
        //     snapshotThumbnail.GetComponent<SpriteRenderer>().sprite = sprite;
        // }
        byte[] bytes = snapshot.EncodeToPNG();
        string filename = string.Format("{0}/Education_screenshots/screen_{1}x{2}_{3}.png", 
                              Application.dataPath, 
                              width, height, 
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }
}
