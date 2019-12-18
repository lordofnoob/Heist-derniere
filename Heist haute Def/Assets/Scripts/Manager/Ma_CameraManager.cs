using UnityEngine;

public class Ma_CameraManager : MonoBehaviour 
{

    public static Ma_CameraManager Instance;

    [Header("Input Controller")]
    public Mb_InputController InputController;

    [Header("Camera Settings")]
    public Camera mainCam;
    public float panSpeed = 20f;
    private float minPanLimitX, maxPanLimitX, minZlimit, maxZlimit, minHeight=10, maxHeight=50;
    public Transform minXZ, maxXZ;
	public float scrollSpeed = 15f;
    private Vector3 targetPos;



    [Header("Camera Follow Settings")]
	public bool hasTarget = false;
	[SerializeField] Vector3 targetOffset = new Vector3(4f,7f,0f);
	public Transform targetFollow;
	public float targetSpeed;

    [Header("Scrolling Cam Mouse")]
    [Range(0.1f,0.4f)] [SerializeField] float scrollingPercentage=.2f;
    [SerializeField] AnimationCurve scrollingGrowth;
    float screenWidth;
    float screenHeight;

    float mouseX;
    float mouseY;



    private void Awake()
    {
      
        Instance = this;
        mainCam = FindObjectOfType<Camera>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        // transform.position = new Vector3(0f, 30f, 0f);
        targetPos = mainCam.transform.position;
        minPanLimitX = minXZ.position.x;
        minZlimit = minXZ.position.z;
        maxPanLimitX = maxXZ.position.x;
        maxZlimit = maxXZ.position.z;
    }

    void Update () {

        mouseX = InputController.mousePosition.x;
        mouseY  = InputController.mousePosition.y;
        mainCam.transform.position = Vector3.LerpUnclamped(mainCam.transform.position, targetPos, 0.1f);


        KeyBoardControl();
        ScrollingUpdate();
        ClavierUpdate();
        ScrollingMouse();
       

    }

  
    public void ClavierUpdate()
    {
        if (InputController.Z)
        {
            hasTarget = false;
            targetPos.z += panSpeed * Time.deltaTime;
        }
        if (InputController.Q)
        {
            hasTarget = false;
            targetPos.x -= panSpeed * Time.deltaTime;
        }
        if (InputController.S)
        {
            hasTarget = false;
            targetPos.z -= panSpeed * Time.deltaTime;
        }
        if (InputController.D)
        {
            hasTarget = false;
            targetPos.x += panSpeed * Time.deltaTime;
        }
    }

    public void ScrollingUpdate()
    {
        float scroll = InputController.scroll;
        if (!hasTarget)
        {
            targetPos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;
        }
        else if (hasTarget)
        {
            targetOffset.y -= scroll * scrollSpeed * 100 * Time.deltaTime;
            targetOffset.x += -scroll * (targetOffset.y / 1.75f);
            targetOffset.y = Mathf.Clamp(targetOffset.y, minHeight, maxHeight);
            targetOffset.x = Mathf.Clamp(targetOffset.x, targetOffset.y / 1.75f, targetOffset.y / 1.75f);
        }

        targetPos.x = Mathf.Clamp(targetPos.x, minPanLimitX, maxPanLimitX);
        targetPos.y = Mathf.Clamp(targetPos.y, minHeight, maxHeight);
        targetPos.z = Mathf.Clamp(targetPos.z, minZlimit, maxZlimit);
        ScrollingMouse();

        if (!hasTarget)
        {
            mainCam.transform.position = targetPos;
        }
        /*
        else	if(hasTarget){
			FollowTarget();
		}*/
    }

    public void SetTarget(Transform target){
		hasTarget = true;
		targetFollow = target;
	}

	public void TargetLooking(Vector3 positionToLook)
    {
		targetPos = new Vector3(positionToLook.x, 0, positionToLook.z) + targetOffset;
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*targetSpeed);

	}

    private void ScrollingMouse()
    {
        

        if (mouseX < screenWidth*scrollingPercentage && mainCam.transform.position.x > minXZ.position.x)
        {
            targetPos.x += (mouseX - screenWidth * scrollingPercentage) * Time.deltaTime* scrollingGrowth.Evaluate((screenWidth * scrollingPercentage- mouseX)/ screenWidth * scrollingPercentage);
        }
        else if (mouseX > screenWidth * (1-scrollingPercentage) && mainCam.transform.position.x < maxXZ.position.x)
        {
            targetPos.x += (mouseX - screenWidth * (1 - scrollingPercentage)) * Time.deltaTime * scrollingGrowth.Evaluate((mouseX - screenWidth * (1 - scrollingPercentage) )/ screenWidth * scrollingPercentage);
    
        }

        if (mouseY < screenHeight * scrollingPercentage && mainCam.transform.position.z > minXZ.position.z)
        {
            targetPos.z += (mouseY - screenHeight * scrollingPercentage) * Time.deltaTime * scrollingGrowth.Evaluate((screenHeight * scrollingPercentage - mouseY) / screenHeight * scrollingPercentage);
        }
        else if (mouseY > screenHeight * (1 - scrollingPercentage) && mainCam.transform.position.z < maxXZ.position.z)
        {
            targetPos.z += (mouseY - screenHeight * (1 - scrollingPercentage)) * Time.deltaTime * scrollingGrowth.Evaluate((mouseY - screenHeight * (1 - scrollingPercentage)) / screenHeight * scrollingPercentage);
        }

    }

    public void CenterCameraOnCharacter(int playerToFocus)
    {
        
        TargetLooking(Ma_PlayerManager.instance.selectedPlayer.transform.position);
    }

    void KeyBoardControl()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CenterCameraOnCharacter(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            CenterCameraOnCharacter(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            CenterCameraOnCharacter(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            CenterCameraOnCharacter(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            CenterCameraOnCharacter(4);
    }
}

