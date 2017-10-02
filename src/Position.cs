using System.Collections.Generic;

///
/// The position class retains the collection of coordinates on our tile-based
/// grid system.
///
class Position
{
  // The respective X and Y coordinates on our grid.
  public int CoordinateX { get; }
  public int CoordinateY { get; }

  public Position(int x, int y)
  {
    this.CoordinateX = x;
    this.CoordinateY = y;
  }

  // A position is traversable if (1) it exists, and (2) it is not blocked.
  public static bool IsTraversable(Position position)
  {
    return position != null && !position.IsBlocked();
  }

  // Gets the position in the given cardinality respective to this position.
  public Position GetPositionIn(Cardinality cardinality)
  {
    return Program.CurrentLevel.GetPositionAt(this, cardinality);
  }

  // Returns all sprites on this position.
  public List<Sprite> Sprites()
  {
    return Program.CurrentLevel.GetSpritesAt(this);
  }

  // Returns true iff there are sprites on this position.
  public bool HasSprite()
  {
    return Sprites().Count > 0;
  }

  // Returns true if there blocks on this position.
  public bool IsBlocked()
  {
    foreach (Sprite s in Sprites())
    {
      if (s is Block)
      {
        return true;
      }
    }
    return false;
  }
}
