using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

namespace EyesJump
{
  class InputFilter: IOleCommandTarget
  {
    private readonly IOleCommandTarget _commandHandler;

    private readonly int[] _wellKnowKeys = {
      4, // tab
      7, // left arrow
      11, // up arrow
      9, // right arrow
      13, // down arrow
      103 // escape
    };

    public event InputHandler Input;

    public InputFilter(IVsTextView adapter)
    {
      adapter.AddCommandFilter(this, out _commandHandler);
    }

    public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
    {
      return _commandHandler.QueryStatus(pguidCmdGroup, cCmds, prgCmds, pCmdText);
    }


    public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
    {
      if (_wellKnowKeys.Contains((int)nCmdID))
      {
        // send '\0' so we can abort
        Input?.Invoke(this, InputHandlerArgs.Empty);
        return VSConstants.S_OK;
      }

      if (TryGetTypedChar(pguidCmdGroup, nCmdID, pvaIn, out char typedChar))
      {
        Input?.Invoke(this, new InputHandlerArgs(typedChar));
        return VSConstants.S_OK;
      }

      return _commandHandler.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
    }

    private static bool TryGetTypedChar(Guid cmdGroup, uint nCmdID, IntPtr pvaIn, out char typedChar)
    {
      typedChar = char.MinValue;

      if (cmdGroup != VSConstants.VSStd2K || nCmdID != (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
        return false;

      typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
      return true;
    }
  }

}
