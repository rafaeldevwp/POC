using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Estudos.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class FilesController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        Guid Id = Guid.NewGuid();

        if (file == null || file.Length == 0)
        {
            return BadRequest();
        }

        //Atribuindo ID ao Arquivo Inserido
        var ArquivoID = $"{Id}.{file.FileName.ToLower()}";

        //Determina onde o arquivo será salvo, apartir do diretorio corrente
        var caminho = Path.Combine(Directory.GetCurrentDirectory(), "file_server", ArquivoID);

        //Atribui o caminho do diretorio ao Stream para criar o arquivo
        using (FileStream fs = new FileStream(caminho, FileMode.CreateNew))
        {
            //Copia o arquivo para o caminho preparado
            await file.CopyToAsync(fs);
        }

        //Lendo os Bytes do arquivo
        var fileBytes = System.IO.File.ReadAllBytes(caminho);

        //Converte os Bytes em Base64
        var fileBase64 = Convert.ToBase64String(fileBytes);

        return Ok();
    }

    [HttpGet(Name = "ObterArquivo")]
    public async Task<IActionResult> GetFile([FromQuery] string Name)
    {
        if (String.IsNullOrEmpty(Name))
        {
            return BadRequest();
        }

        var caminho = Path.Combine(Directory.GetCurrentDirectory(), "file_server");
        var retorno = Directory.GetFiles(caminho).Where(x => x.Contains(Name.ToLower()))?.FirstOrDefault();

        Byte[] FileBytes;
        var fileBase64 = "";

        FileBytes = await System.IO.File.ReadAllBytesAsync(retorno);
        fileBase64 = Convert.ToBase64String(FileBytes);

        return Ok(fileBase64);

    }

    [HttpGet(Name = "ObterTodosArquivos")]
    public IActionResult GetFiles()
    {

        var caminho = Path.Combine(Directory.GetCurrentDirectory(), "file_server");
        var arquivos = Directory.GetFiles(caminho).ToList();

        IList<string> ListaArquivos = new List<string>();
        var guidLength = 0;
       

        arquivos.ForEach(a =>
        {
            guidLength = 54;
            ListaArquivos.Add(a.Substring(guidLength));
        });

        var json = JsonConvert.SerializeObject(ListaArquivos);

        return Ok(json);
    }

}


