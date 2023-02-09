using Microsoft.AspNetCore.Mvc;

namespace Estudos.Controllers;

[ApiController]
[Route("[controller]")]

public class FilesController : ControllerBase {

[HttpPost]
public void PostFile(IFormFile file){



}

}