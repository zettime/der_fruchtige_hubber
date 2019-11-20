using System;
using System.Diagnostics;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        // Back-End Frontend Kommunikation ////////////

        // Vom Chat
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        private static bool threadKeepRunning = false;
        public void startPolling()
        {
            threadKeepRunning = true; ;
            Debug.WriteLine("Got it.");

            new Thread(() =>
            {
                while (threadKeepRunning)
                {
                    Debug.WriteLine("Running: " + threadKeepRunning);
                    System.Threading.Thread.Sleep(500);
                    Send("Backend", new Random().Next(10).ToString());
                }
            }).Start();
        }

        public void stopPolling()
        {
            threadKeepRunning = false;
            Debug.WriteLine("Stop: " + threadKeepRunning);
        }
    }
}