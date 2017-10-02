using SplashKitSDK;

///
/// The enemy is a movable and collidable object that can direct itself up and
/// down
class Enemy : Sprite, IMovable, ICollidable, IDirectionable
{
  // Declare bitmap resources
  private static readonly Bitmap BITMAP_EMPTY = new Bitmap("EnemySpriteEmpty", "EnemyEmpty.png");
  private static readonly Bitmap BITMAP_FULL  = new Bitmap("EnemySpriteFull", "EnemyFull.png");

  // Implement IDirectionable and IMovable properties
  public Cardinality Direction { get; set; }
  public bool ShouldMove { get; set; }

  // The enemy timer allows enemies to move every 1s (not on the CPU timer).
  private Timer timer;

  public Enemy(Position pos) : base(pos, Enemy.BITMAP_EMPTY)
  {
    // Default direction is randomised - internally enums are just integers.
    // We can downcast the enum West into 3 (N=0,S=1,E=2,W=3) and choose a
    // random value between 0 -> 3. Then we can upcast it back into the
    // Cardinality enum type.
    this.Direction = (Cardinality)SplashKit.Rnd((int)Cardinality.West);
    // Indicate that the enemy should be moving from the get-go.
    this.ShouldMove = true;
    // SplashKit requires unique IDs for each of our timers. We can use the
    // unique hash code for this object for that.
    this.timer = new Timer("EnemyTimer" + this.GetHashCode());
    // If the enemy timer has not started, ensure it starts.
    this.timer.Start();
  }
  public void Move()
  {
    // Every second, move the enemy object and reset the timer.
    if (this.timer.Ticks > 1000)
    {
      // Find the new position (i.e., the position that the enemy is heading
      // towards).
      Position newPos = this.CurrentPosition.GetPositionIn(this.Direction);
      // Only flip (turn around) if that new position is not traversable.
      bool shouldFlip = !Position.IsTraversable(newPos);
      if (shouldFlip)
      {
        // Flip the Heading to its inverse, if North/South if West/East etc.
        switch (this.Direction)
        {
          case Cardinality.North:
            this.Direction = Cardinality.South;
            break;
          case Cardinality.South:
            this.Direction = Cardinality.North;
            break;
          case Cardinality.East:
            this.Direction = Cardinality.West;
            break;
          case Cardinality.West:
            this.Direction = Cardinality.East;
            break;
        }
      }
      else
      {
        // Execute a move for the enemy to move to this new position.
        MoveLedger.ExecuteMove(this, newPos);
      }
      // Ensure we reset our 1s timer.
      this.timer.Reset();
    }
  }

  public void CollidesWith(Player player)
  {
    // When the enemy collides with the player, the player must die.
    player.Die();
    // Also, the recycling bin should now show up as 'full' as he's eaten
    // the IE program D:
    this.bitmap = Enemy.BITMAP_FULL;
  }
}
