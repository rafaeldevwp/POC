using Microsoft.AspNetCore.Mvc;

namespace Estudos.Controllers;

[ApiController]
[Route("[controller]")]

public class FilesController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        Guid Id = Guid.NewGuid();

        if (file == null || file.Length == 0){
            return BadRequest();
        }    
        
        //Atribuindo ID ao Arquivo Inserido
        var ArquivoID = $"{Id}.{file.FileName}";

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

    [HttpGet]
    public async Task<IActionResult> GetFile([FromQuery] string Name)
    {
        if(String.IsNullOrEmpty(Name)){
            return BadRequest();
        }

        var caminho = Path.Combine(Directory.GetCurrentDirectory(), "file_server");
        var retorno =  Directory.GetFiles(caminho).Where(x => x.Contains(Name))?.FirstOrDefault();

        Byte[] FileBytes;
        var fileBase64 = "";

            FileBytes = await System.IO.File.ReadAllBytesAsync(retorno);
            fileBase64 = Convert.ToBase64String(FileBytes);

        return Ok(fileBase64);

    }

}


