using System;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.Azure.EventHubs;
using System;
using Microsoft.Azure.EventHubs;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        // Event Hub-compatible endpoint
        // az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {your IoT Hub name}
        private readonly static string s_eventHubsCompatibleEndpoint = "sb://ihsuprodamres022dednamespace.servicebus.windows.net/";

        // Event Hub-compatible name
        // az iot hub show --query properties.eventHubEndpoints.events.path --name {your IoT Hub name}
        private readonly static string s_eventHubsCompatiblePath = "iothub-ehub-derfruchti-2469851-3c70fbb27e";

        // az iot hub policy show --name service --query primaryKey --hub-name {your IoT Hub name}
        private readonly static string s_iotHubSasKey = "TK7iZCqCRl06m+nhsuN1tX42xzz8hE7vTkyXft1RhsI=";
        private readonly static string s_iotHubSasKeyName = "service";
        private static EventHubClient s_eventHubClient;


        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            // Create the receiver using the default consumer group.
            // For the purposes of this sample, read only messages sent since 
            // the time the receiver is created. Typically, you don't want to skip any messages.
            var eventHubReceiver = s_eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            //SendSensor("Create receiver on partition: " + partition);
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                //SendSensor("Listening for messages on: " + partition);
                // Check for EventData - this methods times out if there is nothing to retrieve.
                var events = await eventHubReceiver.ReceiveAsync(100);

                // If there is data in the batch, process it.
                if (events == null) continue;

                foreach (EventData eventData in events)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    //SendSensor("Message received on partition" + partition + ":");
                    // SendSensor(data);

                    JObject obj = JObject.Parse(data);
                    Send("temperature", Convert.ToString(Math.Round(Convert.ToDouble(obj["temperature"]),2)));
                    // Send("temperature", Convert.ToString(obj["temperature"]));
                    // Send("humidity", Convert.ToString(obj["humidity"]));
                    // SendSensor("Application properties (set by device):");
                    Send("humidity", Convert.ToString(Math.Round(Convert.ToDouble(obj["humidity"]), 2)));
                    foreach (var prop in eventData.Properties)
                    {
                        //SendSensor(prop.Key + ": " +prop.Value);
                    }
                    //SendSensor("System properties (set by IoT Hub):");
                    foreach (var prop in eventData.SystemProperties)
                    {
                        //SendSensor(prop.Key + prop.Value);
                    }
                }
            }
        }

        private async Task Sub()
        {
            Debug.WriteLine("IoT Hub Quickstarts - Read device to cloud messages. Ctrl-C to exit.\n");

            // Create an EventHubClient instance to connect to the
            // IoT Hub Event Hubs-compatible endpoint.
            var connectionString = new EventHubsConnectionStringBuilder(new Uri(s_eventHubsCompatibleEndpoint), s_eventHubsCompatiblePath, s_iotHubSasKeyName, s_iotHubSasKey);
            s_eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

            // Create a PartitionReciever for each partition on the hub.
            var runtimeInfo = await s_eventHubClient.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            CancellationTokenSource cts = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
                Debug.WriteLine("Exiting...");
            };

            var tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            }

            // Wait for all the PartitionReceivers to finsih.
            Task.WaitAll(tasks.ToArray());
        }
        /// //////////////////////////////////////////////////////////////////////
        // Back-End Frontend Kommunikation ////////////

        // Vom Chat
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public void SendSensor(string message)
        {
            string name = "Sensor";
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public async Task Send2(string message)
        {
            await Clients.All.NewMessage(message);
        }

        private static bool threadKeepRunning = false;
        public void startPolling()
        {
            threadKeepRunning = true; ;
            Debug.WriteLine("Got it.");

            /*new Thread(() =>
            {
                while (threadKeepRunning)
                {
                    Debug.WriteLine("Running: " + threadKeepRunning);
                    System.Threading.Thread.Sleep(500);
                    Send("Backend", new Random().Next(10).ToString());
                }
            }).Start();*/
            new Thread(() =>
            {
                Sub();
            }).Start();
        }

        public void stopPolling()
        {
            threadKeepRunning = false;
            Debug.WriteLine("Stop: " + threadKeepRunning);
        }
    }
}