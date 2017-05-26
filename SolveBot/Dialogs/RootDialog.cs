using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace SolveBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string expression;
        private bool firstTime = true;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            await this.SendWelcomeMessageAsync(context);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            if (firstTime)
            {
                firstTime = false;
                await context.PostAsync("Hi, I'm the Expression Solver Bot. Let's get started.");
            }
            context.Call(new ReadExpressionDialog(), this.ReadExpressionDialogResumeAfter);
        }

        private async Task ReadExpressionDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                this.expression = await result;
               
                //Process the Expression
                await context.PostAsync($"You sent {expression}");
                dynamic expResult = new Solver().Execute(expression);
                await context.PostAsync($"The result is {expResult}");
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
                await this.SendWelcomeMessageAsync(context);
            }
            finally
            {
                await this.SendWelcomeMessageAsync(context);
            }
        }

    }
}