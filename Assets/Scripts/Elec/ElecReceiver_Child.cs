using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecReceiver_Child : ElecReceiver
{
    public ElecReceiver father;
    public override void ElecReceiving()
    {
        father.ElecReceiving();
    }
}
