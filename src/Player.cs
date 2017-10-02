using SplashKitSDK;

///
/// The player object that moves around on the screen
///
class Player : Sprite, IDirectionable, IMovable
{
  // Sound effects for our player
  private readonly static SoundEffect SOUND_WALK = new SoundEffect("PlayerWalk", "walk.wav");
  private readonly static SoundEffect SOUND_BUMP = new SoundEffect("PlayerBump", "bump.wav");
  private readonly static SoundEffect SOUND_DIE  = new SoundEffect("PlayerDie", "die.wav");
  private readonly static SoundEffect SOUND_COIN = new SoundEffect("PlayerCoin", "coin.wav");
  private readonly static SoundEffect SOUND_WIN  = new SoundEffect("PlayerWin", "win.wav");

  // Static sprite bitmap for the player
  private readonly static Bitmap BITMAP = new Bitmap("PlayerSprite", "Player.png");

  // The direction by which the player moves in and whether or not they should
  // be moving at all.
  public Cardinality Direction { get; set; }
  public bool ShouldMove { get; set; }

  // Constructor for player ensures the default options are set.
  public Player(Position pos) : base(pos, Player.BITMAP)
  {
    // Default heading is east
    this.Direction = Cardinality.East;
    this.ShouldMove = true;
  }

  // The move method captures what direction the player has been requested to
  // be moved in by the player (see the Update method in World) and moves that
  // player in such a direction.
  public void Move()
  {
    // Look for the potential new position to move in (i.e., the position given
    // the direction of the player's movement).
    Position newPos = this.CurrentPosition.GetPositionIn(this.Direction);
    // If the player can walk freely, allow them to.
    if (Position.IsTraversable(newPos))
    {
      Player.SOUND_WALK.Play();
      MoveLedger.ExecuteMove(this, newPos);
    }
    // If the player is about to hit a block...
    else if (newPos != null && newPos.IsBlocked())
    {
      Player.SOUND_BUMP.Play();
      // The player can only move if the new position in that direction
      // is also not blocked, e.g. two sprites next to each other
      Position posInDirection = newPos.GetPositionIn(this.Direction);
      if (posInDirection != null && !posInDirection.HasSprite())
      {
        MoveLedger.ExecuteMove(this, newPos);
      }
    }
  }

  // What happens when you uninstall IE and throw it in the bin :(
  public void Die()
  {
    Player.SOUND_DIE.Play();
    this.Remove();
    Program.GameOver = true;
  }

  // Simply plays a sound effect. Might like to add points or something
  // here...
  public void AwardCoin()
  {
    Player.SOUND_COIN.Play();
  }

  // Signals that the game has been won!
  public void ReachTarget()
  {
    Player.SOUND_WIN.Play();
    Program.GameOver = true;
  }
}
