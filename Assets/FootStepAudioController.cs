using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootStepAudioController : MonoBehaviour
{

    [HideInInspector]
    public AudioClip[] footstepsAudio; //This is the audio we are passing to the player movement function

    public GameObject playerObject; // This needs to be set to player so it can find movement script.




    //These are to store different sounds depending on material type.
    public AudioClip[] groundSounds;
    public AudioClip[] tunnelSounds;
    public AudioClip[] woodSounds;
    public AudioClip[] carpetSounds;
    public AudioClip[] metalSounds;
    // Start is called before the first frame update
    void Start()
    {
        playerObject.GetComponent<PlayerMovement>().footstepsAudio = groundSounds; //Default ground sounds to start
        
    }

    // Update is called once per frame
    void Update()
    {
        //These checks will grab the player raycasts, and then identify the type of ground the player is standing on. It will then change the audio of footsteps to match the ground type.
        if (playerObject.GetComponent<PlayerMovement>().groundType == "Ground") //Default ground
        {
            playerObject.GetComponent<PlayerMovement>().footstepsAudio = groundSounds;
        }
        else if (playerObject.GetComponent<PlayerMovement>().groundType == "Concrete")
        { //echoey, concrete sounds for large interior buildings.

            playerObject.GetComponent<PlayerMovement>().footstepsAudio = tunnelSounds;
        }
        else if (playerObject.GetComponent<PlayerMovement>().groundType == "Wood")
        { //For wooden floors and wooden beams, etc

            playerObject.GetComponent<PlayerMovement>().footstepsAudio = woodSounds;
        }
        else if (playerObject.GetComponent<PlayerMovement>().groundType == "Carpet")
        { //For carpeted floors

            playerObject.GetComponent<PlayerMovement>().footstepsAudio = carpetSounds;
        }
        else if (playerObject.GetComponent<PlayerMovement>().groundType == "Metal")
        { //For metal floors and walkways
            playerObject.GetComponent<PlayerMovement>().footstepsAudio = metalSounds;

        }

        else
        { //Default catch-all in case there are issues with material tags.
            playerObject.GetComponent<PlayerMovement>().footstepsAudio = groundSounds;
        }

    }
}
