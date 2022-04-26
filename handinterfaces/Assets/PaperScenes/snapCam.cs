using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class snapCam : MonoBehaviour
{
    Camera snapcam;
    AudioSource shutterSound;

    private int width;
    private int height;
    [SerializeField]
    private bool withHand = false;
    private string filename;

    // private GameObject snapshotThumbnail;


    // Start is called before the first frame update
    void Awake()
    {
        snapcam = GetComponent<Camera>();
        if (snapcam.targetTexture != null){
            width = snapcam.targetTexture.width;
            height = snapcam.targetTexture.height;
        }
    }
    void Start()
    {
        // snapshotThumbnail = GameObject.Find("snapshotThumbnail");
        shutterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("k")){
            shutterSound.Play();
            TakeSnapshot();
        }
    }

    void TakeSnapshot(){
        Texture2D snapshot = new Texture2D(width, height, TextureFormat.RGB24, true);
        snapcam.Render();
        RenderTexture.active = snapcam.targetTexture;
        snapshot.ReadPixels(new Rect(0,0,width,height),0,0);
        // Sprite sprite = null;
        snapshot.Apply(true);
        // snapshotThumbnail.GetComponent<RawImage>().texture = snapshot;
        // sprite = Sprite.Create(snapshot, new Rect(0, 0, snapshot.width, snapshot.height), new Vector2(0.5f, 0.5f));
        // if (sprite != null)
        // {
        //     snapshotThumbnail.GetComponent<SpriteRenderer>().sprite = sprite;
        // }
        byte[] bytes = snapshot.EncodeToPNG();
        if (!withHand){
            filename = string.Format("{0}/Teaser_screenshots/nohand_{1}x{2}_{3}.png", 
                              Application.dataPath, 
                              width, height, 
                              System.DateTime.Now.ToString("dd_HH-mm-ss"));
        } else {
            filename = string.Format("{0}/Teaser_screenshots_with_hands/withhand_{1}x{2}_{3}.png", 
                              Application.dataPath, 
                              width, height, 
                              System.DateTime.Now.ToString("dd_HH-mm-ss"));
        }
        
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }
}
