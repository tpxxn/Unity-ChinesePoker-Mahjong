using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WT.UI;

public class PokerFastRoomControl  {

    private PokerFastRoom fastRoom;

    public PokerFastRoomControl()
    {
        fastRoom = new PokerFastRoom();
    }

    #region Action

    #endregion

    public void ShowFastRoom()
    {
        WTUIPage.ShowPage("FastRoom", fastRoom);
    }
}
