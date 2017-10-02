using SplashKitSDK;

///
/// The block class is a simple class that can be pushed around on the screen.
///
class Block : Sprite, ICollidable
{
  private static readonly Bitmap BITMAP = new Bitmap("BlockSprite", "Block.png");

  public Block(Position pos) : base(pos, Block.BITMAP)
  {}

  public void CollidesWith(Player plr)
  {
    // We need to check if block's position in the direction of the player
    // is ALSO blocked (can't move two blocks or sprites in a line). So,
    // therefore only move the block if it is not blocked by another block
    // or has another sprite there.
    Position newPos = this.CurrentPosition.GetPositionIn(plr.Direction);
    if (Position.IsTraversable(newPos) && !newPos.HasSprite())
    {
      // We should now move along with the block too, so record that the move
      // has now occured.
      MoveLedger.ExecuteMove(this, newPos);
    }
  }
}
