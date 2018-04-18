using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FortniteBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        bool UserNameSetFlag = false;
        public async Task StartAsync(IDialogContext context)
        {
            var replyMessage = context.MakeMessage();
            replyMessage.Text = "Hi, I'm Fortnite Bot.";
            await context.PostAsync(replyMessage);
            await Respond(context);

            context.Wait(MessageReceivedAsync);
        }

        public async Task Respond(IDialogContext context)
        {
            var userName = String.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue<bool>("GetName", true);
                UserNameSetFlag = true;
            }
            else
            {
                await context.PostAsync(String.Format("Hi {0}.  How can I help you today?", userName));
            }
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var userName = String.Empty;
            var getName = false;
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }

            if (UserNameSetFlag)
            {
                await Respond(context);
                UserNameSetFlag = false;
            }
            context.Done(message);
        }
    }
}