using System;
using SplashKitSDK;

///
/// Main entry point to the program
///
static class Program
{
  // Current world object. You can define the dimensions of the world here.
  public static World CurrentLevel { get; } = new World(15, 15);
  public static bool GameOver = false;

  static void Main()
  {
    // Main game loop... keep running while the window is open.
    while (!Program.CurrentLevel.IsWindowCloseRequested())
    {
      // Process events must be called to check if user input has been made.
      SplashKit.ProcessEvents();
      // Only render and update the current level when the game is not over.
      if (!Program.GameOver)
      {
        Program.CurrentLevel.Update();
        Program.CurrentLevel.Render();
      }
    }
  }
}
