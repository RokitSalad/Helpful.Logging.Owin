using System.Web.Http;

namespace TestApplication
{
    /// <summary>
    /// Showing an input model is found correctly after the debug logging middleware has got in the way
    /// </summary>
    public class ModelReturnController : ApiController
    {
        [Route("modelreturn"), AcceptVerbs("post")]
        public InputModel ReturnModel(InputModel model)
        {
            return model;
        }
    }

    public class InputModel
    {
        public string SomeValue { get; set; }
        public int SomeNumber { get; set; }
    }
}
