using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestEvents : MonoBehaviour
{
    #region Singleton
    public static QuestEvents Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        #endregion
    }

    //Evenements liés au COMBAT :
    /*
    public event Action onEntityKilled;
    public void EntityKilled()
    {
        onEntityKilled?.Invoke();
    }

    public event Action<FideleManager> onThisEntityKilled;
    public void ThisEntityKilled(FideleManager thisFM)
    {
        onThisEntityKilled?.Invoke(thisFM);
    }

    //Evenements liés au RECRUTEMENT :

    public event Action<FideleManager> onEntityRecruited;
    public void EntityRecruited(FideleManager thisFM)
    {
        onEntityRecruited?.Invoke(thisFM);
    }

    //Evenements liés au DIALOGUE :

    public event Action<FideleManager> onThisEntityTalked;
    public void EntityTalked(FideleManager thisFM)
    {
        onThisEntityTalked?.Invoke(thisFM);
    }
    */
}
