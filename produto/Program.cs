using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static string webhookUrl = "https://discord.com/api/webhooks/1211726215069835305/ojZwUKgaTZpAUKJlUuMtBlaXHGbDYQyKgqgZwBOE2wlQTwN8LUEoGL027RBkYHZbAflk";
    static string log = "";

    static void Main(string[] args)
    {
        Task.Run(async () =>
        {
            bool showMenu = true;

            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }).GetAwaiter().GetResult();
    }

    private static async Task SendLogToWebhook(string message)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string json = $"{{ \"content\": \"{message}\" }}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(webhookUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Erro ao enviar mensagem para a Webhook do Discord. Status: " + response.StatusCode);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao enviar a mensagem para a Webhook do Discord: " + ex.Message);
        }
    }

    private static bool AddProduct()
    {
        Console.WriteLine("Digite o produto que deseja adicionar:");
        string product = Console.ReadLine();

        Console.WriteLine("Digite a quantidade de " + product + " que deseja adicionar:");
        string qntdProduct = Console.ReadLine();

        try
        {
            using (StreamWriter writer = new StreamWriter("produtos.txt", true))
            {
                writer.WriteLine("Produto = " + product + " quantidade do produto " + qntdProduct);
            }

            log += $"Produto adicionado: {product}, Quantidade: {qntdProduct}\n";

            SendLogToWebhook($"```Produto adicionado: {product}, Quantidade: {qntdProduct}```");

            Console.WriteLine("Produto adicionado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao adicionar o produto: " + ex.Message);
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
        return true;
    }

    private static bool CleanSystem()
    {
        try
        {
            File.WriteAllText("produtos.txt", string.Empty);
            log += "Todos os produtos foram removidos com sucesso!\n";
            Console.WriteLine("Todos os produtos foram removidos com sucesso!");
            SendLogToWebhook($"```todos os produtos foram excluidos```");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao limpar os produtos: " + ex.Message);
        }

        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
        return true;
    }

    private static bool MainMenu()
    {
        Console.Clear();
        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("[1] Adicionar produto");
        Console.WriteLine("[2] Limpar produtos");
        Console.WriteLine("[3] Sair do sistema");
        Console.Write("\r\nDigite sua escolha: ");

        switch (Console.ReadLine())
        {
            case "1":
                return AddProduct();
            case "2":
                return CleanSystem();
            case "3":
                return false;
            default:
                Console.WriteLine("Opção inválida, tente novamente!");
                Console.WriteLine("Pressione qualquer tecla para continuar...");
                Console.ReadKey();
                return true;
        }
    }
}
