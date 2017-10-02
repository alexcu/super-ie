using System.Collections.Generic;

///
/// This class acts as a ledger of how moves have been recorded in a game. It
/// is used to register the move of a position of a Sprite from A to B.
///
class MoveLedger
{
  // The object to move
  private Sprite mover;
  // Their old and new positions
  private Position oldPos;
  private Position newPos;

  // This stack retains the ledger of all moves that have been made in the game.
  private readonly static Stack<MoveLedger> MOVES_MADE = new Stack<MoveLedger>();

  // This method undoes every move that was made until the player last
  // interacted with the game.
  public static void UndoLastMoves()
  {
    // While player isn't the mover on keep undoing...
    while (MOVES_MADE.Count > 0)
    {
      MoveLedger m = MOVES_MADE.Pop();
      // Swap the current position of the mover back to their old position.
      m.mover.CurrentPosition = m.oldPos;
      if (m.mover is Player)
      {
        break;
      }
    }
  }

  // MoveRecords can only be internally instantiated and not externally
  // created. Use the ExecuteMove function to do that instead.
  private MoveLedger()
  {}

  // The ExecuteMove function registers that the moving sprite is about to
  // move to their new position.
  public static void ExecuteMove(Sprite mover, Position newPos)
  {
    MoveLedger newMove = new MoveLedger();
    newMove.mover = mover;
    newMove.oldPos = mover.CurrentPosition;
    newMove.newPos = newPos;
    // Execute the move
    mover.CurrentPosition = newPos;
    // Record move was made
    MoveLedger.MOVES_MADE.Push(newMove);
  }
}
