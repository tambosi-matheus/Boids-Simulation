using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroupActivate : MonoBehaviour
{
    public int index;

    public void ChangeActiveState()
    {
        Manager.Instance.ChangeGroupState(index);
    }
}
