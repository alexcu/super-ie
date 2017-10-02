///
/// An interface to indicate that this object moves on its own accord
///
interface IMovable
{
  void Move();
  // This property indicates whether or not the given object should currently
  // be moving or not moving.
  bool ShouldMove { get; set; }
}
