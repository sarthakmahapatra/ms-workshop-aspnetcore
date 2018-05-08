using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("Add function processed a request.");

    int sum;

    try
    {
        int value1;
        int value2;
         // Get request body
        dynamic data = await req.Content.ReadAsAsync<object>();
        value1 = int.Parse(data.value1.ToString());
        value2 = int.Parse(data.value2.ToString());

        sum = value1 + value2;
        log.Info($"{value1} + {value2} = {sum}");

    }
    catch(Exception ex)
    {
        log.Error(ex.Message);
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass valid values");
    }
    
    return req.CreateResponse(HttpStatusCode.OK,sum);
}