using SplashKitSDK;

///
/// Coins are made to be collectable in-game so that the target can appear.
///
class Coin : Sprite, ICollidable
{
  private static readonly Bitmap BITMAP = new Bitmap("CoinSprite", "Coin.png");

  public Coin(Position pos) : base(pos, Coin.BITMAP)
  {}

  public void CollidesWith(Player player)
  {
    // On collision with the player, give the player a coin and then remove
    // this sprite from existence.
    player.AwardCoin();
    this.Remove();
  }
}
