﻿using Aron.TintanMiner.Console.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Aron.TintanMiner.Console
{
    internal class Program
    {
        private static AppConfig appConfig;

        private static string currentPath = System.IO.Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {

            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            appConfig = new Models.AppConfig();
            Configuration.Bind("AppConfig", appConfig);

            if (string.IsNullOrEmpty(appConfig.TITAN_STORAGE_PATH))
            {
                appConfig.TITAN_STORAGE_PATH = "/root/.titanedge";
            }





            Process? process = null;
            Thread.Sleep(20000);


            try
            {
                // run titan/titan-edge/titan-edge daemon start --init --url https://test-locator.titannet.io:5000/rpc/v0
                Task task = Task.Run(() =>
                {
                    process = Run($"daemon start --init --url {appConfig.TITAN_NETWORK_LOCATORURL}");
                    process.WaitForExit();

                    Thread.Sleep(5000);
                    System.Console.WriteLine("Exit Code: " + process.ExitCode);

                    if (appConfig.DELETE_STORAGE_AFTER_EXIT)
                    {
                        string command = $"rm -rf \"{Path.Combine(appConfig.TITAN_STORAGE_PATH, "*")}\"";
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/bash",
                            Arguments = $"-c \"{command}\"",
                            RedirectStandardOutput = false,
                            RedirectStandardError = false,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process process = Process.Start(startInfo);
                        System.Console.WriteLine(startInfo.Arguments);
                        process.WaitForExit();
                        process.Dispose();

                    }
                });

                Thread.Sleep(20000);



                process = Run($"bind --hash={appConfig.TITAN_EDGE_ID} {appConfig.TITAN_EDGE_BINDING_URL}");
                System.Console.WriteLine($"bind --hash={appConfig.TITAN_EDGE_ID} {appConfig.TITAN_EDGE_BINDING_URL}");



                task.Wait();
            }
            catch (System.Exception)
            {
                throw;
            }   
            finally
            {
                //process = Run("daemon stop");
                process?.Kill();
                process?.Dispose();
            }
            
            


        }

        private static Process Run(string args)
        {

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "titan/titan-edge",
                Arguments = args,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            startInfo.EnvironmentVariables["TITAN_STORAGE_STORAGEGB"] = appConfig.TITAN_STORAGE_STORAGEGB.ToString();
            startInfo.EnvironmentVariables["TITAN_STORAGE_PATH"] = appConfig.TITAN_STORAGE_PATH;
            startInfo.EnvironmentVariables["TITAN_NETWORK_LOCATORURL"] = appConfig.TITAN_NETWORK_LOCATORURL;
            startInfo.EnvironmentVariables["TITAN_CPU_CORES"] = appConfig.TITAN_CPU_CORES.ToString();
            startInfo.EnvironmentVariables["TITAN_MEMORY_MEMORYGB"] = appConfig.TITAN_MEMORY_MEMORYGB.ToString();

          
            return Process.Start(startInfo);
        }

        private static string RunAndGetOutput(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "titan/titan-edge",
                Arguments = args,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            startInfo.EnvironmentVariables["TITAN_STORAGE_STORAGEGB"] = appConfig.TITAN_STORAGE_STORAGEGB.ToString();
            startInfo.EnvironmentVariables["TITAN_STORAGE_PATH"] = appConfig.TITAN_STORAGE_PATH;
            startInfo.EnvironmentVariables["TITAN_NETWORK_LOCATORURL"] = appConfig.TITAN_NETWORK_LOCATORURL;
            startInfo.EnvironmentVariables["TITAN_CPU_CORES"] = appConfig.TITAN_CPU_CORES.ToString();
            startInfo.EnvironmentVariables["TITAN_MEMORY_MEMORYGB"] = appConfig.TITAN_MEMORY_MEMORYGB.ToString();

            Process process = Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }




    }
}
