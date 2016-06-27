namespace Json2Sql
{
    public class Condition
    {
        #region 描述
        /*
        相等 Same 
       不等于 Different
       大于 GreaterThan
       小于 LessThan 
       大于等于 GreaterThanAndSame
       小于等于 LessThanAndSame
       在某个范围内 Between .. AND ..
       不在某个范围内 NOT Between .. AND ..
       LIKE Possible
       NOT LIKE    NotPossible
       IN
            */
        #endregion

        #region 操作符定义
        public const string IsOperator = " {0} is {1}";
        public const string SameOperator = " {0} = {1}";
        public const string GreaterThanOperator = " {0} > {1}";
        public const string DifferentOperator = " {0} <> {1}";
        public const string LessThanOperator = " {0} <= {1}";
        public const string GreaterThanAndSameOperator = " {0} >= {1}";
        public const string LessThanAndSameOperator = " {0} <= {1}";
        public const string BetweenOperator = " {0} BETWEEN {1} AND {2} ";
        public const string NotBetweenOperator = " {0} NOT BETWEEN {1} AND {2} ";
        public const string PossibleOperator = " {0} LIKE {1}";
        public const string NotPossibleOperator = " {0} NOT LIKE {1}";
        public const string InOperator = " {0} IN ({1})";
        public const string AmpersandOperator = " AND ";
        public const string OrOperator = " OR ";
        public const string OrderOperator = " ORDER BY ";
        public const string LimitOperator = " LIMIT  ";
        public const string InnerJoinOperator = " INNER JOIN ";
        public const string LeftJoinOperator = " LEFT JOIN ";
        public const string RightJoinOperator = " RIGHT JOIN ";
        public const string GroupOperator = " GROUP BY ";
        #endregion

        #region 名称定义

        public const string IsName = "Is";
        public const string SameName = "Equ";
        public const string GreaterThanName = "Greater";
        public const string DifferentName = "NotEqu";
        public const string LessThanName = "Less";
        public const string GreaterThanAndSameName = "GreaterEqu";
        public const string LessThanAndSameName = "LessEqu";
        public const string BetweenName = "Between";
        public const string NotBetweenName = "NotBetween";
        public const string PossibleName = "Like";
        public const string NotPossibleName = "NotLike";
        public const string InName = "In";
        public const string BlockName = "Block";
        public const string AmpersandName = "And";
        public const string OrName = "Or";
        public const string OrderName = "Order";
        public const string LimitName = "Limit";
        public const string GroupName = "Group";
        public const string InnerJoinName = "Link";
        public const string LeftJoinName = "LinkLeft";
        public const string RightJoinName = "LinkRight";

        #endregion

        #region JsonEntity名称
        public const string Entity = "Entity";

        #region Entity子级名称

        public const string Main = "Main";
        public const string Link = "Link";
        public const string LinkLeft = "LinkLeft";
        public const string LinkRight = "LinkRight";
        public const string LinkCondition = "LinkCondition";
        #endregion

        public const string Condition_ = "Condition";

        #endregion


    }
}
