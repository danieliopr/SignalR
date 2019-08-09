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
        private static int numOfClient;

        public async Task ReiniciarControlador(string connectionId)
        {
            await Clients.Client(connectionId).SendAsync("Reiniciar");
        }
        public void StatusTarefa(string frase, string enderecoMac, bool status)
        {
            if(status)
            {
                //Sucesso
                Console.WriteLine(frase + "(MAC: " + enderecoMac + ")");
            }else
            {
                //Erro
                Console.WriteLine(frase + "(MAC: " + enderecoMac + ")");
            }
        } 
        public async Task IniciarJogo(string connectionId, int idJogo)
        {
            await Clients.Client(connectionId).SendAsync("IniciarJogo", idJogo);
        }
        public async Task InformarInicioDownload()
        {
            await Clients.All.SendAsync("ReceiveMessage", "user", "Download Iniciado.");
        }
        public async Task RemoverJogoInstalado(string connectionId, string idJogo)
        {
            await Clients.All.SendAsync("RemoverJogo", idJogo);
            // await Clients.Client(connectionId).SendAsync("RemoverJogo", idJogo);
        }
        public async Task DowngradeAplicacao(string connectionId, string idJogo)
        {
            await Clients.All.SendAsync("DowngradeAplicacao", idJogo);
            // await Clients.Client(connectionId).SendAsync("DowngradeAplicacao", idJogo);
        }
        public async Task DownloadJogo1(string connectionId)
        {
            DadosAtualizador obj1 = new DadosAtualizador {
                Autenticacao=false,
                Nome = "Rota",
                Id=1,
                Versao="1.0",
                PathVideo=@"D:\Games\Rota\1.0\Video\Havaianas1.mp4",
                PathExecutavel=@"D:\Games\Rota\1.0\Executavel\sublime_text.exe - Atalho",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/JogoUm.zip",
                Ativo=true
            };
            string json = JsonConvert.SerializeObject(obj1);
            await Clients.All.SendAsync("Download", json);
            // await Clients.Client(connectionId).SendAsync("Download", json);
        } 
        public async Task DownloadJogo2(string connectionId)
        {
            DadosAtualizador obj2 = new DadosAtualizador {
                Autenticacao=false,
                Nome = "Caipira", 
                Id=2,
                Versao="1.0",
                PathVideo=@"D:\Games\Caipira\1.0\Video\Havaianas2.mp4",
                PathExecutavel=@"D:\Games\Caipira\1.0\Executavel\sublime_text.exe - Atalho",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/JogoDois.zip",
                Ativo=true
            }; 
            string json = JsonConvert.SerializeObject(obj2);
            await Clients.All.SendAsync("Download", json);
            // await Clients.Client(connectionId).SendAsync("Download", json);
        }
        public async Task DownloadJogo3(string connectionId)
        {
            DadosAtualizador obj3 = new DadosAtualizador {
                Autenticacao=false,
                Nome = "Ponte",
                Id=3,
                Versao="1.0",
                PathVideo=@"D:\Games\Ponte\1.0\Video\Havaianas3.mp4",
                PathExecutavel=@"D:\Games\Ponte\1.0\Executavel\sublime_text.exe - Atalho",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/JogoTres.zip",
                Ativo=true
            };
            string json = JsonConvert.SerializeObject(obj3);
            await Clients.All.SendAsync("Download", json);
            // await Clients.Client(connectionId).SendAsync("Download", json);
        }
        public async Task DownloadControlador(string connectionId, string idControlador)
        {
            DadosAtualizador obj3 = new DadosAtualizador {
                Autenticacao=false,
                Nome = "GameController",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/GameController.zip"
            };
            string json = JsonConvert.SerializeObject(obj3);
            await Clients.All.SendAsync("DownloadControlador", json);
            // await Clients.Client(connectionId).SendAsync("Download", json);
        }
        public async Task DownloadJogo4(string connectionId)
        {
            DadosAtualizador obj4 = new DadosAtualizador {
                Autenticacao=false,
                Nome = "Ponte",
                Id=3,
                Versao="2.0",
                PathVideo=@"D:\Games\Ponte\2.0\Video\Havaianas4.mp4",
                PathExecutavel=@"D:\Games\Ponte\2.0\Executavel\sublime_text.exe - Atalho",
                URL_Atualizador="https://s3.us-east-2.amazonaws.com/www.rotasimuladores.com.br/JogoQuatro.zip",
                Ativo=true
            };
            string json = JsonConvert.SerializeObject(obj4);
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
            public bool Ativo { get; set; }
            public string Senha { get; set; }
        }
        public void PrimeiroContato(string enderecoMac, string ListaJogosInstalados)
        {
            Controlador controlador = new Controlador{
                ConnectionId=Context.ConnectionId,
                MAC = enderecoMac
            };

            //TODO verificar se a lista de jogos recebida condiz com a lista de jogos da estação no servidor
            var obj = JsonConvert.DeserializeObject(ListaJogosInstalados);
            Console.WriteLine(obj);
            Console.WriteLine("ID: " + controlador.ConnectionId);
            Console.WriteLine("MAC: " + controlador.MAC);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var ass = Context.ConnectionId;
            await base.OnDisconnectedAsync(exception);
            System.Threading.Interlocked.Decrement(ref numOfClient);
            Console.WriteLine("Desconectado: " + numOfClient);
            Console.WriteLine(ass );
        }
        public override async Task OnConnectedAsync()
        {
            var id = Context.ConnectionId;
            await base.OnConnectedAsync();
            System.Threading.Interlocked.Increment(ref numOfClient);
            Console.WriteLine("Iniciando conexao {0}", numOfClient);
            Console.WriteLine(id);  
        }
        
    }
}