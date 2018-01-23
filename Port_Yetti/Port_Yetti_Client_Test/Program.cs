using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Stress_Client
{
    class Program
    {
        // web api client
        public static HttpClient client = new HttpClient();

        // used to print out multiple threads working states
        // conccurent so more thread safe
        public static ConcurrentDictionary<int, string> threadDictionary = new ConcurrentDictionary<int, string>();

        public static ConcurrentDictionary<int, string> threadPrint = new ConcurrentDictionary<int, string>();

        static int numberOfThreads = 4;
        static int numberOfTests = 10000;
        static int numberOfRepeats = 5;

        public static int threadsFinished = 0;
        public static Object threadLock = new Object();

        static bool[] isThreadsWorking;       

        static ManualResetEvent manualEvent;

        static StringBuilder outputFile = new StringBuilder();

        static void Main(string[] args)
        {
            // Adding JSON file into IConfiguration
            var config = new ConfigurationBuilder()  
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            // Read http server address from IConfig
            string serverAddress = config["ServerAddress"];
            numberOfThreads = int.Parse(config["NumberOfThreads"]);
            numberOfTests = int.Parse(config["NumberofTests"]);
            numberOfRepeats = int.Parse(config["NumberofRepeats"]);

            isThreadsWorking = new bool[numberOfThreads];

            // init http client
            client.BaseAddress = new Uri(serverAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // start stress tests
            Console.WriteLine("Press enter to start stress testing...");
            Console.ReadLine();

            outputFile.AppendLine("Server: " + serverAddress);
            outputFile.AppendLine("NumberOfThreads: " + numberOfThreads);
            outputFile.AppendLine("NumberOfTests: " + numberOfTests);
            outputFile.AppendLine("NumberOfRepeats: " + numberOfRepeats);

            for (int i = 0; i < numberOfRepeats; i++)
            {
                outputFile.AppendLine();
                outputFile.AppendLine("Stress Service");
                outputFile.AppendLine("ApiCall,Total Time,Average Time");

                StressService("post");
                StressService("getall");
                StressService("put");
                StressService("getbyid");
                StressService("getbyname");

                outputFile.AppendLine();
                outputFile.AppendLine("Stress SettingName");
                outputFile.AppendLine("ApiCall,Total Time,Average Time");

                StressSettingName("post");
                StressSettingName("getall");
                StressSettingName("put");
                StressSettingName("getbyid");
                StressSettingName("getbyname");

                outputFile.AppendLine();
                outputFile.AppendLine("Stress Setting");
                outputFile.AppendLine("ApiCall,Total Time,Average Time");

                StressSetting("post");
                StressSetting("getall");
                StressSetting("put");
                StressSetting("getbyid");
                StressSetting("getbyvalue");
                StressSetting("getbysettingidserviceid");

                StartServiceThread(numberOfThreads);
                StartSettingNameThread(numberOfThreads);
                StartSettingThread(numberOfThreads);

                StressServiceLargeIO();
                StressSettingNameLargeIO();
                StressSettingLargeIO();
            }

            string fileDate = DateTime.Now.ToString("yyyy-M-dd--HH-mm-ss");

            File.WriteAllText("port_yetti_stress_test_" + fileDate + ".csv", outputFile.ToString());

            Console.WriteLine("Stress testing finished, press enter to quit...");
            Console.ReadLine();
        }

        #region  SERVICE

        public static void StartServiceThread(int numberOfThreads)
        {
            threadDictionary.Clear();
            threadPrint.Clear();

            manualEvent = new ManualResetEvent(false);

            outputFile.AppendLine();
            outputFile.AppendLine("Stress Service Threaded");
            outputFile.AppendLine("apiCall,threadNumber,totalTime,averageTime");
            
            Console.WriteLine("Starting Service Threaded Stress Test...");

            // loop through and start threaded stress testrs
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadNumber = i + 1;

                // init and start threads
                ThreadStart threadStarter = delegate { RunServiceThread(threadNumber); };
                Thread newWorkerThread = new Thread(threadStarter);
                newWorkerThread.Start();

                // add thread information to concurrent dictionary
                if (!threadDictionary.ContainsKey(threadNumber))
                    threadDictionary.TryAdd(threadNumber, string.Format("Thread {0} Started...", threadNumber));
            }

            // print dict to console
            do
            {
                Thread.Sleep(250);

                // print dict
                foreach (KeyValuePair<int, string> kvp in threadDictionary)
                {
                    Console.WriteLine(kvp.Value);
                }

                // set cursor pos to new line upon new api call
                if (!isThreadsWorking.Contains(true))
                {
                    Console.WriteLine();
                    manualEvent.Set();
                    Thread.Sleep(500);
                }
                // set cursor pos to replace thread update lines
                else
                {
                    ClearCurrentConsoleLines(numberOfThreads);

                    manualEvent.Reset();
                }
            } while (threadsFinished < numberOfThreads);

            foreach (KeyValuePair<int, string> kvp in threadPrint)
            {
                outputFile.Append(kvp.Value);
            }            

            Console.WriteLine();
        }

        public static void RunServiceThread(int threadNumber)
        {
            StressServiceThreaded(threadNumber, "getall", threadNumber);
            StressServiceThreaded(threadNumber, "post", threadNumber + 4);
            StressServiceThreaded(threadNumber, "put", threadNumber + 8);
            StressServiceThreaded(threadNumber, "getbyid", threadNumber + 12);
            StressServiceThreaded(threadNumber, "getbyname", threadNumber + 16);

            lock (threadLock)
            {
                threadsFinished++;
            }
        }

        static void StressServiceThreaded(int threadNumber, string requestKey, int threadKey)
        {
            // worker working
            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = true;
            }

            StringBuilder sb = new StringBuilder();            

            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            string response;
            List<string> responseList = new List<string>();
            List<Service> serviceList = new List<Service>();

            // get all services from database and store as list
            serviceList = ServicePopulateLocalList();

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // populate vars
                id = random.Next(1, serviceList.Count - 1);

                // call relevant api by request key
                if (requestKey == "getall")
                    response = ServiceGetAllRequest();
                else if (requestKey == "post")
                    response = ServicePostRequest(Guid.NewGuid().ToString());
                else if (requestKey == "put")
                    response = ServicePutRequest(id, Guid.NewGuid().ToString());
                else if (requestKey == "getbyid")
                    response = ServiceGetByIdRequest(id);
                else if (requestKey == "getbyname")
                    response = ServiceGetByNameRequest(serviceList[i].name);
                else
                    break;

                responseList.Add(response);

                threadDictionary[threadNumber] = string.Format("Thread {0} Working: {1} | Request: {2} | Response: {3}                                  ", threadNumber, requestKey,  i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            threadDictionary[threadNumber] = string.Format("Thread {0} Finished: {1} | Total Time {2}ms | Average Time {3}ms", threadNumber, requestKey,  totalTime, averageTime);

            // worker finished
            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = false;
            }

            sb.Append(requestKey);
            sb.Append(',');
            sb.Append(threadNumber);
            sb.Append(',');
            sb.Append(totalTime.ToString());
            sb.Append(',');
            sb.AppendLine(averageTime.ToString());

            threadPrint.TryAdd(threadKey - 1, sb.ToString());            

            // wait for main thread
            manualEvent.WaitOne();
        }

        static void StressService(string requestKey)
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id = 0;
            string response = "";
            List<string> responseList = new List<string>();
            List<Service> serviceList = new List<Service>();

            // get all services from database and store as list
             serviceList = ServicePopulateLocalList();

            Console.WriteLine("Starting Service {0} Stress Test...", requestKey);

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // call relevant api by request key
                if (requestKey == "getall")
                    response = ServiceGetAllRequest();
                else if (requestKey == "post")
                    response = ServicePostRequest(Guid.NewGuid().ToString());
                else if (requestKey == "put")
                {
                    id = random.Next(1, serviceList.Count);
                    response = ServicePutRequest(id, Guid.NewGuid().ToString());
                }
                else if (requestKey == "getbyid")
                {
                    id = random.Next(1, serviceList.Count);
                    response = ServiceGetByIdRequest(id);
                }
                else if (requestKey == "getbyname")
                { 
                    id = random.Next(1, serviceList.Count);
                    response = ServiceGetByNameRequest(serviceList[id].name);
                }
                else
                    break;

                responseList.Add(response);

                Console.Write("\rRequest: {0} Response: {1}     ", i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            Console.WriteLine();
            Console.WriteLine("Stress Test Finished...");
            Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);

            // print out occurances of different responses
            PrintDictionary(responseList);

            outputFile.Append(requestKey);
            outputFile.Append(',');
            outputFile.Append(totalTime.ToString());
            outputFile.Append(',');
            outputFile.AppendLine(averageTime.ToString());

            Console.WriteLine();
        }

        static void StressServiceLargeIO()
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            string response;

            // create new string builder / random
            StringBuilder sb = new StringBuilder();
            Random sbRandom = new Random();

            outputFile.AppendLine();
            outputFile.AppendLine("Stress Service Large File");
            outputFile.AppendLine("String Size, Total Time, Average Time");

            List<string> responseList = new List<string>();

            Console.WriteLine("Starting Service LARGE FILE IO Post Stress Test...");

            // loop through and double size of string each time
            for (int i = 1; i < 20; i++)
            {
                responseList.Clear();

                for (int k = 0; k < Math.Pow(2, i); k++)
                {
                    // add char to string builder
                    sb.Append((char)(sbRandom.Next(65, 65 + 26)));
                };

                Console.WriteLine("string size: {0}", sb.Length);

                // reset & start timer
                stopwatch.Reset();
                stopwatch.Start();

                for (int j = 0; j < numberOfTests; j++)
                {
                    // call api and populate vars
                    id = random.Next(1, 1000);
                    response = ServicePostRequest(sb.ToString());
                    responseList.Add(response);

                    Console.Write("\rRequest: {0} Response: {1}                         ", j, response);
                }

                // end timer
                stopwatch.Stop();

                double totalTime = stopwatch.Elapsed.TotalMilliseconds;
                double averageTime = totalTime / numberOfTests;

                Console.WriteLine();
                Console.WriteLine("Stress Test Finished...");
                Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);                

                // print out response message and their count
                PrintDictionary(responseList);

                Console.WriteLine();

                outputFile.Append(sb.Length.ToString());
                outputFile.Append(',');
                outputFile.Append(totalTime.ToString());
                outputFile.Append(',');
                outputFile.AppendLine(averageTime.ToString());
            }
        }

        static string ServiceGetAllRequest()
        {
            // send get all message to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/service").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string ServicePostRequest(string postContent)
        {
            // init data object "Service Post" and init member variables
            ServicePost servicePost = new ServicePost();
            servicePost.name = postContent;

            // convert data object to Json s
            string jString = JsonConvert.SerializeObject(servicePost);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PostAsync("/api/service", content).Result;
            
            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return String.Format(serverResponse.StatusCode.ToString());
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string ServicePutRequest(int id, string putContent)
        {
            // init data object "Service" and init member variables
            Service service = new Service();
            service.id = id;
            service.name = putContent;

            // convert data object to Json Object
            string jString = JsonConvert.SerializeObject(service);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PutAsync("/api/service", content).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return String.Format(serverResponse.StatusCode.ToString() + " ");
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string ServiceGetByIdRequest(int id)
        {
            // send get by id message to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/Service/GetById/" + id).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string ServiceGetByNameRequest(string name)
        {
            // send get by name request to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/service/getbyname/" + name).Result;

            // if OK
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static List<Service> ServicePopulateLocalList()
        {
            List<Service> serviceList = new List<Service>();

            // send get all message to server to get list of "Services"
            HttpResponseMessage serverResponse = client.GetAsync("/api/service").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // convert response string to Json Array
                JArray jArray = JArray.Parse(result);

                for (int i = 0; i < jArray.Count; i++)
                {
                    // convert Json Token to data object "Service"
                    string jName = jArray[i].ToString();

                    serviceList.Add(JsonConvert.DeserializeObject<Service>(jName));
                }

                return serviceList;
            }
            else
                return null;
        }

        #endregion

        #region SETTING

        public static void StartSettingThread(int numberOfThreads)
        {
            threadDictionary.Clear();
            threadPrint.Clear();

            threadsFinished = 0;
            manualEvent = new ManualResetEvent(false);

            outputFile.AppendLine();
            outputFile.AppendLine("Stress Setting Threaded");
            outputFile.AppendLine("apiCall,threadNumber,totalTime,averageTime");

            Console.WriteLine("Starting Setting Threaded Stress Test...");

            // loop through and start threaded stress testrs
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadNumber = i + 1;

                // init and start threads
                ThreadStart threadStarter = delegate { RunSettingThread(threadNumber); };
                Thread newWorkerThread = new Thread(threadStarter);
                newWorkerThread.Start();

                // add thread information to concurrent dictionary
                if (!threadDictionary.ContainsKey(threadNumber))
                    threadDictionary.TryAdd(threadNumber, string.Format("Thread {0} Started...", threadNumber));
            }

            // print dict to console
            do
            {
                Thread.Sleep(500);

                // print dict
                foreach (KeyValuePair<int, string> kvp in threadDictionary)
                {
                    Console.WriteLine(kvp.Value);
                }

                // set cursor pos to new line upon new api call
                if (!isThreadsWorking.Contains(true))
                {
                    Console.WriteLine();
                    manualEvent.Set();
                    Thread.Sleep(500);
                }
                // set cursor pos to replace thread update lines
                else
                {
                    ClearCurrentConsoleLines(numberOfThreads);
                    manualEvent.Reset();
                }
            } while (threadsFinished < numberOfThreads);

            foreach (KeyValuePair<int, string> kvp in threadPrint)
            {
                outputFile.Append(kvp.Value);
            }

            Console.WriteLine();
        }

        public static void RunSettingThread(int threadNumber)
        {
            StressSettingThreaded(threadNumber, "getall", threadNumber);
            StressSettingThreaded(threadNumber, "post", threadNumber + 4);
            StressSettingThreaded(threadNumber, "put", threadNumber + 8);
            StressSettingThreaded(threadNumber, "getbyid", threadNumber + 12);
            StressSettingThreaded(threadNumber, "getbyvalue", threadNumber + 16);
            StressSettingThreaded(threadNumber, "getbysettingidserviceid", threadNumber + 20);

            lock (threadLock)
            {
                threadsFinished++;
            }
        }

        static void StressSettingThreaded(int threadNumber, string requestKey, int threadKey)
        {
            // set thread to working
            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = true;
            }

            StringBuilder sb = new StringBuilder();

            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            int settingNameId;
            int serviceId;
            string response;
            List<string> responseList = new List<string>();
            List<Setting> settingList = new List<Setting>();

            // get all services from database and store as list
            settingList = SettingPopulateLocalList();

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // populate vars
                id = random.Next(1, settingList.Count - 1);
                settingNameId = random.Next(1, settingList.Count - 1);
                serviceId = random.Next(1, settingList.Count - 1);

                // call relevant api by request key
                if (requestKey == "getall")
                    response = SettingGetAllRequest();
                else if (requestKey == "post")
                    response = SettingPostRequest(settingNameId, serviceId, Guid.NewGuid().ToString());
                else if (requestKey == "put")
                    response = SettingPutRequest(id, settingNameId, serviceId, Guid.NewGuid().ToString());
                else if (requestKey == "getbyid")
                    response = SettingGetByIdRequest(id);
                else if (requestKey == "getbyvalue")
                    response = SettingGetByValueRequest(settingList[i].value);
                else if (requestKey == "getbysettingidserviceid")
                    response = SettingGetBySettingServiceId(settingNameId, serviceId);
                else
                    break;

                // log response
                responseList.Add(response);

                // send response to dict for printing in main thread
                threadDictionary[threadNumber] = string.Format("Thread {0} Working: {1} | Request: {2} | Response: {3}                                  ", threadNumber, requestKey, i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            // send finished stats to dict for printing in main thread
            threadDictionary[threadNumber] = string.Format("Thread {0} Finished: {1} | Total Time {2}ms | Average Time {3}ms", threadNumber, requestKey, totalTime, averageTime);

            // set worker thread to stopped working
            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = false;
            }

            sb.Append(requestKey);
            sb.Append(',');
            sb.Append(threadNumber);
            sb.Append(',');
            sb.Append(totalTime.ToString());
            sb.Append(',');
            sb.AppendLine(averageTime.ToString());

            threadPrint.TryAdd(threadKey - 1, sb.ToString());

            // wait for main thread to allow continuation of thread
            manualEvent.WaitOne();
        }

        static void StressSetting(string requestKey)
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id = 0;
            int settingNameId = 0;
            int serviceId = 0;
            string response = "";
            List<string> responseList = new List<string>();
            List<Setting> settingList = new List<Setting>();
            List<Service> serviceList = new List<Service>();
            List<SettingName> settingNameList = new List<SettingName>();

            // get list of settings from database and add to settingList
            settingList = SettingPopulateLocalList();
            serviceList = ServicePopulateLocalList();
            settingNameList = SettingNamePopulateLocalList();

            Console.WriteLine("Starting Setting {0} Stress Test...", requestKey);

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // call relevant api by request key
                if (requestKey == "getall")
                    response = SettingGetAllRequest();
                else if (requestKey == "post")
                {
                    settingNameId = random.Next(1, settingNameList.Count - 1);
                    serviceId = random.Next(1, serviceList.Count - 1);
                    response = SettingPostRequest(settingNameId, serviceId, Guid.NewGuid().ToString());
                }
                else if (requestKey == "put")
                {
                    id = random.Next(1, settingList.Count - 1);
                    settingNameId = random.Next(1, settingNameList.Count - 1);
                    serviceId = random.Next(1, serviceList.Count - 1);
                    response = SettingPutRequest(id, settingNameId, serviceId, Guid.NewGuid().ToString());
                }
                else if (requestKey == "getbyid")
                {
                    id = random.Next(1, settingList.Count - 1);
                    response = SettingGetByIdRequest(id);
                }
                else if (requestKey == "getbyvalue")
                {
                    id = random.Next(1, settingList.Count - 1);
                    response = SettingGetByValueRequest(settingList[id].value);
                }
                else if (requestKey == "getbysettingidserviceid")
                    response = SettingGetBySettingServiceId(settingNameId, serviceId);
                else
                    break;

                responseList.Add(response);

                Console.Write("\rRequest: {0} Response: {1}     ", i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            Console.WriteLine();
            Console.WriteLine("Stress Test Finished...");
            Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);

            // print out response messages and no of occurances
            PrintDictionary(responseList);

            outputFile.Append(requestKey);
            outputFile.Append(',');
            outputFile.Append(totalTime.ToString());
            outputFile.Append(',');
            outputFile.AppendLine(averageTime.ToString());

            Console.WriteLine();
        }

        static void StressSettingLargeIO()
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            int serviceId;
            int settingNameId;
            string response;

            // create new string builder / random
            StringBuilder sb = new StringBuilder();
            Random sbRandom = new Random();

            outputFile.AppendLine();
            outputFile.AppendLine("Stress Setting Large File");

            List<string> responseList = new List<string>();

            Console.WriteLine("Starting Setting LARGE FILE IO Post Stress Test...");

            // loop through and double size of string each time
            for (int i = 1; i < 20; i++)
            {
                responseList.Clear();

                for (int k = 0; k < Math.Pow(2, i); k++)
                {
                    // add char to string builder
                    sb.Append((char)(sbRandom.Next(65, 65 + 26)));
                };

                Console.WriteLine("string size: {0}", sb.Length);

                // reset & start timer
                stopwatch.Reset();
                stopwatch.Start();

                for (int j = 0; j < numberOfTests; j++)
                {
                    // call api and populate vars
                    id = random.Next(1, 1000);
                    settingNameId = random.Next(1, 1000);
                    serviceId = random.Next(1, 1000);

                    response = SettingPostRequest(settingNameId, serviceId, sb.ToString());
                    responseList.Add(response);
                    Console.Write("\rRequest: {0} Response: {1}                         ", j, response);
                }

                // end timer
                stopwatch.Stop();

                double totalTime = stopwatch.Elapsed.TotalMilliseconds;
                double averageTime = totalTime / numberOfTests;

                Console.WriteLine();
                Console.WriteLine("Stress Test Finished...");
                Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);

                // print response messages and no of occurances
                PrintDictionary(responseList);

                Console.WriteLine();

                outputFile.Append(sb.Length.ToString());
                outputFile.Append(',');
                outputFile.Append(totalTime.ToString());
                outputFile.Append(',');
                outputFile.AppendLine(averageTime.ToString());
            }
        }

        static string SettingGetAllRequest()
        {
            // send get all request to server to get list of "Settings"
            HttpResponseMessage serverResponse = client.GetAsync("/api/setting").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingPostRequest(int settingNameId, int serviceId, string value)
        {
            // create new data object "Setting" and init variables
            SettingPost settingPost = new SettingPost();

            settingPost.settingNameId = settingNameId;
            settingPost.serviceId = serviceId;
            settingPost.value = value;

            // convert data object to Json Object
            string jString = JsonConvert.SerializeObject(settingPost);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PostAsync("/api/setting", content).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read in response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return String.Format(serverResponse.StatusCode.ToString());
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingPutRequest(int id, int settingNameId, int serviceId, string value)
        {
            // create new data object "Setting" and init variables
            Setting settingPut = new Setting();

            settingPut.id = id;
            settingPut.serviceId = serviceId;
            settingPut.settingNameId = settingNameId;
            settingPut.value = value;

            // convert data object to Json Object
            string jString = JsonConvert.SerializeObject(settingPut);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PutAsync("/api/setting", content).Result;

            // if OK
            if (serverResponse.IsSuccessStatusCode)
            {
                // get response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return
                return String.Format(serverResponse.StatusCode.ToString() + " ");
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingGetByIdRequest(int id)
        {
            // send get by id request to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/setting/getbyid/" + id).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingGetByValueRequest(string value)
        {
            // send get by name request
            HttpResponseMessage serverResponse = client.GetAsync("/api/setting/getbyvalue/" + value).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingGetBySettingServiceId(int settingId, int serviceId)
        {
            // send get by setting & service id request to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/setting/GetBySettingIdServiceId/" + settingId + "/" + serviceId).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static List<Setting> SettingPopulateLocalList()
        {
            List <Setting> settingList = new List<Setting>();

            // send get message to server to get list of all settings
            HttpResponseMessage serverResponse = client.GetAsync("/api/setting").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // convert from string to Json Array
                JArray jArray = JArray.Parse(result);

                // loop through array
                for (int i = 0; i < jArray.Count; i++)
                {
                    // convert jtoken to string
                    string jName = jArray[i].ToString();

                    // convert json string to data object "Setting"
                    settingList.Add(JsonConvert.DeserializeObject<Setting>(jName));
                }

                return settingList;
            }
            else
                return null;
        }

        #endregion

        #region  SETTINGNAME

        public static void StartSettingNameThread(int numberOfThreads)
        {
            threadDictionary.Clear();
            threadPrint.Clear();
            threadsFinished = 0;
            manualEvent = new ManualResetEvent(false);

            Console.WriteLine("Starting SettingName Threaded Stress Test...");

            outputFile.AppendLine();
            outputFile.AppendLine("Stress SettingName Threaded");
            outputFile.AppendLine("apiCall,threadNumber,totalTime,averageTime");

            // loop through and start threaded stress testrs
            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadNumber = i + 1;

                // init and start threads
                ThreadStart threadStarter = delegate { RunSettingNameThread(threadNumber); };
                Thread newWorkerThread = new Thread(threadStarter);
                newWorkerThread.Start();

                // add thread information to concurrent dictionary
                if (!threadDictionary.ContainsKey(threadNumber))
                    threadDictionary.TryAdd(threadNumber, string.Format("Thread {0} Started...", threadNumber));
            }

            // print dict to console
            do
            {
                Thread.Sleep(500);

                // print dict
                foreach (KeyValuePair<int, string> kvp in threadDictionary)
                {
                    Console.WriteLine(kvp.Value);
                }

                // set cursor pos to new line upon new api call
                if (!isThreadsWorking.Contains(true))
                {
                    Console.WriteLine();
                    manualEvent.Set();
                    Thread.Sleep(500);
                }
                // set cursor pos to replace thread update lines
                else
                {
                    ClearCurrentConsoleLines(numberOfThreads);
                    manualEvent.Reset();
                }
            } while (threadsFinished < numberOfThreads);

            foreach (KeyValuePair<int, string> kvp in threadPrint)
            {
                outputFile.Append(kvp.Value);
            }

            threadPrint.Clear();

            Console.WriteLine();
        }

        public static void RunSettingNameThread(int threadNumber)
        {
            StressSettingNameThreaded(threadNumber, "getall", threadNumber);
            StressSettingNameThreaded(threadNumber, "post", threadNumber + 4);
            StressSettingNameThreaded(threadNumber, "put", threadNumber + 8);
            StressSettingNameThreaded(threadNumber, "getbyid", threadNumber + 12);
            StressSettingNameThreaded(threadNumber, "getbyname", threadNumber + 16);

            lock (threadLock)
            {
                threadsFinished++;
            }
        }

        static void StressSettingNameThreaded(int threadNumber, string requestKey, int threadKey)
        {
            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = true;
            }

            StringBuilder sb = new StringBuilder();

            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            string response;
            List<string> responseList = new List<string>();
            List<SettingName> settingNameList = new List<SettingName>();

            // get all services from database and store as list
            settingNameList = SettingNamePopulateLocalList();

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // populate vars
                id = random.Next(1, settingNameList.Count - 1);

                // call relevant api by request key
                if (requestKey == "getall")
                    response = SettingNameGetAllRequest();
                else if (requestKey == "post")
                    response = SettingNamePostRequest(Guid.NewGuid().ToString());
                else if (requestKey == "put")
                    response = SettingNamePutRequest(id, Guid.NewGuid().ToString());
                else if (requestKey == "getbyid")
                    response = SettingNameGetByIdRequest(id);
                else if (requestKey == "getbyname")
                    response = SettingNameGetByNameRequest(settingNameList[i].name);
                else
                    break;

                responseList.Add(response);

                threadDictionary[threadNumber] = string.Format("Thread {0} Working: {1} | Request: {2} | Response: {3}                              ", threadNumber, requestKey, i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            threadDictionary[threadNumber] = string.Format("Thread {0} Finished: {1} | Total Time {2}ms | Average Time {3}ms", threadNumber, requestKey, totalTime, averageTime);

            lock (threadLock)
            {
                isThreadsWorking[threadNumber - 1] = false;
            }

            sb.Append(requestKey);
            sb.Append(',');
            sb.Append(threadNumber);
            sb.Append(',');
            sb.Append(totalTime.ToString());
            sb.Append(',');
            sb.AppendLine(averageTime.ToString());

            threadPrint.TryAdd(threadKey - 1, sb.ToString());

            manualEvent.WaitOne();
        }

        static void StressSettingName(string requestKey)
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id = 0;
            string response = "";
            List<string> responseList = new List<string>();
            List<SettingName> settingNameList = new List<SettingName>(10);
            
            settingNameList = SettingNamePopulateLocalList();

            Console.WriteLine("Starting SettingName {0} Stress Test...", requestKey);

            // start timer
            stopwatch.Start();

            for (int i = 0; i < numberOfTests; i++)
            {
                // call relevant api by request key
                if (requestKey == "getall")
                    response = SettingNameGetAllRequest();
                else if (requestKey == "post")
                    response = SettingNamePostRequest(Guid.NewGuid().ToString());
                else if (requestKey == "put")
                {
                    id = random.Next(1, settingNameList.Count);
                    response = SettingNamePutRequest(id, Guid.NewGuid().ToString());
                }
                else if (requestKey == "getbyid")
                {
                    id = random.Next(1, settingNameList.Count);
                    response = SettingNameGetByIdRequest(id);
                }
                else if (requestKey == "getbyname")
                {
                    id = random.Next(1, settingNameList.Count);
                    response = SettingNameGetByNameRequest(settingNameList[id].name);
                }
                else
                    break;

                responseList.Add(response);

                Console.Write("\rRequest: {0} Response: {1}     ", i, response);
            }

            // end timer
            stopwatch.Stop();

            double totalTime = stopwatch.Elapsed.TotalMilliseconds;
            double averageTime = totalTime / numberOfTests;

            Console.WriteLine();
            Console.WriteLine("Stress Test Finished...");
            Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);

            // print response messages and no of occurances
            PrintDictionary(responseList);

            outputFile.Append(requestKey);
            outputFile.Append(',');
            outputFile.Append(totalTime.ToString());
            outputFile.Append(',');
            outputFile.AppendLine(averageTime.ToString());

            Console.WriteLine();
        }

        static void StressSettingNameLargeIO()
        {
            // init stuff
            Stopwatch stopwatch = new Stopwatch();
            Random random = new Random();
            int id;
            string response;

            // create new string builder / random
            StringBuilder sb = new StringBuilder();
            Random sbRandom = new Random();

            outputFile.AppendLine();
            outputFile.AppendLine("Stress SettingName Large File");

            List<string> responseList = new List<string>();

            Console.WriteLine("Starting SettingName LARGE FILE IO Post Stress Test...");

            // loop through and double size of string each time
            for (int i = 1; i < 20; i++)
            {
                responseList.Clear();

                for (int k = 0; k < Math.Pow(2, i); k++)
                {
                    // add char to string builder
                    sb.Append((char)(sbRandom.Next(65, 65 + 26)));
                };

                Console.WriteLine("string size: {0}", sb.Length);

                // reset & start timer
                stopwatch.Reset();
                stopwatch.Start();

                for (int j = 0; j < numberOfTests; j++)
                {
                    // call api and populate vars
                    id = random.Next(1, 1000);
                    response = SettingNamePostRequest(sb.ToString());
                    responseList.Add(response);

                    Console.Write("\rRequest: {0} Response: {1}                         ", j, response);
                }

                // end timer
                stopwatch.Stop();

                double totalTime = stopwatch.Elapsed.TotalMilliseconds;
                double averageTime = totalTime / numberOfTests;

                Console.WriteLine();
                Console.WriteLine("Stress Test Finished...");
                Console.WriteLine("Total: {0}ms Average: {1}ms", totalTime, averageTime);

                // print respons emessages and their no of occurances
                PrintDictionary(responseList);

                Console.WriteLine();

                outputFile.Append(sb.Length.ToString());
                outputFile.Append(',');
                outputFile.Append(totalTime.ToString());
                outputFile.Append(',');
                outputFile.AppendLine(averageTime.ToString());
            }            
        }

        static string SettingNameGetAllRequest()
        {
            // send get all message to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/settingName").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingNamePostRequest(string postContent)
        {
            // init data object "SettingName Post" and init member variables
            SettingNamePost settingNamePost = new SettingNamePost();
            settingNamePost.name = postContent;

            // convert data object to Json Object
            string jString = JsonConvert.SerializeObject(settingNamePost);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PostAsync("/api/settingName", content).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return String.Format(serverResponse.StatusCode.ToString());
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingNamePutRequest(int id, string putContent)
        {
            // init data object "SettingName" and init member variables
            SettingName settingName = new SettingName();
            settingName.id = id;
            settingName.name = putContent;

            // convert data object to Json Object
            string jString = JsonConvert.SerializeObject(settingName);
            var content = new StringContent(jString, Encoding.UTF8, "application/json");

            // send post message to server
            HttpResponseMessage serverResponse = client.PutAsync("/api/settingName", content).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return String.Format(serverResponse.StatusCode.ToString() + " ");
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingNameGetByIdRequest(int id)
        {
            // send get by id message to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/settingName/getbyid/" + id).Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static string SettingNameGetByNameRequest(string name)
        {
            // send get by name request to server
            HttpResponseMessage serverResponse = client.GetAsync("/api/settingName/getbyname/" + name).Result;

            // if OK
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // return response
                return serverResponse.StatusCode.ToString();
            }
            else
                return serverResponse.StatusCode.ToString();
        }

        static List<SettingName> SettingNamePopulateLocalList()
        {
            List<SettingName> settingNameList = new List<SettingName>();

            // send get all message to server to get list of "SettingNames"
            HttpResponseMessage serverResponse = client.GetAsync("/api/settingName").Result;

            // if ok
            if (serverResponse.IsSuccessStatusCode)
            {
                // read response
                var responseObject = serverResponse.Content.ReadAsStringAsync();
                string result = responseObject.Result;

                // convert response string to Json Array
                JArray jArray = JArray.Parse(result);

                for (int i = 0; i < jArray.Count; i++)
                {
                    // convert Json Token to data object "SettingName"
                    string jName = jArray[i].ToString();

                    settingNameList.Add(JsonConvert.DeserializeObject<SettingName>(jName));
                }

                return settingNameList;
            }
            else
                return null;
        }

        #endregion

        public static void PrintDictionary(List<string> fromList)
        {
            Dictionary<string, int> toDictionary = new Dictionary<string, int>();

            // add response messages and occurances to dict
            for (int i = 0; i < fromList.Count; i++)
            {
                if (toDictionary.ContainsKey(fromList[i]))
                    toDictionary[fromList[i]]++;
                else
                    toDictionary.Add(fromList[i], 1);
            }

            // print dict
            foreach (KeyValuePair<string, int> kvp in toDictionary)
            {
                Console.WriteLine("Response Message: {0} | Count: {1}", kvp.Key, kvp.Value);
            }
        }

        public static void ClearCurrentConsoleLines(int numberOfLines)
        {
            int currentLineCursor = Console.CursorTop - numberOfLines;
            Console.SetCursorPosition(0, Console.CursorTop);

            for (int i = 0; i < numberOfLines; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLineCursor);
        }

    }

    #region MODELS

    class ServicePost
    {
        public string name { get; set; }
    }

    class Service : ServicePost
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    class SettingPost
    {
        public int settingNameId { get; set; }
        public int serviceId { get; set; }
        public string value { get; set; }
    }

    class Setting : SettingPost
    {
        public int id { get; set; }
        public int settingNameId { get; set; }
        public int serviceId { get; set; }
        public string value { get; set; }
    }

    class SettingNamePost
    {
        public string name { get; set; }
    }

    class SettingName : SettingNamePost
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    #endregion
}
