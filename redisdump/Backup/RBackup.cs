using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redisdump
{
    public class RBackup
    {
        private static Redis _client = null;

        public static Redis GetClient(ApplicationArguments options)
        {
            if (_client != null)
                return _client;

            _client = new Redis(options.Host, options.Port);
            _client.Password = options.Password;

            try
            {
                _client.Db = options.DbID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Setting database: " + ex.Message);
                Environment.Exit(1);
            }
            return _client;
        }

        internal static void DoBackup(ApplicationArguments options)
        {
            Redis redis = GetClient(options);
            string[] keys = new string[] { };
            try
            {
                keys = redis.GetKeys("*");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Getting Keys: " + ex.Message);
                Environment.Exit(1);
            }

            RBackup backup = new RBackup();

            foreach (string key in keys)
            {
                try
                {
                    int ttl = redis.TimeToLive(key);
                    Redis.KeyType type = redis.TypeOf(key);
                    switch (type)
                    {
                        case Redis.KeyType.String:
                            string rstring = Encoding.UTF8.GetString(redis.Get(key));
                            backup.Strings.Add(new RString()
                            {
                                Data = rstring,
                                TTL = ttl,
                                Key = key
                            });
                            break;
                        case Redis.KeyType.List:
                            List<string> rlist = new List<string>();
                            foreach (byte[] row in redis.ListRange(key, 0, -1))
                            {
                                rlist.Add(Encoding.UTF8.GetString(row));
                            }
                            backup.Lists.Add(new RList()
                            {
                                Data = rlist,
                                TTL = ttl,
                                Key = key
                            });
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Error getting key {0}: {1}", key, ex.Message));
                }
            }

            string json = JsonConvert.SerializeObject(backup, Formatting.Indented);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(options.File))
            {
                file.Write(json);
            }
        }

        internal static void DoRestore(ApplicationArguments options)
        {
            string json = "";
            using (System.IO.StreamReader file = new System.IO.StreamReader(options.File))
            {
                json = file.ReadToEnd();
            }
            RBackup backup = null;
            try
            {
                backup = JsonConvert.DeserializeObject<RBackup>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading json backup: " + ex.Message);
                Environment.Exit(1);
            }
            Console.WriteLine(String.Format("Restoring backup from {0}. You have 5 seconds to cancel!", backup.BackupTime));
            Thread.Sleep(5000);

            Redis redis = GetClient(options);

            Console.WriteLine("Removing all entries from Redis");
            ClearDB(redis);

            foreach (RString data in backup.Strings)
            {
                Console.WriteLine(String.Format("Writing string key: {0}. TTL: {2}. Data: {1}", data.Key, data.Data, data.TTL));
                data.SaveTo(redis);
            }

            foreach (RList list in backup.Lists)
            {
                Console.WriteLine(String.Format("Writing list key: {0}. TTL: {1}", list.Key, list.TTL));
                list.SaveTo(redis);
            }

        }

        private static void ClearDB(Redis redis)
        {
            string[] keys = new string[] { };
            try
            {
                keys = redis.GetKeys("*");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Getting Keys: " + ex.Message);
                Environment.Exit(1);
            }

            foreach (string key in keys)
            {
                redis.Remove(key);
            }
            Console.WriteLine(String.Format("Removed {0} entries from Redis", keys.Length));
        }

        public DateTime BackupTime { get; set; }
        public List<RString> Strings { get; set; }
        public List<RList> Lists { get; set; }

        public RBackup()
        {
            BackupTime = DateTime.Now;
            Strings = new List<RString>();
            Lists = new List<RList>();
        }

    }
}
