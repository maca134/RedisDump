using Fclp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redisdump
{
    class RedisDump
    {
        static void Main(string[] args)
        {
            FluentCommandLineParser<ApplicationArguments> p = new FluentCommandLineParser<ApplicationArguments>();

            p.Setup(arg => arg.File).As('f', "file").Required();
            p.Setup(arg => arg.Backup).As('b', "backup").SetDefault(false);
            p.Setup(arg => arg.Restore).As('r', "restore").SetDefault(false);

            p.Setup(arg => arg.Host).As('h', "host").SetDefault("localhost");
            p.Setup(arg => arg.Port).As('p', "port").SetDefault(6379);
            p.Setup(arg => arg.Password).As('w', "password").SetDefault("");
            p.Setup(arg => arg.DbID).As('d', "db").SetDefault(0);

            var result = p.Parse(args);

            if (result.HasErrors == true)
            {
                Console.WriteLine("Dumps and restores redis databases");
                Console.WriteLine("");
                Console.WriteLine("\t--file\t\tSets the backup/restore file.");
                Console.WriteLine("\t--host\t\tSets redis host.");
                Console.WriteLine("\t--port\t\tSets redis port.");
                Console.WriteLine("\t--password\t\tSets redis password.");
                Console.WriteLine("\t--db\t\tSets redis DB ID");
                Console.WriteLine("\t--backup\t\tPerforms a backup");
                Console.WriteLine("\t--restore\tPerforms a restore");
                Console.WriteLine("");
                Console.WriteLine("Examples:");

                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " /db 1 /file=\"redis.dump\" /backup");
                Console.WriteLine(System.AppDomain.CurrentDomain.FriendlyName + " /file=\"redis.dump\" /restore");
                Environment.Exit(0);
            }
            ApplicationArguments options = p.Object as ApplicationArguments;

            if ((!options.Restore && !options.Backup) || (options.Restore && options.Backup))
            {
                Console.WriteLine("Backup and restore are both set or neither are set.");
                Environment.Exit(0);
            }

            if (options.Backup)
            {
                try
                {
                    RBackup.DoBackup(options);
                    Console.WriteLine("Backup complete");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error doing backup: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    RBackup.DoRestore(options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error doing backup: " + ex.Message);
                }
            }
        }
    }
}
