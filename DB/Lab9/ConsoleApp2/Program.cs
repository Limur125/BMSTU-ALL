using Redis.OM;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Data.Linq;
using Redis.OM.Modeling;
using Redis.OM.Searching;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System.Windows.Forms.DataVisualization;
using StackExchange.Redis;
using static ServiceStack.Script.Lisp;

internal class Program
{
    static string connectionString = "Data Source=localhost,1436;Initial Catalog=VideoGames;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    static DataContext sqlServer = new DataContext(connectionString);
    static RedisConnectionProvider redis = new RedisConnectionProvider("redis://localhost:6379");
    private static void Main(string[] args)
    {
        var sales = GetTop10();
        TimerCallback timerCallbackSelectFromSqlServer = new TimerCallback(SelectFromSqlServerTimerCallback);
        //SelectFromSqlServerTimerCallback(null);
        Timer timerSqlServer = new Timer(timerCallbackSelectFromSqlServer, null, 0, 5000);
        TimerCallback timerCallbackSelectFromRedis = new TimerCallback(SelectFromRedisTimerCallback);
        //SelectFromRedisTimerCallback(null);
        Timer timerRedis = new Timer(timerCallbackSelectFromSqlServer, null, 0, 5000);
        foreach (var s in sales)
            Console.WriteLine($"{s.title} {s.na_sales} {s.eu_sales} {s.jp_sales} {s.other_sales} {s.global_sales}");
    }
    static IEnumerable<GameSales> GetTop10()
    {
        using RedisClient redisClient = new RedisClient("localhost", 6379);
        IRedisTypedClient <GameSales> gameSalesR = redisClient.As<GameSales>();
        IList<GameSales> gameSalesData = gameSalesR.GetAll();
        if (!gameSalesData.Any())
        {
            var gameSalesSqlServer = from gs in sqlServer.GetTable<GameSales>() select gs;
            redisClient.StoreAll(gameSalesSqlServer);
        }
        gameSalesData = gameSalesR.GetAll();
        var sales = (from gs in gameSalesData
                     orderby gs.global_sales descending
                     select gs).Take(10);
        return sales;
    }
    static void SelectFromSqlServerTimerCallback(object? obj)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        SqlCommand command = new SqlCommand(
            "select * from GameSales " +
            "where gloabl_sales > 40", connection);
        Console.WriteLine("SqlServer select");
    }
    static void SelectFromRedisTimerCallback(object? obj)
    {
        using RedisClient redisClient = new RedisClient("localhost", 6379);
        IRedisTypedClient<GameSales> gameSalesR = redisClient.As<GameSales>();
        IList<GameSales> gameSalesData = gameSalesR.GetAll();
        if (!gameSalesData.Any())
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(
                "select * from GameSales " +
                "where gloabl_sales > 40", connection);
            SqlDataReader r = command.ExecuteReader();
            if (r.HasRows)
            {
                while (r.Read())
                {
                    GameSales gameSale = new GameSales();
                    gameSale.title = (string)r["title"];
                    gameSale.na_sales = decimal.Parse(r["na_sales"].ToString());
                    gameSale.eu_sales = decimal.Parse(r["eu_sales"].ToString());
                    gameSale.jp_sales = decimal.Parse(r["jp_sales"].ToString());
                    gameSale.other_sales = decimal.Parse(r["other_sales"].ToString());
                    gameSale.global_sales = decimal.Parse(r["global_sales"].ToString());
                    gameSalesData.Add(gameSale);
                }
            }
            redisClient.StoreAll(gameSalesData);
        }
        gameSalesData = gameSalesR.GetAll();
        var sales = from gs in gameSalesData
                    where gs.global_sales > 40
                    select gs;
        Console.WriteLine("Redis select");
    }
}

[Table]
class GameSales
{
    [Column(IsPrimaryKey = true)]
    public string title { get; set; }
    [Column]
    public decimal na_sales { get; set; }
    [Column]
    public decimal eu_sales { get; set; }
    [Column]
    public decimal jp_sales { get; set; }
    [Column]
    public decimal other_sales { get; set; }
    [Column]
    public decimal global_sales { get; set; }
}

/*
 * 
class spacecraft_listener(object):
  
  def __init__(self):
    self.redis = redis.Redis()
    self.sub = self.redis.pubsub()
    self.sub.psubscribe(['channel1'])

  def listen(self):
    for blob in self.sub.listen():
      if blob['type'] == "message":
        data    = json.loads(blob['data'])
        print(data)
        sender  = data['data']['sender']
        channel = data['data']['channel']
        text    = data['data']['message']
        



# 2. Приложение выполняет запрос каждые 5 секунд через Redis в качестве кэша.
def task_03(cur, id):
    threading.Timer(5.0, task_02, [cur, id]).start()
    
    redis_client = redis.Redis(host="localhost", port=6379, db=0)
    
    # sub = spacecraft_listener()
    # sub.listen()

    cache_value = redis_client.get("hotels_cityid_" + str(id))
    if cache_value is not None:
        redis_client.close()
        return json.loads(cache_value)

    cur.execute("select *\
                   from packages.hotels\
                   where cityd = %s;", (id, ))

    result = cur.fetchall()
    data = json.dumps(result)
    print("FFFFFFFFFFFF")
    redis_client.set("hotels_cityid_" + str(id), data)
    redis_client.close()

    return result


def dont_do(cur):
    # print("simple\n")
    # threading.Timer(10.0, dont_do, [cur]).start()
    redis_client = redis.Redis(host="localhost", port=6379, db=0)

    t1 = time()
    cur.execute("select *\
                   from hotel_copy\
                   where hotelid = 1;")
    t2 = time()

    result = cur.fetchall()

    data = json.dumps(result)
    cache_value = redis_client.get("h1")
    if cache_value is not None:
        pass
    else:
        redis_client.set("h1", data)

    
    t11 = time()
    redis_client.get("h1")
    t22 = time()

    redis_client.close()

    return t2 - t1, t22 - t11


def del_tour(cur, con):
    redis_client = redis.Redis()
    # print("delete\n")
    # threading.Timer(10.0, del_tour, [cur, con]).start() 

    hid = randint(1, 1000)

    t1 = time()
    cur.execute("delete from public.hotel_copy\
         where hotelid = %s;", (hid, ))
    t2 = time()

    t11 = time()
    redis_client.delete("h"+str(hid))
    t22 = time()

    redis_client.close()
    
    con.commit()

    return t2-t1, t22-t11

def ins_tour(cur, con):
    redis_client = redis.Redis()
    # print("insert\n")
    # threading.Timer(10.0, ins_tour, [cur, con]).start() 

    hid = randint(1, 1000)
    
    t1 = time()
    cur.execute("insert into public.hotel_copy values(%s, 'AAA', 33, 'AAARegion', 2);", (hid, ))
    t2 = time()

    cur.execute("select * from public.hotel_copy\
         where hotelid = %s;", (hid, ))
    result = cur.fetchall()

    data = json.dumps(result)
    t11 = time()
    redis_client.set("h"+str(hid), data)
    t22 = time()

    redis_client.close()
    
    con.commit()

    return t2-t1, t22-t11

def upd_tour(cur, con):

    redis_client = redis.Redis()
    # print("update\n")
    # threading.Timer(10.0, upd_tour, [cur, con]).start() 

    hid = randint(1000, 2000)
    
    t1 = time()
    cur.execute("UPDATE hotel_copy SET cityd = 1 WHERE hotelid = %s;", (hid, ))
    t2 = time()

    cur.execute("select * from public.hotel_copy\
         where hotelid = %s;", (hid, ))

    result = cur.fetchall()
    data = json.dumps(result)

    t11 = time()
    redis_client.set("h"+str(hid), data)
    t22 = time()

    redis_client.close()
    
    con.commit()

    return t2-t1, t22-t11

# гистограммы
def task_04(cur, con):
    # simple 
    t1 = 0
    t2 = 0
    for i in range(1000):
        b1, b2 = dont_do(cur)
        t1 += b1
        t2 += b2
    print("simple 100 db redis", t1 / 1000, t2 / 1000)
    index = ["БД", "Redis"]
    values = [t1 / 1000, t2/ 1000]
    plt.bar(index,values)
    plt.title("Без изменения данных")
    plt.show()

    # delete 
    t1 = 0
    t2 = 0
    for i in range(1000):
        b1, b2 = del_tour(cur, con)
        t1 += b1
        t2 += b2
    print("delete 100 db redis", t1 / 1000, t2 / 1000)

    index = ["БД", "Redis"]
    values = [t1 / 1000, t2/ 1000]
    plt.bar(index,values)
    plt.title("При добавлении новых строк каждые 10 секунд")
    plt.show()

    # insert 
    t1 = 0
    t2 = 0
    for i in range(1000):
        b1, b2 = ins_tour(cur, con)
        t1 += b1
        t2 += b2
    print("ins_tour 100 db redis", t1 / 1000, t2 / 1000)

    index = ["БД", "Redis"]
    values = [t1 / 1000, t2/ 1000]
    plt.bar(index,values)
    plt.title("При удалении строк каждые 10 секунд")
    plt.show()

    # updata 
    t1 = 0
    t2 = 0
    for i in range(1000):
        b1, b2 = upd_tour(cur, con)
        t1 += b1
        t2 += b2
    print("updata 100 db redis", t1 / 1000, t2 / 1000)

    index = ["БД", "Redis"]
    values = [t1 / 1000, t2/ 1000]
    plt.bar(index,values)
    plt.title("При изменении строк каждые 10 секунд")
    plt.show()


def do_cache(cur):
    redis_client = redis.Redis(host="localhost", port=6379, db=0)

    for id in range(1000):
        cache_value = redis_client.get("h" + str(id))
        if cache_value is not None:
            redis_client.close()
            return json.loads(cache_value)

        cur.execute("select *\
                    from packages.hotels\
                    where hotelid = %s;", (id, ))

        result = cur.fetchall()
        print("FFFFFFFFFFFF")
        redis_client.set("h" + str(id), json.dumps(result))
        redis_client.close()

    return result

if __name__ == '__main__':

    #do_cache(cur)


    print("1. Отели категории 1 (задание 2)\n"
          "2. Приложение выполняет запрос каждые 5 секунд на стороне БД. (задание 3.1)\n"
          "3. Приложение выполняет запрос каждые 5 секунд через Redis вкачестве кэша. (задание 3.2)\n"
          "4. Гистограммы (задание 3.3)\n\n"
    ) 

    con = connection()
    cur = con.cursor()

    while True:
        c = int(input("Выбор: "))

        if c == 1:
            res = get_hotel_1(cur)

            for elem in res:
                print(elem)

        elif c == 2:
            citid = int(input("ID города (от 0 до 1000): "))

            res = task_02(cur, citid)

            for elem in res:
                print(elem)

        elif c == 3:
            citid = int(input("ID города (от 0 до 1000): "))

            res = task_03(cur, citid)

            for elem in res:
                print(elem)

        elif c == 4:
            task_04(cur, con)
        else:
            print("Ошибка\n")
            break

    cur.close()

    print("BY!")
 */