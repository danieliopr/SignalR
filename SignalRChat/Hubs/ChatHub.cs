using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SignalRChat.Models;
using Newtonsoft.Json;
namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public List<Controlador> lista = new List<Controlador>();

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SendMessageClient(string connectionId, string user, string message)
        {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }
        public async Task ReiniciarControlador(string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("Reiniciar");
        }
        public async Task SendMessageClientConnectionId(string connectionId)
        {
            GetConnectionId("teste");
            await Clients.All.SendAsync("SendMessageClientConnectionId", connectionId);
        }
        public async Task ShutDown(string reason)
        {
            Console.Write(reason);
            await base.OnDisconnectedAsync(null);
        }

        public async Task IniciarJogo(string connectionId, int idJogo)
        {
            await Clients.Client(connectionId).SendAsync("IniciarJogo", idJogo);
        }
        public async Task InformarInicioDownload()
        {
            await Clients.All.SendAsync("ReceiveMessage", "user", "Download Iniciado.");
        }
        public async Task Download(string connectionId)
        {
            DadosAtualizador obj = new DadosAtualizador {
                Autenticacao=false,
                Nome = "Rota",
                Id=0,
                Versao="2.0",
                Login="Nada",
                Senha="Nda tbm",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/Rota2.zip" 

            };
            string json = JsonConvert.SerializeObject(obj);
            await Clients.All.SendAsync("Download", json);
            // await Clients.Client(connectionId).SendAsync("Download", json);
        }
        public class DadosAtualizador
        {
            public int Id{ get; set; }
            public string Nome { get; set; }
            public string PathVideo { get; set; }
            public string PathExecutavel { get; set; } 
            public string Versao { get; set; }
            //public string URL_Versao { get; set; }
            public string URL_Atualizador { get; set; }
            public bool Autenticacao { get; set; }
            public string Login { get; set; }
            public string Senha { get; set; }
        }

        public void GetConnectionId(string enderecoMac)
        {
            Controlador controlador = new Controlador{
                ConnectionId=Context.ConnectionId,
                MAC = enderecoMac
            };
            lista.Add(controlador);
            foreach (var item in lista)
            {
                Console.Write("ID: " +item.ConnectionId+ " - ");
                Console.WriteLine("MAC: " +item.MAC);
                
            }
        }
    
    }
}