using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ص��ӿ�
public enum SSActionEventType : int { Started, Competed }
public interface ISSActionCallback
{
    void SSActionEvent(SSAction source,
        SSActionEventType events = SSActionEventType.Competed,
        int intParam = 0,
        string strParam = null,
        Object objectParam = null);
}