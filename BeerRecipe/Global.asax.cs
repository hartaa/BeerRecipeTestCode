using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace BeerRecipe
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ConfigureMongo();
        }

        protected void Application_End()
        {
            KillMongoD();
        }

        protected void ConfigureMongo()
        {
            try
            {
                var outputDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                
                if (outputDir == null) 
                    throw new Exception("Cannot get current executing assembly directory");
                
                outputDir = new Uri(outputDir).LocalPath;
                var root = Path.GetPathRoot(outputDir);
                var mongoDataDir = root + @"\data\db";
                var mongodExe = Path.Combine(outputDir, "mongod.exe");

                if (!File.Exists(mongodExe))
                    throw new Exception("Cannot locate mongod.exe");


                if (!Directory.Exists(mongoDataDir))
                    Directory.CreateDirectory(mongoDataDir);

                StartMongoD(mongodExe, mongoDataDir);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static void StartMongoD(string exePath, string mongoDataDir)
        {
            try
            {
                if (IsMongoStarted()) return;

                //Then mongod is not running so let's start it.
                var start = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = @"--dbpath " + mongoDataDir
                };

                var mongod = Process.Start(start);
            }
            catch (Exception e)
            {
                throw new Exception("Could not start mongod.exe");
            }
        }

        private static void KillMongoD()
        {
            var processes = GetMongoProcesses();

            //Kill mongo processes
            if (processes != null)
                processes.ToList().ForEach(p => p.Kill());
        }

        private static bool IsMongoStarted()
        {
            var pname = GetMongoProcesses();

            return pname.Length != 0;
        }

        private static Process[] GetMongoProcesses()
        {
            var pname = Process.GetProcessesByName("mongod");
            return pname;
        }
    }
}
