using UnityEngine;

public class Ma_CameraManager : MonoBehaviour 
{

    public static Ma_CameraManager Instance;

    [Header("Input Controller")]
    public Mb_InputController InputController;

    [Header("Camera Settings")]
    public Camera mainCam;
    public float panSpeed = 20f;
	public float minPanLimitX, maxPanLimitX, minusPanLimitY, maxPanLimitY, minHeight, maxHeight;
	public float scrollSpeed = 15f;
    private Vector3 pos;

    [Header("Camera Follow Settings")]
	public bool hasTarget = false;
	[SerializeField] Vector3 targetOffset = new Vector3(4f,7f,0f);
	public Transform targetFollow;
	public float targetSpeed;

    [Header("Scrolling Cam Mouse")]
    [SerializeField] float scrollingPercentage=.3f;
    [SerializeField] AnimationCurve scrollingGrowth;
    float screenWidth;
    float screenHeight;
    

    private void Awake()
    {
        Instance = this;
        mainCam = FindObjectOfType<Camera>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        transform.position = new Vector3(0f, 30f, 0f);
    }

    void Update () {

		pos = mainCam.transform.localPosition;

        //camera au clavier
        #region
        if (InputController.Z){
            hasTarget = false;
            pos.z += panSpeed * Time.deltaTime;
		}
		if(InputController.Q){
            hasTarget = false;
            pos.x -= panSpeed * Time.deltaTime;
		}
		if(InputController.S){
            hasTarget = false;
            pos.z -= panSpeed * Time.deltaTime;
		}
		if(InputController.D){
            hasTarget = false;
            pos.x += panSpeed * Time.deltaTime;
		}
        #endregion

        float scroll = InputController.scroll;
		if(!hasTarget){
			pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;
		}else if(hasTarget){
			targetOffset.y -= scroll * scrollSpeed * 100 * Time.deltaTime;
			targetOffset.x += -scroll * (targetOffset.y/1.75f);
			targetOffset.y = Mathf.Clamp(targetOffset.y, minHeight, maxHeight);
			targetOffset.x = Mathf.Clamp(targetOffset.x, targetOffset.y/1.75f, targetOffset.y/1.75f);

		}

		pos.x = Mathf.Clamp(pos.x, minPanLimitX, maxPanLimitX);
		pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
		pos.z = Mathf.Clamp(pos.z, minusPanLimitY, maxPanLimitY);
        ScrollingMouse();

        if (!hasTarget){
			mainCam.transform.position = pos;
		}else	if(hasTarget){
			FollowTarget();
		}
	}

	public void SetTarget(Transform target){
		hasTarget = true;
		targetFollow = target;
	}

	private void FollowTarget(){
		Vector3 targetPos = new Vector3(targetFollow.position.x, 0, targetFollow.position.z) + targetOffset;
		transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*targetSpeed);
	}

    private void ScrollingMouse()
    {
        float mouseX = InputController.mousePosition.x;
        float mouseY = InputController.mousePosition.y;

        if (mouseX < screenWidth*scrollingPercentage)
        {
            pos.x += (mouseX - screenWidth * scrollingPercentage) * Time.deltaTime* scrollingGrowth.Evaluate((screenWidth * scrollingPercentage- mouseX)/ screenWidth * scrollingPercentage);
        }
        else if (mouseX > screenWidth * (1-scrollingPercentage))
        {
             pos.x += (mouseX - screenWidth * (1 - scrollingPercentage)) * Time.deltaTime * scrollingGrowth.Evaluate((mouseX - screenWidth * (1 - scrollingPercentage) )/ screenWidth * scrollingPercentage);
    
        }

        if (mouseY < screenHeight * scrollingPercentage)
        {
            pos.z += (mouseY - screenHeight * scrollingPercentage) * Time.deltaTime * scrollingGrowth.Evaluate((screenHeight * scrollingPercentage - mouseY) / screenHeight * scrollingPercentage);
        }
        else if (mouseY > screenHeight * (1 - scrollingPercentage))
        {
            pos.z += (mouseY - screenHeight * (1 - scrollingPercentage)) * Time.deltaTime * scrollingGrowth.Evaluate((mouseY - screenHeight * (1 - scrollingPercentage)) / screenHeight * scrollingPercentage);
        }

    }
        
}

