using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI出牌
/// </summary>
public class PokerAI {

    #region 电脑根据上家的牌出牌  
    public static Dictionary<int, List<int>> FindPromptCards(List<int> myCards,
        List<int> lastCards, DDZ_POKER_TYPE lastCardType)
    {
        Dictionary<int, List<int>> PromptCards = new Dictionary<int, List<int>>();
        Hashtable tempMyCardHash = SortCardUseHash1(myCards);

        // 上一手牌的个数  
        int prevSize = lastCards.Count;
        // 我手牌的个数
        int mySize = myCards.Count;

        int prevGrade = 0;
        if (prevSize > 0)
        {
            prevGrade = lastCards[0];
            //Debug.Log("prevGrade" + prevGrade);
        }

        // 我先出牌，上家没有牌  
        if (prevSize == 0 && mySize != 0)
        {
            if (MyUtil.GetRange(0,2) == 0 && mySize > 1)
            {
                lastCards.Add(0);
                lastCards.Add(0);
                prevSize = 2;

                int tempCount = 0;
                List<int> myCardsHashKey = new List<int>();
                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 2)
                    {
                        List<int> tempIntList = new List<int>();
                        tempIntList.Add(myCardsHashKey[i]);
                        tempIntList.Add(myCardsHashKey[i]);
                        PromptCards.Add(tempCount, tempIntList);
                        tempCount++;
                    }

                }
                if(PromptCards.Count == 0)
                {
                    lastCards.Clear();
                    prevSize = 0;
                    //把所有牌权重存入返回  
                    Debug.Log("上家没有牌");
                    //List<int> myCardsHashKey = new List<int>();
                    foreach (int key in tempMyCardHash.Keys)
                    {
                        myCardsHashKey.Add(key);
                    }
                    myCardsHashKey.Sort();
                    for (int i = 0; i < myCardsHashKey.Count; i++)
                    {
                        List<int> tempIntList = new List<int>();
                        tempIntList.Add(myCardsHashKey[i]);
                        PromptCards.Add(i, tempIntList);

                    }
                }
            }
            else
            {
                //把所有牌权重存入返回  
                Debug.Log("上家没有牌");
                List<int> myCardsHashKey = new List<int>();
                foreach (int key in tempMyCardHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                for (int i = 0; i < myCardsHashKey.Count; i++)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    PromptCards.Add(i, tempIntList);

                }
            }
            
        }

        // 集中判断是否王炸，免得多次判断王炸  
        if (lastCardType == DDZ_POKER_TYPE.KING_BOMB)
        {
            Debug.Log("上家王炸，肯定不能出。");
        }
        
        // 比较2家的牌，主要有2种情况，1.我出和上家一种类型的牌，即对子管对子；  
        // 2.我出炸弹，此时，和上家的牌的类型可能不同  
        // 王炸的情况已经排除  

        // 上家出单  
        if (lastCardType == DDZ_POKER_TYPE.SINGLE)
        {
            int tempCount = 0;
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }


            }


        }
        // 上家出对子  
        else if (lastCardType == DDZ_POKER_TYPE.TWIN)
        {
            int tempCount = 0;
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 2)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }

            }

        }
        // 上家出3不带  
        else if (lastCardType == DDZ_POKER_TYPE.TRIPLE)
        {
            int tempCount = 0;
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }


            }

        }
        // 上家出3带1  
        else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_SINGLE)
        {
            // 3带1 3不带 比较只多了一个判断条件  
            if (mySize < 4)
            {

            }
            int grade3 = 0;
            foreach (int key in tempMyCardHash.Keys)
            {
                if (int.Parse(tempMyCardHash[key].ToString()) == 1)
                {
                    grade3 = key;
                    break;
                }
            }
            int tempCount = 0;
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(grade3);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }


            }

        }
        // 上家出3带2  
        else if (lastCardType == DDZ_POKER_TYPE.TRIPLE_WITH_TWIN)
        {
            // 3带1 3不带 比较只多了一个判断条件  
            if (mySize < 5)
            {

            }
            int grade3 = 0;
            int grade4 = 0;
            foreach (int key in tempMyCardHash.Keys)
            {
                if (int.Parse(tempMyCardHash[key].ToString()) == 2)
                {
                    grade3 = key;
                    grade4 = key;
                    break;
                }
            }
            int tempCount = 0;
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardHash[myCardsHashKey[i]] >= 3)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(grade3);
                    tempIntList.Add(grade4);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }


            }

        }
        // 上家出炸弹  
        else if (lastCardType == DDZ_POKER_TYPE.FOUR_BOMB)
        {
            int tempCount = 0;
            // 4张牌可以大过上家的牌  
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    if (grade0 > prevGrade)
                    {
                        // 把四张牌存进去  
                        List<int> tempIntList = new List<int>();
                        tempIntList.Add(grade0);
                        tempIntList.Add(grade1);
                        tempIntList.Add(grade2);
                        tempIntList.Add(grade3);

                        PromptCards.Add(tempCount, tempIntList);
                        tempCount++;
                    }
                }
            }

        }
        // 上家出4带2   
        else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
        {
            // 4张牌可以大过上家的牌  
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    // 只要有炸弹，则返回true  

                }
            }
        }
        // 上家出4带2 对子  
        else if (lastCardType == DDZ_POKER_TYPE.FOUR_WITH_SINGLE)
        {
            // 4张牌可以大过上家的牌  
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    // 只要有炸弹，则返回true  

                }
            }
        }
        // 上家出顺子  
        else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                List<int> tempMyCards = new List<int>();
                tempMyCards = myCards;
                Hashtable myCardsHash = SortCardUseHash(tempMyCards);
                if (myCardsHash.Count < prevSize)
                {
                    Debug.Log("hash的总数小于顺子的count 肯定fales");

                }
                List<int> myCardsHashKey = new List<int>();
                foreach (int key in myCardsHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                int tempCount = 0;
                for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                {
                    List<int> cards = new List<int>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                    }
                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    bool isRule = PokerRules.PopEnable(cards, out myCardType);

                    if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                    {
                        int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后  
                        int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后  

                        if (myGrade2 > prevGrade2)
                        {
                            //存进去PromptCards  
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }

            }

        }
        // 上家出连对  
        else if (lastCardType == DDZ_POKER_TYPE.STRAIGHT_TWIN)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                List<int> tempMyCards = new List<int>();
                tempMyCards = myCards;
                Hashtable myCardsHash = SortCardUseHash(tempMyCards);
                if (myCardsHash.Count < prevSize)
                {
                    Debug.Log("hash的总数小于顺子的count 肯定fales");

                }
                List<int> myCardsHashKey = new List<int>();
                foreach (int key in myCardsHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                int tempCount = 0;
                for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                {

                    List<int> cards = new List<int>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                    }
                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    bool isRule = PokerRules.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                    {
                        int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后  
                        int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后  

                        if (myGrade2 > prevGrade2)
                        {
                            for (int ii = 0; ii < cards.Count; ii++)
                            {
                                if ((int)myCardsHash[cards[ii]] < 2)
                                {
                                    Debug.Log("是顺子但不是双顺");
                                    return PromptCards;
                                }
                                else
                                {
                                    for (int iii = 0; iii < cards.Count; iii++)
                                    {
                                        cards.Add(cards[iii]);
                                    }
                                    //存进去PromptCards  
                                    PromptCards.Add(tempCount, cards);
                                    tempCount++;
                                }
                            }
                        }
                    }
                }
            }

        }
        //上家出飞机  
        else if (lastCardType == DDZ_POKER_TYPE.PLANE_PURE)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                int tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<int> cards = new List<int>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    bool isRule = PokerRules.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        int myGrade4 = cards[4];//  
                        int prevGrade4 = lastCards[4];//  

                        if (myGrade4 > prevGrade4)
                        {
                            //存进去PromptCards  
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }
            }
        }
        //上家出飞机带单  
        else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_SINGLE)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                int tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<int> cards = new List<int>();
                    for (int j = 0; j < prevSize - prevSize / 4; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    bool isRule = PokerRules.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        int myGrade4 = cards[4];//  
                        int prevGrade4 = lastCards[4];//  

                        if (myGrade4 > prevGrade4)
                        {
                            int ii = 0;
                            //存进去PromptCards 然后再找一个最小的两个单  
                            foreach (int key in tempMyCardHash.Keys)
                            {
                                if (int.Parse(tempMyCardHash[key].ToString()) == 1)
                                {
                                    cards.Add(key);
                                    ii++;
                                    if (ii == prevSize / 4)
                                    {
                                        break;
                                    }
                                }
                            }
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }
            }
        }

        //上家出飞机带双  
        else if (lastCardType == DDZ_POKER_TYPE.PLANE_WITH_TWIN)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                int tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<int> cards = new List<int>();
                    for (int j = 0; j < prevSize - prevSize / 5; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    bool isRule = PokerRules.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        int myGrade4 = cards[4];//  
                        int prevGrade4 = lastCards[4];//  

                        if (myGrade4 > prevGrade4)
                        {
                            List<int> tempTwoList = new List<int>();
                            for (int ii = 0; ii < cards.Count; ii++)
                            {
                                int tempInt = 0;
                                for (int j = 0; j < cards.Count; j++)
                                {

                                    if (cards[ii] == cards[j])
                                    {
                                        tempInt++;
                                    }

                                }
                                if (tempInt == 2)
                                {
                                    tempTwoList.Add(cards[ii]);
                                }

                            }
                            if (tempTwoList.Count / 2 < prevSize / 5)
                            {


                            }
                            else
                            {
                                //存进去  
                                int iii = 0;
                                //存进去PromptCards 然后再找一个最小的两个单  
                                foreach (int key in tempMyCardHash.Keys)
                                {
                                    if (int.Parse(tempMyCardHash[key].ToString()) == 2)
                                    {
                                        cards.Add(key);
                                        cards.Add(key);
                                        iii++;
                                        if (iii == prevSize / 5)
                                        {
                                            break;
                                        }
                                    }
                                }
                                PromptCards.Add(tempCount, cards);
                                tempCount++;
                            }
                        }
                    }
                }
            }
        }




        // 集中判断对方不是炸弹，我出炸弹的情况  
        if (lastCardType != DDZ_POKER_TYPE.FOUR_BOMB)
        {

            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if ((int)tempMyCardHash[myCardsHashKey[i]] == 4)
                {
                    List<int> tempIntList = new List<int>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    Debug.Log("PromptCards.Count" + PromptCards.Count);
                    PromptCards.Add(PromptCards.Count, tempIntList);

                }


            }

        }
        if (mySize >= 2)
        {
            List<int> myCardsHashKey = new List<int>();
            foreach (int key in tempMyCardHash.Keys)
            {
                myCardsHashKey.Add(key);
            }
            if (myCardsHashKey.Contains(53) && myCardsHashKey.Contains(54))
            {
                List<int> tempIntList = new List<int>();
                tempIntList.Add(53);
                tempIntList.Add(54);
                PromptCards.Add(PromptCards.Count, tempIntList);
            }
        }

        return PromptCards;
    }
    #endregion

    #region 使用哈希去存所有的牌  顺子连对用 不用存2 王  
    /// <summary>  
    /// 使用哈希去存所有的牌  
    /// </summary>  
    /// <param name="ownCards"></param>  
    /// <returns></returns>  
    public static Hashtable SortCardUseHash(List<int> cards)
    {
        Hashtable temp = new Hashtable();
        List<int> tempJoker = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == 16)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == 17)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == 16)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (temp.Contains(cards[i]))
            {
                temp[cards[i]] = (int)temp[cards[i]] + 1;
            }
            else
            {
                temp.Add(cards[i], 1);
            }
        }

        return temp;
    }

    #endregion

    #region 使用哈希存所有牌 key 权重  value个数  
    public static Hashtable SortCardUseHash1(List<int> cards)
    {
        Hashtable temp = new Hashtable();


        for (int i = 0; i < cards.Count; i++)
        {
            if (temp.Contains(cards[i]))
            {
                temp[cards[i]] = (int)temp[cards[i]] + 1;
            }
            else
            {
                temp.Add(cards[i], 1);
            }
        }

        return temp;
    }
    #endregion

}
