﻿using System.Reflection;
using TelegaSharpProject.Application.Bot.Buttons.Base;
using TelegaSharpProject.Application.Bot.Commands;
using TelegaSharpProject.Application.Bot.Settings;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static TelegaSharpProject.Application.Bot.SolverChat;

namespace TelegaSharpProject.Application.Bot;

public class SolverBot
{
    private readonly ReceiverOptions _receiverOptions;
    public static ITelegramBotClient BotClient { get; set; }

    private static readonly Dictionary<string, MethodInfo> CommandsDict = new();
    public static readonly Dictionary<string, ButtonBase> buttonsDict = new();

    public SolverBot(ButtonBase[] buttons, AppSettings settings)
    {
        foreach (var button in buttons)
            buttonsDict.Add(button.Data, button);
        
        BotClient = new TelegramBotClient(settings.Token);

        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery,
            },
            ThrowPendingUpdates = true,
        };
        
        LoadCommands();
    }

    public async Task Start()
    {
        using var cts = new CancellationTokenSource();
        BotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        var me = await BotClient.GetMeAsync(cancellationToken: cts.Token);
        Console.WriteLine($"{me.FirstName} запущен!");
    }

    private void LoadCommands()
    {
        var classType = new CommandModule();
        var methods = classType.GetType().GetMethods().Where(m => m.GetCustomAttributes(typeof(SolverCommand), false).Length > 0)
            .ToArray();

        foreach (var method in methods)
        {
            CommandsDict.Add(method.GetCustomAttribute<SolverCommand>().Command.ToLower(), method);
            if(method.GetCustomAttribute<SolverCommand>().Aliases.Length > 0)
                foreach (var alias in method.GetCustomAttribute<SolverCommand>().Aliases)
                {
                    CommandsDict.Add(alias.ToLower(), method);
                }
        }
    }

    private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        var message = update.Message;
                        var textUser = message.From;
                        Console.WriteLine($"{textUser.FirstName} ({textUser.Id}) написал: {message.Text}");

                        if (CommandsDict.TryGetValue(message.Text.ToLower(), out var method))
                        {
                            CommandExecuter.ExecuteCommand(method, message, null);
                            //CommandExecuter.ExecuteCommand(method,message, new object[] { "messege" });
                        }
                        else
                        {
                            SolverChat.GetSolverChat(message).SendInput(message);
                        }
                        return;
                    }
                case UpdateType.CallbackQuery:
                    {
                        var callbackQuery = update.CallbackQuery;
                        var CallbackUser = callbackQuery.From;
                        Console.WriteLine($"{CallbackUser.FirstName} ({CallbackUser.Id}) нажал на: {callbackQuery.Data}");
                        GetSolverChat(callbackQuery).chatState = ChatState.WaitForCommand;

                        if (buttonsDict.TryGetValue(callbackQuery.Data.ToLower(), out var method))
                            method.Execute(callbackQuery);

                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return;
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}