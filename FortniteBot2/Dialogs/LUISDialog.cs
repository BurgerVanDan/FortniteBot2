using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FortniteBot.Dialogs
{
    [Serializable]
    [LuisModel("ef14217d-6759-43f6-adb0-9a8888ae22d5", "1a78fc74e0fc4abc95eb079d30403cc8")]
    public class LUISDialog : LuisDialog<ForniteData>
    {
        private readonly BuildFormDelegate<ForniteData> GatherData;

        public LUISDialog(BuildFormDelegate<ForniteData> gatherData)
        {
            this.GatherData = gatherData;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent("CreateForm")]
        public async Task FormCreation(IDialogContext context, LuisResult result)
        {
            var form = new FormDialog<ForniteData>(new ForniteData(), this.GatherData, FormOptions.PromptInStart);
            context.Call(form, Callback);
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
    }
}