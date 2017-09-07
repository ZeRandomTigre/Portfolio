using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ACW_08346_464814_ServiceLibrary;
using System.Threading;
using System.Security.Cryptography;

namespace ACW_08346_464814_Server
{
    class Program
    {
        static bool IS_TEST = false;
                
        static void Main(string[] args)
        {
            string[] consoleInput = new string[8];

            // add test server input
            if (IS_TEST == true)
            {
                consoleInput[0] = "0c5c750943dcfa8545fc926d07d2a2148a146a8d0359e22426e7dd00422820dcfd0ffe1454bbd83e821dbfde8916f3f92cef7c0ac37b8fdde431dd62d4cefc91b6633e342711db86a03e53e025ad1b2b5fd98ca7533ca76b2d1fecc85e5cc6921172804737eed46e38cd08f9aa845136e4895899bc9581b8ab80a37a217871cd";
                consoleInput[1] = "3d8a50d650ae55aaedba2276ffe8976e256b601b59d4a13a77562345dee9ba250a5f210814ece48864ea9f16c9ab0c9834f5967ee59bafd5bf057e2e9761b96d";
                consoleInput[2] = "96ccd1e1b2bad0a2c030af827cf721d0804c466feb3109f1308084f4ac9406eccbef08d8401a806bd5896509edd0e4d4aee873c2d4697f93af6661ffe314df0d";
                consoleInput[3] = "010001";
                consoleInput[4] = "4a3376fe05461bb23d380ba94f9da7127fc2ff83df4e81759781ec7f8d69b2ce396bcdae1e62719e68338a85294b8847f628a22c96afb299c36f10027b610685";
                consoleInput[5] = "8ac707555bb033abe8d68dc38923cd87fa40ccf34cc337acb8fa7dffbe6929e8726a4f2bde3d652a66aca772d4b718476f4f38d301a8a84a07669436ea0afaed686af06f14df35119071b4b27cfafe55b923652a32a83ec1dc63426f348062c4bdfa7e3d4ba81b344b1e067c83f6fa1551f9a7b75cc51b9577969228e40bb8df";
                consoleInput[6] = "b724818dad3d4a2842d4d3f496beaf3d69685690f5b3440a6c1df9c4ecdc6ad71c341e4ee28de2a0abb58e23f3d6b050bd810d30269fc985170808f9abcb4793";
                consoleInput[7] = "c1fc50b0fa425ace79728263e2cdc7deed58c1dbe1314b5410071a35e24963e1f744186ac5b884f7fec7b5b07dcbb46f3a73126c252f759f4452acef96dd4105";
            }
            else
            {
                // read input for server
                for (int i = 0; i < consoleInput.Length; i++)
                {
                    consoleInput[i] = Console.ReadLine();
                }
            }


            // start server and serivce instance
            ACW_Service ACW_Service_Instance = new ACW_Service();
            ServiceHost ACW_Server = new ServiceHost(ACW_Service_Instance);

            // initialise RSA Parameters from server input
            ACW_Service_Instance.initRSAParams(consoleInput);

            // opern server
            ACW_Server.Open();

            Console.WriteLine("Server running...");
            
            // start new thread, wait 10 seconds then close server
            ParameterizedThreadStart ts = new ParameterizedThreadStart(ServerListening);
            Thread t = new Thread(ts);
            t.Start(ACW_Server);
        }
        
        private static void ServerListening(object param)
        {
            ServiceHost ACW_Server = (ServiceHost)param;

            if (IS_TEST == true)
                Thread.Sleep(60000);
            else
                Thread.Sleep(9999);

            ACW_Server.Abort();
        }
    }
}
