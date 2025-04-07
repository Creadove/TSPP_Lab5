using System;
using System.Collections.Generic;

//Singleton
public class CacheManager
{
    private static readonly Lazy<CacheManager> _instance = new(() => new CacheManager());
    private Dictionary<string, string> _cache = new();

    private CacheManager() { }

    public static CacheManager Instance => _instance.Value;

    public void Add(string key, string value) => _cache[key] = value;
    public string Get(string key) => _cache.TryGetValue(key, out var value) ? value : "Not found";
}

//Adapter
public interface IDatabase
{
    void Connect();
    void Query(string sql);
}

public class MySQLAdapter : IDatabase
{
    public void Connect() => Console.WriteLine("Connected to MySQL");
    public void Query(string sql) => Console.WriteLine($"MySQL executing: {sql}");
}

public class PostgreSQLAdapter : IDatabase
{
    public void Connect() => Console.WriteLine("Connected to PostgreSQL");
    public void Query(string sql) => Console.WriteLine($"PostgreSQL executing: {sql}");
}

public class SQLiteAdapter : IDatabase
{
    public void Connect() => Console.WriteLine("Connected to SQLite");
    public void Query(string sql) => Console.WriteLine($"SQLite executing: {sql}");
}

//Observer
public class Blog
{
    public event Action<string> ArticlePublished;
    public void PublishArticle(string title)
    {
        Console.WriteLine($"New blog post: {title}");
        ArticlePublished?.Invoke($"New article published: {title}");
    }
}

public class BlogUser
{
    private string _name;

    public BlogUser(string name, Blog blog)
    {
        _name = name;
        blog.ArticlePublished += ReceiveNotification;
    }

    private void ReceiveNotification(string message)
    {
        Console.WriteLine($"{_name} received notification: {message}");
    }
}

//Test
class Program
{
    static void Main()
    {
        //Singleton test
        var cache = CacheManager.Instance;
        cache.Add("user", "Dima Vinnitskiy");
        Console.WriteLine(cache.Get("user"));

        //Adapter test
        IDatabase db = new MySQLAdapter();
        db.Connect();
        db.Query("SELECT * FROM users");

        //Observer test
        var blog = new Blog();
        var user1 = new BlogUser("Free fight", blog);
        var user2 = new BlogUser("Gym", blog);

        blog.PublishArticle("New schedule");
    }
}