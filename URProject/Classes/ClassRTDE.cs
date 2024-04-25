﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace URProject.Classes {
    public class ClassRTDE {

        FormMain mainForm;
        IPAddress ipAddress;
        IPEndPoint ipEndPoint;

        public ClassRTDE(FormMain mainForm) {
            this.mainForm = mainForm;
            Logging.LogInformation(1, "ClassRTDE - Initialization Completed");
        }

        public bool checkRobotConnection()
        {
            Logging.LogInformation(1, "ClassRTDE checkRobotConnection - Checking robot connection...");
            Ping pinger = new Ping();
            PingReply reply = pinger.Send(ClassData.robotIp);
            return reply.Status == IPStatus.Success;
        }

        public void connectSocket()
        {
            try {
                this.ipAddress = IPAddress.Parse(ClassData.robotIp);
                this.ipEndPoint = new IPEndPoint(ipAddress, ClassData.robotPort);
                ClassData.client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                ClassData.client.Connect(ipEndPoint);
                Logging.LogInformation(1, "ClassRTDE connectSocket - Socket Created");
            } catch(Exception err) {
                Logging.LogInformation(3, "ClassRTDE connectSocket - " + err.Message);
            }
            
        }

        public void getRobotPos()
        {
            var message = "actual_TCP_pose";
            var messageBytes = Encoding.UTF8.GetBytes(message);
            ClassData.client.Send(messageBytes);
            Logging.LogInformation(0, "ClassRTDE getRobotPos - Message Send: " + message);

            byte[] messageReceived = new byte[1024];
            int byteRecv = ClassData.client.Receive(messageReceived);

            Logging.LogInformation(0, "ClassRTDE getRobotPos - Message Recieved: " + Encoding.UTF8.GetString(messageReceived, 0, byteRecv));
        }
    }
}
