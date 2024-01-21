using TelegaSharpProject.Application.Bot.Commands.Attributes;
using TelegaSharpProject.Application.Bot.MessageBuilder.Interfaces;
using Telegram.Bot.Types;

namespace TelegaSharpProject.Application.Bot.Commands.Abstracts;

public abstract class Command
{
    public SolverCommand SolverCommand { get; }
    
    private readonly IMessageBuilder _messageBuilder;

    public Command(IMessageBuilder messageBuilder)
    {
        _messageBuilder = messageBuilder;
        var attributes = GetType().GetCustomAttributes(typeof(SolverCommand), true);
        if (attributes.Length > 0)
        {
            var solverCommand = attributes[0] as SolverCommand;
            SolverCommand = solverCommand;
        }
        else
        {
            throw new Exception("No Attribute SolverCommand");
        }
    }

    public abstract void Execute(Message message);
}