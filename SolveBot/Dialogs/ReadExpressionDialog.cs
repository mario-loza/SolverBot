using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace SolveBot.Dialogs
{
    [Serializable]
    public class ReadExpressionDialog : IDialog<string>
    {

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("What expression do you want to solve?");

            context.Wait(this.MessageReceivedAsync);
        }

        private int attempts = 3;

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                context.Done(message.Text);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand. What is the expression to solve?");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                    context.Fail(new TooManyAttemptsException("Message was not a string or was an empty string."));
            }
        }
    }
}