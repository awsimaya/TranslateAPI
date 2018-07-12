using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Amazon.Translate;

namespace TranslateAPI.Controllers
{
    [Route("api/[controller]")]
    public class TranslateController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> Post(string translateText, string inputLanguage, string outputLanguage)
        {
            try
            {
                using (var translateClient = new AmazonTranslateClient())
                {
                    var translateTextResponse = await translateClient.TranslateTextAsync(
                        new Amazon.Translate.Model.TranslateTextRequest()
                        {
                            Text = translateText,
                            SourceLanguageCode = inputLanguage,
                            TargetLanguageCode = outputLanguage
                        });
                    
                    return new JsonResult(translateTextResponse.HttpStatusCode == System.Net.HttpStatusCode.OK ?
                           translateTextResponse.TranslatedText : "Error processing request");

                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult("Get Response");
        }
    }
}
