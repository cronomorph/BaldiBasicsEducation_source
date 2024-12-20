using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
	public CursorControllerScript cursorController;
	public PlayerScript player;
	public Transform playerTransform;
	public Transform cameraTransform;
	public EntranceScript entrance_0;
	public EntranceScript entrance_1;
	public EntranceScript entrance_2;
	public EntranceScript entrance_3;
	public GameObject baldiTutor;
	public GameObject baldi;
	public BaldiScript baldiScrpt;
	public AudioClip aud_Prize;
	public AudioClip aud_AllNotebooks;
	public GameObject principal;
	public GameObject crafters;
	public GameObject playtime;
	public PlaytimeScript playtimeScript;
	public GameObject gottaSweep;
	public GameObject bully;
 	private Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	private RaycastHit raycastHit;
	public GameObject firstPrize;
	public FirstPrizeScript firstPrizeScript;
	public GameObject quarter;
	public AudioSource tutorBaldi;
	public string mode;
	public int notebooks;
	public GameObject[] notebookPickups;
	public int failedNotebooks;
	public float time;
	public bool spoopMode;
	public bool finaleMode;
	public bool debugMode;
	public bool mouseLocked;
	public int exitsReached;
	public int itemSelected = Mathf.Clamp(itemSelected, 0, 2);;
	public int[] item = new int[3];
	public RawImage[] itemSlot = new RawImage[3];
	private string[] itemNames = new string[10]
	{
		"Nothing",
		"Energy flavored Zesty Bar",
		"Yellow Door Lock",
		"Principal's Keys",
		"BSODA",
		"Quarter",
		"Baldi Anti Hearing and Disorienting Tape",
		"Alarm Clock",
		"WD-NoSquee (Door Type)",
		"Safety Scissors"
	};
	public Text itemText;
	public Object[] items = new Object[10];
	public Texture[] itemTextures = new Texture[10];
	public GameObject bsodaSpray;
	public GameObject alarmClock;
	public Text notebookCount;
	public GameObject pauseText;
	public GameObject highScoreText;
	public GameObject baldiNod;
	public GameObject baldiShake;
	public GameObject warning;
	public GameObject reticle;
	public RectTransform itemSelect;
	private int[] itemSelectOffset = new int[3]
	{
		-80,
		-40,
		0
	};
	private bool gamePaused;
	private bool learningActive;
	private float gameOverDelay;
	private AudioSource audioDevice;
	public AudioClip aud_Soda;
	public AudioClip aud_Spray;
	public AudioClip aud_buzz;
	public AudioClip aud_Hang;
	public AudioClip aud_MachineQuiet;
	public AudioClip aud_MachineStart;
	public AudioClip aud_MachineRev;
	public AudioClip aud_MachineLoop;
	public AudioClip aud_Switch;
	public AudioSource schoolMusic;
	public AudioSource learnMusic;
 
	private void Start()
	{
		audioDevice = base.GetComponent<AudioSource>();
		mode = PlayerPrefs.GetString("CurrentMode");
		if (mode == "endless")
		{
			baldiScrpt.endless = true;
		}
		schoolMusic.Play();
		cursorController.LockMouse();
		UpdateNotebookCount();
		itemSelected = 0;
		gameOverDelay = 0.5f;
	}

	private void Update()
	{
		if (!learningActive)
		{
			if (Input.GetButtonDown("Pause"))
			{
				if (!gamePaused)
				{
					PauseGame();
				}
				else
				{
					UnpauseGame();
				}
			}
   			if (gamePaused) {
      				if (Input.GetKeyDown(KeyCode.Y)) {
	  				SceneManager.LoadScene("MainMenu");
      				}
	  			if (Input.GetKeyDown(KeyCode.N)) {
      					UnpauseGame();
      				}
      	
      			}
  			else if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
			if (Input.GetMouseButtonDown(1))
			{
				UseItem();
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f)
			{
				itemSelected--;
				itemSelect.anchoredPosition = new Vector3((float)itemSelectOffset[itemSelected], 0f, 0f);
				UpdateItemName();
			}
			else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
			{
				itemSelected++;
				itemSelect.anchoredPosition = new Vector3((float)itemSelectOffset[itemSelected], 0f, 0f);
				UpdateItemName();
			}
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				itemSelected = 0;
				UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				itemSelected = 1;
				UpdateItemSelection();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				itemSelected = 2;
				UpdateItemSelection();
			}
		}
  		if (player.stamina < 0f) {
    			warning.SetActive(true);
    		}
      		else {
			warning.SetActive(false;)
		}
		if (player.gameOver)
		{
			Time.timeScale = 0f;
			gameOverDelay -= Time.unscaledDeltaTime;
			audioDevice.PlayOneShot(aud_buzz);
			if (gameOverDelay <= 0f)
			{
				if (mode == "endless")
				{
					if (notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", notebooks);
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(time));
						highScoreText.SetActive(true);
					}
					else if (notebooks == PlayerPrefs.GetInt("HighBooks") & Mathf.FloorToInt(time) > PlayerPrefs.GetInt("HighTime"))
					{
						PlayerPrefs.SetInt("HighTime", Mathf.FloorToInt(time));
						highScoreText.SetActive(true);
					}
					PlayerPrefs.SetInt("CurrentBooks", notebooks);
					PlayerPrefs.SetInt("CurrentTime", Mathf.FloorToInt(time));
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene("GameOver");
			}
		}
		if (finaleMode && !audioDevice.isPlaying && exitsReached == 3)
		{
			audioDevice.clip = aud_MachineLoop;
			audioDevice.loop = true;
			audioDevice.Play();
		}
		time += Time.deltaTime;
	}

	private void UpdateNotebookCount()
	{
		if (mode == "story")
		{
			notebookCount.text = notebooks.ToString() + "/7 Notebooks";
   			if (notebooks == 7) {
      				ActivateFinalMode();
	  			audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
      			}
		}
		else
		{
			notebookCount.text = notebooks.ToString() + " Notebooks";
		}
	}

	public void CollectNotebook()
	{
		notebooks++;
		UpdateNotebookCount();
		time = 0f;
	}

	private void PauseGame()
	{
		Time.timeScale = 0f;
		gamePaused = true;
		pauseText.SetActive(true);
		baldiNod.SetActive(true);
		baldiShake.SetActive(true);
	}

	private void UnpauseGame()
	{
		Time.timeScale = 1f;
		gamePaused = false;
		pauseText.SetActive(false);
		baldiNod.SetActive(false);
		baldiShake.SetActive(false);
	}

	public void ActivateSpoopMode()
	{
		spoopMode = true;
		entrance_0.Lower();
		entrance_1.Lower();
		entrance_2.Lower();
		entrance_3.Lower();
		baldiTutor.SetActive(false);
		baldi.SetActive(true);
		principal.SetActive(true);
		crafters.SetActive(true);
		playtime.SetActive(true);
		gottaSweep.SetActive(true);
		bully.SetActive(true);
		firstPrize.SetActive(true);
		audioDevice.PlayOneShot(aud_Hang);
		learnMusic.Stop();
		schoolMusic.Stop();
	}

	private void ActivateFinaleMode()
	{
		finaleMode = true;
		entrance_0.Raise();
		entrance_1.Raise();
		entrance_2.Raise();
		entrance_3.Raise();
	}

	public void GetAngry(float value)
	{
		if (!spoopMode)
		{
			ActivateSpoopMode();
		}
		baldiScrpt.GetAngry(value);
	}

	public void ActivateLearningGame()
	{
		learningActive = true;
		cursorController.UnlockMouse();
		tutorBaldi.Stop();
		if (!spoopMode)
		{
			schoolMusic.Stop();
			learnMusic.Play();
		}
	}

	public void DeactivateLearningGame(GameObject subject)
	{
		learningActive = false;
		Object.Destroy(subject);
		cursorController.LockMouse();
		if (player.stamina != 100f) {
  			player.stamina = 100f;
  		}
  		if (!spoopMode) {
			schoolMusic.Play();
			learnMusic.Stop();
   			if (notebooks == 1) {
      				quarter.SetActive(true);
	  			tutorBaldi.PlayOneShot(aud_Prize);
      			}
    		}
	}

	private void UpdateItemSelection()
	{
		itemSelect.anchoredPosition = new Vector3((float)itemSelectOffset[itemSelected], 0f, 0f);
		UpdateItemName();
	}

	public void CollectItem(int item_ID)
	{
		if (item[0] == 0)
		{
			item[0] = item_ID;
			itemSlot[0].texture = itemTextures[item_ID];
		}
		else if (item[1] == 0)
		{
			item[1] = item_ID;
			itemSlot[1].texture = itemTextures[item_ID];
		}
		else if (item[2] == 0)
		{
			item[2] = item_ID;
			itemSlot[2].texture = itemTextures[item_ID];
		}
		else
		{
			item[itemSelected] = item_ID;
			itemSlot[itemSelected].texture = itemTextures[item_ID];
		}
		UpdateItemName();
	}

	private void UseItem()
	{
		if (item[itemSelected] != 0)
		{
			switch (item[itemSelected]) {
				case 1:
    				player.stamina = player.maxStamina * 2f;
    				break;

 				case 2:
     				if (Physics.Raycast(ray, out raycastHit) && (raycastHit.collider.tag == "SwingingDoor" & Vector3.Distance(playerTransform.position, raycastHit.transform.position) <= 10f))	{
	 				raycastHit.collider.gameObject.GetComponent<SwingingDoorScript>.LockDoor(15f);
      					ResetItem();
				}
     				break;

  				case 3:
      				if (Physics.Raycast(ray, out raycastHit2) && (raycastHit.collider.tag == "Door" & Vector3.Distance(playerTransform.position, raycastHit.transform.position) <= 10f)) {
	  				raycastHit.collider.gameObject.GetComponent<DoorScript>().UnlockDoor();
       					raycastHit.collider.gameObject.GetComponent<DoorScript>().OpenDoor();
	    				ResetItem();
	  			}
      				break;
	  			case 4:
      				Object.Instantiate(bsodaSpray, playerTransform.position, cameraTransform.rotation);
	  			ResetItem();
      				player.ResetGuilt("drink", 1f);
	  			audioDevice.PlayOneShot(aud_Soda);
      				break;
	  			case 5:
      				if (Physics.Raycast(ray, out raycastHit)) {
	  				if (Vector3.Distance(playerTransform.position, raycastHit.transform.position) <= 10f) {
						switch (raycastHit.collider.name) {
							case "BSODAMachine":
       							ResetItem();
	      						CollectItem(4);
      							break;
	     						case "ZestyMachine":
	    						ResetItem();
	   						CollectItem(1);
	    						break;
	   						case "PayPhone":
	  						ResetItem();
	  						raycastHit.collider.gameObject.GetComponent<TapePlayerScript>().Play();
	  						break;
     						}
	  				}
   				}
      				break;
				case 6:
    				if (Physics.Raycast(ray, out raycastHit) {
					if (raycastHit.collider.name "TapePlayer") {
     						if (Vector3.Distance(playeTransform.position, raycastHit.transform.position) <= 10f) {
	   						raycastHit.collider.gameObject.GetComponent<TapePlayerScript>().Play();
	  						ResetItem();
	   					}
     					}
 				}
    				break;
				case 7:
    				GameObject gameobject = Object.Instantiate(alarmClock, playerTransform.position, cameraTransform.position);
				gameObject.GetComponent<AlarmClockScript>().baldi = baldiScrpt;
    				break;
				case 8:
    				if (Physics.Raycast(ray, out raycastHit)) {
					if (raycastHit.collider.tag == "Door") {
     						if (Vector3.Distance(playerTransform.position, raycastHit.transform.position) <= 10f) {
	   						raycastHit.collider.gameObject.GetComponent<DoorScript>.SilenceDoor();
	  						ResetItem();
	 						audioDevice.PlayOneShot(aud_Spray);
	   					}
     					}
				}
    				break;
				case 9:
    				if (player.jumpRope) {
					player.DeactivateJumpRope();
    					playtimeScript.Disappoint();
					ResetItem();
				}
    				if (Physics.Raycast(ray, out raycastHit)) {
					if (Vector3.Distance(playerTransform.position, raycastHit.transform.position) <= 10f) {
						if (raycastHit.collider.name == "1st Prize") {
      							firstPrizeScript.GoCrazy();
	     						ResetItem();
      						}
     					}
				}
    				break;
   			}
  
		}
	}

	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		UpdateItemName();
	}

	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		UpdateItemName();
	}

	private void UpdateItemName()
	{
		itemText.text = itemNames[item[itemSelected]];
	}

	public void ExitReached()
	{
		exitsReached++;
  		switch (exitsReached) {
    			case 1:
       			RenderSettings.ambientLight = Color.red;
			RenderSettings.fog = true;
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
			audioDevice.clip = aud_MachineQuiet;
			audioDevice.loop = true;
			audioDevice.Play();
   			break;
      			case 2:
	 		audioDevice.volume = 0.8f;
			audioDevice.clip = aud_MachineStart;
			audioDevice.loop = true;
			audioDevice.Play();
	 		break;
    			case 3:
       			audioDevice.clip = aud_MachineRev;
	  		audioDevice.loop = false;
     			audioDevice.Play();
       			break;
    		}
	}

	public void DespawnCrafters()
	{
		crafters.SetActive(false);
	}
}
