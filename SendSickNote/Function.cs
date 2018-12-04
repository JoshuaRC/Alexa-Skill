using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SendSickNote
{
    public class Function
    {
    
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task <SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            string outputText = "";
            var requestType = input.GetRequestType();

            if (requestType == typeof(LaunchRequest))
            {
                return BodyResponse("Welcome to Sick Note, please say the name of a professor to send them a sick note", false);
            }

            else if (requestType == typeof(IntentRequest))
            {
                //outputText += "Request type is Intent";
                var intent = input.Request as IntentRequest;

                if (intent.Intent.Name.Equals("StudentSickNotes"))
                {
                    var professor = intent.Intent.Slots["Professor"].Value;

                    if (professor == null)
                    {
                        return BodyResponse("I did not understand the name of the professor you wanted, please try again.", false);
                    }

                     //var playerInfo = await GetPlayerInfo(professor, context);
                    {
                        outputText = $"Email successfully sent to {professor}";
                    }

                    return BodyResponse(outputText, true);
                }

                else if (intent.Intent.Name.Equals("AMAZON.StopIntent"))
                {

                    return BodyResponse("You have now exited Sick Note", true);
                }

                else
                {
                    return BodyResponse("I did not understand this intent, please try again", true);
                }
            }

            else
            {
                return BodyResponse("I did not understand your request, please try again", true);
            }
            //return input?.ToUpper();

        }

        private SkillResponse BodyResponse(string outputSpeech,
        bool shouldEndSession,
        string repromptText = "Just say, I want to send a sick note. To exit, say, exit.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;
        }


        //private async Task<Professor> GetProfessorInfo(string lastName, string firstName, ILambdaContext context)
        //{
        //    Professor professor = new Professor();
        //    var uri = new Uri($"https://nba-players.herokuapp.com/players-stats/{lastName}/{firstName}");

        //    try
        //    {
        //        //This is the actual GET request
        //        //var response = await httpClient.GetStringAsync(uri);
        //        context.Logger.LogLine($"Response from URL:\n{response}");
        //        // TODO: (PMO) Handle bad requests
        //        //Conver the below from the JSON output into a list of player objects
        //        professor = JsonConvert.DeserializeObject<Professor>(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Logger.LogLine($"\nException: {ex.Message}");
        //        context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
        //    }

        //    return professor;
        }
    }

