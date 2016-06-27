
public class Rootobject
{
    public Ampersand Ampersand { get; set; }
    public Entity entity { get; set; }
}

public class Ampersand
{
    public Same Same { get; set; }
    public Block Block { get; set; }
}

public class Same
{
    public string username { get; set; }
    public string password { get; set; }
    public int type { get; set; }
}

public class Block
{
    public Greaterthan GreaterThan { get; set; }
    public Lessthan LessThan { get; set; }
}

public class Greaterthan
{
    public int point { get; set; }
}

public class Lessthan
{
    public int maney { get; set; }
}

public class Entity
{
    public User[] user { get; set; }
    public Task[] task { get; set; }
}

public class User
{
    public string header { get; set; }
}

public class Task
{
    public string header { get; set; }
}
