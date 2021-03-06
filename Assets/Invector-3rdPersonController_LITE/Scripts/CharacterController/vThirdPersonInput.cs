using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode kickInput = KeyCode.Q;
        public KeyCode punchInput = KeyCode.E;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode fireInput = KeyCode.Mouse0;
        public KeyCode flyInput = KeyCode.Z;
        public KeyCode increaseHeightInput = KeyCode.R;
        public KeyCode decreaseHeightInput = KeyCode.F;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public AttackHandler attackHandler;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;

        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            InputHandle();                  // update the input methods
            cc.UpdateAnimator();            // updates the Animator Parameters
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();
            attackHandler = GetComponent<AttackHandler>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
            KickInput();
            PunchInput();
            FireInput();
            FlyInput();
            HeightDownInput();
            HeightUpInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool IntenseActionConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove && !cc.isKicking
                && !cc.isPunching && !cc.isFiring;
        }
        
        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && IntenseActionConditions())
                cc.Jump();
        }

        protected virtual void KickInput()
        {
            if (Input.GetKeyDown(kickInput) && IntenseActionConditions())
            {
                cc.Kick();
                attackHandler.ExecuteKick();
            }
        }

        protected virtual void PunchInput()
        {
            if (Input.GetKeyDown(punchInput) && IntenseActionConditions())
            {
                cc.Punch();
                attackHandler.ExecutePunch();
            }
        }

        protected virtual void FireInput()
        {
            if (Input.GetKeyDown(fireInput))
            {
                cc.Fire();
                attackHandler.Fire();
            }
        }

        protected virtual void FlyInput()
        {
            if (Input.GetKeyDown(flyInput))
                cc.ToggleFlying();
        }

        protected virtual void HeightUpInput()
        {
            if (Input.GetKey(increaseHeightInput))
                cc.IncreaseHeight();
        }

        protected virtual void HeightDownInput()
        {
            if (Input.GetKey(decreaseHeightInput))
                cc.DecreaseHeight();
        }

        #endregion       
    }
}