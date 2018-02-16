using System;

namespace EyesJump
{
  internal delegate void InputHandler(object sender, InputHandlerArgs args);

  internal class InputHandlerArgs
  {
    public static InputHandlerArgs Empty { get; } = new InputHandlerArgs();

    private char TypedChar { get; }

    public InputHandlerArgs(char typedChar)
    {
      TypedChar = typedChar;
    }

    public InputHandlerArgs()
    {
    }
  }
}