using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 麻将胡牌的判断
/// </summary>
public class MajiangRules
{

    /// <summary>
    /// 碰判断
    /// </summary>
    /// <param name="list"></param>
    /// <param name="mCardInfo"></param>
    /// <returns></returns>
    public static int IsPeng(List<MCardInfo> list, MCardInfo mCardInfo)
    {
        int n = -1;

        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].cardIndex == list[i + 1].cardIndex && list[i].cardIndex == mCardInfo.cardIndex)
            {
                n = list[i].cardIndex;
                break;
            }
        }

        return n;
    }

    /// <summary>
    /// 判断是否能杠牌
    /// </summary>
    /// <param name="list"></param>
    /// <param name="mCardInfo"></param>
    /// <returns></returns>
    public static int IsGang(List<MCardInfo> list, MCardInfo mCardInfo)
    {
        int n = -1;

        for (int i = 0; i < list.Count - 2; i++)
        {
            if (list[i].cardIndex == list[i + 1].cardIndex && list[i].cardIndex == mCardInfo.cardIndex
                && list[i + 2].cardIndex == list[i + 1].cardIndex)
            {
                n = list[i].cardIndex;
                break;
            }
        }

        return n;
    }

    /// <summary>
    /// 判断是否能胡牌
    /// </summary>
    /// <param name="list"></param>
    /// <param name="mCardInfo"></param>
    /// <returns></returns>
    public static bool IsCanHU(List<MCardInfo> list, MCardInfo mCardInfo)
    {
        List<int> pais = new List<int>();
        for (int i = 0; i < list.Count; i++)
        {
            pais.Add(list[i].cardIndex);
        }
        if (mCardInfo != null)
            pais.Add(mCardInfo.cardIndex);

        //只有两张牌
        if (pais.Count == 2)
        {
            return pais[0] == pais[1];
        }

        //先排序
        pais.Sort();

        //依据牌的顺序从左到右依次分出将牌
        for (int i = 0; i < pais.Count; i++)
        {
            List<int> paiT = new List<int>(pais);
            List<int> ds = pais.FindAll(delegate (int d)
            {
                return pais[i] == d;
            });

            //判断是否能做将牌
            if (ds.Count >= 2)
            {
                //移除两张将牌
                paiT.Remove(pais[i]);
                paiT.Remove(pais[i]);

                //避免重复运算 将光标移到其他牌上
                i += ds.Count;

                if (HuPaiPanDin(paiT))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool HuPaiPanDin(List<int> mahs)
    {
        if (mahs.Count == 0)
        {
            return true;
        }

        List<int> fs = mahs.FindAll(delegate (int a)
        {
            return mahs[0] == a;
        });

        //组成克子
        if (fs.Count == 3)
        {
            mahs.Remove(mahs[0]);
            mahs.Remove(mahs[0]);
            mahs.Remove(mahs[0]);

            return HuPaiPanDin(mahs);
        }
        else
        { //组成顺子
            if (mahs.Contains(mahs[0] + 1) && mahs.Contains(mahs[0] + 2))
            {
                mahs.Remove(mahs[0] + 2);
                mahs.Remove(mahs[0] + 1);
                mahs.Remove(mahs[0]);

                return HuPaiPanDin(mahs);
            }
            return false;
        }
    }


    /// <summary>
    /// 排序当前手牌
    /// </summary>
    private static List<MCardInfo> SortCards(List<MCardInfo> list)
    {
        if (list.Count == 0)
            return null;

        list.Sort(delegate (MCardInfo x, MCardInfo y)
        {
            return x.cardIndex.CompareTo(y.cardIndex);
        });

        return list;
    }



}
