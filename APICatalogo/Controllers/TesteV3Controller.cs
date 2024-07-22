using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/teste")]
    [ApiController]
    [ApiVersion(3)]
    [ApiVersion(4)]
    public class TesteV3Controller : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion(3)]
        public string GetVersion3()
        {
            return "TesteV3 - get - api versão 2.0";
        }

        [HttpGet]
        [MapToApiVersion(4)]
        public string GetVersion4()
        {
            return "TesteV4 - get - api versão 2.0";
        }
    }
}
