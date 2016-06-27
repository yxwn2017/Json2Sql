namespace Json2Sql
{

    public interface IJsonSql
    {

        string Json2InsertSql(string json);


        string Json2UpdateSql(string json);

        string Json2DeleteSql(string json);


        string Json2SelectSql(string json);
       
    }
}
