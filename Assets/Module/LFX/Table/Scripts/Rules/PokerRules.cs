using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 扑克牌规则
/// </summary>

#pragma warning disable 0219, 0414
public class PokerRules
{

    #region 判断玩家出的牌是否符合出牌规则

    /// <summary>  
    /// 是否是单  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsSingle(List<int> cards)
    {
        if (cards.Count == 1)
            return true;
        else
            return false;
    }

    /// <summary>  
    /// 是否是对子  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsDouble(List<int> cards)
    {
        if (cards.Count == 2)
        {
            if (cards[0] == cards[1])
                return true;
        }

        return false;
    }

    /// <summary>  
    /// 是否是顺子  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsStraight(List<int> cards)
    {
        if (cards.Count < 5 || cards.Count > 12)
            return false;
        for (int i = 0; i < cards.Count - 1; i++)
        {
            int w = cards[i];
            if (cards[i + 1] - w != 1)
                return false;

            //不能超过A
            if (w > 12 || cards[i + 1] > 12)
                return false;
        }

        return true;
    }

    /// <summary>  
    /// 是否是双顺子  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsDoubleStraight(List<int> cards)
    {
        if (cards.Count < 6 || cards.Count % 2 != 0)
            return false;

        for (int i = 0; i < cards.Count; i += 2)
        {
            if (cards[i + 1] != cards[i])
                return false;

            if (i < cards.Count - 2)
            {
                if (cards[i + 2] - cards[i] != 1)
                    return false;

                //不能超过A  
                if (cards[i] > 12 || cards[i + 2] > 12)
                    return false;
            }
        }

        return true;
    }

    /// <summary>  
    /// 三不带  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsOnlyThree(List<int> cards)
    {
        if (cards.Count % 3 != 0)
            return false;
        if (cards[0] != cards[1])
            return false;
        if (cards[1] != cards[2])
            return false;
        if (cards[0] != cards[2])
            return false;

        return true;
    }

    /// <summary>  
    /// 三带一  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsThreeAndOne(List<int> cards)
    {
        if (cards.Count != 4)
            return false;

        if (cards[0] == cards[1] &&
            cards[1] == cards[2])
            return true;
        else if (cards[1] == cards[2] &&
            cards[2] == cards[3])
            return true;
        return false;
    }

    /// <summary>  
    /// 三代二  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsThreeAndTwo(List<int> cards)
    {
        if (cards.Count != 5)
            return false;

        if (cards[0] == cards[1] &&
            cards[1] == cards[2])
        {
            if (cards[3] == cards[4])
                return true;
        }

        else if (cards[2] == cards[3] &&
            cards[3] == cards[4])
        {
            if (cards[0] == cards[1])
                return true;
        }

        return false;
    }

    /// <summary>  
    /// 炸弹  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsBoom(List<int> cards)
    {
        if (cards.Count != 4)
            return false;

        if (cards[0] != cards[1])
            return false;
        if (cards[1] != cards[2])
            return false;
        if (cards[2] != cards[3])
            return false;

        return true;
    }

    /// <summary>  
    /// 王炸  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsJokerBoom(List<int> cards)
    {
        if (cards.Count != 2)
            return false;
        if (cards[0] == 16)
        {
            if (cards[1] == 17)
                return true;
            return false;
        }
        else if (cards[0] == 17)
        {
            if (cards[1] == 16)
                return true;
            return false;
        }

        return false;
    }

    /// <summary>  
    /// 飞机不带  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsTripleStraight(List<int> cards)
    {
        if (cards.Count < 6 || cards.Count % 3 != 0)
            return false;

        for (int i = 0; i < cards.Count; i += 3)
        {
            if (cards[i + 1] != cards[i])
                return false;
            if (cards[i + 2] != cards[i])
                return false;
            if (cards[i + 1] != cards[i + 2])
                return false;

            if (i < cards.Count - 3)
            {
                if (cards[i + 3] - cards[i] != 1)
                    return false;

                //不能超过A  
                if (cards[i] > 12 || cards[i + 3] > 12)
                    return false;
            }
        }

        return true;
    }
    /// <summary>  
    /// 飞机带单  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool isPlaneWithSingle(List<int> cards)
    {
        if (!HaveFour(cards))
        {
            List<int> tempThreeList = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                int tempInt = 0;
                for (int j = 0; j < cards.Count; j++)
                {

                    if (cards[i] == cards[j])
                    {
                        tempInt++;
                    }

                }
                if (tempInt == 3)
                {
                    tempThreeList.Add(cards[i]);
                }
            }
            if (tempThreeList.Count % 3 != cards.Count % 4)
            {

                return false;
            }
            else
            {
                if (IsTripleStraight(tempThreeList))
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
        }

        return false;
    }
    /// <summary>  
    /// 飞机带双  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool isPlaneWithTwin(List<int> cards)
    {
        if (!HaveFour(cards))
        {
            List<int> tempThreeList = new List<int>();
            List<int> tempTwoList = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                int tempInt = 0;
                for (int j = 0; j < cards.Count; j++)
                {

                    if (cards[i] == cards[j])
                    {
                        tempInt++;
                    }

                }
                if (tempInt == 3)
                {
                    tempThreeList.Add(cards[i]);
                }
                else if (tempInt == 2)
                {
                    tempTwoList.Add(cards[i]);
                }

            }
            if (tempThreeList.Count % 3 != cards.Count % 5 && tempTwoList.Count % 2 != cards.Count % 5)
            {

                return false;
            }
            else
            {
                if (IsTripleStraight(tempThreeList))
                {
                    if (IsAllDouble(tempTwoList))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {

                    return false;
                }
            }
        }
        return false;
    }
    /// <summary>  
    /// 判断牌里面是否是拥有4张牌  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool HaveFour(List<int> cards)
    {

        for (int i = 0; i < cards.Count; i++)
        {
            int tempInt = 0;
            for (int j = 0; j < cards.Count; j++)
            {

                if (cards[i] == cards[j])
                {
                    tempInt++;
                }
            }
            if (tempInt == 4)
            {
                return true;
            }
        }
        //Debug.Log(false);
        return false;
    }

    /// <summary>  
    /// 判断牌里面全是对子  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool IsAllDouble(List<int> cards)
    {
        for (int i = 0; i < cards.Count % 2; i += 2)
        {
            if (cards[i] != cards[i + 1])
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>  
    /// 判断是否是四带二  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <returns></returns>  
    public static bool isSiDaiEr(List<int> cards)
    {
        bool flag = false;
        if (cards != null && cards.Count == 6)
        {


            for (int i = 0; i < 3; i++)
            {
                int grade1 = cards[i];
                int grade2 = cards[i + 1];
                int grade3 = cards[i + 2];
                int grade4 = cards[i + 3];

                if (grade2 == grade1 && grade3 == grade1 && grade4 == grade1)
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    /// <summary>  
    /// 判断是否符合出牌规则  
    /// </summary>  
    /// <param name="cards"></param>  
    /// <param name="type"></param>  
    /// <returns></returns>  
    public static bool PopEnable(List<int> cards, out DDZ_POKER_TYPE type)
    {
        //public static bool PopEnable(List<int> cards, out DDZ_POKER_TYPE type)
        type = DDZ_POKER_TYPE.DDZ_PASS;
        bool isRule = false;
        switch (cards.Count)
        {
            case 1:
                isRule = true;
                type = DDZ_POKER_TYPE.SINGLE;
                break;
            case 2:
                if (IsDouble(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.TWIN;
                }
                else if (IsJokerBoom(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.KING_BOMB;
                }
                break;
            case 3:
                if (IsOnlyThree(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.TRIPLE;
                }
                break;
            case 4:
                if (IsBoom(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.FOUR_BOMB;
                }
                else if (IsThreeAndOne(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE;
                }

                break;
            case 5:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsThreeAndTwo(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.TRIPLE_WITH_TWIN;
                }
                break;
            case 6:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_PURE;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isSiDaiEr(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.FOUR_WITH_SINGLE;   //四带二  
                }
                break;
            case 7:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                break;
            case 8:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isPlaneWithSingle(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;   //飞机带单  
                }
                break;
            case 9:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_PURE;
                }
                break;
            case 10:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isPlaneWithTwin(cards))           //飞机带对  
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                }
                break;

            case 11:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                break;
            case 12:
                if (IsStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_SINGLE;
                }
                else if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isPlaneWithSingle(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;   //飞机带单  
                }
                else if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_PURE;
                }
                break;
            case 13:
                break;
            case 14:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                break;
            case 15:
                if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_PURE;
                }
                else if (isPlaneWithTwin(cards))           //飞机带对  
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                }
                break;
            case 16:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isPlaneWithSingle(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;   //飞机带单  
                }
                break;
            case 17:
                break;
            case 18:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (IsTripleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_PURE;
                }
                break;
            case 19:
                break;

            case 20:
                if (IsDoubleStraight(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.STRAIGHT_TWIN;
                }
                else if (isPlaneWithSingle(cards))
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_SINGLE;   //飞机带单  
                }
                else if (isPlaneWithTwin(cards))           //飞机带对  
                {
                    isRule = true;
                    type = DDZ_POKER_TYPE.PLANE_WITH_TWIN;
                }
                break;
            default:
                break;
        }

        return isRule;
    }

    #endregion

    #region 是否比上家的牌大
    
    //比较当前出的牌和上个玩家的牌的大小
    public static bool isSelectCardCanPut(List<int> myCards, DDZ_POKER_TYPE myCardType, List<int> lastCards, DDZ_POKER_TYPE lastCardTye)
    {

        // 上一首牌的个数  
        int prevSize = lastCards.Count;
        int mySize = myCards.Count;

        // 集中判断是否王炸，免得多次判断王炸  
        if (lastCardTye == DDZ_POKER_TYPE.KING_BOMB)
        {
            Debug.Log("上家王炸，肯定不能出。");
            return false;
        }
        else if (myCardType == DDZ_POKER_TYPE.KING_BOMB)
        {
            Debug.Log("我王炸，肯定能出。");
            return true;
        }

        // 集中判断对方不是炸弹，我出炸弹的情况  
        if (lastCardTye != DDZ_POKER_TYPE.FOUR_BOMB && myCardType == DDZ_POKER_TYPE.FOUR_BOMB)
        {
            return true;
        }

        //王炸判断过了，所以牌数不相同，就不能出
        if(myCards.Count != lastCards.Count)
        {
            return false;
        }

        //*所有牌提前是必须排序过了 (升序) 

        //我出的牌的权值和上家牌的权值，根据大小来（0-14）
        int myGrade = myCards[0];
        int prevGrade = lastCards[0];

        // 比较2家的牌，主要有2种情况，1.我出和上家一种类型的牌，即对子管对子；  
        // 2.我出炸弹，此时，和上家的牌的类型可能不同  
        // 王炸的情况已经排除  

        // 单  
        if (lastCardTye == DDZ_POKER_TYPE.SINGLE && myCardType == DDZ_POKER_TYPE.SINGLE)
        {
            // 一张牌可以大过上家的牌  
            return compareGrade(myGrade, prevGrade);
        }
        // 对子  
        else if (lastCardTye == DDZ_POKER_TYPE.TWIN
                && myCardType == DDZ_POKER_TYPE.TWIN)
        {
            // 2张牌可以大过上家的牌  
            return compareGrade(myGrade, prevGrade);

        }
        // 3不带  
        else if (lastCardTye == DDZ_POKER_TYPE.TRIPLE
                && myCardType == DDZ_POKER_TYPE.TRIPLE)
        {
            // 3张牌可以大过上家的牌  
            return compareGrade(myGrade, prevGrade);
        }
        // 炸弹  
        else if (lastCardTye == DDZ_POKER_TYPE.FOUR_BOMB
                && myCardType == DDZ_POKER_TYPE.FOUR_BOMB)
        {
            // 4张牌可以大过上家的牌  
            return compareGrade(myGrade, prevGrade);

        }
        // 3带1  
        else if (lastCardTye == DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
        {

            // 3带1只需比较第2张牌的大小  
            myGrade = myCards[1];
            prevGrade = lastCards[1];
            return compareGrade(myGrade, prevGrade);

        }
        else if (lastCardTye == DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
        {
            // 3带2只需比较第3张牌的大小  
            myGrade = myCards[2];
            prevGrade = lastCards[2];
            return compareGrade(myGrade, prevGrade);

        }
        // 4带2  
        else if (lastCardTye == DDZ_POKER_TYPE.FOUR_WITH_SINGLE
                && myCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
        {

            // 4带2只需比较第3张牌的大小  
            myGrade = myCards[2];
            prevGrade = lastCards[2];

        }
        // 顺子  
        else if (lastCardTye == DDZ_POKER_TYPE.STRAIGHT_SINGLE
                && myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
        {
            if (mySize != prevSize)
            {
                return false;
            }
            else
            {
                // 顺子只需比较最大的1张牌的大小  
                myGrade = myCards[mySize - 1];
                prevGrade = lastCards[prevSize - 1];
                return compareGrade(myGrade, prevGrade);
            }

        }
        // 连对  
        else if (lastCardTye == DDZ_POKER_TYPE.STRAIGHT_TWIN
                && myCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN)
        {
            if (mySize != prevSize)
            {
                return false;
            }
            else
            {
                // 顺子只需比较最大的1张牌的大小  
                myGrade = myCards[mySize - 1];
                prevGrade = lastCards[prevSize - 1];
                return compareGrade(myGrade, prevGrade);
            }

        }
        // 飞机不带  
        else if (lastCardTye == DDZ_POKER_TYPE.PLANE_PURE
                && myCardType == DDZ_POKER_TYPE.PLANE_PURE)
        {
            if (mySize != prevSize)
            {
                return false;
            }
            else
            {
                //333444555666算飞机不带 不算飞机带单  
                myGrade = myCards[4];
                prevGrade = lastCards[4];
                return compareGrade(myGrade, prevGrade);
            }
        }
        //飞机带单  
        else if (lastCardTye == DDZ_POKER_TYPE.PLANE_WITH_SINGLE
                && myCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
        {
            if (mySize != prevSize)
            {
                return false;
            }
            else
            {
                List<int> tempThreeList = new List<int>();
                for (int i = 0; i < myCards.Count; i++)
                {
                    int tempInt = 0;
                    for (int j = 0; j < myCards.Count; j++)
                    {

                        if (myCards[i] == myCards[j])
                        {
                            tempInt++;
                        }

                    }
                    if (tempInt == 3)
                    {
                        tempThreeList.Add(myCards[i]);
                    }
                }
                myGrade = tempThreeList[4];
                prevGrade = lastCards[4];
                return compareGrade(myGrade, prevGrade);
            }
        }
        //飞机带双  
        else if (lastCardTye == DDZ_POKER_TYPE.PLANE_WITH_TWIN
                && myCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN)
        {
            if (mySize != prevSize)
            {
                return false;
            }
            else
            {
                List<int> tempThreeList = new List<int>();
                List<int> tempTwoList = new List<int>();
                for (int i = 0; i < myCards.Count; i++)
                {
                    int tempInt = 0;
                    for (int j = 0; j < myCards.Count; j++)
                    {

                        if (myCards[i] == myCards[j])
                        {
                            tempInt++;
                        }

                    }
                    if (tempInt == 3)
                    {
                        tempThreeList.Add(myCards[i]);
                    }
                    else if (tempInt == 2)
                    {
                        tempTwoList.Add(myCards[i]);
                    }

                }
                myGrade = tempThreeList[4];
                prevGrade = lastCards[4];
                if (compareGrade(myGrade, prevGrade))
                {
                    return IsAllDouble(tempTwoList);
                }
            }
        }

        // 默认不能出牌  
        return false;
    }
    #endregion

    #region 比较两张牌的大小
    private static bool compareGrade(int grade1, int grade2)
    {
        return grade1 > grade2;
    }
    #endregion

    

}
