using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    public GameCamps enemyZoneCamp;

    public List<FideleManager> enemyFM; //Liste des ennemis concernés par cette zone

    public List<FideleManager> attackableFM;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<FideleManager>().myCamp == GameCamps.Fidele)
        {
            //ajouter un fidele à la liste des attaquables
            attackableFM.Add(collision.GetComponentInParent<FideleManager>());
            RefreshAttackableDetection();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<FideleManager>().myCamp == GameCamps.Fidele)
        {
            //retirer un fidele de la liste des attaquables
            attackableFM.Remove(collision.GetComponentInParent<FideleManager>());
            RefreshAttackableDetection();
        }
    }

    public void RefreshAttackableDetection()
    {
        if (attackableFM.Count > 0)
        {
            foreach (FideleManager efm in enemyFM)
            {
                efm.GetComponentInChildren<MovementEnemy>().isAttackableUnitInMyZone = true;
            }
        }
        else
        {
            foreach (FideleManager efm in enemyFM)
            {
                efm.GetComponentInChildren<MovementEnemy>().isAttackableUnitInMyZone = false;
            }
        }
    }
}
