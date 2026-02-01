using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    private bool cameraPause;
    public float cutsceneValue;
    private float cutsceneThreshold = 60;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (cutsceneValue > cutsceneThreshold)
        {
            PlayCutscene();
        }
        else
        {
            cutsceneValue += Time.deltaTime;
        }
    }

    void PlayCutscene()
    {
        //stop the player from moving the camera
        Camera camera = Camera.main;
        CameraMovement cameraMovement = camera.gameObject.GetComponent<CameraMovement>();
        cameraMovement.allowMove = false;

        //move camera
        camera.transform.rotation = Quaternion.LookRotation(Vector3.back);

    }
}
