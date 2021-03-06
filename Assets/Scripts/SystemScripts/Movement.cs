using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;

    private bool isInsideMapLimits = true;
    private bool isInAnObstacle = false;
    private bool isInDeadZone = true;

    private Vector2 startPosition;

    public bool isMoving = false;
    public bool hasMoved = false;

    private bool isLanbable = false;

    private bool isInEnemyZone = false;
    private EnemyZone collidingEnemyZone;

    //public Sprite testDlcmtSprite;

    //public GameObject myCollideZone;
    private FideleManager myFM;
    public GameObject myMoveZone;
    public GameObject myDeadZone;
    public SpriteRenderer myShadow;
    private Collider2D myInteractionZoneCollider;
    private AnimationManager myAnimationManager;
    private Interaction myInteraction;

    private Collider2D myMapLimit;

    private bool isInTutoZone;

    // Start is called before the first frame update
    void Start()
    {
        myMapLimit = GameObject.FindGameObjectWithTag("MapLimit").GetComponent<Collider2D>();

        myFM = GetComponentInParent<FideleManager>();
        myInteraction = myFM.GetComponentInChildren<Interaction>();

        DragCamera2D.Instance.followTarget = null;

        myAnimationManager = GetComponentInParent<AnimationManager>();

        myMoveZone = myAnimationManager.GetComponentInChildren<MovementZoneDetection>().gameObject;
        myDeadZone = myMoveZone.GetComponentInChildren<DeadZone>().gameObject;
        myInteractionZoneCollider = myMoveZone.GetComponent<PolygonCollider2D>();

        myDeadZone.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), myAnimationManager.GetComponent<Collider2D>());

        if (isMoving && Input.GetMouseButton(0) && hasMoved == false)
        {
            myShadow.enabled = true;
            CursorManager.Instance.SetCursorToMovement();
            InfoCharacter.Instance.CloseInformationWindow();
            myFM.currentFideleSprite.sprite = myFM.movingFideleSprite;
            myDeadZone.SetActive(true);
            // ICI jouer VFX de déplacement en cours
            // ICI jouer SFX de déplacement en cours
            // ICI jouer Anim de déplacement en cours

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, 0.15f);

            foreach (FideleManager fmir in myFM.unitsInRange)
            {
                if (fmir.myCamp != myFM.myCamp)
                {
                    fmir.GetComponent<AnimationManager>().DisplayInteraction();
                    fmir.GetComponent<AnimationManager>().DisplayInteractionIcon();
                    fmir.GetComponent<AnimationManager>().DarkInteractionIcon();
                    fmir.GetComponent<AnimationManager>().LowOpacityInteractionIcon();
                }
            }

            //myCam.followTarget = transform.parent.gameObject;
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            CursorManager.Instance.SetCursorToDefault();
            myDeadZone.SetActive(false);
            // ICI jouer VFX de déplacement terminé
            // ICI jouer SFX de déplacement terminé
            // ICI jouer Anim de déplacement terminé

            if (isLanbable)
            {
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
                hasMoved = true;

                myAnimationManager.CheckActionsLeftAmout();
                StartCoroutine(DragCamera2D.Instance.FollowTargetCamera(myFM.gameObject));

                EffectManager.Instance.EndDeplacement(myFM.transform);

                for (int i = 0; i < myInteraction.myCollideInteractionList.Count; i++)
                {
                    QuestManager.Instance.OnUnitReached(myInteraction.myCollideInteractionList[i].GetComponent<Interaction>());
                }

                if (GameManager.Instance.isMapTuto)
                {
                    GameManager.Instance.firstFideleToMoveHasMoved = true;
                }

                if (myAnimationManager.isSelectable)
                {
                    RaycastInteraction.Instance.SetFideleSelectedInteractionLauncher(myFM);
                }

                if (isInEnemyZone)
                {
                    collidingEnemyZone.FideleEnteredEnemyZone();
                }
            }
            else
            {
                transform.localPosition = Vector3.zero;
                if (GameManager.Instance.isMapTuto)
                {
                    myFM.GetComponent<AnimationManager>().DesactivateCursorIndicator();
                }
            }

            myShadow.enabled = false;
            myFM.currentFideleSprite.sprite = myFM.idleFideleSprite;
            myAnimationManager.HideMovement();
            myAnimationManager.HideInteraction();

            foreach (FideleManager fmir in GameManager.Instance.allMapUnits)
            {
                if (fmir.myCamp != myFM.myCamp)
                {
                    fmir.GetComponent<AnimationManager>().HideInteraction();
                    fmir.GetComponent<AnimationManager>().HideInteractionIcon();
                    //fmir.GetComponent<AnimationManager>().WhiteInteractionIcon();

                    fmir.GetComponent<AnimationManager>().DesactivateReceiverSelection();

                    fmir.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
                }
            }

            myInteraction.FideleDisplayInteractionFeedbacks();
            myInteraction.CheckForAvaibleInteractions();

            isInsideMapLimits = true;
            isInAnObstacle = false;
            isInDeadZone = true;

            isMoving = false;
        }
    }

    public void MovingCharacter()
    {
        if (GameManager.Instance.isMapTuto)
        {
            if (GameManager.Instance.firstFideleToMoveHasMoved == false)
            {
                if (myFM == GameManager.Instance.firstFideleToMove)
                {
                    if (hasMoved == false)
                    {
                        // ICI jouer VFX de début de déplacement
                        // ICI jouer SFX de début de déplacement
                        // ICI jouer Anim de déplacement

                        EffectManager.Instance.LaunchDeplacement(myFM.transform, isMoving);

                        isMoving = true;
                        myFM.GetComponent<AnimationManager>().DesactivateCursorIndicator();

                        // ICI utiliser Coroutine pour attendre la fin des effets pour déplacer

                        myAnimationManager.DisplayInteraction();
                        myAnimationManager.DisplayMovement();
                    }
                    else
                    {
                        myInteraction.FideleDisplayInteractionFeedbacks();
                    }
                }
            }
            else if (GameManager.Instance.firstFideleToInteractWithHasInteracted)
            {
                if (hasMoved == false)
                {
                    // ICI jouer VFX de début de déplacement
                    // ICI jouer SFX de début de déplacement
                    // ICI jouer Anim de déplacement

                    EffectManager.Instance.LaunchDeplacement(myFM.transform, isMoving);

                    isMoving = true;

                    // ICI utiliser Coroutine pour attendre la fin des effets pour déplacer

                    myAnimationManager.DisplayInteraction();
                    myAnimationManager.DisplayMovement();
                }
                else
                {
                    myInteraction.FideleDisplayInteractionFeedbacks();
                }
            }
        }
        else
        {
            if (hasMoved == false)
            {
                // ICI jouer VFX de début de déplacement
                // ICI jouer SFX de début de déplacement
                // ICI jouer Anim de déplacement
                isMoving = true;

                // ICI utiliser Coroutine pour attendre la fin des effets pour déplacer

                myAnimationManager.DisplayInteraction();
                myAnimationManager.DisplayMovement();
            }
            else
            {
                myInteraction.FideleDisplayInteractionFeedbacks();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMoving)
        {
            if (GameManager.Instance.isMapTuto)
            {
                if (GameManager.Instance.firstFideleToMoveHasMoved == false)
                {
                    if (collision.tag == ("TutoZone"))
                    {
                        isInTutoZone = true;
                        //myAnimationManager.AbleToLand();
                    }
                }
            }
            else
            {
                if (collision.GetComponent<EnemyZone>() != null)
                {
                    collidingEnemyZone = collision.GetComponent<EnemyZone>();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isMoving)
        {
            if (GameManager.Instance.isMapTuto)
            {
                if (GameManager.Instance.firstFideleToMoveHasMoved == false)
                {
                    isLanbable = false;
                    myAnimationManager.UnableToLand();
                    if (collision.tag == ("TutoZone"))
                    {
                        isInTutoZone = false;
                        isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }
                    if (collision.gameObject == myMoveZone)
                    {
                        isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }
                }
                else
                {
                    if (collision.GetComponent<EnemyZone>() != null)
                    {
                        isInEnemyZone = false;
                    }
                    if (collision.gameObject == myMoveZone)
                    {
                        isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }
                    if (collision.gameObject == myDeadZone)
                    {
                        isInDeadZone = false;
                    }
                    if (collision.tag == ("Obstacle"))
                    {
                        isInAnObstacle = false;
                    }
                    if (collision.tag == ("MapLimit"))
                    {
                        isInsideMapLimits = true;
                    }
                }
            }
            else
            {
                if (collision.GetComponent<EnemyZone>() != null)
                {
                    isInEnemyZone = false;
                    collidingEnemyZone = null;
                }
                if (collision.gameObject == myMoveZone)
                {
                    isLanbable = false;
                    myAnimationManager.UnableToLand();
                }
                if (collision.gameObject == myDeadZone)
                {
                    isInDeadZone = false;
                }
                if (collision.tag == ("Obstacle"))
                {
                    isInAnObstacle = false;
                }
                if (collision.tag == ("MapLimit"))
                {
                    isInsideMapLimits = true;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isMoving)
        {
            if (GameManager.Instance.isMapTuto)
            {
                if (GameManager.Instance.firstFideleToMoveHasMoved == false)
                {
                    if (collision.gameObject == myMoveZone && isInTutoZone)
                    {
                        isLanbable = true;
                        myAnimationManager.AbleToLand();
                    }
                }
                else
                {
                    if (collision.GetComponent<EnemyZone>() != null)
                    {
                        isInEnemyZone = true;
                    }
                    if (collision.tag == ("Obstacle"))
                    {
                        isInAnObstacle = true;
                        //isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }
                    if (collision.tag == ("MapLimit"))
                    {
                        isInsideMapLimits = false;
                        //isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }
                    if (collision.gameObject == myDeadZone)
                    {
                        isInDeadZone = true;
                        //isLanbable = false;
                        myAnimationManager.UnableToLand();
                    }

                    if (collision.gameObject == myMoveZone && isInAnObstacle == false && isInsideMapLimits && isInDeadZone == false)
                    {
                        isLanbable = true;
                        myAnimationManager.AbleToLand();
                    }
                }
            }
            else
            {
                if (collision.GetComponent<EnemyZone>() != null)
                {
                    isInEnemyZone = true;
                }
                if (collision.tag == ("Obstacle"))
                {
                    isInAnObstacle = true;
                    //isLanbable = false;
                    myAnimationManager.UnableToLand();
                }
                if (collision.tag == ("MapLimit"))
                {
                    isInsideMapLimits = false;
                    //isLanbable = false;
                    myAnimationManager.UnableToLand();
                }
                if (collision.gameObject == myDeadZone)
                {
                    isInDeadZone = true;
                    //isLanbable = false;
                    myAnimationManager.UnableToLand();
                }
                if (collision.gameObject == myMoveZone && isInAnObstacle == false && isInsideMapLimits && isInDeadZone == false)
                {
                    isLanbable = true;
                    myAnimationManager.AbleToLand();
                }
            }
        }
    }
}
