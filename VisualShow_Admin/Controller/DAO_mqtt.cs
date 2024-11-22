
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace VisualShow_Admin.Controller
{
    public class DAO_mqtt
    {
        string brokerAddress = "raspberrypimatheo1";
        int brokerPort = 1883;
        string username = "matheo";
        string password = "matheo";
        string clientId = Guid.NewGuid().ToString();
        IMqttClient clientmqtt;
        public Action<bool> ConnectionStatusChanged;
        public async void ConnexionBroker()
        {
            //constructeur de la connexion
            var mqttnet = new MqttFactory();
            clientmqtt = mqttnet.CreateMqttClient();

            //constructeur des paramètres de connexion
            var parametres_connexion_client = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(brokerAddress, brokerPort)
            .WithCredentials(username, password)
            .WithCleanSession();
            var parametres_mqtt = parametres_connexion_client.Build();

            try
            {
                await clientmqtt.ConnectAsync(parametres_mqtt);
                ConnectionStatusChanged?.Invoke(true);
            }
            catch (Exception)
            {
                ConnectionStatusChanged?.Invoke(false);
            }
            return;
        }

        public Task PublishTopicMessage(string cheminTopic, string message)
        {
            try
            {
                var message_mqtt = new MqttApplicationMessageBuilder()
                .WithTopic(cheminTopic)
                .WithPayload(message)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();
                clientmqtt.PublishAsync(message_mqtt);
                MessageBox.Show("Message envoyé aux applications clientes", "Message", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Un problème est survenu lors de la diffusion du message aux applications clientes", "Message", MessageBoxButton.OK);
            }
            return Task.CompletedTask;
        }
    }
