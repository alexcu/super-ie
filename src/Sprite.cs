using SplashKitSDK;

///
/// The sprite represents all possible game entities in the world.
///
abstract class Sprite : IRenderable
{
  // The current position the sprite is on.
  public Position CurrentPosition { get; set; }

  // The bitmap to be drawn (rendered by this sprite) and whether or not
  // we should actually render that sprite.
  protected Bitmap bitmap;
  private bool rendered;

  // Returns true iff this sprite is at the given position.
  public bool IsAt(Position position)
  {
    return this.CurrentPosition.Equals(position);
  }

  // Returns true iff the provided sprite is on top of this sprite (i.e., at
  // the same position).
  public bool IsAt(Sprite sprite)
  {
    return this.IsAt(sprite.CurrentPosition);
  }

  public Sprite(Position pos, Bitmap bmp)
  {
    this.CurrentPosition = pos;
    this.bitmap = bmp;
    this.rendered = true;
  }

  // Kills the sprite from the world.
  public void Remove()
  {
    Program.CurrentLevel.RemoveSprite(this);
  }

  // Renders the sprite.
  public void Render()
  {
    if (this.rendered)
    {
      // Ensure we map the "grid" coordinates to "pixel" coordinates.
      double drawX = this.CurrentPosition.CoordinateX * World.TILE_PIXELS;
      double drawY = this.CurrentPosition.CoordinateY * World.TILE_PIXELS;
      // Draw the current bitmap
      this.bitmap.Draw(drawX, drawY);
    }
  }
}
