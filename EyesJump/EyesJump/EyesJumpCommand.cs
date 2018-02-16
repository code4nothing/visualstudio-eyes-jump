using System;
using System.ComponentModel.Design;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace EyesJump
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal sealed class EyesJumpCommand
  {
    /// <summary>
    /// Command ID.
    /// </summary>
    public const int CommandId = 0x0100;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("bfee7a2b-87b5-41ba-9905-bd792212a23b");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly Package _package;

    /// <summary>
    /// Initializes a new instance of the <see cref="EyesJumpCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    private EyesJumpCommand(Package package)
    {
      _package = package ?? throw new ArgumentNullException(nameof(package));

      if (ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
      {
        var menuCommandId = new CommandID(CommandSet, CommandId);
        var menuItem = new MenuCommand(MenuItemCallback, menuCommandId);
        commandService.AddCommand(menuItem);
      }
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static EyesJumpCommand Instance
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private IServiceProvider ServiceProvider => _package;

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static void Initialize(Package package)
    {
      Instance = new EyesJumpCommand(package);
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void MenuItemCallback(object sender, EventArgs e)
    {
      var message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", GetType().FullName);
      var title = "EyesJumpCommand";

      // Show a message box to prove we were here
      VsShellUtilities.ShowMessageBox(
          ServiceProvider,
          message,
          title,
          OLEMSGICON.OLEMSGICON_INFO,
          OLEMSGBUTTON.OLEMSGBUTTON_OK,
          OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
    }
  }
}
