# 将json转换为MySql语句

例如下面的一段json：
``` json
{
  "Entity": {
    "Main": {
      "note_details": true
    }
  },
  "Condition": {
    "Equ": {
      "note_details": {
        "title": "hello,world."
      }
    }
  }
}

```
转换为MySql语句如下：
```  sql
SELECT `note_details`.`id`,`note_details`.`title`,`note_details`.`content`,`note_details`.`weather`,`note_details`.`create_time`,`note_details`.`update_time`,`note_details`.`modified_time`,`note_details`.`valid` FROM   `note_details`    WHERE  `note_details`.`title` = 'hello,world.' 
```

使用C#转换

``` cs
var jsonEntity = new JsonEntityToSql("server=127.0.0.1;user id=root;password=123456;persistsecurityinfo=True;database=one_note");

var sql = jsonEntity.Json2SelectSql("{\"Entity\":{\"Main\":{\"note_details\":true}},\"Condition\":{\"Equ\":{\"note_details\":{\"title\":\"hello,world.\"}}}}");
```

SQL操作符对应的JSON变量：



SQL的关键字|JSON对应的变量
-------- | ---
=|	Equ
&lt;&gt;|	NotEqu
&gt;	|Greater
&lt;	|Less
&gt;= | GreaterEqu
&lt;=|	LessEqu
BETWEEN AND|	Between
NOT BETWEEN AND|	NotBetween
LIKE|	Like
NOT LIKE	|NotLike
AND	|And
OR|	Or
()|	Block
ORDER BY|	Order
INNER JOIN	|Link
LIMIT|	Limit
LEFT JOIN	|LinkLeft
RIGHT JOIN|	LinkRight

由于工作时间的缘故，目前还没有时间，更复杂的JSON转SQL文档，以后有时间会发布出来。
下面是较复杂的json示例
``` json
{
  "Entity": {
    "Main": {
      "note_details": {
        "id": true,
        "title": true,
        "content": "descript"
      },
      "Link": {
        "user": {
          "realname": true
        }
      }
    },
    "LinkCondition": {
      "Equ": {
        "note_details": {
          "user_id": "$`user`.id"
        }
      }
    }
  },
  "Condition": {
    "Equ": {
      "note_details": {
        "title": "日记标题"
      }
    }
  }
}
```
