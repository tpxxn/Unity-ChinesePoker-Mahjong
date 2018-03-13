using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 电脑AI出牌
/// </summary>
public class MajiangAI
{

    public static MCardInfo AIPlayCard(List<MCardInfo> list)
    {
        var x = _findDropCard(list);

        for(int i=0;i<list.Count;i++)
        {
            if(list[i].cardIndex == x)
            {
                return list[i];
            }
        }

        return list[list.Count - 1];
    }

    /// <summary>
    /// 找到不是顺子，不是对子的牌，出掉
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private static int _findDropCard(List<MCardInfo> list)
    {
        int index = -1;
        List<int> cards = new List<int>();

        for (int i = 0; i < list.Count; i++)
        {
            cards.Add(list[i].cardIndex);
        }

        if (cards[0] != cards[1] && (cards[0] + 1) != cards[1])
            return cards[0];
    
        for(int i=1;i<cards.Count-1;i++)
        {
            if (cards[i] != cards[i - 1] && cards[i] != cards[i + 1] && (cards[i] - 1) != cards[i - 1] && (cards[i] + 1) != cards[i + 1])
                return cards[i];
        }
        
        for(int i=0;i<cards.Count;i++)
        {
            if(cards[cards.Count-1] != cards[cards.Count-2])
            {
                return cards[cards.Count - 1];
            }
        }


        return index;

    }

}
