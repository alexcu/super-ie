using SplashKitSDK;
using System.Collections.Generic;

///
/// The world encapsulates all data that exists for interaction between
/// different sprites.
///
class World : IRenderable
{
  // Declare the player and target. Target is set to null until such conditions
  // are satisified such that it should be created.
  private Player player;
  private Target target;

  // Positions is a 2D array representing our grid of where sprites can be.
  // This constant TILE_PIXELS declares how big each tile is in our "grid" in
  // pixels, while width and height represent how big the grid actually is.
  private Position[,] positions;
  private int width, height;
  public const int TILE_PIXELS = 32;

  // This list encapsulates all sprites given on the screen.
  private List<Sprite> sprites;

  // The SplashKit rendering window.
  private Window window;

  public World(int width, int height)
  {
    this.width = width;
    this.height = height;
    this.window = new Window("Super Internet Explorer",
                             width * TILE_PIXELS, height * TILE_PIXELS);
    SetupWorld();
  }

  // This method randomly places sprites (coins, enemies blocks) around
  // on the grid.
  private void PlaceRandomSpriteAt(Position pos)
  {
    float rnd = SplashKit.Rnd();

    if (rnd > 0.75f && rnd <= 0.8f)
    {
      sprites.Add(new Coin(pos));
    }
    else if (rnd > 0.8f && rnd <= 0.85)
    {
      sprites.Add(new Enemy(pos));
    }
    else if (rnd > 0.85)
    {
      sprites.Add(new Block(pos));
    }
  }

  // This function re-initialises the entire world by scrapping all previous
  // data and re-creating the grid and all sprites.
  private void SetupWorld()
  {
    this.positions = new Position[width, height];
    this.sprites = new List<Sprite>();
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        Position pos = new Position(x, y);
        this.positions[x,y] = pos;
        // ALWAYS place the player in the top left.
        if (x == 0 && y == 0)
        {
          this.player = new Player(pos);
          sprites.Add(this.player);
        }
        else
        {
          PlaceRandomSpriteAt(pos);
        }
      }
    }
  }

  // Alias method for SplashKit's CloseRequested property.
  public bool IsWindowCloseRequested()
  {
    return this.window.CloseRequested;
  }

  // Returns the position of the grid at the given grid's x and y coordinate.
  public Position GetPositionAt(int x, int y)
  {
    if (x >= this.width || y >= this.height || x < 0 || y < 0)
    {
      return null;
    }
    return this.positions[x,y];
  }

  // Returns the position with respective cardinality direction of the given
  // position. E.g., give me the position north of positionX.
  public Position GetPositionAt(Position position, Cardinality direction)
  {
    switch (direction)
    {
      case Cardinality.North:
        return GetPositionAt(position.CoordinateX, position.CoordinateY - 1);
      case Cardinality.South:
        return GetPositionAt(position.CoordinateX, position.CoordinateY + 1);
      case Cardinality.East:
        return GetPositionAt(position.CoordinateX + 1, position.CoordinateY);
      case Cardinality.West:
        return GetPositionAt(position.CoordinateX - 1, position.CoordinateY);
      default:
        return null;
    }
  }

  // Returns a list of all sprites at a given position. This method needs
  // to be re-written as it is very slow.
  public List<Sprite> GetSpritesAt(Position position)
  {
    // Result sprites.
    List<Sprite> resultSprites = new List<Sprite>();
    foreach (Sprite spr in this.sprites)
    {
      // Get only those sprites who are at this position AND where spr's
      // type matches that of the subtype parameter
      if (spr.IsAt(position))
      {
        resultSprites.Add(spr);
      }
    }
    return resultSprites;
  }

  // Removes a sprite from existence.
  public void RemoveSprite(Sprite sprite)
  {
    this.sprites.Remove(sprite);
  }

  // Renders the world.
  public void Render()
  {
    // Gives the background the classic Win95 feel.
    this.window.Clear(SplashKit.ColorTeal());
    foreach (Sprite spr in this.sprites)
    {
      spr.Render();
    }
    // Must call refresh to actually render our canvas to screen.
    this.window.Refresh();
  }

  // Updates the entire world on each loop within the main loop in Program.cs
  public void Update()
  {
    // Reset World requested?
    if (SplashKit.KeyTyped(KeyCode.RKey))
    {
      SetupWorld();
    }
    // Undo requested?
    if (SplashKit.KeyTyped(KeyCode.ZKey))
    {
      MoveLedger.UndoLastMoves();
    }
    // When we modify each sprite, we may remove them from the list (i.e.,
    // by calling Remove(Sprite sprite). If we do that, we modify the foreach
    // list during enumerating through the list. Therefore we must use a
    // C-style for loop.
    for (int i = 0; i < this.sprites.Count; i++)
    {
      Sprite spr = this.sprites[i];
      // If this sprite is collidable and it collides with the player...
      if (spr is ICollidable)
      {
        if (spr.IsAt(this.player))
        {
          // Downcast sprite 's' into collidable object and this.player into
          // a collide object
          ICollidable collidableSpr = ((ICollidable)spr);
          collidableSpr.CollidesWith(this.player);
        }
      }
      // Try move the player first
      if (spr.Equals(this.player))
      {
        // Key typed events
        if (SplashKit.KeyTyped(KeyCode.UpKey))
        {
          this.player.Direction = Cardinality.North;
          this.player.Move();
        }
        if (SplashKit.KeyTyped(KeyCode.DownKey))
        {
          this.player.Direction = Cardinality.South;
          this.player.Move();
        }
        if (SplashKit.KeyTyped(KeyCode.LeftKey))
        {
          this.player.Direction = Cardinality.West;
          this.player.Move();
        }
        if (SplashKit.KeyTyped(KeyCode.RightKey))
        {
          this.player.Direction = Cardinality.East;
          this.player.Move();
        }
      }
      // Check if this sprite is another Movable kind of sprite.
      else if (spr is IMovable)
      {
        // Downcast sprite 's' into movable object
        IMovable movableSpr = ((IMovable)spr);
        if (movableSpr.ShouldMove)
        {
          movableSpr.Move();
        }
      }
      // Check if all coins have been `eaten' up. If so, create the target
      // somewhere randomly on our screen.
      if (this.AllCoinsEaten() && this.target == null)
      {
        // Until the target has been set...
        while (this.target == null)
        {
          int rndX = SplashKit.Rnd(width);
          int rndY = SplashKit.Rnd(height);
          Position rndPos = this.GetPositionAt(rndX, rndY);
          // Find a random position that has no sprites and place
          // the target there :)
          if (!rndPos.HasSprite())
          {
            this.target = new Target(rndPos);
            this.sprites.Add(this.target);
          }
        }
      }
    }
  }

  // Returns truee if there are no more coins in the world.
  private bool AllCoinsEaten()
  {
    foreach (Sprite s in this.sprites)
    {
      if (s is Coin)
      {
        return false;
      }
    }
    return true;
  }

  // Returns trus if the given sprite is at the given coordinate.
  public bool IsAt(Sprite sprite)
  {
    return this.IsAt(sprite);
  }
}
