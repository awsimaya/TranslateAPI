using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        [HttpPost]
        public async Task<JsonResult> Post(string commentText)
        {
            //Save the comment to your blog here
            //
            //

            // Use Comprehend to check the comment sentiment
            try
            {
                using (var comprehendClient = new AmazonComprehendClient())
                {
                    var sentimentResults = await comprehendClient.DetectSentimentAsync(
                        new DetectSentimentRequest()
                        {
                            Text = commentText,
                            LanguageCode = LanguageCode.En
                        });

                    if (sentimentResults.Sentiment.Value == "NEGATIVE")
                    {
                        using (var snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.USEast1))
                        {
                            var response = await snsClient.PublishAsync(new PublishRequest()
                            {
                                Subject = "Negative Comment",
                                Message = $"Someone posted this negative comment on your blog. Check it out {Environment.NewLine} {commentText}",
                                TargetArn = "arn:aws:sns:us-east-1:831210339789:CommentNotifier"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            return new JsonResult("Post Successful");
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult("Get Response");
        }
    }
}
