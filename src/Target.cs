using SplashKitSDK;

///
/// The target is the aim of the game for the player to reach and should
/// only be created when all Coins have been collected.
///
class Target : Sprite, ICollidable
{
  // Static sprite bitmap for the player.
  private readonly static Bitmap BITMAP = new Bitmap("TargetSprite", "Target.png");

  // Audio effects.
  private readonly static SoundEffect SOUND_POWERUP = new SoundEffect("SoundPowerUp", "powerup.wav");

  public Target(Position pos) : base(pos, Target.BITMAP)
  {
    // When target is created, play the power up noise.
    SOUND_POWERUP.Play();
  }

  // On colision with the player, the player wins and reaches the target.
  public void CollidesWith(Player player)
  {
    player.ReachTarget();
  }
}
