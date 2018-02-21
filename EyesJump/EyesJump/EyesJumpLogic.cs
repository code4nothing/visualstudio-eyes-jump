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
      _logic = new Logic();
    }

    private readonly IServiceProvider _serviceProvider;
    private Logic _logic;

    public event EventHandler SuspendInput;

    public void ManageInput(InputHandlerArgs args)
    {
      _logic = _logic.RunLogic();
    }
  }

  internal class Logic
  {
    public Logic RunLogic()
    {
      ShowMessage("Eyes Jump", "Plugin is running");
      return this;
    }

    private void ShowMessage(string title, string message)
    {
      var statusbar = Package.GetGlobalService(typeof(IVsStatusbar)) as IVsStatusbar;
      statusbar.SetText($"{title}; {message}");
    }
  }
}