﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegaSharpProject.Application.Bot.Commands
{
    public class CommandModule
    {
        [Command("/title", new[] { "главная","/start" })]
        public void ToTitle(Message ctx)
        {
            SolverChat.GetSolverChat(ctx).ToTitle();
        }

        [Command("/leaders", new[] { "лидеры", })]
        public void ToLeaders(Message ctx)
        {
            SolverBot.botClient.SendTextMessageAsync(
                ctx.Chat.Id,
                MessageBuilder.GetLeaderBoard()
            );
        }

        [Command("/mytasks")]
        public void ToMyTasks(Message ctx)
        {
            SolverBot.botClient.SendTextMessageAsync(
                ctx.Chat.Id,
                MessageBuilder.GetMyTasks(ctx.From)
            );
        }

        [Command("/tasks")]
        public void ToTasks(Message ctx)
        {
            SolverChat.GetSolverChat(ctx).SetPage(1);
            SolverBot.botClient.SendTextMessageAsync(
                ctx.Chat.Id,
                MessageBuilder.GetTasks(1),
                replyMarkup: MessageBuilder.GetTasksMarkup()
            );
        }

        [Command("/ab")]
        public void Abobus(Message ctx)
        {
            SolverBot.botClient.SendTextMessageAsync(ctx.Chat.Id, "Сам такой");
        }
    }
}
