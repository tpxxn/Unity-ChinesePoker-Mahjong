//单张1，对子2，三张3，三带单4，三带对5，单顺6，双顺7，飞机8，飞机带单9，飞机带双10，四带两单11，四带对12，炸弹13，火箭14
public enum DDZ_POKER_TYPE
{
    DDZ_PASS = 0,   //过牌，不出  
    SINGLE = 1,
    TWIN = 2,
    TRIPLE = 3,
    TRIPLE_WITH_SINGLE = 4,
    TRIPLE_WITH_TWIN = 5,
    STRAIGHT_SINGLE = 6,
    STRAIGHT_TWIN = 7,
    PLANE_PURE = 8,
    PLANE_WITH_SINGLE = 9,
    PLANE_WITH_TWIN = 10,
    FOUR_WITH_SINGLE = 11,
    FOUR_WITH_TWIN = 12,
    FOUR_BOMB = 13,
    KING_BOMB = 14,
    NONE = 15,
}