using System;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EyesJump
{
  internal class EyesJumpLogic
  {
    public EyesJumpLogic(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
      _logic = new InitLogic(this);
    }

    private readonly IServiceProvider _serviceProvider;
    private ILogic _logic;

    public event EventHandler StopHandler;

    public delegate void MessageHandler(string title, string message);
    public event MessageHandler Message;

    public event EventHandler StartHandler;

    public void ManageInput(InputHandlerArgs args)
    {
      _logic = _logic.RunLogic();
    }

    protected virtual void OnStopHandler()
    {
      StopHandler?.Invoke(this, EventArgs.Empty);
    }

    protected internal virtual void OnMessage(string title, string message)
    {
      Message?.Invoke(title, message);
    }

    protected virtual void OnStartHandler()
    {
      StartHandler?.Invoke(this, EventArgs.Empty);
    }
  }

  internal interface ILogic
  {
    ILogic RunLogic();
  }

  internal class InitLogic : ILogic
  {
    private EyesJumpLogic eyesJumpLogic;

    public InitLogic(EyesJumpLogic eyesJumpLogic)
    {
      this.eyesJumpLogic = eyesJumpLogic;
    }

    public ILogic RunLogic()
    {
      eyesJumpLogic.OnMessage("Eyes Jump", "Plugin logic starts");
      return this;
    }
  }
}